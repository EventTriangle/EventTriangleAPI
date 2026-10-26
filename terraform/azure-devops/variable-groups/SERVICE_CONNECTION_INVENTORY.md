# Azure DevOps service-connection inventory

Historical inventory date: 2026-09-25. IDs and authentication schemes below
describe the original objects, not fixed creation inputs. The current code
creates connections from credentials; recreated IDs will differ and GitHub
uses PAT (`Token`) authentication instead of the original GitHub App.

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

The Terraform resource supplies the created connection ID to the pipeline
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

The consumer is `.azdo/build/build-auth.yml`, which can enable SonarCloud
and pass the connection name to
`.azdo/templates/build-test-scan-jobs.yml`. Sender and Consumer builds disable
SonarCloud, and the PR-validation entry points do not use this connection.
Analysis currently defaults to disabled, and `manage_sonarcloud=false` defers
creation until a working token is available.

### `Azure_Connection`

No current YAML entry point references this connection. Terraform Create and
Terraform Destroy still receive `ARM_*` credentials from the
`Terraform_Azure_Credentials` variable group. TASK-08 creates
this connection before deciding whether those two pipelines should be migrated
to service-connection authentication. Cloudflare DNS does not consume Azure
credentials.

### `Docker_Hub_Connection`

No current YAML entry point references this connection. It is retained for the
Docker Hub publication work explicitly assigned to TASK-09; TASK-08 should
create it without wiring it into the current ACR pipelines.

## Creation boundary

This document does not export or recover endpoint secrets. Creation and later
updates require real credential inputs from their owners. Names are preserved,
but recreated IDs differ. GitHub intentionally switches from GitHub App to PAT
authentication to support Terraform creation; review the transition procedure
and required token permissions in `DEVELOPER_ACCESS.md` before applying.

## Protected inputs

Supply credentials only as environment variables or through an equivalent
protected CI secret source. Do not commit them to `.tfvars` files:

```bash
export TF_VAR_github_pat='<GitHub PAT; not the Azure DevOps PAT>'
export TF_VAR_docker_hub_token='<Docker Hub token>'
export TF_VAR_azure_acr_password='<ACR password>'
export TF_VAR_azure_client_secret='<Azure service-principal secret>'
```

Only supply `TF_VAR_sonarcloud_token` when enabling `manage_sonarcloud`.

The existing backend, Cloudflare, and Azure DevOps provider inputs are still
required by this Terraform root. Azure DevOps does not return stored endpoint
passwords or tokens through the inventory API, so their current values must be
obtained from the credential owners before a full plan or apply.

The former `imports.tf` used the `PROJECT_ID/SERVICE_ENDPOINT_ID` format and
has now been removed. GitHub, Docker Hub, ACR, and Azure were originally
imported into the shared remote state on 2026-10-13 without replacing or deleting
them. SonarCloud remains outside state while `manage_sonarcloud=false`.
Until TASK-08 is merged, apply only this branch's configuration against the
shared state: the older main configuration lacks these imported resources.

Terraform declares narrowly scoped endpoint permissions:

- `EventTriangle`: every managed pipeline, because every definition reads the
  GitHub repository through it;
- `Azure_ACR_Connection`: the three Build and three PR Validation pipelines;
- `SonarCloud_Service_Connection`: Build Auth only.

`Docker_Hub_Connection` receives no per-pipeline grant until TASK-09 adds a
consumer. `Azure_Connection` receives no grant to a maintained pipeline while
Terraform Create and Destroy use the `Terraform_Azure_Credentials` variable
group rather than an Azure task that accepts a service connection. Its legacy
`Configure Platform` permission is not managed here and will not survive
connection recreation automatically.

## Existing permission cleanup

The 2026-10-07 permission inventory found configuration broader than the
consumer map:

- `Docker_Hub_Connection` and `Azure_ACR_Connection` allow every pipeline;
- `SonarCloud_Service_Connection` allows Build Auth, Build Consumer, and Build
  Sender even though only Build Auth enables analysis;
- `Azure_Connection` remains authorized for legacy `Configure Platform` (ID
  35), which is outside the maintained pipeline set.

Terraform grants ACR only to the six image pipelines and SonarCloud only to
Build Auth. Existing all-pipeline access and stale SonarCloud grants require
the one-time portal cleanup documented in `DEVELOPER_ACCESS.md`. The legacy
Azure permission is not declared in Terraform. Recreating `Azure_Connection`
drops that grant; check whether `Configure Platform` is still needed before
recreation and coordinate any required authorization with its owner.

## Validation status

On 2026-10-26, after confirming manual deletion in Azure DevOps, stale endpoint
and associated grant entries were removed from the shared Azure state with a
protected backup. A targeted Terraform apply recreated Docker Hub, ACR and Azure
connections: three additions, no changes or deletions. GitHub PAT-based creation
and dependent pipeline permissions remain pending a least-scope token and the
token owner's webhook access. This partial recovery does not establish full
convergence or live credential/consumer acceptance for the recreated endpoints.
Historical results below validate the old import-based configuration only.

As of 2026-10-13:

- Apply completed: four imports, six additions, twelve updates, no deletions;
- a full post-apply plan reported `No changes` with the same test credentials;
- Docker Hub username `petrokolosov` and its PAT successfully authenticated;
- ACR credentials successfully authenticated against the registry;
- test Azure client `f4b7fe16-77ee-4dcc-9bf6-cfd3b8614843` successfully
  authenticated, but resource-group enumeration in the subscription returned
  `403 AuthorizationFailed`. Azure RBAC access still needs the subscription
  owner's intervention;
- Build Auth/Consumer/Sender validation runs 1663/1664/1665 on TASK-08 all
  succeeded after apply, including GitHub checkout, ACR login/image publication,
  compilation, and integration tests. No SonarCloud tasks were included in Auth.

Prior consuming-pipeline checks:

- GitHub checkout succeeds through `EventTriangle` in the maintained pipelines;
- ACR login and image publication succeed through `Azure_ACR_Connection`;
- SonarCloud preparation reaches the external service but authentication fails,
  so its token must be replaced before TASK-08 can be completed;
- Docker Hub has no active consumer until TASK-09;
- `Azure_Connection` has no maintained consumer and is still associated with
  legacy `Configure Platform` only.
