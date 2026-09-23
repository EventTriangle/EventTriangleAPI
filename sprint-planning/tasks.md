# Remaining sprint tasks: automated infrastructure and deployment

## Main objective

Automate infrastructure and platform so that microservices are deployed autimatically in a single pipeline run in Azure DevOps. This pipeline run should configure Azure infrastructure, Cloudflare records, FluxCD instance in AKS, Microservices deployment as fluxCD manifests.

## TASK-01 — Document required developer access and credentials

**Dependencies:** None.

- [ ] Create `developer-access.md` and link it and the deployment design from the root README.
- [ ] For each access requirement, document purpose, required scope, owner, provisioning steps, secure storage location, consumers, and rotation/revocation procedure. Include no actual secret values.
- [ ] Cover Azure subscription/resource permissions, ACR role-assignment rights, AKS access, and Terraform backend data access.
- [ ] Cover Azure DevOps project/pipeline administration, repository connections, service connections, variable groups, and Terraform provider authentication.
- [ ] Cover scoped Cloudflare DNS access and any zone-discovery permission.
- [ ] Cover Docker Hub publisher access and conditional private-image pull credentials.
- [ ] Cover GitHub repository access, Actions/GHCR publishing, Flux read access, optional automation write access, and conditional private-chart pull credentials.
- [ ] Cover application-specific PostgreSQL, RabbitMQ, Redis, and Entra ID credentials and the selected secret/TLS mechanism.
- [ ] Include credentials required by retained pipeline steps, such as SonarCloud where enabled, after auditing active consumers.
- [ ] Distinguish required credentials, conditional credentials, and non-secret identifiers; document authentication choices that avoid static tokens.

## TASK-02 — Configure WSL with Ubuntu 26.04 and install development tools

**Dependencies:** None.

- [x] Enable WSL 2 on Windows, install Ubuntu 26.04 using the verified distribution identifier, and confirm the distribution runs under WSL 2.
- [x] Configure the Linux user, sudo access, package updates, Git, and repository checkout under the Linux home directory; document any required WSL networking or systemd settings.
- [x] Review the installers in `scripts/` for Ubuntu 26.04 and CPU architecture compatibility before executing them. Resolve package-source compatibility, including the Azure CLI installer's hard-coded `noble` repository and Terraform's codename-based repository.
- [x] Install prerequisites and run the existing installers for Azure CLI (`scripts/azure-cli/install-az-cli.sh`), Terraform (`scripts/install-terraform/install-terraform.sh`), Python/pip/venv/pipx (`scripts/install-python/install-python.sh`), and HELM (`scripts/install-helm/install-helm.sh`).
- [x] Run the existing installers for kubectl (`scripts/kubectl/install-kubectl.sh`), Flux CLI (`scripts/install-fluxcd-client/install-fluxcd-client.sh`), k9s (`scripts/k9s/install-k9s.sh`), jq (`scripts/jq/install-jq.sh`), and yq (`scripts/yq/install-yq.sh`).
- [x] Install the Azure DevOps CLI extension and configure authentication using the developer access guide. Treat login, image-build, and deployment scripts separately from software installers.
- [x] Configure Docker access from WSL and verify Docker Buildx is available for the repository's build scripts; document any additional setup not covered by existing installers.
- [x] Verify installed tool versions, PATH availability in a fresh shell, Docker connectivity, and safe re-running of installers; fix installer compatibility issues discovered during setup.
- [x] Document the Windows and Ubuntu setup steps, installer execution order, tested versions, and troubleshooting in `scripts/README.md`, and link it from the root README.

**Acceptance criteria:** A fresh WSL 2 Ubuntu 26.04 environment can install and run all listed tools using the documented steps. Version checks and Docker connectivity checks pass, and no undocumented installer compatibility workarounds are required.

## TASK-03 — Document one-run deployment orchestration

**Dependencies:** None.

One click on pipeline should fully deploy infrastructure and microservices.

- [ ] Write `sprint-planning/deployment-design.md` describing prerequisites, stage contracts, ownership, failure handling, and rollback.
- [ ] Add the canonical deployment entry point under `.azdo/infrastructure/` and reusable stage templates.
- [ ] Parameterize environment, Azure connection, Flux repository/ref/path, release versions, DNS configuration, and bounded timeouts.
- [ ] Define whether deployment consumes existing published artifacts or builds them in-run. Verify exact selected images/charts exist before infrastructure deployment; no second manual pipeline invocation should be necessary.
- [ ] Implement the following dependent stages:
  1. Validate prerequisites and selected artifacts.
  2. Plan/apply Azure Terraform and export AKS identifiers.
  3. Authenticate to AKS and bootstrap the Flux operator and managed Flux instance using a verified, pinned installation approach.
  4. Configure repository authentication, source/ref/path, and root reconciliation; wait for operator/controllers and source readiness.
  5. Wait for platform/application rollouts and a valid Traefik address.
  6. Plan/apply Cloudflare Terraform with that address.
  7. Check DNS, TLS where configured, and externally accessible application endpoints.
