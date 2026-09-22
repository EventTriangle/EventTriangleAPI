variable "resource_group_name" {
  type        = string
  description = "Base name of the AKS resource group. The environment prefix is appended to it."
  default     = "rg-aks"
}

variable "resource_group_location" {
  type        = string
  description = "Azure region for the AKS resource group and cluster."
  default     = "northeurope"
}

variable "prefix" {
  type        = string
  description = "Environment suffix appended to resource names."
  default     = "d01"
}

variable "cluster_name" {
  type        = string
  description = "Base name of the AKS cluster. The environment prefix is appended to it."
  default     = "my-aks-cluster"
}

variable "kubernetes_version" {
  type        = string
  description = "Kubernetes version used by AKS."
  default     = "1.35.1"
}

variable "default_node_pool_vm_size" {
  type        = string
  description = "Virtual machine size for the AKS system node pool."
  default     = "Standard_DS2_v2"
}

variable "default_node_pool_type" {
  type        = string
  description = "AKS system node pool type."
  default     = "VirtualMachineScaleSets"
}

variable "system_node_count" {
  type        = number
  description = "Number of nodes in the AKS system node pool."
  default     = 3

  validation {
    condition     = var.system_node_count >= 1
    error_message = "system_node_count must be at least 1."
  }
}

variable "acr_name" {
  type        = string
  description = "Name of the existing Azure Container Registry granted to AKS."
  default     = "acrsharedd01"
}

variable "acr_resource_group_name" {
  type        = string
  description = "Resource group containing the existing Azure Container Registry."
  default     = "rg-acr-d01"
}
