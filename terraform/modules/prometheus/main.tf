resource "azapi_resource" "prometheus" {
  type      = "microsoft.monitor/accounts@2021-06-03-preview"
  name      = var.prometheus_name
  parent_id = var.resource_group_id
  location  = var.resource_group_location
}

resource "null_resource" "enable_azure_monitor_metrics" {
  provisioner "local-exec" {
    interpreter = ["PowerShell", "-Command"]
    command     = <<-EOT

      az aks update --enable-azure-monitor-metrics `
                    -g ${var.aks_resource_group} `
                    -n ${var.aks_name} `
                    --azure-monitor-workspace-resource-id ${azapi_resource.prometheus.id}
    EOT
  }

  depends_on = [
    azapi_resource.prometheus
  ]
}
