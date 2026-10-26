# Azure DevOps Terraform access guide

This Terraform root manages real project-level Azure DevOps resources. Run it
only with an identity authorized to read and manage build definitions, service
connections, variable groups, environments, and pipeline permissions in the
`EventTriangle` project.

## Bootstrap access

The operator needs:

- an Azure DevOps PAT exposed as `AZDO_PERSONAL_ACCESS_TOKEN`;
- a separate GitHub PAT exposed as `TF_VAR_github_pat`;
- read/write access to the existing Azure Storage backend;
- current credentials from the owners of Docker Hub, ACR, SonarCloud, and the
  Azure service principal;
- authority to create service connections and authorize the maintained pipelines
  in the existing Azure DevOps project.

Do not put credentials in Terraform source, committed `.tfvars`, command-line
arguments, pipeline logs, or documentation. Use protected environment
variables locally and secret variables in CI.

Terraform creates the four enabled connections from these inputs. No existing
connection ID or manually bootstrapped GitHub App connection is required. The
project, backend and credentials themselves remain external prerequisites.

### GitHub authentication and permissions

`EventTriangle` is configured with `auth_personal` and `var.github_pat`, so
Terraform can create it from scratch. This is a GitHub token, not the Azure
DevOps PAT used by the provider. The previously imported connection used the
Azure Pipelines GitHub App (`InstallationToken`); its authentication cannot be
automatically recovered or recreated by provider 1.16.0.

