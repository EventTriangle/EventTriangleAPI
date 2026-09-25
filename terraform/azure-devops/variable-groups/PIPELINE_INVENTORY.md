# Azure DevOps pipeline inventory

Inventory date: 2026-09-23.

All existing definitions use the GitHub repository
`EventTriangle/EventTriangleAPI`, default branch `refs/heads/main`, hosted queue
`Azure Pipelines`, and GitHub service connection
`00d968fc-300a-44de-b20a-026b3555785b` (`EventTriangle`). Their former
`azure-pipelines/...` YAML paths are replaced by maintained `.azdo/...` paths.

| Terraform key | Existing ID | Azure DevOps name | Maintained YAML entry point |
| --- | ---: | --- | --- |
| `pr_validation_auth` | 23 | PR Validation Auth | `.azdo/pr-validation/pr-validation-auth.yml` |
| `pr_validation_sender` | 24 | PR Validation Sender | `.azdo/pr-validation/pr-validation-sender.yml` |
| `pr_validation_consumer` | 25 | PR Validation Consumer | `.azdo/pr-validation/pr-validation-consumer.yml` |
| `build_auth` | 26 | Build Auth | `.azdo/build/build-auth.yml` |
| `build_consumer` | 27 | Build Consumer | `.azdo/build/build-consumer.yml` |
| `build_sender` | 28 | Build Sender | `.azdo/build/build-sender.yml` |
| `terraform_create` | 32 | Terraform Create | `.azdo/infrastructure/terraform-create.yml` |
| `terraform_destroy` | 33 | Terraform Destroy | `.azdo/infrastructure/terraform-destroy.yml` |
| `cloudflare_dns` | new | Cloudflare DNS | `.azdo/cloudflare/terraform-dns.yml` |

`Configure Platform` (ID 35) is deliberately not imported. It points to the
retired `azure-pipelines/infrastructure/configure-platform.yml` entry point and
has no maintained `.azdo/` counterpart. TASK-03 replaces it with the canonical
one-run deployment pipeline. Keep the old definition disabled until that
replacement is validated, then delete it through an explicit retirement step.

## Required imports

`imports.tf` declares all eight imports. A normal `terraform plan` proposes
their import and in-place update. Do not also run manual `terraform import`
commands for the same definitions.

Do not import `cloudflare_dns`; no definition with that name currently exists.
Terraform creates it after the plan is reviewed.

## External dependencies

- The GitHub `EventTriangle` connection exists and remains an external input
  until TASK-08 manages it.
- ACR/Docker Hub and SonarCloud connections remain external until TASK-08.
  Authorize them manually only for the pipelines that consume them.
- Terraform creates the `dev` environment and authorizes only the three
  infrastructure pipelines that use it.
- The canonical all-in-one deployment YAML belongs to TASK-03 and is not added
  until that entry point exists.
