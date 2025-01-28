provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id
}

provider "azuread" {
  # default takes current user/identity tenant
}

provider "azapi" {
  tenant_id       = var.tenant_id
  client_id       = var.client_id
  subscription_id = var.subscription_id
  client_secret   = var.client_secret
}