Use a **fine-grained GitHub PAT** restricted to resource owner `EventTriangle`
and only repository `EventTriangle/EventTriangleAPI`. The authoritative setup
and repository permission list are in
[README: Fine-grained GitHub PAT permissions](README.md#fine-grained-github-pat-permissions).
Do not use the former unrestricted classic token or translate classic scopes
into extra fine-grained permissions. The selected API-based permission set
still requires end-to-end Azure Pipelines acceptance with the new token.
Token-owner webhook access and any required organization approval are separate
prerequisites; selecting token permissions cannot elevate the owner's access.

Supply the value only as the protected `TF_VAR_github_pat` input; do not paste it
into Terraform code. `sensitive = true` hides it in normal CLI output but does
not remove it from state or saved plans. Protect backend and plan-file access.
The token must stay valid for ongoing checkout, triggers and status reporting;
it is not a one-time bootstrap token. In Azure DevOps, Terraform authorizes the
connection only for the nine maintained pipelines.

PAT authentication does not provide GitHub App Checks. Validate commit status
reporting and adjust required status-check rules with the repository owner if
they currently depend on the App's checks. Do not remove the GitHub App
installation globally: other projects may still use it.

References:

- [Microsoft: repository-scoped fine-grained PAT authentication](https://learn.microsoft.com/en-us/azure/devops/pipelines/security/secure-access-to-repos?view=azure-devops#authentication-to-github-repositories)
- [Provider 1.16.0: GitHub connection limitations](https://github.com/microsoft/terraform-provider-azuredevops/blob/v1.16.0/website/docs/r/serviceendpoint_github.html.markdown)

## Create and apply

Use this configuration consistently with the shared state. A plan from an older
branch that lacks these resources can propose their deletion. The old fixed-ID
`imports.tf` is removed: existing state entries remain managed, but a fresh
installation or a refresh after deletion can create connections with new IDs.

Before applying:

1. Export all protected inputs listed in `README.md`.
2. Run `terraform plan -out=azure-devops.tfplan`.
3. For a fresh installation, confirm four connection creations (five when
   `manage_sonarcloud=true`), plus required pipeline definitions and permissions,
   without unrelated deletion. Supply `azure_client_secret` for the target project client
   `ab0a5dc1-ee52-4574-96e0-469f237928a6`; both Azure_Connection and the pipeline
   variable group use this pair. Review the transition from the temporary test
   identity in the variable group to the project identity.
4. Apply the saved plan.
5. Complete the portal permission cleanup below, then exercise each active
   consumer: repository checkout, ACR login/push, and
   Build Auth compilation and tests. SonarCloud preparation and analysis are
   deferred while `sonarCloudEnabled=false`; test them separately once a working
   token is available and `manage_sonarcloud=true` has been applied.
6. Run a second plan and confirm that no unintended changes remain.

Docker Hub and `Azure_Connection` currently have no active pipeline consumer;
validate their credentials with the owner or provider-specific tooling without
granting broad pipeline access.

### Transition and recreation acceptance

The 2026-10-13 import/apply validated management of existing connections, not
creation of their authentication. The PAT-based creation path still needs live
acceptance with a newly supplied GitHub PAT.

1. Agree a maintenance window and stop new runs; record unmanaged consumers,
   pipeline permissions and sharing before deleting or replacing anything.
   Inventory files contain historical IDs, not desired creation inputs.
2. Supply every credential, including `TF_VAR_github_pat`, and use the existing
   backend. Do not use `terraform state rm` to force recreation of resources
   that still exist, or switch to an empty state: this root also owns existing
   pipelines and variable groups. For already deleted endpoints that fail
   refresh, use the narrowly scoped recovery procedure below.
3. To replace the currently imported GitHub App connection, explicitly request
   replacement rather than an in-place authentication update:

   ```bash
   terraform plan -replace=azuredevops_serviceendpoint_github.github -out=azure-devops.tfplan
   ```

   Review the replacement, downstream build-definition connection updates and
   permissions before applying. Keep the original connection name; do not use
   `create_before_destroy` to create a same-name duplicate. Azure DevOps may
   block deletion while dependencies exist; resolve those only within the
   approved maintenance procedure, not by force-deleting unrelated pipelines.
4. For the reviewer's delete/recreate test, explicitly approve deletion of the
   four enabled endpoints, not unrelated connections or the deferred SonarCloud
   endpoint. After deletion, a normal refreshing plan should propose creations
   and dependent ID/permission updates; inspect and apply that saved plan. No
   old-ID imports should be attempted. If Azure DevOps reports a referenced
   endpoint/dependency error, stop and resolve it before applying further.
5. Confirm GitHub checkout, PR and CI triggers and commit statuses, ACR login
   and publication, Docker Hub authentication and Azure sign-in. Check endpoint
   grants remain limited to the declared consumers. Recreated connections do
   not retain undeclared grants, including legacy `Configure Platform` access
   to `Azure_Connection`; coordinate with its owner if it is still needed.
6. Run a second plan with the same inputs and require `No changes`. SonarCloud
   creation and its consuming run are separate acceptance once a valid token
   is available and both management and analysis are enabled.

Deleting a service connection does not delete the remote registry or Azure
resources, but can break pipeline authentication. Reverting Git cannot restore
deleted connection IDs or credentials; recovery requires the inputs and apply.

### Recovery after manual endpoint deletion

Provider 1.16.0 can fail with `unexpected nil service endpoint` when Azure DevOps
returns HTTP 200 without an endpoint ID for an already deleted connection.
This is not a backend storage failure. A normal refresh cannot clear those
stale entries in this case.

Before changing state, save a protected `terraform state pull` backup outside
Git and compare its endpoint IDs with the Azure DevOps API inventory. Preserve
the credential inputs securely: the backup can contain secrets which the
service does not return. Only after confirming the four managed endpoints are
absent, remove their stale state entries and the grants referencing them:

```bash
terraform state rm -lock-timeout=30s \
  'azuredevops_serviceendpoint_github.github' \
  'azuredevops_serviceendpoint_dockerregistry.docker_hub' \
  'azuredevops_serviceendpoint_dockerregistry.azure_acr' \
  'azuredevops_serviceendpoint_azurerm.azure' \
  'azuredevops_pipeline_authorization.github_repository' \
  'azuredevops_pipeline_authorization.azure_acr'
```

This command forgets only the confirmed deleted endpoints and their grants;
it does not delete pipelines, variable groups, the deployment environment or
Azure infrastructure. Use a shorter explicit address list if only some objects
were deleted. Do not remove SonarCloud or other objects without verifying them.
Run a new normal plan against the same Azure backend, inspect creations and
dependent ID updates, then apply the reviewed plan and repeat the plan to check
convergence. Do not apply a diagnostic plan made against a local state copy.

## Credential rotation

Rotate one credential at a time:

1. Create the replacement credential at its owning service.
2. Export only the matching `TF_VAR_*` value together with the other required
   protected inputs.
3. Review the plan and require an in-place update of only the intended service
   connection.
4. Apply and run its consuming pipeline.
5. Revoke the previous credential only after the pipeline succeeds.
6. Run a final plan to detect drift.

For SonarCloud, verify that the token owner can access organization
`event-triangle` and project `event-triangle-api`. A connection may appear
`Ready` in Azure DevOps even when SonarCloud rejects its stored token.

For Azure and ACR, coordinate rotation with the subscription owner. Do not
replace a client ID with a secret issued for a different service principal.

## Least-privilege rules

- Keep `grant access permission to all pipelines` disabled.
- Authorize only the pipelines declared in `pipelines.tf`.
- Do not authorize Docker Hub before TASK-09 adds a consumer.
- Do not authorize `Azure_Connection` until infrastructure YAML consumes it.
- Never expose endpoint credentials through Terraform outputs.

## One-time permission cleanup

After Terraform has created the required individual grants, open Azure DevOps
`Project settings > Service connections`, select each connection, and open its
`Pipeline permissions` settings:

1. `Azure_ACR_Connection`: disable access for all pipelines. Keep the three Build
   and three PR Validation pipelines explicitly authorized.
2. `Docker_Hub_Connection`: disable access for all pipelines. No maintained
   pipeline consumes this connection yet.
3. `SonarCloud_Service_Connection`: retain Build Auth and remove Build Consumer
   and Build Sender.
4. Preserve `Azure_Connection` access for legacy Configure Platform until that
   pipeline is retired.

This cleanup is manual because the supported Terraform authorization resource
models grants rather than explicit denials. Do not re-enable broad access;
check the portal permissions when reviewing later access changes.
