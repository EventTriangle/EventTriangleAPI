# Azure DevOps Terraform

This Terraform root manages the retained variable groups, maintained YAML
pipeline definitions, the `dev` deployment environment, retained service
connections created from credential inputs, and pipeline-specific resource
authorizations.

The root remains in `terraform/azure-devops/variable-groups/` to preserve the
working TASK-06 layout. Its existing variable-group resource addresses and
remote backend are unchanged:

- storage account: `tfstatestorage011`
- container: `tfstatecontainer01`
- key: `event.triangle.azdo.libraries.tfstate`

Always initialize this exact state before planning. See
`PIPELINE_INVENTORY.md` for the Terraform-managed definitions and
`SERVICE_CONNECTION_INVENTORY.md` for the historical connection inventory and
current creation contract.

## Bootstrap order

1. Create or identify the Azure Storage backend outside this root.
2. Identify the existing Azure DevOps project, repository and credential owners.
   Create a repository-scoped fine-grained GitHub PAT as described below. Use only a compatible
   TASK-08 configuration against the shared state; an older branch that lacks
   these resources could propose their deletion.
3. Authenticate the Azure DevOps provider, initialize the existing state, and
   supply every protected input listed below.
4. Plan creation of the four enabled service connections (five with SonarCloud
   enabled). There are no fixed-ID import blocks. On an existing installation,
   follow the GitHub App transition procedure in `DEVELOPER_ACCESS.md` instead
   of attempting an in-place authentication change.
5. Confirm that no still-required variable group, pipeline, environment, or
   secret is being destroyed.
6. Apply the service connections and narrowly scoped pipeline permissions.
7. Run each consuming pipeline, then produce a second plan and require no
   unintended changes.

