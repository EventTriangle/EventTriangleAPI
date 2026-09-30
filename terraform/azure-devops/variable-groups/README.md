# Azure DevOps Terraform

This Terraform root manages the retained variable groups, maintained YAML
pipeline definitions, the `dev` deployment environment, and pipeline-specific
resource authorizations. Service-connection resources remain in TASK-08.

The root remains in `terraform/azure-devops/variable-groups/` to preserve the
working TASK-06 layout. Its existing variable-group resource addresses and
remote backend are unchanged:

- storage account: `tfstatestorage011`
- container: `tfstatecontainer01`
- key: `event.triangle.azdo.libraries.tfstate`

Always initialize this exact state before planning. See
`PIPELINE_INVENTORY.md` before the first pipeline apply because eight existing
definitions must be imported rather than recreated.

## Bootstrap order

1. Create or identify the Azure Storage backend outside this root.
2. Authenticate the Azure DevOps provider and initialize the existing state.
3. Plan the declarative imports from `imports.tf` and confirm the eight existing
   definitions are imported and updated in place.
4. Confirm that no still-required legacy variable groups or secrets are being
   destroyed.
5. Apply the retained variable groups, pipeline definitions, `dev` environment,
   and their permissions.
6. Run the infrastructure and Cloudflare pipelines only after reviewing their
   own Terraform plans.

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

Supply variable-group secrets through protected environment variables:

```bash
export TF_VAR_backend_sas_token="<backend SAS token>"
export TF_VAR_azure_client_secret="<Azure service-principal secret>"
export TF_VAR_cloudflare_api_key="<Cloudflare API key>"
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

## External service connections

The existing GitHub connection is required to create or import the YAML
definitions. ACR/Docker Hub and SonarCloud connections remain external until
TASK-08. Until then, authorize those external connections manually only for the
pipelines that consume them.

## Migration notes

- `library-state-file` points directly to `azure.tfstate`; it no longer depends
  on `Prefix_Library` expansion.
- The Cloudflare zone identifier is the `cloudflare_zone_id` default in the
  Cloudflare Terraform environment.
- Azure service-principal credentials remain until TASK-08 migrates the active
  infrastructure pipelines to a maintained service connection.
- PostgreSQL, Redis, and Entra ID secrets must be migrated to their selected
  GitOps secret mechanism before their legacy variable groups are deleted.
