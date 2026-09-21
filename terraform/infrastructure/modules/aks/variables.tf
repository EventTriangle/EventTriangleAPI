variable "aks_name" {
  type        = string
  description = "Name of the AKS cluster."
}

variable "kubernetes_version" {
  type        = string
  description = "Kubernetes version used by the AKS cluster."
}

variable "resource_group_location" {
  type        = string
  description = "Azure region of the AKS resource group."
}

variable "resource_group_name" {
  type        = string
  description = "Name of the AKS resource group."
}

variable "system_node_count" {
  type        = number
  description = "Number of nodes in the AKS system node pool."
}

variable "default_node_pool_vm_size" {
  type        = string
  description = "Virtual machine size for the AKS system node pool."
}

variable "default_node_pool_type" {
  type        = string
  description = "AKS system node pool type."
}

variable "aks_node_resource_group_name" {
  type        = string
  description = "Name of the AKS-managed node resource group."
}
