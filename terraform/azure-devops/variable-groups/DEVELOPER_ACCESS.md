# Azure DevOps Terraform access guide

This Terraform root manages real project-level Azure DevOps resources. Run it
only with an identity authorized to read and manage build definitions, service
connections, variable groups, environments, and pipeline permissions in the
`EventTriangle` project.

## Bootstrap access

The operator needs:

- an Azure DevOps PAT exposed as `AZDO_PERSONAL_ACCESS_TOKEN`;
- read/write access to the existing Azure Storage backend;
- current credentials from the owners of Docker Hub, ACR, SonarCloud, and the
  Azure service principal;
- access to the existing GitHub App connection in Azure DevOps.

Do not put credentials in Terraform source, committed `.tfvars`, command-line
arguments, pipeline logs, or documentation. Use protected environment
variables locally and secret variables in CI.

The GitHub App connection is a bootstrap dependency. Create or repair it in
Azure DevOps before running Terraform; its installation token cannot be
exported through the provider. Terraform imports the existing connection and
manages which pipelines may use it.

### GitHub authentication and permissions

The existing `EventTriangle` connection uses the Azure Pipelines GitHub App
(`InstallationToken`), not a personal access token. Do not add `auth_personal`
to the imported resource: that selects PAT authentication rather than preserving
the current connection.

In GitHub organization settings, configure the installed Azure Pipelines App
with `Only select repositories` and select `EventTriangle/EventTriangleAPI`.
An organization administrator must verify this installation scope. GitHub App
permissions are defined by the application; limit repository access through the
installation settings instead of creating an additional user PAT. In Azure
DevOps, authorize the connection only for the nine maintained pipelines.

Provider 1.16.0 requires `description = ""` for GitHub App connections and cannot
create or update their App authentication. The other service connections have
descriptions; the GitHub description intentionally remains empty.

References:

- [Microsoft: secure GitHub repository access](https://learn.microsoft.com/en-us/azure/devops/pipelines/security/secure-access-to-repos?view=azure-devops)
- [Provider 1.16.0: GitHub connection limitations](https://github.com/microsoft/terraform-provider-azuredevops/blob/v1.16.0/website/docs/r/serviceendpoint_github.html.markdown)

## Import and apply

Import service connections into the shared state only after the TASK-08
configuration is merged. Otherwise a plan from `main`, which does not yet
declare those state addresses, can propose deleting the imported connections.

Before applying:

1. Export all protected inputs listed in `README.md`.
2. Run `terraform plan -out=azure-devops.tfplan`.
3. Confirm four in-place imports (five when `manage_sonarcloud=true`) and no service-connection replacement or
   deletion. Supply `azure_client_secret` for the target project client
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
- Authorize Docker Hub only for Build Auth, Build Consumer, and Build Sender;
  PR Validation must not receive publisher credentials.
- Do not authorize `Azure_Connection` until infrastructure YAML consumes it.
- Never expose endpoint credentials through Terraform outputs.

## One-time permission cleanup

The initial cleanup was completed during TASK-08. When auditing permissions,
open Azure DevOps
`Project settings > Service connections`, select each connection, and open its
`Pipeline permissions` settings:

1. `Azure_ACR_Connection`: disable access for all pipelines. After TASK-09 is
   merged and its Terraform permissions are applied, remove the six old grants.
2. `Docker_Hub_Connection`: disable access for all pipelines. No maintained
   PR Validation pipeline should consume this connection. Allow only the three
   Build pipelines.
3. `SonarCloud_Service_Connection`: retain Build Auth and remove Build Consumer
   and Build Sender.
4. Preserve `Azure_Connection` access for legacy Configure Platform until that
   pipeline is retired.

This cleanup is manual because the supported Terraform authorization resource
models grants rather than explicit denials. Do not re-enable broad access;
check the portal permissions when reviewing later access changes.
