variable "prometheus_name" {
  type        = string
  description = "Name of the Prometheus instance"
}

variable "resource_group_id" {
  type        = string
  description = "ID of the resource group where the Prometheus instance will be created"
}

variable "resource_group_location" {
  type        = string
  description = "Location of the resource group where the Prometheus instance will be created"
}

variable "aks_resource_group" {
  type        = string
  description = "Name of the resource group where the AKS cluster is located"
}

variable "aks_name" {
  type        = string
  description = "Name of the AKS cluster"
}
