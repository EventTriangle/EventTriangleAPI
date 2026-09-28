# Azure DevOps service-connection inventory

Inventory date: 2026-09-25.

Command used:

```bash
bash scripts/list-service-connections.sh \
  29327428-805a-440b-9d16-fcf0ac20edb2 \
  https://dev.azure.com/EventTriangle
```

The inventory intentionally excludes credentials and authorization parameters.

| Name | ID | Type | Authentication scheme | Shared | Ready | Terraform resource |
| --- | --- | --- | --- | --- | --- | --- |
| `EventTriangle` | `00d968fc-300a-44de-b20a-026b3555785b` | `GitHub` | `InstallationToken` | No | Yes | `azuredevops_serviceendpoint_github` |
| `Docker_Hub_Connection` | `4310e77d-d641-4f84-9751-999b5b55b8d4` | `dockerregistry` | `UsernamePassword` | No | Yes | `azuredevops_serviceendpoint_dockerregistry` |
| `Azure_ACR_Connection` | `fbc5ab6c-e38a-4c08-9b7f-ae4220ee733a` | `dockerregistry` | `UsernamePassword` | No | Yes | `azuredevops_serviceendpoint_dockerregistry` |
| `SonarCloud_Service_Connection` | `993a42ad-5193-4df4-9e9c-b29332e94e4f` | `sonarcloud` | `Token` | No | Yes | `azuredevops_serviceendpoint_sonarcloud` |
| `Azure_Connection` | `3b7f602e-7ba3-4476-a138-d54923b9da29` | `azurerm` | `ServicePrincipal` | No | Yes | `azuredevops_serviceendpoint_azurerm` |

All five discovered types have dedicated resources in Azure DevOps provider
version 1.16.0. No unsupported connection type was found.

The list API initially reported `https://hub.docker.com/` as the URL of
`Azure_ACR_Connection`. The provider read performed during import returned the
actual registry URL `https://acrsharedd01.azurecr.io`; the Terraform resource
uses the provider-read value. Recheck this discrepancy in the full plan before
any apply.

## Consumer map

### `EventTriangle`

The GitHub connection is the repository connection for every build definition
managed by `azuredevops_build_definition.pipeline` in `pipelines.tf`:

- PR Validation Auth, Sender, and Consumer;
- Build Auth, Sender, and Consumer;
- Terraform Create and Destroy;
- Cloudflare DNS.

The imported Terraform resource now supplies the connection ID to the pipeline
definitions. The existing per-pipeline endpoint authorizations are managed by
`azuredevops_pipeline_authorization.github_repository`.

### `Azure_ACR_Connection`

The connection is passed as `dockerServiceConnection` to the shared
`.azdo/templates/docker-build-push-jobs.yml` template by:

- `.azdo/build/build-auth.yml`;
- `.azdo/build/build-sender.yml`;
- `.azdo/build/build-consumer.yml`;
- `.azdo/pr-validation/pr-validation-auth.yml`;
- `.azdo/pr-validation/pr-validation-sender.yml`;
- `.azdo/pr-validation/pr-validation-consumer.yml`.

### `SonarCloud_Service_Connection`

The active consumer is `.azdo/build/build-auth.yml`, which enables SonarCloud
and passes the connection name to
`.azdo/templates/build-test-scan-jobs.yml`. Sender and Consumer builds disable
SonarCloud, and the PR-validation entry points do not use this connection.

### `Azure_Connection`

No current YAML entry point references this connection. Terraform Create and
Terraform Destroy still receive `ARM_*` credentials from the
`Terraform_Azure_Credentials` variable group. TASK-08 can retain and import
this connection before deciding whether those two pipelines should be migrated
to service-connection authentication. Cloudflare DNS does not consume Azure
credentials.

### `Docker_Hub_Connection`

No current YAML entry point references this connection. It is retained for the
Docker Hub publication work explicitly assigned to TASK-09; TASK-08 should
import it without wiring it into the current ACR pipelines.

## Import boundary

This document does not export or recover endpoint secrets. Import reads and
associates the existing objects without updating them, but a subsequent plan
and apply require the real credential inputs from their owners. Confirm that
each resource preserves its authentication scheme and endpoint ID before
applying any changes.

## Protected inputs

Supply credentials only as environment variables or through an equivalent
protected CI secret source. Do not commit them to `.tfvars` files:

```bash
export TF_VAR_docker_hub_token='<Docker Hub token>'
export TF_VAR_azure_acr_password='<ACR password>'
export TF_VAR_sonarcloud_token='<SonarCloud token>'
export TF_VAR_azure_client_secret='<Azure service-principal secret>'
```

The existing backend, Cloudflare, and Azure DevOps provider inputs are still
required by this Terraform root. Azure DevOps does not return stored endpoint
passwords or tokens through the inventory API, so their current values must be
obtained from the credential owners before a full plan or apply.

All five connections were imported into the existing remote state using the
`PROJECT_ID/SERVICE_ENDPOINT_ID` format declared in `imports.tf`. Import only
associated the existing objects with Terraform; it did not update their remote
configuration.

Terraform declares narrowly scoped endpoint permissions:

- `EventTriangle`: every managed pipeline, because every definition reads the
  GitHub repository through it;
- `Azure_ACR_Connection`: the three Build and three PR Validation pipelines;
- `SonarCloud_Service_Connection`: Build Auth only.

`Docker_Hub_Connection` remains unauthorized until TASK-09 adds a consumer.
`Azure_Connection` remains unauthorized while Terraform Create and Destroy use
the `Terraform_Azure_Credentials` variable group rather than an Azure task that
accepts a service connection.
