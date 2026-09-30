resource "azuredevops_variable_group" "terraform_backend_settings" {
  project_id   = var.project_id
  name         = "Terraform_Backend_StateFile_Settings"
  description  = "Terraform Backend StateFile Settings"
  allow_access = false

  variable {
    name         = "library-sas-token"
    is_secret    = true
    secret_value = var.backend_sas_token
  }

  variable {
    name  = "library-state-file"
    value = var.infrastructure_state_key
  }

  variable {
    name  = "library-storage-account"
    value = var.backend_storage_account_name
  }

  variable {
    name  = "library-storage-container"
    value = var.backend_container_name
  }
}
