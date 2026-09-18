# Sprint tasks: automated infrastructure and GitOps deployment

## Objective

Deliver one Azure DevOps pipeline run that provisions Azure AKS, bootstraps the FluxCD operator and its repository configuration, waits for platform and application deployments, and creates Cloudflare DNS records through Terraform. Flux manages platform Helm releases and microservice releases. Application images are hosted on Docker Hub; application Helm charts are published to GitHub Container Registry (GHCR) as OCI artifacts.

Source: [objective.txt](objective.txt). This document is an implementation backlog; checkboxes describe work to perform, not completed changes.

## Scope and implementation decisions

- Target `dev` first and keep environment configuration separate from reusable modules so additional environments can be added.
- Keep Azure Terraform focused on AKS, its supporting resource group/configuration, and the explicitly requested ACR pull permission. Remove Azure observability modules and their inputs, outputs, and dependencies.
- Retain the requested ACR pull permission during the Docker Hub migration. Record its remaining purpose and whether it becomes optional; removing it entirely requires resolving the conflicting requirements in objectives 2.1 and 4.
- Use `deprecated/` consistently; the `depracated` spelling in the objective is treated as a typo.
- Bootstrap prerequisites such as cloud accounts, Terraform state storage, service connections, and credentials must be documented. Once established, normal deployment requires one pipeline invocation without a separate platform run.
- Keep maintained documentation in the root README, component READMEs, and `sprint-planning/`; remove `docs/` after migrating information that is still needed.
- Application chart publication and image publication are release prerequisites. The deployment pipeline must select and verify exact existing versions, or invoke and await their builds within the same run; it must never depend on manually triggering a second deployment pipeline.

## Proposed repository layout

```text
terraform/
  infrastructure/
    modules/
      aks/
      acr-access/
    environments/
      dev/
    README.md
  cloudflare/
    environments/
      dev/
    README.md
  azure-devops/
    README.md
charts/
  authorization/
  sender/
  consumer/
azure-pipelines/
  infrastructure/
  templates/
.github/workflows/
  helm-validation.yml
  helm-publication.yml
deprecated/
  platform/
  kubernetes/
  azure-pipelines/
sprint-planning/
  objective.txt
  tasks.md
  deployment-design.md
  developer-access.md
```

Flux environment manifests and platform Helm releases belong in the selected Flux repository. Chart names and workflow filenames above are proposed implementation targets.

## Detailed backlog

### TASK-01 — Define deployment contracts and migration inventory

**Objective coverage:** Main objective, 14. **Dependencies:** None.

- [ ] Inventory active pipeline entry points, template consumers, Terraform roots/state keys, container build scripts, Kubernetes manifests, secret charts, and platform installers.
- [ ] Map the authorization, sender, and consumer applications to image names, ports, environment variables, probes, service names, ingress routes, and dependencies.
- [ ] Inventory PostgreSQL, RabbitMQ, Redis, cert-manager, ingress, and existing persistent data; define migration and rollback ownership.
- [ ] Specify the Flux repository URL, branch/ref, environment path, namespace conventions, reconciliation interval, and authentication method.
- [ ] Define pipeline inputs: environment, Azure subscription/service connection, cluster settings, Flux source/ref/path, application release versions, DNS zone/hostnames, and timeouts.
- [ ] Record the ACR permission decision, secret delivery mechanism, TLS issuance method, and whether deployment consumes published artifacts or builds them in-run.
- [ ] Write `sprint-planning/deployment-design.md` with stage inputs/outputs and ownership boundaries between Terraform, pipeline bootstrap, and Flux.

**Acceptance criteria:** Every existing deployment responsibility has a destination or explicit retirement decision. Deployment inputs and prerequisites are documented, including all unresolved choices before dependent implementation starts.

### TASK-02 — Restructure Azure Terraform and remove observability

**Objective coverage:** 2, 2.1. **Dependencies:** TASK-01.

- [ ] Move the existing Azure Terraform into `terraform/infrastructure/`, separating reusable modules from `environments/dev/` root configuration.
- [ ] Preserve AKS and ACR pull-permission modules and required supporting resources. Parameterize the currently hard-coded ACR resource group.
- [ ] Remove `log_analytics`, `prometheus`, and `grafana` modules and all related locals, variables, outputs, flags, AKS settings, and dependencies.
- [ ] Separate environment values from module defaults; declare provider/version constraints and commit appropriate provider lock files.
- [ ] Export the resource group, cluster name, and other non-secret values required by later pipeline stages.
- [ ] Document backend initialization, state locking, state-key ownership, and path migration. Preserve existing state associations; use explicit state migration only where addresses/backends change.
- [ ] Update infrastructure create/destroy and plan/apply templates to use the new paths.
- [ ] Review the migration plan for unintended AKS replacement and explicitly identify observability resources scheduled for removal.

