# Cloudflare DNS Terraform

The development root in `environments/dev` manages the public application DNS
record that points to the Traefik LoadBalancer. The zone itself remains outside
this Terraform state and is referenced by its Cloudflare zone ID.

The current application endpoint is:

- `auth-eventtriangle.razumovsky.me`

The root manages one `A` record whose content is the Traefik public IPv4 address
obtained by the deployment Bash script.

## Authentication and permissions

Never put the Cloudflare token in a `.tf`, `.tfvars`, backend file, command-line
argument, or committed pipeline variable. Export it through the provider's
standard environment variable:

```bash
export CLOUDFLARE_API_TOKEN="<protected token>"
```

Use a scoped API token with these permissions for the `razumovsky.me` zone:

- Zone / Zone / Read, so Terraform can discover the zone ID.
- Zone / DNS / Read, so Terraform can refresh and plan records.
- Zone / DNS / Edit, so Terraform can create and update records.

## Separate state

Cloudflare development state uses the existing Azure Blob backend but a
different key from the Azure infrastructure state:

- storage account: `tfstatestorage011`
- container: `tfstatecontainer01`
- key: `cloudflare-dev.tfstate`

Copy `environments/dev/backend.hcl.example` to the ignored
`environments/dev/backend.hcl`, add the SAS token locally, and initialize with:

```bash
cd terraform/cloudflare/environments/dev
cp backend.hcl.example backend.hcl
terraform init -backend-config=backend.hcl
```

For automation, pass the same backend values as protected pipeline variables.
The Cloudflare state key must stay different from `azure.tfstate`.

## Import the existing record

The record existed before this root was introduced, so it must be imported
instead of recreated. Inspect it first through the Cloudflare dashboard or API,
then import it with the zone ID and record ID:

```bash
export TF_VAR_traefik_public_ip="<current Traefik public IPv4 address>"
terraform import \
  cloudflare_dns_record.application \
  '<zone-id>/<dns-record-id>'
```

The development record was inspected and imported when this root was created.
Do not repeat the import unless the state has been deliberately replaced.

## Local plan and apply

Validate the Traefik address before asking Terraform to contact Cloudflare:

```bash
../../validate-traefik-endpoint.sh "$TF_VAR_traefik_public_ip"
terraform fmt -check -recursive ../..
terraform validate
terraform plan -out=cloudflare.tfplan
terraform apply cloudflare.tfplan
```

Review the plan before applying it. Changing the Traefik LoadBalancer address is
expected to update the existing DNS record in place; it must not create a second
record with the same name.

## Pipeline input

`.azdo/cloudflare/terraform-dns.yml` authenticates to AKS and calls
`get-traefik-public-ip.sh`. The script reads
`status.loadBalancer.ingress[0].ip` from the Traefik service with `kubectl` and
sets `TF_VAR_traefik_public_ip` for the Terraform plan. A plan-only test falls
back to the variable default `10.10.190.1` when Traefik is not installed; an
apply refuses to continue without a live service IP. The pipeline passes the
Cloudflare token only as `CLOUDFLARE_API_TOKEN` from the protected
`Cloudflare_API_Key` variable group.

TASK-03 will connect this root to the complete deployment-stage output. TASK-11
will define the final Traefik service name and readiness command.
