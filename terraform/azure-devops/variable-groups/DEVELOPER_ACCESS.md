# Azure DevOps access and maintenance

For setup, secret inputs and GitHub PAT permissions, see the [README](README.md).

## Access

- The applying identity must manage service connections, pipelines, variable
  groups, environments and pipeline authorizations in the EventTriangle project.
- Backend access must allow reading/writing state and acquiring its lock.
- Obtain service credentials from their owners; Azure DevOps cannot export them.
- Keep `Grant access permission to all pipelines` disabled. Terraform grants only
  the access declared in `pipelines.tf`; remove stale broad grants in the portal.
- Protect state and saved plans: `sensitive = true` hides CLI output, not storage.

## Rotate a credential

1. Create a replacement at its owning service and update the matching `TF_VAR_*`.
2. Review and apply the plan; verify the consuming pipeline or authentication.
3. Revoke the old credential only after the replacement works.

Azure client IDs and secrets must match. GitHub tokens are needed for ongoing
pipeline access, not just initial creation.

## Replace or recover a connection

Pause affected runs and review consumers before replacement. Connection IDs
change; Terraform updates managed references, but undeclared permissions such
as legacy `Configure Platform` access are not restored automatically.

For an existing GitHub App connection, use explicit replacement rather than an
in-place authentication change:

```bash
terraform plan -replace=azuredevops_serviceendpoint_github.github -out=azure-devops.tfplan
```

If manual deletion causes `unexpected nil service endpoint` in provider 1.16.0:

1. Save a protected `terraform state pull` backup outside Git.
2. Compare IDs with the Azure DevOps inventory and confirm the objects are absent.
3. Use `terraform state rm '<confirmed-missing-address>'` only for those entries
   and their obsolete endpoint grants, then review a new normal plan and apply.

Do not clear the whole state or forget resources that still exist. Keep the
existing Azure backend and credential inputs. Review required GitHub status-check
rules when switching from GitHub App Checks to PAT-based commit statuses.
