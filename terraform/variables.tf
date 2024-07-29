variable "resource_group_name" {
  type        = string
  description = "Resource group name"
}

variable "resource_group_location" {
  type        = string
  description = "Resource group location"
}

variable "prefix" {
  type        = string
  description = "Prefix for all resources"
}

variable "cluster_name" {
  type        = string
  description = "Name of the AKS cluster"
}

variable "default_node_pool_vm_size" {
  type        = string
  description = "Default node pool VM size"
}

variable "default_node_pool_type" {
  type        = string
  description = "Default node pool type"
}

variable "system_node_count" {
  type        = number
  description = "Number of system nodes"
}

variable "acr_name" {
  type        = string
  description = "Name of the ACR"
}

variable "log_analytics_sku" {
  type        = string
  description = "Log analytics SKU"
}

variable "kubernetes_version" {
  type        = string
  description = "Kubernetes version"
}

variable "should_deploy_acr" {
  type        = bool
  description = "Should deploy ACR"
}

variable "should_deploy_log_analytics" {
  type        = bool
  description = "Should deploy log analytics"
}

variable "should_deploy_prometheus" {
  type        = bool
  description = "Should deploy prometheus"
}
