variable "acr_name" {
  type        = string
  description = "The name of the Azure Container Registry"
}

variable "resource_group_name" {
  type        = string
  description = "The name of the Azure Resource Group"
}

variable "resource_group_location" {
  type        = string
  description = "The location of the Azure Resource Group"
}

variable "aks_identity_principal_id" {
  type        = string
  description = "The principal id of the AKS identity"
}
