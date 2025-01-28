provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
}

provider "azuread" {
  # default takes current user/identity tenant
}

provider "azapi" {
  # Configuration options
}