- [ ] Inspect the objective's Ansible example before choosing bootstrap parameters; verify the repository URL and pin a revision.
- [ ] Split pre-DNS readiness from post-DNS certificate/external checks so TLS issuance cannot deadlock the DNS stage.
- [ ] Pass outputs explicitly between stages; prevent DNS updates after failed deployment readiness.
- [ ] Add concurrency control, bounded waits, secret-safe diagnostics, and safe retries after partial failure.
- [ ] Document initial account/state/service-connection bootstrapping separately from the normal single-run deployment, plus controlled teardown and persistent-data handling.

## TASK-04 — Restructure and simplify infrastructure Terraform

**Dependencies:** TASK-02

- [x] Move Azure configuration into `terraform/infrastructure/`, with reusable `modules/aks`, `modules/acr-access`, and an `environments/dev/` root.
- [x] Separate environment values, backend configuration, provider constraints, and module inputs; retain appropriate dependency lock files.
- [x] Move retained configuration values from `terraform/terraform.auto.tfvars.json` into `default` attributes of the corresponding variable declarations in the new infrastructure root's `variables.tf`, then remove the redundant auto.tfvars file. Keep environment overrides explicit and supply secrets through protected inputs.
- [x] Remove Log Analytics, Prometheus, and Grafana modules and associated variables, outputs, locals, tfvars, and AKS monitoring configuration.
- [x] Retain AKS, required supporting resources, and the requested ACR pull permission. Parameterize the hard-coded ACR resource group.
- [x] Document why ACR pull permission remains alongside Docker Hub application images; do not silently remove the explicitly requested permission.
- [x] Export non-secret AKS identifiers needed by subsequent pipeline stages.
- [x] Update `.azdo/infrastructure/` and Terraform templates to use the new root and variable defaults; remove the obsolete `terraform/terraform.auto.tfvars.json` transformation step and its unused inputs.
- [x] Preserve existing backend/state associations; document any required state migration and review the plan for unintended cluster replacement.

## TASK-05 — Implement Cloudflare DNS Terraform

**Dependencies:** TASK-02

No Cloudflare Terraform root exists in the current repository.

- [x] Add `terraform/cloudflare/` with provider/version configuration, variables, outputs, and separate dev environment/backend state.
- [x] Define zone, hostnames, record types, TTL, and proxy settings from the required application endpoints.
- [x] Accept the Traefik external IP or hostname as a pipeline input; validate it before planning DNS changes.
- [x] Supply the Cloudflare token through protected pipeline environment variables rather than committed configuration.
- [x] Inspect existing DNS records and import those that Terraform should own before applying changes.
- [x] Document standalone plan/apply instructions and how the deployment pipeline supplies ingress outputs.

## TASK-06 — Cleanup Azure DevOps Variable Groups Terraform

**Dependencies:** TASK-02

- [x] Move the folder `terraform-azdo-libraries` under `terraform/azure-devops/`, preserving resource/state associations and updating its README.
- [x] Keep the Terraform backend configuration library as the initial minimal library and parameterize account/container/state settings.
- [x] Map every remaining variable-group consumer in active `.azdo/` pipelines before removing unused Cloudflare, PostgreSQL, Redis, AKS, prefix, Entra ID, and transformation settings.
- [ ] Migrate still-required configuration and credentials to their chosen destination before deleting old groups.
- [x] Replace the provider's local token-file dependency with documented secure authentication suitable for local use and automation.
- [x] Document bootstrap ordering for state storage, provider access, libraries, and the pipelines that consume them to avoid a circular dependency.

## TASK-07 — Manage Azure DevOps pipelines through Terraform

**Dependencies:** TASK-02

- [ ] Inventory existing Azure DevOps pipeline definitions and map them to the maintained `.azdo/` YAML entry points.
- [ ] Add provider-managed definitions in `terraform/azure-devops/` for the required build, PR-validation, deployment, and retained teardown pipelines.
- [ ] Configure repository connection, default branch, YAML path, naming, and trigger ownership consistently with the YAML definitions.
- [ ] Import existing definitions where appropriate instead of creating duplicates; record required repository/service-connection identifiers as inputs.
- [ ] Configure required variable-group, service-connection, and environment permissions where supported; document any external bootstrap requirements.
- [ ] Update stale PR path filters from `azure-pipelines/**` to `.azdo/**` and ensure referenced templates exist.

