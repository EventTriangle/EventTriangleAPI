variable "log_analytics_workspace_name" {
  type        = string
  description = "The name of the Log Analytics Workspace"

}

variable "log_analytics_resource_group_name" {
  type        = string
  description = "The name of the resource group in which the Log Analytics Workspace should be created"
}

variable "log_analytics_location" {
  type        = string
  description = "The location of the Log Analytics Workspace"
}

variable "log_analytics_sku" {
  type        = string
  description = "The SKU of the Log Analytics Workspace"
}
