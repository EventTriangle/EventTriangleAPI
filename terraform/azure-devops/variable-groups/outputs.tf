output "retained_variable_groups" {
  description = "Variable groups still consumed by active pipelines."
  value = {
    terraform_backend = {
      id   = azuredevops_variable_group.terraform_backend_settings.id
      name = azuredevops_variable_group.terraform_backend_settings.name
    }
    terraform_azure_credentials = {
      id   = azuredevops_variable_group.terraform_azure_credentials.id
      name = azuredevops_variable_group.terraform_azure_credentials.name
    }
    cloudflare_api = {
      id   = azuredevops_variable_group.cloudflare_api_key.id
      name = azuredevops_variable_group.cloudflare_api_key.name
    }
  }
}

output "pipeline_definitions" {
  description = "Azure DevOps pipeline IDs, names, and maintained YAML entry points."
  value = {
    for key, definition in azuredevops_build_definition.pipeline : key => {
      id        = definition.id
      name      = definition.name
      yaml_path = local.pipelines[key].yaml_path
    }
  }
}

output "deployment_environment" {
  description = "Azure DevOps environment used by Terraform apply stages."
  value = {
    id   = azuredevops_environment.dev.id
    name = azuredevops_environment.dev.name
  }
}
