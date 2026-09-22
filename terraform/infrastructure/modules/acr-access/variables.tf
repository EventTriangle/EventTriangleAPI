variable "acr_name" {
  type        = string
  description = "Name of the existing Azure Container Registry."
}

variable "acr_resource_group_name" {
  type        = string
  description = "Resource group containing the existing Azure Container Registry."
}

variable "aks_identity_principal_id" {
  type        = string
  description = "Object ID of the AKS control-plane managed identity."
}

variable "aks_kubelet_identity_principal_id" {
  type        = string
  description = "Object ID of the AKS kubelet managed identity."
}
