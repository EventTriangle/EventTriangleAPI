# Azure DevOps Terraform

Creates variable groups, nine YAML pipelines, service connections and the `dev`
environment in the existing EventTriangle Azure DevOps project.

## Run locally

Requires Terraform 1.16+, an Azure DevOps PAT with access to manage these
resources, backend storage access and the service credentials below.

From the repository root:

```bash
cd terraform/azure-devops/variable-groups
```

Create the ignored `backend.hcl` locally:

```hcl
storage_account_name = "tfstatestorage011"
container_name       = "tfstatecontainer01"
key                  = "event.triangle.azdo.libraries.tfstate"
sas_token            = "<backend SAS token>"
```

Supply secrets through protected environment variables:

```bash
export AZDO_ORG_SERVICE_URL="https://dev.azure.com/EventTriangle"
export AZDO_PERSONAL_ACCESS_TOKEN="<Azure DevOps PAT>"
export TF_VAR_github_pat="<fine-grained GitHub PAT>"
export TF_VAR_backend_sas_token="<backend SAS token>"
export TF_VAR_azure_client_secret="<Azure service-principal secret>"
export TF_VAR_cloudflare_api_key="<Cloudflare API key>"
export TF_VAR_docker_hub_token="<Docker Hub token>"
export TF_VAR_azure_acr_password="<ACR password>"
```

Defaults and optional overrides are in `variables.tf`. `azure_client_id` and
`azure_client_secret` must belong to the same service principal; both the Azure
connection and Azure credential variable group use this pair.

```bash
terraform init -reconfigure -backend-config=backend.hcl
terraform fmt -check
terraform validate
terraform plan -out=azure-devops.tfplan
```

Review the plan for unintended changes or deletions, then apply:

```bash
terraform apply azure-devops.tfplan
terraform plan
```

The second plan should report `No changes`. Secrets, backend configuration,
state backups and saved plans must stay out of Git and have restricted access.

## Fine-grained GitHub PAT permissions

In GitHub, open `Settings > Developer settings > Personal access tokens >
Fine-grained tokens > Generate new token` and select:

- Resource owner: `EventTriangle`.
- Repository access: `Only select repositories` → `EventTriangle/EventTriangleAPI`.
- An explicit expiration; obtain organization approval if required.

Under **Repository permissions**, enable only:

- **Contents — Read-only:** checkout code and read existing pipeline YAML.
- **Pull requests — Read-only:** read PR metadata and changed files.
- **Commit statuses — Read and write:** publish build results on commits.
- **Webhooks — Read and write:** configure CI and PR triggers.
- **Metadata — Read-only:** included automatically by GitHub.

Leave other permissions unselected, including Actions, Workflows, Checks,
Administration, Packages and organization/account permissions. The token owner's
repository access must also allow webhook management; token permissions cannot
elevate it. Fine-grained tokens have restrictions for outside collaborators.

`AZDO_PERSONAL_ACCESS_TOKEN` creates Azure DevOps resources;
`TF_VAR_github_pat` authenticates their GitHub connection. The fine-grained setup
still needs end-to-end verification of checkout, triggers and commit statuses.
PAT reporting uses commit statuses, not GitHub App Checks.

References: [Microsoft: GitHub authentication](https://learn.microsoft.com/en-us/azure/devops/pipelines/security/secure-access-to-repos?view=azure-devops#authentication-to-github-repositories),
[GitHub: fine-grained tokens](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).

## SonarCloud (optional)

Disabled by default. To create its connection, set `TF_VAR_manage_sonarcloud=true`
and `TF_VAR_sonarcloud_token`, then plan/apply. Keep management enabled once the
connection is managed: `prevent_destroy` protects it from accidental removal.
An unmanaged same-name connection must be explicitly adopted or removed first.

Build Auth analysis is controlled separately by `sonarCloudEnabled`, default
`false`. Enable it for a manual run, or change its YAML default for automatic runs.

## Reference

- [Service connections](SERVICE_CONNECTION_INVENTORY.md)
- [Pipelines](PIPELINE_INVENTORY.md)
- [Variable groups](VARIABLE_GROUP_CONSUMERS.md)
- [Access and maintenance](DEVELOPER_ACCESS.md)
