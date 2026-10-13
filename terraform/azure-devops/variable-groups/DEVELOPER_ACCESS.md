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

## Import and apply

Import service connections into the shared state only after the TASK-08
configuration is merged. Otherwise a plan from `main`, which does not yet
declare those state addresses, can propose deleting the imported connections.

Before applying:

1. Export all protected inputs listed in `README.md`.
2. Run `terraform plan -out=azure-devops.tfplan`.
3. Confirm five in-place imports and no service-connection replacement or
   deletion. Supply `azure_client_secret` for the target project client
   `ab0a5dc1-ee52-4574-96e0-469f237928a6`; both Azure_Connection and the pipeline
   variable group use this pair. Review the transition from the temporary test
   identity in the variable group to the project identity.
4. Apply the saved plan.
5. Complete the portal permission cleanup below, then exercise each active
   consumer: repository checkout, ACR login/push, and
   Build Auth SonarCloud preparation and analysis.
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
