# Azure DevOps variable groups Terraform

This root manages only the variable groups still consumed by active pipelines.
See `VARIABLE_GROUP_CONSUMERS.md` for the complete consumer and removal map.

The root moved from `terraform-azdo-libraries/` without adding a module layer,
so retained resource addresses are unchanged:

- `azuredevops_variable_group.terraform_backend_settings`
- `azuredevops_variable_group.terraform_azure_credentials`
- `azuredevops_variable_group.cloudflare_api_key`

It continues to use the existing remote state blob:

- storage account: `tfstatestorage011`
- container: `tfstatecontainer01`
- key: `event.triangle.azdo.libraries.tfstate`

No `terraform state mv` is required for the directory move. Always initialize
this root against that exact state key before planning, otherwise Terraform will
attempt to create duplicate variable groups.

## Bootstrap order

This Terraform root cannot create its own backend because Terraform must access
the backend before it can plan resources. The initial order is therefore:

1. Create or identify the Azure Storage account and container outside this root.
2. Obtain a backend SAS token and initialize this root against the existing
   `event.triangle.azdo.libraries.tfstate` blob.
3. Authenticate the Azure DevOps provider.
4. Plan/apply the retained variable groups.
5. Authorize and run pipelines that consume those groups.

The backend group then supplies backend settings to normal infrastructure and
Cloudflare pipeline runs. It does not bootstrap the state used to create itself.

## Local authentication

Do not create `azdo-pat-token.txt` or put a PAT in Terraform configuration.
The provider reads its documented environment variables:

```bash
export AZDO_ORG_SERVICE_URL="https://dev.azure.com/EventTriangle"
export AZDO_PERSONAL_ACCESS_TOKEN="<scoped Azure DevOps PAT>"
```

Use the narrowest PAT scopes that allow variable-group read/manage operations.
For automation, map a protected secret variable to
`AZDO_PERSONAL_ACCESS_TOKEN`. An Azure Pipeline may instead map
`$(System.AccessToken)` after enabling OAuth-token access and granting its Build
Service identity permission to manage variable groups.

## Backend initialization

Copy the example to an ignored file and add the protected SAS token:

```bash
cp backend.hcl.example backend.hcl
terraform init -backend-config=backend.hcl
```

The backend account, container, and state key published to infrastructure
pipelines are ordinary Terraform inputs and can be overridden without editing
resource files:

```bash
export TF_VAR_backend_storage_account_name="tfstatestorage011"
export TF_VAR_backend_container_name="tfstatecontainer01"
export TF_VAR_infrastructure_state_key="azure.tfstate"
```

Supply secrets only through protected environment variables:

```bash
export TF_VAR_backend_sas_token="<backend SAS token>"
export TF_VAR_azure_client_secret="<Azure service-principal secret>"
export TF_VAR_cloudflare_api_token="<Cloudflare API token>"
```

Then run:

```bash
terraform fmt -check
terraform validate
terraform plan -out=variable-groups.tfplan
terraform apply variable-groups.tfplan
```

Review every plan. Removing a variable-group resource from this root deletes the
corresponding group in Azure DevOps on apply.

## Migration notes

- `library-state-file` now points directly to `azure.tfstate`; it no longer
  depends on the removed `Prefix_Library` expansion.
- `cloudflare-zone-name` moved into the Cloudflare Terraform environment
  defaults and is no longer stored in Azure DevOps.
- Azure service-principal credentials remain temporarily because active
  infrastructure pipelines still consume them. TASK-08 will migrate those
  pipelines to a service connection before this group can be removed.
