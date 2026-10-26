# Variable groups

Terraform authorizes each group only for its active consumers under `.azdo/`.

## Terraform_Backend_StateFile_Settings

Used by Terraform Create, Terraform Destroy and Cloudflare DNS:

- `library-storage-account`, `library-storage-container`: Azure backend location.
- `library-sas-token`: protected backend credential.
- `library-state-file`: `azure.tfstate`, used by Create/Destroy. Cloudflare uses
  the separate `cloudflare-dev.tfstate` key.

## Terraform_Azure_Credentials

Used by Terraform Create and Terraform Destroy:

- `library-client-id`, `library-subscription-id`, `library-tenant-id`.
- `library-client-secret`: protected service-principal secret.

Keep this group while the infrastructure YAML uses its `ARM_*` inputs rather
than `Azure_Connection`. Cloudflare DNS does not consume Azure credentials.

## Cloudflare_API_Key

Used only by Cloudflare DNS. `cloudflare-api-key` supplies `CLOUDFLARE_API_TOKEN`.
The zone ID comes from the Cloudflare Terraform variable default.

Legacy AKS, prefix, PostgreSQL, Redis, Entra ID and tfvars-transformation groups
have no active `.azdo/` consumers. Required application/platform secrets belong
in the GitOps secret workflow, not restored legacy groups.