## TASK-08 — Migrate Azure DevOps service connections to Terraform

**Dependencies:** TASK-02

- [ ] Run `bash scripts/list-service-connections.sh PROJECT_ID [ORGANIZATION_URL]` to inventory existing connection IDs, names, types, authentication schemes, and sharing status.
- [ ] Map each retained connection to its pipeline consumers and the appropriate Azure DevOps Terraform provider resource under `terraform/azure-devops/`; document unsupported types and their handling.
- [ ] Define connection configuration using environment inputs and protected credential sources. Obtain any required credentials from their owners; the inventory does not export secrets.
- [ ] Import existing connections using their IDs and the resource-specific import format, preserving names and shared-project references to avoid breaking consumers.
- [ ] Manage required pipeline authorizations and usage permissions, coordinating ownership with TASK-07 and avoiding unrestricted access where unnecessary.
- [ ] Review plans for unexpected replacement/deletion, validate each retained connection through its consuming pipeline, and verify a second plan has no unintended changes.
- [ ] Document imports, authentication setup, credential rotation, and any bootstrap requirements in the Azure DevOps Terraform README and developer access guide.

## TASK-09 — Complete Docker Hub image migration

**Dependencies:** TASK-02

Application build scripts still tag images for `acrsharedd01.azurecr.io`. The three `.azdo/build/` entry points reference `docker-build-push-acr-jobs.yml`, extend it for dockerhub repositories too.

DockerHub url: https://hub.docker.com/repositories/petrokolosov

- [ ] Define Docker Hub repositories under `petrokolosov` for authorization, sender, and consumer and confirm repository visibility/access.
- [ ] Add dedicated `scripts/dockerhub/build-auth.sh`, `scripts/dockerhub/build-sender.sh`, and `scripts/dockerhub/build-consumer.sh` scripts using Docker Hub for version, latest, and cache references. Preserve the existing ACR build scripts.
- [ ] Reuse `scripts/docker-build.sh` from the new scripts, resolving Dockerfile and shared-context paths correctly from `scripts/dockerhub/` and accepting the application version as an argument.
- [ ] Wire all three build entry points to the maintained `.azdo/templates/docker-build-push-jobs.yml` template with matching parameter names and the new Docker Hub script paths.
- [ ] Replace ACR publication connections and active PR-validation registry references with the appropriate Docker Hub configuration.
- [ ] Correct registry examples and document Docker Hub authentication, required environment variables, and commands for running each new script in the maintained README.
- [ ] Ensure application tests gate release publication and untrusted PR validation cannot push images or access publisher credentials.
- [ ] Record immutable version tags/digests for deployment and rollback; configure runtime pull credentials if needed.
- [ ] Remove broad environment dumps from publishing jobs where credentials could be exposed.

## TASK-10 — Migrate `.deprecated/platform/` to the FluxCD repository

**Dependencies:** TASK-15

- [ ] Inventory installers, manifests, configuration, and helper scripts in `.deprecated/platform/` and map each required responsibility to its destination in the FluxCD repository.
- [ ] Inspect the target FluxCD repository and reuse existing platform resources before adding missing releases or environment values.
- [ ] Convert required PostgreSQL, RabbitMQ, Redis, and cert-manager installations into Flux-managed HELM releases with pinned versions and supporting manifests.
- [ ] Define dev namespaces, chart sources, reconciliation dependencies, persistent storage, and external secret references without copying plaintext credentials from archived files.
- [ ] Record a migration or retirement decision for remaining platform assets, including monitoring configuration and bootstrap helpers; coordinate Flux operator bootstrap with TASK-03.
- [ ] Document persistence/data migration and release ownership to avoid two controllers managing the same resources.
- [ ] Define platform readiness checks and document the mapping from archived assets to maintained FluxCD resources. Handle ingress replacement in TASK-11.

## TASK-11 — Implement Traefik ingress through HELM

**Dependencies:** TASK-10

- [ ] Add a pinned Traefik HELM release and chart source to the FluxCD repository with dev environment values.
- [ ] Configure its namespace, ingress class, entry points, and external LoadBalancer service.
- [ ] Replace required NGINX routing behavior with Traefik-compatible configuration, preserving application host/path routing and coordinating chart ingress values with TASK-12.
- [ ] Define TLS issuance, certificate ownership, and redirects where required, including whether DNS must exist before certificates can become ready.
- [ ] Define how the deployment pipeline discovers and validates the Traefik external IP or hostname for Cloudflare Terraform.
- [ ] Add bounded ingress readiness checks and verify application routing after application releases are available.

