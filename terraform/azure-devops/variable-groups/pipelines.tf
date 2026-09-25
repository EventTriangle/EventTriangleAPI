locals {
  pipelines = {
    pr_validation_auth = {
      name      = "PR Validation Auth"
      folder    = "\\PR Validation"
      yaml_path = ".azdo/pr-validation/pr-validation-auth.yml"
    }
    pr_validation_sender = {
      name      = "PR Validation Sender"
      folder    = "\\PR Validation"
      yaml_path = ".azdo/pr-validation/pr-validation-sender.yml"
    }
    pr_validation_consumer = {
      name      = "PR Validation Consumer"
      folder    = "\\PR Validation"
      yaml_path = ".azdo/pr-validation/pr-validation-consumer.yml"
    }
    build_auth = {
      name      = "Build Auth"
      folder    = "\\Build"
      yaml_path = ".azdo/build/build-auth.yml"
    }
    build_sender = {
      name      = "Build Sender"
      folder    = "\\Build"
      yaml_path = ".azdo/build/build-sender.yml"
    }
    build_consumer = {
      name      = "Build Consumer"
      folder    = "\\Build"
      yaml_path = ".azdo/build/build-consumer.yml"
    }
    terraform_create = {
      name      = "Terraform Create"
      folder    = "\\Infrastructure"
      yaml_path = ".azdo/infrastructure/terraform-create.yml"
    }
    terraform_destroy = {
      name      = "Terraform Destroy"
      folder    = "\\Infrastructure"
      yaml_path = ".azdo/infrastructure/terraform-destroy.yml"
    }
    cloudflare_dns = {
      name      = "Cloudflare DNS"
      folder    = "\\Infrastructure"
      yaml_path = ".azdo/cloudflare/terraform-dns.yml"
    }
  }

  variable_group_pipeline_authorizations = {
    backend_create = {
      variable_group_id = azuredevops_variable_group.terraform_backend_settings.id
      pipeline_key      = "terraform_create"
    }
    backend_destroy = {
      variable_group_id = azuredevops_variable_group.terraform_backend_settings.id
      pipeline_key      = "terraform_destroy"
    }
    backend_cloudflare = {
      variable_group_id = azuredevops_variable_group.terraform_backend_settings.id
      pipeline_key      = "cloudflare_dns"
    }
    azure_create = {
      variable_group_id = azuredevops_variable_group.terraform_azure_credentials.id
      pipeline_key      = "terraform_create"
    }
    azure_destroy = {
      variable_group_id = azuredevops_variable_group.terraform_azure_credentials.id
      pipeline_key      = "terraform_destroy"
    }
    cloudflare_dns = {
      variable_group_id = azuredevops_variable_group.cloudflare_api_key.id
      pipeline_key      = "cloudflare_dns"
    }
  }
}

resource "azuredevops_build_definition" "pipeline" {
  for_each = local.pipelines

  project_id      = var.project_id
  name            = each.value.name
  path            = each.value.folder
  agent_pool_name = "Azure Pipelines"
  queue_status    = "enabled"

  ci_trigger {
    use_yaml = true
  }

  pull_request_trigger {
    use_yaml = true

    forks {
      enabled       = false
      share_secrets = false
    }
  }

  repository {
    repo_type             = "GitHub"
    repo_id               = var.github_repository_id
    branch_name           = var.pipeline_default_branch
    yml_path              = each.value.yaml_path
    service_connection_id = var.github_service_connection_id
    report_build_status   = true
  }

  features {
    skip_first_run = true
  }
}

resource "azuredevops_environment" "dev" {
  project_id  = var.project_id
  name        = "dev"
  description = "Deployment environment used by the maintained Terraform apply pipelines."
}

resource "azuredevops_pipeline_authorization" "github_repository" {
  for_each = azuredevops_build_definition.pipeline

  project_id  = var.project_id
  resource_id = var.github_service_connection_id
  type        = "endpoint"
  pipeline_id = each.value.id
}

resource "azuredevops_pipeline_authorization" "variable_group" {
  for_each = local.variable_group_pipeline_authorizations

  project_id  = var.project_id
  resource_id = each.value.variable_group_id
  type        = "variablegroup"
  pipeline_id = azuredevops_build_definition.pipeline[each.value.pipeline_key].id
}

resource "azuredevops_pipeline_authorization" "environment" {
  for_each = toset(["terraform_create", "terraform_destroy", "cloudflare_dns"])

  project_id  = var.project_id
  resource_id = azuredevops_environment.dev.id
  type        = "environment"
  pipeline_id = azuredevops_build_definition.pipeline[each.value].id
}
