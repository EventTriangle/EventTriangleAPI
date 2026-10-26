# Managed pipelines

`pipelines.tf` creates nine pipelines using the `EventTriangle` GitHub connection,
repository `EventTriangle/EventTriangleAPI` and hosted `Azure Pipelines` pool.

- PR Validation Auth: `.azdo/pr-validation/pr-validation-auth.yml`
- PR Validation Sender: `.azdo/pr-validation/pr-validation-sender.yml`
- PR Validation Consumer: `.azdo/pr-validation/pr-validation-consumer.yml`
- Build Auth: `.azdo/build/build-auth.yml`
- Build Sender: `.azdo/build/build-sender.yml`
- Build Consumer: `.azdo/build/build-consumer.yml`
- Terraform Create: `.azdo/infrastructure/terraform-create.yml`
- Terraform Destroy: `.azdo/infrastructure/terraform-destroy.yml`
- Cloudflare DNS: `.azdo/cloudflare/terraform-dns.yml`

Terraform manages names, folders, repository connections and default branches.
YAML owns CI/PR triggers. `pipeline_default_branch` defaults to `refs/heads/main`;
override it with `TF_VAR_pipeline_default_branch` when needed.

Terraform also creates the `dev` environment and grants access to the three
infrastructure pipelines. `terraform output pipeline_definitions` shows actual
pipeline IDs and paths.

Build and PR Validation both run application checks before publishing through
`Docker_Hub_Connection` and the shared Docker build template. Same-repository
PRs may publish; fork PR triggers and secret sharing are disabled. Terraform
authorizes these six publishing pipelines explicitly, without granting access
to every pipeline in the project.

Legacy `Configure Platform` is not managed here. Keep it disabled; there is no
maintained all-in-one deployment entry point yet.