**Acceptance criteria:** Formatting and validation pass. A reviewed plan contains only intended changes, retains AKS and requested registry access, and has no remaining observability module dependencies. A second apply is idempotent.

### TASK-03 — Replace Cloudflare PowerShell with Terraform

**Objective coverage:** 1, 14. **Dependencies:** TASK-01; integration depends on TASK-09 and TASK-12.

- [ ] Inventory record behavior in `cloudflare/*.ps1`, including hostnames, record types, proxy settings, and TTL.
- [ ] Create an independent Terraform root under `terraform/cloudflare/environments/dev/` using the Cloudflare provider with declared version constraints.
- [ ] Define inputs for zone identification, desired records, ingress address, TTL, and proxy behavior; supply credentials through secret environment variables.
- [ ] Give Cloudflare a separate backend state key from Azure infrastructure and Azure DevOps configuration.
- [ ] Accept the ingress IP or hostname emitted by the readiness stage, validate it, and choose the corresponding DNS record type.
- [ ] Document/import existing records before managing them to prevent duplicate record creation or unintended removal.
- [ ] Replace all active PowerShell DNS invocations, then remove the obsolete `cloudflare/` scripts.

**Acceptance criteria:** Terraform creates/updates the required records from pipeline outputs; a repeated apply produces no unintended changes. No active workflow calls the removed scripts, and invalid or missing ingress addresses stop the DNS stage.

### TASK-04 — Move and simplify Azure DevOps Terraform

**Objective coverage:** 6. **Dependencies:** TASK-01, TASK-02.

- [ ] Move `terraform-azdo-libraries/` into `terraform/azure-devops/` and update references and backend instructions.
- [ ] Start with the Terraform backend configuration variable group as the minimal retained library.
- [ ] Audit platform-era libraries for Cloudflare, database, Redis, AKS, prefix, Entra ID, Azure credentials, and tfvars transformation against the replacement design.
- [ ] Remove unused values/groups only after their active consumers are removed or migrated. Keep or recreate values required by the unified pipeline in their documented destination.
- [ ] Preserve Terraform resource/state associations where possible and review planned variable-group deletions.
- [ ] Document the initial provisioning order so creating Azure DevOps libraries does not depend on the pipeline that consumes them.

**Acceptance criteria:** The new root validates, the backend library is available, and all active variable references resolve. Retired platform settings are absent from the maintained configuration.

### TASK-05 — Publish application images to Docker Hub

**Objective coverage:** 4. **Dependencies:** TASK-01.

- [ ] Define repositories under `docker.io/petrokolosov` for authorization, sender, and consumer, and document their visibility.
- [ ] Update `azure-pipelines/build/`, `azure-pipelines/templates/docker-build-push-jobs.yml`, and applicable scripts to authenticate, build, and push to Docker Hub.
- [ ] Correct the existing template example that uses `petrkolosov` instead of the requested `petrokolosov` namespace.
- [ ] Adopt reproducible version tags and record image digests for deployment and rollback; avoid relying on `latest`.
- [ ] Update maintained image references and remove active dependence on ACR login scripts/service connections for application publication.
- [ ] Configure cluster pull credentials if repositories require them; document pull limits and authentication requirements during implementation.
- [ ] Remove broad environment dumps from image-build jobs where newly supplied credentials could be exposed.

**Acceptance criteria:** All three application images can be published and pulled by AKS using documented credentials and exact references. Existing build/test steps still run successfully.

### TASK-06 — Implement application Helm charts

**Objective coverage:** 9. **Dependencies:** TASK-01, TASK-05 for final image defaults.

- [ ] Create charts under `charts/authorization`, `charts/sender`, and `charts/consumer`, with metadata, defaults, templates, and README examples.
- [ ] Translate required behavior from `kubernetes/` into Deployments, Services, configurable ingress, service accounts where needed, and optional autoscaling.
- [ ] Parameterize replicas, image repository/tag/digest, ports, resources, probes, scheduling, ingress hostnames, and pull-secret references.
- [ ] Define application configuration and external Secret references for database, RabbitMQ, Redis, and Entra ID dependencies as applicable.
- [ ] Replace the operational role of `helm/auth-secrets`, `helm/sender-secrets`, and `helm/consumer-secrets`; document their retirement or migration so two systems do not own the same Secret.
- [ ] Ensure release/namespace naming supports multiple environments; add values validation for required configuration where useful.
- [ ] Provide dev example values without credentials and document upgrade, rollback, and dependency expectations.

