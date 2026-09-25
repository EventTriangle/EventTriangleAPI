# Azure DevOps variable-group consumers

This inventory covers active YAML entry points under `.azdo/`. Files under
`.deprecated/` are archived references and are not active consumers.

## Retained groups

### `Terraform_Backend_StateFile_Settings`

Active consumers:

- `.azdo/infrastructure/terraform-create.yml`
- `.azdo/infrastructure/terraform-destroy.yml`
- `.azdo/cloudflare/terraform-dns.yml`

Variables:

- `library-storage-account`: Azure Blob backend storage account.
- `library-storage-container`: Azure Blob backend container.
- `library-sas-token`: protected backend credential.
- `library-state-file`: `azure.tfstate`, used only by the Azure infrastructure
  create/destroy pipelines. The Cloudflare pipeline deliberately uses its own
  `cloudflare-dev.tfstate` key.

### `Terraform_Azure_Credentials`

Active consumers:

- `.azdo/infrastructure/terraform-create.yml`
- `.azdo/infrastructure/terraform-destroy.yml`

Variables:

- `library-client-id`
- `library-client-secret`
- `library-subscription-id`
- `library-tenant-id`

This group remains until TASK-08 moves Azure authentication to the maintained
service connection. Removing it before that migration would break both active
infrastructure pipelines.

### `Cloudflare_API_Key`

Active consumer:

- `.azdo/cloudflare/terraform-dns.yml`

Variable:

- `cloudflare-api-key`: protected Cloudflare API key.

The former `cloudflare-zone-name` variable was replaced by the
`cloudflare_zone_id` default in
`terraform/cloudflare/environments/dev/variables.tf`; the active pipeline no
longer consumes a zone value from Azure DevOps.

## Removed groups

These groups have no consumers under `.azdo/` and existed only for archived
PowerShell/HELM workflows or Terraform features removed in TASK-04:

- `AKS_Settings`
- `Prefix_Library`
- `Postgres_Settings`
- `Redis_Credentials`
- `Entra_ID_Auth_Secret`
- `Terraform_Auto_Tfvars_Json_Transform`

The maintained GitOps repository will own future application/platform
configuration. Required secrets must be introduced through its selected secret
management design instead of restoring these legacy variable groups.
