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
