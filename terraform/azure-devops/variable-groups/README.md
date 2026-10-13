# Azure DevOps Terraform

This Terraform root manages the retained variable groups, maintained YAML
pipeline definitions, the `dev` deployment environment, retained service
connections, and pipeline-specific resource authorizations.

The root remains in `terraform/azure-devops/variable-groups/` to preserve the
working TASK-06 layout. Its existing variable-group resource addresses and
remote backend are unchanged:

- storage account: `tfstatestorage011`
- container: `tfstatecontainer01`
- key: `event.triangle.azdo.libraries.tfstate`

Always initialize this exact state before planning. See
`PIPELINE_INVENTORY.md` for the Terraform-managed definitions and
`SERVICE_CONNECTION_INVENTORY.md` for the existing connections imported by
TASK-08.

## Bootstrap order

1. Create or identify the Azure Storage backend outside this root.
2. Merge the TASK-08 configuration before importing connections into the shared
   state. Applying imports from a feature branch while `main` lacks the matching
   resources would make plans from `main` propose their deletion.
3. Authenticate the Azure DevOps provider, initialize the existing state, and
   supply every protected input listed below.
4. Plan the declarative service-connection imports from `imports.tf`. Confirm
   that four connections (five with SonarCloud enabled) are imported in place with their existing IDs and
   that no replacement or deletion is proposed.
5. Confirm that no still-required variable group, pipeline, environment, or
   secret is being destroyed.
6. Apply the service connections and narrowly scoped pipeline permissions.
7. Run each consuming pipeline, then produce a second plan and require no
   unintended changes.

This root cannot create its own backend, provider PAT, or initial GitHub service
connection. The applying identity needs permission to administer build
definitions, variable groups, environments, and pipeline resource
authorizations in the Azure DevOps project.

## Authentication and initialization

Do not commit an Azure DevOps PAT or backend SAS token. Export them locally:

```bash
export AZDO_ORG_SERVICE_URL="https://dev.azure.com/EventTriangle"
export AZDO_PERSONAL_ACCESS_TOKEN="<scoped Azure DevOps PAT>"
```

Create the ignored `backend.hcl` locally with the storage account, container,
state key, and protected SAS token, then initialize:

```bash
terraform init -reconfigure -backend-config=backend.hcl
```

Supply variable-group and service-connection secrets through protected
environment variables:

```bash
export TF_VAR_backend_sas_token="<backend SAS token>"
export TF_VAR_azure_client_secret="<Azure service-principal secret>"
export TF_VAR_cloudflare_api_key="<Cloudflare API key>"
export TF_VAR_docker_hub_token="<Docker Hub access token>"
export TF_VAR_azure_acr_password="<ACR credential>"
export TF_VAR_sonarcloud_token="<SonarCloud token>"
```

Then run:

```bash
terraform fmt -check
terraform validate
terraform plan -out=azure-devops.tfplan
terraform apply azure-devops.tfplan
```

Review every plan. This root owns real Azure DevOps configuration, so removing
a managed resource can delete or deauthorize the corresponding object.

## Pipeline ownership

Terraform owns each pipeline's name, folder, GitHub repository connection,
default branch, YAML path, and queue status. The `.azdo/` entry points own their
CI and PR trigger rules; Terraform uses the YAML trigger configuration instead
of duplicating those rules.

The variable groups are not open to every pipeline. Terraform grants access
only where the active YAML consumes a group:

- backend settings: `Terraform Create`, `Terraform Destroy`, `Cloudflare DNS`;
- Azure credentials: `Terraform Create`, `Terraform Destroy`;
- Cloudflare API key: `Cloudflare DNS`.

The Cloudflare pipeline deliberately receives no Azure service-principal
credentials.

## Service-connection ownership

SonarCloud is temporarily deferred. `manage_sonarcloud` defaults to `false`, so
this root imports four connections and does not require a SonarCloud token or
modify the existing SonarCloud endpoint. When a working token is available,
set `TF_VAR_manage_sonarcloud=true` and `TF_VAR_sonarcloud_token`, then review
and apply the additional import and authorization. Keep `manage_sonarcloud`
enabled once imported: `prevent_destroy` blocks accidental endpoint deletion.

Build Auth independently exposes the boolean Run Pipeline parameter
`sonarCloudEnabled`, default `false`. The shared template excludes SonarCloud
steps when disabled, including their service-connection inputs. Enable the
parameter for a manual run on `main` after repairing the token. To enable
analysis for automatic runs, change the parameter's YAML default to `true`.
Disabling pipeline analysis does not require disabling Terraform management.

TASK-08 retains and imports the existing `EventTriangle`,
`Docker_Hub_Connection`, `Azure_ACR_Connection`,
`SonarCloud_Service_Connection`, and `Azure_Connection` objects. Import keeps
their IDs and therefore does not require pipeline YAML to change connection
names.

Terraform grants endpoint access only to current consumers:

- `EventTriangle`: all Terraform-managed pipelines;
- `Azure_ACR_Connection`: the three Build and three PR Validation pipelines;
- `SonarCloud_Service_Connection`: Build Auth only.

`Docker_Hub_Connection` remains unauthorized until TASK-09 adds its consumers.
`Azure_Connection` is not granted to any maintained pipeline while the
infrastructure YAML uses the `Terraform_Azure_Credentials` variable group; its
existing permission for legacy `Configure Platform` is retained until that
definition is retired.

Remove pre-existing broad access from Docker Hub and ACR and stale SonarCloud
access from Build Consumer and Build Sender using the one-time portal procedure
in `DEVELOPER_ACCESS.md`, after Terraform has granted access to the intended
consumers. Terraform manages these grants with the supported
`azuredevops_pipeline_authorization` resource.

`Azure_Connection` and the pipeline variable group share `azure_client_id` and
`azure_client_secret`. The target client ID is the default
`ab0a5dc1-ee52-4574-96e0-469f237928a6`; supply its matching secret. A plan may
replace the temporary test identity in the variable group with this project
identity.

The GitHub App installation token cannot be exported or recreated by this
root. Bootstrap `EventTriangle` in Azure DevOps first, then import it. Azure
DevOps also does not return stored Docker, SonarCloud, ACR, or Azure secrets;
their owners must provide current values before the import plan is applied.
See `DEVELOPER_ACCESS.md` for access and rotation procedures.

## Migration notes

- `library-state-file` points directly to `azure.tfstate`; it no longer depends
  on `Prefix_Library` expansion.
- The Cloudflare zone identifier is the `cloudflare_zone_id` default in the
  Cloudflare Terraform environment.
- Azure service-principal credentials remain in the variable group until the
  infrastructure YAML is explicitly migrated to an Azure task that consumes
  `Azure_Connection`.
- PostgreSQL, Redis, and Entra ID secrets must be migrated to their selected
  GitOps secret mechanism before their legacy variable groups are deleted.