**Acceptance criteria:** Each chart lints and renders valid manifests for default/example values and the intended dev configuration. All three workloads start with the configured dependencies and no plaintext credentials are committed.

### TASK-07 — Add GitHub Actions chart validation

**Objective coverage:** 11. **Dependencies:** TASK-06.

- [ ] Add a pull-request/push workflow covering chart changes and workflow changes.
- [ ] Run Helm lint and template rendering for each affected chart and relevant example/dev values.
- [ ] Validate rendered Kubernetes resources against the target Kubernetes schema with an explicitly selected validation tool.
- [ ] Validate any required custom-resource schemas explicitly; document exceptions rather than silently skipping all unknown kinds.
- [ ] Check chart dependencies and report actionable failures without requiring deployment credentials.
- [ ] Document and configure the validation check required before chart release/merge, where repository permissions permit.

**Acceptance criteria:** A valid chart change passes; a deliberate invalid value/template/schema fixture fails the appropriate check. Pull requests from untrusted branches cannot publish artifacts.

### TASK-08 — Publish Helm charts to GHCR OCI

**Objective coverage:** 10. **Dependencies:** TASK-06, TASK-07.

- [ ] Define a release trigger and chart-version policy, including behavior when multiple charts change.
- [ ] Implement a GitHub Actions workflow that validates, packages, authenticates to GHCR, and publishes versioned OCI charts.
- [ ] Grant only required workflow permissions and restrict publication to the intended trusted release context.
- [ ] Define the package namespace and make intended packages publicly readable, verifying the objective's free-publication requirement against account/package settings.
- [ ] Prevent unintended replacement of an existing chart version; record artifact version/digest and source revision.
- [ ] Document OCI pull/install examples and the matching Flux source configuration.

**Acceptance criteria:** A versioned release publishes all selected charts, and an independent client can pull the intended public artifacts without publication credentials. Validation failure prevents publication.

### TASK-09 — Move platform installation to Flux and adopt Traefik

**Objective coverage:** 3, 13. **Dependencies:** TASK-01.

- [ ] Inspect the referenced Flux repository and agree the target environment layout before making repository changes.
- [ ] Represent required platform components as Flux-managed Helm releases and supporting manifests: PostgreSQL, RabbitMQ, Redis, cert-manager where needed, and Traefik.
- [ ] Replace active NGINX ingress installation/configuration with the Traefik Helm chart, pinned chart versions, ingress class, service exposure, and environment values.
- [ ] Preserve host/path routing requirements and define TLS behavior, redirects, and certificate references. Do not expose the Traefik dashboard unintentionally.
- [ ] Define reconciliation dependencies for namespaces, CRDs/controllers, secrets, backing services, and application releases.
- [ ] Define persistent storage and data migration for stateful platform services; prevent resource ownership conflicts during migration.
- [ ] Keep Azure observability modules removed; inventory any platform monitoring releases and document whether they are retained as Flux-managed components or retired.
- [ ] Remove the standalone configure-platform entry point and its exclusive template once replacement deployment is verified.

**Acceptance criteria:** Flux reconciles the required platform releases, Traefik receives a usable external address, and ingress routes can serve the applications. No ongoing platform installation depends on a manually executed PowerShell or Helm command.

### TASK-10 — Define Flux application releases and secret delivery

**Objective coverage:** Main objective, 3, 9. **Dependencies:** TASK-05, TASK-06, TASK-08, TASK-09.

- [ ] Add Flux OCI chart sources and application Helm releases in the chosen Flux repository/environment path.
- [ ] Pin chart versions and application image versions/digests, with explicit environment values and namespaces.
- [ ] Implement the secret delivery mechanism selected in TASK-01, including initial bootstrap and rotation requirements.
- [ ] Ensure required secrets/configuration and backing services are available before dependent application releases reconcile.
- [ ] Define how a deployment selects a Git revision and how artifact changes reach that revision; include repository write access only if automation actually needs to update Git.
- [ ] Document application rollback through a Git/version change and reconcile it in the dev environment.

**Acceptance criteria:** Flux deploys all three applications from published OCI charts and Docker Hub images. A Git-controlled version update and rollback both reconcile successfully, without secrets in Git or chart artifacts.

### TASK-11 — Automate Flux operator bootstrap

**Objective coverage:** 14. **Dependencies:** TASK-02, TASK-04, TASK-09, TASK-10.

