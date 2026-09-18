# Remaining sprint tasks: automated infrastructure and deployment

Source: `sprint-planning/objective.txt`.

Deliver one Azure DevOps pipeline run that provisions AKS, configures the FluxCD operator and repository reconciliation, waits for platform/application readiness, and provisions Cloudflare DNS through Terraform.

This backlog contains unfinished work identified from the current repository. It uses the existing `.azdo/` pipeline layout and `.deprecated/` archive convention. External service configuration and Flux repository contents have not been verified; integration tasks must inspect and reuse any existing implementation before adding resources.

## TASK-01 — Document required developer access and credentials

**Objective:** 5. **Dependencies:** Begin immediately; finalize against TASK-04, TASK-05, TASK-09, TASK-10, TASK-11, TASK-12, TASK-13.

- [ ] Create `sprint-planning/developer-access.md` and link it and the deployment design from the root README.
- [ ] For each access requirement, document purpose, required scope, owner, provisioning steps, secure storage location, consumers, and rotation/revocation procedure. Include no actual secret values.
- [ ] Cover Azure subscription/resource permissions, ACR role-assignment rights, AKS access, and Terraform backend data access.
- [ ] Cover Azure DevOps project/pipeline administration, repository connections, service connections, variable groups, and Terraform provider authentication.
- [ ] Cover scoped Cloudflare DNS access and any zone-discovery permission.
- [ ] Cover Docker Hub publisher access and conditional private-image pull credentials.
- [ ] Cover GitHub repository access, Actions/GHCR publishing, Flux read access, optional automation write access, and conditional private-chart pull credentials.
- [ ] Cover application-specific PostgreSQL, RabbitMQ, Redis, and Entra ID credentials and the selected secret/TLS mechanism.
- [ ] Include credentials required by retained pipeline steps, such as SonarCloud where enabled, after auditing active consumers.
- [ ] Distinguish required credentials, conditional credentials, and non-secret identifiers; document authentication choices that avoid static tokens.

**Acceptance criteria:** Every active secret consumer has a documented source, and a developer can establish prerequisites and deploy without discovering undocumented access requirements.

## TASK-02 — Restructure and simplify infrastructure Terraform

**Objective:** 2, 2.1. **Dependencies:** None.

The Azure Terraform root remains directly under `terraform/`, and observability modules are still referenced by `terraform/main.tf`.

- [ ] Move Azure configuration into `terraform/infrastructure/`, with reusable `modules/aks`, `modules/acr-access`, and an `environments/dev/` root.
- [ ] Separate environment values, backend configuration, provider constraints, and module inputs; retain appropriate dependency lock files.
- [ ] Remove Log Analytics, Prometheus, and Grafana modules and associated variables, outputs, locals, tfvars, and AKS monitoring configuration.
- [ ] Retain AKS, required supporting resources, and the requested ACR pull permission. Parameterize the hard-coded ACR resource group.
- [ ] Document why ACR pull permission remains alongside Docker Hub application images; do not silently remove the explicitly requested permission.
- [ ] Export non-secret AKS identifiers needed by subsequent pipeline stages.
- [ ] Update `.azdo/infrastructure/` and Terraform templates, including the hard-coded `terraform/terraform.auto.tfvars.json` transformation path.
- [ ] Preserve existing backend/state associations; document any required state migration and review the plan for unintended cluster replacement.

**Acceptance criteria:** Terraform formatting and validation pass. The reviewed migration plan removes only intended observability resources without unintended AKS replacement. A subsequent plan after apply has no unexpected changes.

## TASK-03 — Implement Cloudflare DNS Terraform

**Objective:** 1. **Dependencies:** TASK-09 for final ingress output integration.

No Cloudflare Terraform root exists in the current repository.

- [ ] Add `terraform/cloudflare/` with provider/version configuration, variables, outputs, and separate dev environment/backend state.
- [ ] Define zone, hostnames, record types, TTL, and proxy settings from the required application endpoints.
- [ ] Accept the Traefik external IP or hostname as a pipeline input; validate it before planning DNS changes.
- [ ] Supply the Cloudflare token through protected pipeline environment variables rather than committed configuration.
- [ ] Inspect existing DNS records and import those that Terraform should own before applying changes.
- [ ] Document standalone plan/apply instructions and how the deployment pipeline supplies ingress outputs.

