"""Update one FluxPay service in the GitOps overlay."""

import argparse
import base64
import os
from pathlib import Path
import re
import subprocess
import tempfile
import time

from ruamel.yaml import YAML


SERVICES = ("authorization", "consumer", "sender")
OVERLAY = Path("overlays/aks/fluxpay")
BASE_BRANCH = "master"
CHART_REGISTRY = "oci://ghcr.io/fluxpayproject/charts"
VERSION = re.compile(r"[0-9]+\.[0-9]+\.[0-9]+(?:-[A-Za-z0-9.-]+)?-run\.([0-9]+)")
DIGEST = re.compile(r"sha256:[0-9a-f]{64}")


def run_id(tag):
    match = VERSION.fullmatch(str(tag))
    return int(match[1]) if match else 0


def replace_values(path, changes):
    lines = path.read_text().splitlines(keepends=True)
    for mapping, key, replacement in changes:
        line, column = mapping.lc.value(key)
        current = re.match(r"""(?:"[^"]*"|'[^']*'|[^\s#]+)""", lines[line][column:])
        if not current:
            raise ValueError(f"Could not locate the {key} scalar in {path}")
        value = current[0]
        quote = value[0] if value[0] in "\"'" else ""
        lines[line] = lines[line][:column] + quote + replacement + quote + lines[line][column + len(value):]
    path.write_text("".join(lines))


def update_overlay(checkout, service, version, digest, *, existing_branch):
    yaml = YAML()
    overlay = checkout / OVERLAY
    sources_path = overlay / "sources.yml"
    sources = [
        source for source in yaml.load_all(sources_path)
        if source and source.get("kind") == "OCIRepository"
        and source["metadata"]["name"] == service
    ]
    if len(sources) != 1:
        raise ValueError(f"Expected one Flux OCIRepository for {service}")
    chart = sources[0]["spec"]
    chart_ref = chart["ref"]
    chart_url = f"{CHART_REGISTRY}/{service}"
    if chart_ref.get("digest") or chart_ref.get("semver"):
        raise ValueError(f"Expected a tag-based chart pin for {service}")

    values_path = overlay / f"values-{service}.yaml"
    image = yaml.load(values_path)["image"]
    if not DIGEST.fullmatch(str(image.get("digest"))):
        raise ValueError(f"Expected an existing image digest pin for {service}")
    tags = [chart_ref["tag"], image["tag"]]
    if chart["url"] == chart_url and all(tag == version for tag in tags) and image["digest"] == digest:
        print(f"Flux already references {service} {version}")
        return False
    if existing_branch and max(map(run_id, tags)) > run_id(version):
        print(f"Skipping {service} {version}; a newer Azure run is already recorded")
        return False

    replace_values(sources_path, [(chart, "url", chart_url), (chart_ref, "tag", version)])
    replace_values(values_path, [(image, "digest", digest), (image, "tag", version)])
    return True


def publish(repository, service, version, source_commit, branch, digest):
    token = os.environ.pop("GITOPS_TOKEN", "")
    if not token:
        raise ValueError("GITOPS_TOKEN is required")
    auth = base64.b64encode(f"x-access-token:{token}".encode()).decode()
    git_env = dict(
        os.environ,
        GIT_CONFIG_COUNT="1",
        GIT_CONFIG_KEY_0="http.https://github.com/.extraheader",
        GIT_CONFIG_VALUE_0=f"Authorization: Basic {auth}",
        GIT_TERMINAL_PROMPT="0",
    )

    with tempfile.TemporaryDirectory(prefix="fluxpay-gitops-") as directory:
        checkout = Path(directory)

        def git(*args, check=True, capture=False):
            return subprocess.run(
                ["git", *args], cwd=checkout, env=git_env, check=check,
                stdout=subprocess.PIPE if capture else None, text=True,
            )

        git("init", "--quiet")
        git("remote", "add", "origin", repository)
        git("config", "user.name", "FluxPay Azure Pipelines")
        git("config", "user.email", "fluxpay-ci@users.noreply.github.com")

        for attempt in range(3):
            target = f"refs/heads/{branch}"
            remote = git("ls-remote", "--exit-code", "--heads", "origin", target,
                         check=False, capture=True)
            if remote.returncode not in (0, 2):
                remote.check_returncode()
            if remote.returncode == 2 and branch == BASE_BRANCH:
                raise RuntimeError(f"GitOps base branch {BASE_BRANCH} does not exist")
            base_ref = target if remote.returncode == 0 else f"refs/heads/{BASE_BRANCH}"
            git("fetch", "--quiet", "--depth=1", "origin", base_ref)
            git("checkout", "--quiet", "--detach", "--force", "FETCH_HEAD")

            if not update_overlay(checkout, service, version, digest, existing_branch=remote.returncode == 0):
                return
            with tempfile.TemporaryFile() as rendered:
                subprocess.run(
                    ["kubectl", "kustomize", str(OVERLAY)], cwd=checkout,
                    stdout=rendered, check=True,
                )
            git("add", "--", str(OVERLAY / "sources.yml"),
                str(OVERLAY / f"values-{service}.yaml"))
            git("diff", "--cached", "--check")
            git("commit", "-m", f"Deploy FluxPay {service} {version}", "-m", f"Source: {source_commit}")
            if git("push", "origin", f"HEAD:{target}", check=False).returncode == 0:
                return
            if attempt < 2:
                time.sleep(2 ** attempt)
        raise RuntimeError(f"Could not push FluxPay {version} to {branch} after three attempts")


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--repository", required=True)
    parser.add_argument("--service", choices=SERVICES, required=True)
    parser.add_argument("--version", required=True)
    parser.add_argument("--source-commit", required=True)
    parser.add_argument("--release-dir", type=Path, required=True)
    parser.add_argument("--pull-request", type=int)
    args = parser.parse_args()
    if not re.fullmatch(r"https://github\.com/[\w.-]+/fluxcd-repository(?:\.git)?", args.repository):
        parser.error("--repository must be the HTTPS URL of fluxcd-repository on GitHub")
    if not run_id(args.version) or len(args.version) > 128:
        parser.error("--version must be a valid Azure release tag")
    if not re.fullmatch(r"[0-9a-f]{40}", args.source_commit):
        parser.error("--source-commit must be a Git SHA")
    if args.pull_request is not None and args.pull_request <= 0:
        parser.error("--pull-request must be positive")
    digest = (args.release_dir / "image.digest").read_text().strip()
    if not DIGEST.fullmatch(digest):
        parser.error("Release artifact must contain a valid SHA-256 image digest")
    branch = f"preview/pr-{args.pull_request}" if args.pull_request else BASE_BRANCH
    publish(args.repository, args.service, args.version, args.source_commit, branch, digest)


if __name__ == "__main__":
    main()