## TASK-12 — Implement microservice HELM charts

**Dependencies:** TASK-02

- [ ] Create `charts/authorization/`, `charts/sender/`, and `charts/consumer/`, each with chart metadata, values, templates, and usage documentation.
- [ ] Translate required application behavior into Deployments, Services, configurable ingress, and optional autoscaling, using archived manifests only as reference.
- [ ] Parameterize names/namespaces, replicas, image tags/digests, ports, requests/limits, scheduling, and startup/readiness/liveness probes.
- [ ] Map each service's database, RabbitMQ, Redis, Entra ID, and interservice configuration as applicable.
- [ ] Reference externally supplied Secrets and image-pull secrets; keep credentials out of chart defaults and rendered examples.
- [ ] Support Traefik ingress class, host/path routing, and configurable TLS references.
- [ ] Add dev example values and useful values-schema constraints; document dependencies and upgrade/rollback behavior.

## TASK-13 — Publish HELM charts to GHCR OCI

**Dependencies:** TASK-12

- [ ] Define chart versioning and a trusted release trigger; add a GitHub Actions workflow to validate, package, and publish charts to GHCR OCI.
- [ ] Automate semantic versioning using GitVersion setup and execute tasks in the publication workflow. Fetch full Git history and tags, use the repository's `GitVersion.yml` configuration, and apply the calculated semantic version to each packaged chart's `version` without manual version edits.
- [ ] Configure minimum required workflow permissions and intended public package visibility; verify the free-publication requirement against the target account settings.
- [ ] Prevent accidental replacement of a released version and record source revision, chart version, and artifact digest.
- [ ] Document chart pull/install examples and confirm published public artifacts can be pulled without publisher credentials.

## TASK-14 — Add HELM chart validation in GitHub Actions

**Dependencies:** TASK-12

- [ ] Add a GitHub Actions workflow triggered by chart/workflow changes on PRs and relevant pushes.
- [ ] Run HELM lint, template rendering for supported values, and Kubernetes schema validation for all affected charts.
- [ ] Explicitly validate required custom resources or document narrowly scoped schema exceptions.
- [ ] Ensure PR validation requires no publication secrets and produces actionable failures.

## TASK-15 — Create a private GitOps FluxCD manifest repository

**Dependencies:** TASK-02

See example: https://github.com/kolosovpetro/fluxcd-repository

- [ ] Establish the repository foundation first: create the private repository, configure access, define the dev environment path and namespace/secret conventions, and add a minimal root reconciliation structure usable by TASK-16.
- [ ] Inspect existing Flux application configuration and add only missing OCI sources, HELM releases, and dev values.
- [ ] Pin application image versions/digests and chart versions; configure dependency ordering and reconciliation timeouts.
- [ ] Implement secret delivery and bootstrap requirements for application and repository credentials without plaintext secrets in Git.
- [ ] Define how the selected release revision reaches Flux and how upgrades/rollbacks are performed through Git.

## TASK-16 — Create a private Ansible role repository for FluxCD operator bootstrap

**Dependencies:** TASK-02

See example: https://github.com/kolosovpetro/fluxcd-operator-install-ansible

- [ ] Create a private Git repository for the Ansible role and configure developer and Azure DevOps pipeline access through the credentials documented in TASK-01.
- [ ] Add a reusable role structure with defaults, tasks, templates, metadata, pinned dependencies, and an example playbook for configuring the target AKS cluster.
- [ ] Implement installation and configuration of the FluxCD operator and its managed Flux instance, with configurable namespace and pinned operator/controller versions.
- [ ] Parameterize Kubernetes authentication, Flux manifest repository URL, branch/tag/commit reference, environment path, reconciliation interval, and readiness timeouts.
- [ ] Configure the managed Flux instance to synchronize with the manifest repository from TASK-15, including the Git source and root reconciliation needed to apply its manifests.
- [ ] Provision repository authentication from protected inputs or existing Kubernetes Secrets. Keep credentials out of Git and Ansible logs, and document separate access for fetching the private role repository and reading the manifest repository.
- [ ] Make bootstrap idempotent and wait for operator, controllers, source synchronization, and root reconciliation readiness with bounded timeouts and useful failure diagnostics.
- [ ] Add Ansible lint/syntax checks and verify bootstrap against a test cluster, including a second run and a repository authentication failure.
- [ ] Document local and Azure DevOps invocation, required permissions, variable examples, credential rotation, and upgrade/rollback steps. Define the pipeline integration in TASK-03 using a pinned role repository revision.
