variable "aks_name" {
  type        = string
  description = "The name of the AKS cluster"
}

variable "kubernetes_version" {
  type        = string
  description = "The version of Kubernetes to use for the AKS cluster"
}

variable "resource_group_location" {
  type        = string
  description = "The location of the resource group in which to create the AKS cluster"
}

variable "resource_group_name" {
  type        = string
  description = "The name of the resource group in which to create the AKS cluster"
}

variable "system_node_count" {
  type        = number
  description = "The number of system nodes to create"
}

variable "default_node_pool_vm_size" {
  type        = string
  description = "The size of the default node pool VMs"
}

variable "default_node_pool_type" {
  type        = string
  description = "The type of the default node pool"
}

variable "should_deploy_log_analytics" {
  type        = bool
  description = "Whether or not to deploy a Log Analytics workspace for the AKS cluster"
}

variable "log_analytics_workspace_id" {
  type        = string
  description = "The ID of the Log Analytics workspace to use for the AKS cluster"
}
