output "rg_name" {
  value = azurerm_resource_group.public.name
}

output "aks_name" {
  value = module.aks.name
}

output "subscription" {
  value = data.azurerm_client_config.current.subscription_id
}

output "cluster_connect_command" {
  value = "az aks get-credentials --resource-group ${azurerm_resource_group.public.name} --name ${module.aks.name} --subscription ${var.subscription_id}"
}

output "grafana_endpoint" {
  value = length(module.grafana) > 0 ? module.grafana[0].grafana_endpoint : null
}
