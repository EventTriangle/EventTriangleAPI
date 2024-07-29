terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=3.71.0"
    }

    azuread = {
      source  = "hashicorp/azuread"
      version = "=2.43.0"
    }

    azapi = {
      source  = "Azure/azapi"
      version = "=1.9.0"
    }
  }

  backend "azurerm" {}
}
