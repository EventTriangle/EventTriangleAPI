resource "azuredevops_variable_group" "aks_settings" {
  project_id   = var.project_id
  name         = "AKS_Settings"
  description  = "AKS_Settings"
  allow_access = true

  variable {
    name  = "library-aks-cluster-name"
    value = "my-aks-cluster-$(library-prefix)"
  }

  variable {
    name  = "library-aks-resource-group"
    value = "rg-aks-$(library-prefix)"
  }

  variable {
    name  = "library-aks-namespace"
    value = "event-triangle-dev"
  }
}