This root creates the service connections, not their upstream credentials or
the Azure DevOps project. The backend, provider PAT, GitHub PAT and other external
credentials must already exist. The applying identity needs permission to
administer service connections, build definitions, variable groups, environments,
and pipeline resource authorizations in the Azure DevOps project.

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
export TF_VAR_github_pat="<repository-scoped fine-grained GitHub PAT>"
export TF_VAR_azure_client_secret="<Azure service-principal secret>"
export TF_VAR_cloudflare_api_key="<Cloudflare API key>"
export TF_VAR_docker_hub_token="<Docker Hub access token>"
export TF_VAR_azure_acr_password="<ACR credential>"
```

Only when enabling SonarCloud, also set `TF_VAR_manage_sonarcloud=true` and
`TF_VAR_sonarcloud_token` to a working SonarCloud token.

Then run:

```bash
terraform fmt -check
terraform validate
terraform plan -out=azure-devops.tfplan
terraform apply azure-devops.tfplan
```

Review every plan. This root owns real Azure DevOps configuration, so removing
a managed resource can delete or deauthorize the corresponding object.

### Fine-grained GitHub PAT permissions

Create the token in GitHub `Settings > Developer settings > Personal access
tokens > Fine-grained tokens > Generate new token`:

- **Resource owner:** `EventTriangle`.
- **Repository access:** `Only select repositories`.
- **Selected repository:** `EventTriangle/EventTriangleAPI` only.
- Set an explicit expiration and rotate before it expires. If the organization
  requires approval, wait for approval before testing the integration.

Under **Repository permissions**, select only:

- **Contents — Read-only:** checkout code and read existing YAML, branches and
  tags. The pipelines do not need to push code or generate YAML in GitHub.
- **Pull requests — Read-only:** read PR metadata and changed files for PR
  validation. The integration does not create, merge or comment on PRs.
- **Commit statuses — Read and write:** publish the Azure Pipelines build result
  on the commit, as configured by `report_build_status = true`.
- **Webhooks — Read and write:** register and maintain the repository hooks for
  CI and PR triggers when pipeline definitions are created or updated.
- **Metadata — Read-only:** repository metadata; GitHub includes this required
  permission automatically.

Leave all other permissions unselected. In particular, do not add
`Administration`, `Actions`, `Workflows`, `Checks`, `Issues`, `Packages`, or
organization/account permissions. Azure Pipelines executes these builds, not
GitHub Actions; PAT-based reporting uses commit statuses, not GitHub App Checks.
Docker Hub, ACR and Azure authentication use their own credentials, not this PAT.

This list is the least-privilege starting configuration derived from the GitHub
API operations used by our existing-YAML workflow. Full Azure Pipelines
acceptance with a fine-grained token is still pending: verify Terraform
creation, checkout, CI/PR triggers and status reporting before marking it
validated. Do not grant unrelated permissions merely to make an error disappear;
identify the failed operation and its required access first.

GitHub permissions do not replace Azure DevOps permissions. The provider uses
`AZDO_PERSONAL_ACCESS_TOKEN` to create service connections, pipeline definitions
and grants. `TF_VAR_github_pat` is the credential stored inside the GitHub
connection. Its owner must already have the repository rights needed to manage
webhooks; selecting token permissions cannot elevate the owner's access.
An outside collaborator may also encounter GitHub's fine-grained token
limitations; resolve organization membership/access rather than silently
switching to an unrestricted classic token.

References:

- [Microsoft: prefer repository-scoped fine-grained PATs when using PAT authentication](https://learn.microsoft.com/en-us/azure/devops/pipelines/security/secure-access-to-repos?view=azure-devops#authentication-to-github-repositories).
- [GitHub: read repository contents](https://docs.github.com/en/rest/repos/contents#get-repository-content).
- [GitHub: read pull requests](https://docs.github.com/en/rest/pulls/pulls#list-pull-requests).
- [GitHub: create commit statuses](https://docs.github.com/en/rest/commits/statuses#create-a-commit-status).
- [GitHub: create repository webhooks](https://docs.github.com/en/rest/repos/webhooks#create-a-repository-webhook).
- [GitHub: fine-grained token limitations and organization approval](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).

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
this root manages four connections and does not require a SonarCloud token or
modify the existing SonarCloud endpoint. When a working token is available,
set `TF_VAR_manage_sonarcloud=true` and `TF_VAR_sonarcloud_token`, then review
and apply creation of the additional connection and authorization. If an
unmanaged SonarCloud connection already has the same name, explicitly adopt it
or remove it in an approved maintenance window first; there is no automatic
import. Keep `manage_sonarcloud` enabled once managed: `prevent_destroy` blocks
accidental endpoint deletion.

Build Auth independently exposes the boolean Run Pipeline parameter
`sonarCloudEnabled`, default `false`. The shared template excludes SonarCloud
steps when disabled, including their service-connection inputs. Enable the
parameter for a manual run on `main` after repairing the token. To enable
analysis for automatic runs, change the parameter's YAML default to `true`.
Disabling pipeline analysis does not require disabling Terraform management.

TASK-08 creates `EventTriangle`, `Docker_Hub_Connection`,
`Azure_ACR_Connection`, and `Azure_Connection` from the supplied credentials.
`SonarCloud_Service_Connection` is also created when explicitly enabled.
Pipeline definitions and endpoint authorizations reference Terraform resource
IDs, not historical IDs. Recreated connections keep their names, so YAML inputs
do not need renaming. An unmanaged same-name connection must be explicitly
adopted or removed before creation to avoid conflicts.

Terraform grants endpoint access only to current consumers:

- `EventTriangle`: all Terraform-managed pipelines;
- `Azure_ACR_Connection`: the three Build and three PR Validation pipelines;
- `SonarCloud_Service_Connection`: Build Auth only.

`Docker_Hub_Connection` remains unauthorized until TASK-09 adds its consumers.
`Azure_Connection` is not granted to any maintained pipeline while the
infrastructure YAML uses the `Terraform_Azure_Credentials` variable group; its
existing permission for legacy `Configure Platform` is not declared here.
Recreating the connection drops that legacy grant; coordinate with its owner
before recreation if that unmanaged pipeline is still needed.

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

`EventTriangle` now uses a GitHub PAT (`auth_personal`), so Terraform can create
its authentication without a manually bootstrapped GitHub App connection. This
changes authentication from the previously imported GitHub App; it is not an
automatic transfer of its installation token. PAT authentication reports commit
statuses rather than GitHub App Checks; review any required-check branch rules
when switching. Azure DevOps does not return stored Docker, SonarCloud, ACR,
Azure or GitHub secrets; their owners must provide current values.
See `DEVELOPER_ACCESS.md` for PAT permissions, the controlled transition,
recreation acceptance and credential rotation.

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
