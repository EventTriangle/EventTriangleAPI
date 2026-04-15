resource "azuredevops_variable_group" "terraform_backend_settings" {
  project_id   = var.project_id
  name         = "Terraform_Backend_StateFile_Settings"
  description  = "Terraform Backend StateFile Settings"
  allow_access = true

  variable {
    name         = "library-sas-token"
    is_secret    = true
    secret_value = var.sas_token
  }

  variable {
    name  = "library-state-file"
    value = "aks.event.triangle.$(library-prefix).tfstate"
  }

  variable {
    name  = "library-storage-account"
    value = "tfstatestorage011"
  }

  variable {
    name  = "library-storage-container"
    value = "tfstatecontainer01"
  }
}