- [ ] Inspect the referenced Ansible bootstrap repository and choose a pinned revision and invocation contract.
- [ ] Prepare the pipeline agent with the required tools and authenticated access to AKS.
- [ ] Install/configure the Flux operator and its managed Flux instance using the selected approach; distinguish operator readiness from Flux controller readiness.
- [ ] Configure the selected Git source, ref, path, and credentials, plus the root reconciliation resource that applies repository contents.
- [ ] Wait for the operator, controllers, Git source, and root reconciliation to become ready with bounded retries and useful diagnostics.
- [ ] Make repeated bootstrap runs safe and document controller/operator upgrade ownership.

**Acceptance criteria:** On a newly provisioned dev cluster, automated bootstrap starts reconciliation from the intended repository revision/path. Re-running it does not create duplicate installations or require manual commands.

### TASK-12 — Implement the single-run Azure DevOps deployment pipeline

**Objective coverage:** Main objective, 14. **Dependencies:** TASK-02 through TASK-11.

- [ ] Create one deployment entry point and reusable stage templates using the agreed parameters and service connections.
- [ ] Implement the sequence below, with stage outputs passed explicitly rather than hard-coded resource names.
  1. Validate configuration, access, tooling, and selected application/chart artifacts; build/publish them here if that is the chosen release contract.
  2. Initialize, plan, and apply `terraform/infrastructure/environments/dev`.
  3. Read AKS outputs, authenticate, and bootstrap the Flux operator/instance and repository reconciliation.
  4. Wait for required Flux releases, application rollouts, and the Traefik external address.
  5. Initialize, plan, and apply `terraform/cloudflare/environments/dev` using that address.
  6. Verify DNS resolution, certificate readiness where applicable, and external application smoke tests.
- [ ] Separate pre-DNS workload readiness from post-DNS TLS/external checks. If certificate issuance or probes require DNS, design the dependency order to avoid a readiness deadlock.
- [ ] Configure bounded timeouts, failure propagation, protected diagnostic artifacts, and environment deployment concurrency so overlapping runs cannot race state or release selection.
- [ ] On reconciliation failures, collect relevant Flux status, pod events, and rollout status without exposing secrets; prevent the DNS stage from running after failed deployment readiness.
- [ ] Define safe re-run behavior after partial failure and rollback through pinned artifact/Git versions. Do not automatically destroy persistent infrastructure on deployment failure.
- [ ] Update active create/destroy and build documentation to identify the canonical deployment entry point and the separately controlled teardown procedure.

**Acceptance criteria:** One run deploys a fresh dev environment from documented bootstrap prerequisites through working DNS and applications. A repeated run is safe. Failed rollout or missing ingress address blocks DNS mutation and produces actionable diagnostics.

### TASK-13 — Deprecate legacy assets and remove obsolete documentation

**Objective coverage:** 3, 7, 8, 12. **Dependencies:** TASK-09, TASK-12, TASK-14 documentation replacements.

- [ ] Move `platform/` to `deprecated/platform/` after its responsibilities are handled by Flux/bootstrap.
- [ ] Move `kubernetes/` to `deprecated/kubernetes/` after application charts and Flux manifests cover active deployments.
- [ ] Move `azure-pipelines/deprecated/` to `deprecated/azure-pipelines/`.
- [ ] Add an archive README explaining that these files are historical and identifying their replacements.
- [ ] Migrate necessary setup/runbook content from `docs/` to maintained READMEs or sprint planning documents, then remove `docs/`.
- [ ] Resolve the legacy `helm/` secret charts and helper scripts according to TASK-06; remove all active use of superseded assets.
- [ ] Update README links, pipeline working directories, path filters, and scripts; exclude deprecated files from active chart/deployment discovery.

**Acceptance criteria:** The requested legacy folders are archived in the correct locations, `docs/` is absent, and maintained pipelines/documentation do not rely on removed or deprecated operational paths.

### TASK-14 — Document developer access and the deployment runbook

**Objective coverage:** 5, 14. **Dependencies:** Start with TASK-01; finalize after TASK-12.