**Acceptance criteria:** Terraform manages the required records without duplicates, a second apply is idempotent, and missing/invalid ingress addresses fail before DNS mutation.

## TASK-04 — Relocate and simplify Azure DevOps configuration Terraform

**Objective:** 6. **Dependencies:** TASK-02 for backend/environment conventions.

`terraform-azdo-libraries/` still contains platform-era variable groups and reads provider credentials from a local PAT file.

- [ ] Move the root into `terraform/azure-devops/`, preserving resource/state associations and updating its README.
- [ ] Keep the Terraform backend configuration library as the initial minimal library and parameterize account/container/state settings.
- [ ] Map every remaining variable-group consumer in active `.azdo/` pipelines before removing unused Cloudflare, PostgreSQL, Redis, AKS, prefix, Entra ID, and transformation settings.
- [ ] Migrate still-required configuration and credentials to their chosen destination before deleting old groups.
- [ ] Replace the provider's local token-file dependency with documented secure authentication suitable for local use and automation.
- [ ] Document bootstrap ordering for state storage, provider access, libraries, and the pipelines that consume them to avoid a circular dependency.

**Acceptance criteria:** The relocated root validates and its plan preserves intended resources. Active pipeline variables resolve, and unused platform libraries are removed from maintained Terraform.

## TASK-05 — Manage Azure DevOps pipelines through Terraform

**Objective:** 6.1. **Dependencies:** TASK-04; finalize deployment registration after TASK-13.

- [ ] Inventory existing Azure DevOps pipeline definitions and map them to the maintained `.azdo/` YAML entry points.
- [ ] Add provider-managed definitions in `terraform/azure-devops/` for the required build, PR-validation, deployment, and retained teardown pipelines.
- [ ] Configure repository connection, default branch, YAML path, naming, and trigger ownership consistently with the YAML definitions.
- [ ] Import existing definitions where appropriate instead of creating duplicates; record required repository/service-connection identifiers as inputs.
- [ ] Configure required variable-group, service-connection, and environment permissions where supported; document any external bootstrap requirements.
- [ ] Update stale PR path filters from `azure-pipelines/**` to `.azdo/**` and ensure referenced templates exist.

**Acceptance criteria:** Applying Terraform creates or adopts the intended pipeline definitions, every definition resolves its YAML path, and a second apply does not create duplicates. Each retained pipeline can be queued with its documented access.

## TASK-06 — Complete Docker Hub image migration

**Objective:** 4. **Dependencies:** None.

Application build scripts still tag images for `acrsharedd01.azurecr.io`. The three `.azdo/build/` entry points reference `docker-build-push-acr-jobs.yml`, which is no longer an active template.

- [ ] Define Docker Hub repositories under `petrokolosov` for authorization, sender, and consumer and confirm repository visibility/access.
- [ ] Update `scripts/build-auth.sh`, `scripts/build-sender.sh`, and `scripts/build-consumer.sh` to use Docker Hub for version, latest, and cache references.
- [ ] Wire all three build entry points to the maintained `.azdo/templates/docker-build-push-jobs.yml` template with matching parameter names and script paths.
- [ ] Replace ACR publication connections and active PR-validation registry references with the appropriate Docker Hub configuration.
- [ ] Correct registry examples and update maintained README image/build instructions.
- [ ] Ensure application tests gate release publication and untrusted PR validation cannot push images or access publisher credentials.
- [ ] Record immutable version tags/digests for deployment and rollback; configure runtime pull credentials if needed.
- [ ] Remove broad environment dumps from publishing jobs where credentials could be exposed.

**Acceptance criteria:** All three image builds publish to `docker.io/petrokolosov`, their pipelines resolve active templates, and AKS can pull the selected versions. Application image publication no longer depends on ACR.

## TASK-07 — Implement microservice Helm charts

**Objective:** 9. **Dependencies:** TASK-06 for final image references.

