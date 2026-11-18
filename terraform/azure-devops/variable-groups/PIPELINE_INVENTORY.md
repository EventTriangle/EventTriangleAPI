# Managed pipelines

`pipelines.tf` creates nine pipelines using the `FluxPay` GitHub connection,
repository `FluxPayProject/FluxPayAPI` and hosted `Azure Pipelines` pool.

- PR Validation Auth: `.azdo/pr-validation/pr-validation-auth.yml`
- PR Validation Consumer: `.azdo/pr-validation/pr-validation-consumer.yml`
- PR Validation Sender: `.azdo/pr-validation/pr-validation-sender.yml`
- Build Auth: `.azdo/build/build-auth.yml`
- Build Consumer: `.azdo/build/build-consumer.yml`
- Build Sender: `.azdo/build/build-sender.yml`
- Terraform Create: `.azdo/infrastructure/terraform-create.yml`
- Terraform Destroy: `.azdo/infrastructure/terraform-destroy.yml`
- Cloudflare DNS: `.azdo/cloudflare/terraform-dns.yml`

Terraform manages names, folders, repository connections and default branches.
YAML owns CI/PR triggers. `pipeline_default_branch` defaults to `refs/heads/main`;
override it with `TF_VAR_pipeline_default_branch` when needed.

Terraform also creates the `dev` environment and grants access to the three
infrastructure pipelines. `terraform output pipeline_definitions` shows actual
pipeline IDs and paths.

## FluxPay delivery

Each service has a Build and PR Validation entry point using the same three
stage templates: Docker → Helm → GitOps. Build keeps its branch and tag
triggers and adds `develop`. PR Validation keeps service path filters and also
watches the matching chart and deployment scripts.
A run publishes one image and one chart with a shared GitVersion-based tag.
GitOps updates that service's chart tag, image tag, and image digest in one
commit. Concurrent service runs reapply their changes on top of the latest
GitOps commit if a push races with another run.

- Build publishes the image and chart on `main`, `develop`, and `v*` tags.
  Build Auth runs SonarCloud on `main` and `develop`, with Quality Gate status published on `main`. Only builds from
  `develop` update `fluxcd-repository/master`.
- PR Validation runs on same-repository PRs targeting `main` or `develop`.
  Its CI credentials allow publication, so only use it for trusted
  contributors. Fork triggers and secret sharing are disabled.
  PRs use a separate Docker cache and commit versions to `preview/pr-<number>`.
  Flux tracks its configured GitOps branch, so preview branches are version
  records. A preview cluster would require its own Flux configuration.

### Rollout

1. Supply the protected Terraform inputs `TF_VAR_fluxpay_ghcr_user`,
   `TF_VAR_fluxpay_ghcr_token`, `TF_VAR_fluxpay_gitops_repository`, and
   `TF_VAR_fluxpay_gitops_token`. Use separate credentials for package write
   access and for Git contents write access to the GitOps repository.
   Set `TF_VAR_manage_sonarcloud=true` and configure the SonarCloud connection.
2. Make sure both API branches have the new YAML. Review and apply the
   Terraform plan to grant the existing pipelines the new variable groups and
   service connection permissions. Confirm Build Auth can access the SonarCloud project `event-triangle-api`.
3. Verify all three Build pipelines publish their respective image and chart
   and that builds from `develop` commit the matching GitOps pins.
4. Verify Flux reconciliation and rollout once the cluster is running. If a
   release needs to be rolled back, revert its GitOps commit to restore the
   previous chart and image pins together.

Legacy `Configure Platform` is not managed here. Keep it disabled; there is no
maintained all-in-one deployment entry point yet.
