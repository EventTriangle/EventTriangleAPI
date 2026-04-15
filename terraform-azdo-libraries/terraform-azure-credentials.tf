resource "azuredevops_variable_group" "terraform_azure_credentials" {
  project_id   = var.project_id
  name         = "Terraform_Azure_Credentials"
  description  = "Terraform Azure Credentials"
  allow_access = true

  variable {
    name  = "library-client-id"
    value = "ab0a5dc1-ee52-4574-96e0-469f237928a6"
  }

  variable {
    name         = "library-client-secret"
    is_secret    = true
    secret_value = var.azure_rm_client_secret
  }

  variable {
    name  = "library-subscription-id"
    value = "1b08b9a2-ac6d-4b86-8a2f-8fef552c8371"
  }

  variable {
    name  = "library-tenant-id"
    value = "b40a105f-0643-4922-8e60-10fc1abf9c4b"
  }
}