- [ ] Create `charts/authorization/`, `charts/sender/`, and `charts/consumer/`, each with chart metadata, values, templates, and usage documentation.
- [ ] Translate required application behavior into Deployments, Services, configurable ingress, and optional autoscaling, using archived manifests only as reference.
- [ ] Parameterize names/namespaces, replicas, image tags/digests, ports, requests/limits, scheduling, and startup/readiness/liveness probes.
- [ ] Map each service's database, RabbitMQ, Redis, Entra ID, and interservice configuration as applicable.
- [ ] Reference externally supplied Secrets and image-pull secrets; keep credentials out of chart defaults and rendered examples.
- [ ] Support Traefik ingress class, host/path routing, and configurable TLS references.
- [ ] Add dev example values and useful values-schema constraints; document dependencies and upgrade/rollback behavior.

**Acceptance criteria:** Each chart lints, renders valid resources, and runs its application with the required backing services. Multiple namespaces/releases do not collide, and charts contain no real credentials.

## TASK-08 — Migrate `.deprecated/platform/` to the FluxCD repository

**Objective:** Remaining platform migration from 3. **Dependencies:** None; feeds TASK-09, TASK-12, and TASK-13.

- [ ] Inventory installers, manifests, configuration, and helper scripts in `.deprecated/platform/` and map each required responsibility to its destination in the FluxCD repository.
- [ ] Inspect the target FluxCD repository and reuse existing platform resources before adding missing releases or environment values.
- [ ] Convert required PostgreSQL, RabbitMQ, Redis, and cert-manager installations into Flux-managed Helm releases with pinned versions and supporting manifests.
- [ ] Define dev namespaces, chart sources, reconciliation dependencies, persistent storage, and external secret references without copying plaintext credentials from archived files.
- [ ] Record a migration or retirement decision for remaining platform assets, including monitoring configuration and bootstrap helpers; coordinate Flux operator bootstrap with TASK-13.
- [ ] Document persistence/data migration and release ownership to avoid two controllers managing the same resources.
- [ ] Define platform readiness checks and document the mapping from archived assets to maintained FluxCD resources. Handle ingress replacement in TASK-09.

**Acceptance criteria:** Every required responsibility from `.deprecated/platform/` has a maintained FluxCD destination or an explicit handoff to TASK-09/TASK-13. Flux reconciles the migrated platform services without manual installer scripts or Helm commands, and persistent data and secret handling are accounted for.

## TASK-09 — Implement Traefik ingress through Helm

**Objective:** 13. **Dependencies:** TASK-08 for FluxCD environment conventions and certificate-controller integration; feeds TASK-03, TASK-12, and TASK-13.

- [ ] Add a pinned Traefik Helm release and chart source to the FluxCD repository with dev environment values.
- [ ] Configure its namespace, ingress class, entry points, and external LoadBalancer service.
- [ ] Replace required NGINX routing behavior with Traefik-compatible configuration, preserving application host/path routing and coordinating chart ingress values with TASK-07.
- [ ] Define TLS issuance, certificate ownership, and redirects where required, including whether DNS must exist before certificates can become ready.
- [ ] Define how the deployment pipeline discovers and validates the Traefik external IP or hostname for Cloudflare Terraform.
- [ ] Add bounded ingress readiness checks and verify application routing after application releases are available.

**Acceptance criteria:** Flux installs and reconciles Traefik through Helm, its service exposes a usable external address, and application routes work with the intended TLS configuration. The pipeline can consume its address without a manual lookup.

## TASK-10 — Add Helm chart validation in GitHub Actions

**Objective:** 11. **Dependencies:** TASK-07.

- [ ] Add a GitHub Actions workflow triggered by chart/workflow changes on PRs and relevant pushes.
- [ ] Run Helm lint, template rendering for supported values, and Kubernetes schema validation for all affected charts.
- [ ] Explicitly validate required custom resources or document narrowly scoped schema exceptions.
- [ ] Ensure PR validation requires no publication secrets and produces actionable failures.

**Acceptance criteria:** Valid charts pass lint, rendering, and schema validation; invalid charts fail CI with actionable diagnostics. PR validation does not require publication credentials.

## TASK-11 — Publish Helm charts to GHCR OCI

**Objective:** 10. **Dependencies:** TASK-07, TASK-10.

