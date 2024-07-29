variable "grafana_name" {
  type        = string
  description = "Name of the Grafana instance"
}

variable "resource_group_name" {
  type        = string
  description = "Name of the resource group"
}

variable "resource_group_location" {
  type        = string
  description = "Location of the resource group"
}

variable "prometheus_id" {
  type        = string
  description = "ID of the Prometheus instance"
}

variable "grafana_admin_object_id" {
  type        = string
  description = "Object ID of the Grafana admin user"
}
