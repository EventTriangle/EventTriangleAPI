import argparse
import os
import re
import subprocess
from pathlib import Path


def main():
    parser = argparse.ArgumentParser(description="Package and publish the service Helm charts.")
    parser.add_argument("--sem-ver", required=True, help="Semantic version from GitVersion")
    parser.add_argument("--release-target", required=True, help="Target branch of the GitHub release")
    args = parser.parse_args()
    semver = args.sem_ver
    if not semver:
        parser.error("GitVersion did not set SemVer")

    commit = os.environ["GITHUB_SHA"]
    short_commit = commit[:7]
    release_target = args.release_target.removeprefix("refs/heads/")
    branch = re.sub(r"[^a-z0-9]+", "-", release_target.lower()).strip("-")
    branch = (branch or "unknown")[:32]
    build_id = f"{os.environ['GITHUB_RUN_ID']}-{os.environ['GITHUB_RUN_ATTEMPT']}"
    version = f"{semver}+{short_commit}.{branch}.{build_id}"
    registry = f"oci://ghcr.io/{os.environ['GITHUB_REPOSITORY_OWNER'].lower()}/charts"

    subprocess.run(
        ["helm", "registry", "login", "ghcr.io", "-u", os.environ["GITHUB_ACTOR"], "--password-stdin"],
        input=os.environ["GH_TOKEN"],
        text=True,
        check=True,
    )

    with Path(os.environ["GITHUB_STEP_SUMMARY"]).open("a", encoding="utf-8") as summary:
        summary.write(f"Source commit: {commit}\nChart version: {version}\n\n")

        for chart in ("authorization", "sender", "consumer"):
            chart_dir = Path("charts") / chart
            chart_file = chart_dir / "Chart.yaml"
            metadata = chart_file.read_text(encoding="utf-8")
            metadata, count = re.subn(r"(?m)^version:.*$", f"version: {version}", metadata)
            if count != 1:
                raise SystemExit(f"Expected one version field in {chart_file}")
            chart_file.write_text(metadata, encoding="utf-8")

            package_dir = Path(os.environ["RUNNER_TEMP"]) / chart
            archive = package_dir / f"{chart}-{version}.tgz"
            subprocess.run(["helm", "lint", chart_dir], check=True)
            subprocess.run(["helm", "package", chart_dir, "--destination", package_dir], check=True)
            push = subprocess.run(
                ["helm", "push", archive, registry],
                stdout=subprocess.PIPE,
                stderr=subprocess.STDOUT,
                text=True,
            )
            print(push.stdout, end="", flush=True)
            summary.write(push.stdout)
            push.check_returncode()


if __name__ == "__main__":
    main()