- [ ] Define chart versioning and a trusted release trigger; add a GitHub Actions workflow to validate, package, and publish charts to GHCR OCI.
- [ ] Configure minimum required workflow permissions and intended public package visibility; verify the free-publication requirement against the target account settings.
- [ ] Prevent accidental replacement of a released version and record source revision, chart version, and artifact digest.
- [ ] Document chart pull/install examples and confirm published public artifacts can be pulled without publisher credentials.

**Acceptance criteria:** A trusted release publishes versioned OCI charts only after validation passes. Public artifacts can be pulled without publisher credentials, and existing released versions are protected from accidental replacement.

## TASK-12 — Configure Flux application releases

**Objective:** Application deployment portion of the main objective. **Dependencies:** TASK-06, TASK-07, TASK-08, TASK-09, TASK-11.

- [ ] Inspect existing Flux application configuration and add only missing OCI sources, Helm releases, and dev values.
- [ ] Pin application image versions/digests and chart versions; configure dependency ordering and reconciliation timeouts.
- [ ] Implement secret delivery and bootstrap requirements for application and repository credentials without plaintext secrets in Git.
- [ ] Define how the selected release revision reaches Flux and how upgrades/rollbacks are performed through Git.

**Acceptance criteria:** Flux deploys all three applications from the intended Docker Hub images and published chart versions without plaintext secrets in Git. A Git-controlled upgrade and rollback reconcile successfully.

## TASK-13 — Implement and document one-run deployment orchestration

**Objective:** Automate infrastructure and platform so that microservices are deployed autimatically in a single pipeline run in Azure DevOps. This pipeline run should configure Azure infrastructure, Cloudflare records, FluxCD instance in AKS, Microservices deployment as fluxCD manifests., 14. **Dependencies:** TASK-02, TASK-03, TASK-04, TASK-06, TASK-08, TASK-09, TASK-11, TASK-12; register the entry point through TASK-05.

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

**Acceptance criteria:** A fresh dev environment reaches working application endpoints in one pipeline run after documented prerequisites. Re-running is safe; readiness failure prevents DNS changes and produces useful diagnostics.

## TASK-14 — Verify the integrated result

**Objective:** End-to-end acceptance of remaining work. **Dependencies:** TASK-01 through TASK-13.

- [ ] Validate all Terraform roots and review migration plans for unintended replacements or removals.
- [ ] Verify chart validation/publication and image publication with the intended runtime pull access.
- [ ] Run the unified pipeline in dev; record the run, Git revisions, artifact versions, and resulting endpoints.
- [ ] Check Flux reconciliation, application health, ingress routing, DNS, TLS, and a representative authenticated application flow.
- [ ] Re-run deployment to check idempotency and exercise an application upgrade/rollback.
- [ ] Exercise unavailable artifacts, invalid repository credentials, readiness timeout, and Cloudflare authentication failure; verify bounded failures and documented recovery.
- [ ] Check active files for broken template paths, stale `azure-pipelines/` filters, obsolete Terraform paths, and application ACR references.
- [ ] Record verification evidence and remaining limitations in the deployment design.

**Acceptance criteria:** The single-run objective is demonstrated with reproducible evidence and no undocumented manual deployment steps.

## Delivery order

1. Start with the access inventory (01), then begin Terraform restructuring (02), Azure DevOps configuration (04), Docker Hub migration (06), and platform migration (08).
2. Add Cloudflare Terraform (03), pipeline definitions (05), application charts (07), and Traefik ingress (09) as their inputs become available.
3. Complete chart validation (10), then OCI publication (11), then Flux application releases (12).
4. Integrate the deployment pipeline (13), finalize its Terraform registration (05) and access documentation (01).
5. Execute integrated acceptance checks (14).

## External implementation references

- [Flux repository example supplied in the objective](https://github.com/kolosovpetro/fluxcd-repository)
- [Ansible example URL supplied in the objective](https://github.com/kolosovpetro/.fluxcd-operator-install-ansible)
- [Target Docker Hub namespace](https://hub.docker.com/repositories/petrokolosov)

The GitHub examples could not be retrieved during this review. Their implementation status and interfaces remain unverified; inspect them before implementing the related integration work.
