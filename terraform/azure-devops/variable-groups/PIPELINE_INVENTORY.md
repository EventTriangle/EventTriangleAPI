# Azure DevOps pipeline inventory

Inventory date: 2026-09-30.

All existing definitions use the GitHub repository
`EventTriangle/EventTriangleAPI`, default branch `refs/heads/main`, hosted queue
`Azure Pipelines`, and GitHub service connection
`00d968fc-300a-44de-b20a-026b3555785b` (`EventTriangle`). Their former
`azure-pipelines/...` YAML paths are replaced by maintained `.azdo/...` paths.

| Terraform key | Existing ID | Azure DevOps name | Maintained YAML entry point |
| --- | ---: | --- | --- |
| `pr_validation_auth` | 47 | PR Validation Auth | `.azdo/pr-validation/pr-validation-auth.yml` |
| `pr_validation_sender` | 41 | PR Validation Sender | `.azdo/pr-validation/pr-validation-sender.yml` |
| `pr_validation_consumer` | 45 | PR Validation Consumer | `.azdo/pr-validation/pr-validation-consumer.yml` |
| `build_auth` | 44 | Build Auth | `.azdo/build/build-auth.yml` |
| `build_consumer` | 43 | Build Consumer | `.azdo/build/build-consumer.yml` |
| `build_sender` | 48 | Build Sender | `.azdo/build/build-sender.yml` |
| `terraform_create` | 46 | Terraform Create | `.azdo/infrastructure/terraform-create.yml` |
| `terraform_destroy` | 42 | Terraform Destroy | `.azdo/infrastructure/terraform-destroy.yml` |
| `cloudflare_dns` | 40 | Cloudflare DNS | `.azdo/cloudflare/terraform-dns.yml` |

`Configure Platform` (ID 35) is deliberately not imported. It points to the
retired `azure-pipelines/infrastructure/configure-platform.yml` entry point and
has no maintained `.azdo/` counterpart. TASK-03 replaces it with the canonical
one-run deployment pipeline. Keep the old definition disabled until that
replacement is validated, then delete it through an explicit retirement step.

## Terraform ownership

All nine maintained definitions were deleted and recreated by Terraform. Their
new IDs are recorded in the table above and are managed directly in Terraform
state. No pipeline import blocks are required.

## External dependencies

- The GitHub `EventTriangle` connection exists and remains an external input
  until TASK-08 manages it.
- ACR/Docker Hub and SonarCloud connections remain external until TASK-08.
  Authorize them manually only for the pipelines that consume them.
- Terraform creates the `dev` environment and authorizes only the three
  infrastructure pipelines that use it.
- The canonical all-in-one deployment YAML belongs to TASK-03 and is not added
  until that entry point exists.