- [ ] Create `sprint-planning/developer-access.md` with every required account, credential, service connection, and permission. Record purpose, scope, owner, storage location, provisioning steps, rotation/revocation, and consuming workflow; never record actual secret values.
- [ ] Cover Azure subscription/resource group access, resource provisioning permissions, role-assignment permissions for ACR access, AKS authentication/authorization, and Terraform backend data access.
- [ ] Cover Azure DevOps project/repository access, pipeline/service-connection use, variable-group administration, and the authentication method needed by the Azure DevOps Terraform provider.
- [ ] Cover Cloudflare DNS-edit access limited to the relevant zone and any read access needed for zone discovery.
- [ ] Cover Docker Hub push credentials and conditional private-image pull credentials; separate publisher and runtime identities where applicable.
- [ ] Cover GitHub source access, GitHub Actions/GHCR publication permissions, Flux repository read credentials, optional automation write credentials, and conditional private-chart pull credentials.
- [ ] Cover application credentials for PostgreSQL, RabbitMQ, Redis, and Entra ID as used by the applications, plus credentials/keys required by the selected secret-management and TLS mechanisms.
- [ ] Distinguish required, conditional, and non-secret configuration values; identify where workload identity or public read access avoids an additional token.
- [ ] Finalize `sprint-planning/deployment-design.md` with bootstrap, deployment, readiness gates, DNS/TLS ordering, rollback, troubleshooting, and teardown/data-retention instructions.
- [ ] Link both documents from the root README and add component README links for Terraform, charts, and workflows.

**Acceptance criteria:** A developer with the documented access can provision prerequisites and run the deployment without discovering undocumented credentials or manual platform steps. Every secret-consuming job has a documented credential source.

### TASK-15 — Validate the complete migration

**Objective coverage:** All objectives. **Dependencies:** TASK-12, TASK-13, TASK-14.

- [ ] Execute Terraform formatting/validation and review plans for all three roots with their appropriate initialization configuration.
- [ ] Run chart lint/template/schema checks, publish a test release, and confirm chart/image pulls using the same access model as Flux and AKS.
- [ ] Run the unified pipeline against a fresh dev environment after documented account/backend bootstrapping.
- [ ] Verify platform and application health, Traefik routing, DNS targets, TLS where configured, and representative authorized application flows.
- [ ] Run it again to verify idempotency and run an application upgrade/rollback to verify GitOps ownership.
- [ ] Exercise invalid Flux repository credentials, an unavailable image/chart version, readiness timeout, and rejected Cloudflare credentials; confirm bounded failures and recovery instructions.
- [ ] Review moved-state plans for unintended replacement and confirm persistent data remains intact during migration.
- [ ] Search maintained files for stale paths, ACR application-image references, NGINX configuration, removed observability inputs, and retired platform library names.
- [ ] Record run links, selected revisions/artifact versions, validation results, and any accepted limitations in the deployment design document.

**Acceptance criteria:** The main objective is demonstrated with reproducible evidence; all required migration paths and failure gates have been checked, and there are no undocumented manual steps between pipeline start and application availability.

## Suggested execution order

1. **Design and foundation:** TASK-01, then TASK-02, TASK-04, and the initial access inventory in TASK-14.
2. **Artifacts and platform:** TASK-03, TASK-05, TASK-06, and TASK-09 can progress independently after their design inputs are agreed.
3. **Chart automation and GitOps:** TASK-07 → TASK-08 → TASK-10 → TASK-11.
4. **Integration:** TASK-12, followed by final documentation in TASK-14.
5. **Cleanup and acceptance:** TASK-13 → TASK-15.

## Objective traceability

| Source objective | Implementation tasks |
| --- | --- |
| Main objective: single pipeline deployment | 01, 10, 11, 12, 15 |
| 1: Cloudflare Terraform | 03 |
| 2 and 2.1: infrastructure structure and cleanup | 02 |
| 3: remove platform pipeline; Flux-managed Helm; archive platform | 09, 10, 13 |
| 4: Docker Hub images | 05 |
| 5: developer tokens and access | 14 |
| 6: Azure DevOps Terraform relocation and cleanup | 04 |
| 7: archive Kubernetes folder | 13 |
| 8: remove docs folder | 13 |
| 9: application Helm charts | 06 |
| 10: OCI chart publication | 08 |
| 11: Helm CI validation | 07 |
| 12: relocate deprecated pipelines | 13 |
| 13: Traefik through Helm | 09 |
| 14: end-to-end pipeline design and implementation | 01, 03, 11, 12, 14, 15 |

## References supplied in the objective

- [Flux repository example](https://github.com/kolosovpetro/fluxcd-repository)
- [Flux operator Ansible example](https://github.com/kolosovpetro/fluxcd-operator-install-ansible)
- [Target Docker Hub namespace](https://hub.docker.com/repositories/petrokolosov)

The external example repositories could not be retrieved during planning. Their exact structure, variables, and bootstrap interfaces must be verified in TASK-01, TASK-09, and TASK-11 before implementation; this backlog does not assume their contents.
