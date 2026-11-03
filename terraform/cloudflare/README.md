# Cloudflare DNS Terraform

The dev root `environments/dev` manages the `A` record
`auth-eventtriangle.razumovsky.me`. The existing zone ID and temporary
`traefik_public_ip` default are in `variables.tf`; the zone is not managed here.
Set a real public ingress IP before applying DNS changes.

## Run locally

Use a Cloudflare API token restricted to the `razumovsky.me` zone with DNS
read/edit access. Zone read access is only needed if discovering the zone ID.

From the repository root:

```bash
cd terraform/cloudflare/environments/dev
export CLOUDFLARE_API_TOKEN="<Cloudflare token>"
```

Create the ignored `backend.hcl` locally:

```hcl
storage_account_name = "tfstatestorage011"
container_name       = "tfstatecontainer01"
key                  = "cloudflare-dev.tfstate"
sas_token            = "<backend SAS token>"
```

Keep this state separate from Azure infrastructure's `azure.tfstate`.

```bash
terraform init -backend-config=backend.hcl
terraform fmt -check -recursive ../..
terraform validate
terraform plan -out=cloudflare.tfplan
```

Review the record/IP change, then apply:

```bash
terraform apply cloudflare.tfplan
```

If an existing record is not yet in state, inspect its ID and import it once
before planning; do not create a duplicate:

```bash
terraform import cloudflare_dns_record.application '<zone-id>/<dns-record-id>'
```

## Pipeline

`.azdo/cloudflare/terraform-dns.yml` reuses the Terraform templates: PRs run
plan, pushes to `main` can run apply. The token comes from `Cloudflare_API_Key`.
The pipeline uses the Terraform IP default; it does not query AKS for an address.
