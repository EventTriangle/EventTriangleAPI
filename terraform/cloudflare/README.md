# Cloudflare DNS Terraform

The development root in `environments/dev` manages the public application DNS
records that point to the Traefik LoadBalancer. The zone itself is discovered by
name and remains outside this Terraform state.

The current application endpoint is:

- `auth-eventtriangle.razumovsky.me`

All configured hostnames use the same Traefik endpoint. An IPv4 address creates
an `A` record, an IPv6 address creates an `AAAA` record, and a DNS hostname
creates a `CNAME` record. Set `record_type` explicitly only when automatic
detection is not appropriate.

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
export TF_VAR_traefik_endpoint="<current A/AAAA address or hostname>"
terraform import \
  'cloudflare_dns_record.application["auth-eventtriangle"]' \
  '<zone-id>/<dns-record-id>'
```

The development record was inspected and imported when this root was created.
Do not repeat the import unless the state has been deliberately replaced.

## Local plan and apply

Validate the Traefik address before asking Terraform to contact Cloudflare:

```bash
../../validate-traefik-endpoint.sh "$TF_VAR_traefik_endpoint" AUTO
terraform fmt -check -recursive ../..
terraform validate
terraform plan -out=cloudflare.tfplan
terraform apply cloudflare.tfplan
```

Review the plan before applying it. Changing the Traefik LoadBalancer address is
expected to update the existing DNS record in place; it must not create a second
record with the same name.

## Pipeline input

`.azdo/cloudflare/terraform-dns.yml` accepts `traefikEndpoint` as a runtime
parameter. The deployment pipeline should obtain the address from
`status.loadBalancer.ingress[0].ip` or `.hostname` on the Traefik service, pass
it to this pipeline/root as `TF_VAR_traefik_endpoint`, and run DNS only after
Traefik readiness succeeds. The pipeline passes the Cloudflare token only as
`CLOUDFLARE_API_TOKEN` from the protected `Cloudflare_API_Key` variable group.

TASK-03 will connect this root to the complete deployment-stage output. TASK-11
will define the final Traefik service name and readiness command.
