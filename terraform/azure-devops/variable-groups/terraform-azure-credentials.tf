resource "azuredevops_variable_group" "terraform_azure_credentials" {
  project_id   = var.project_id
  name         = "Terraform_Azure_Credentials"
  description  = "Terraform Azure Credentials"
  allow_access = true

  variable {
    name  = "library-client-id"
    value = var.azure_client_id
  }

  variable {
    name         = "library-client-secret"
    is_secret    = true
    secret_value = var.azure_client_secret
  }

  variable {
    name  = "library-subscription-id"
    value = var.azure_subscription_id
  }

  variable {
    name  = "library-tenant-id"
    value = var.azure_tenant_id
  }
}
