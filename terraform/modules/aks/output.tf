output "principal_id" {
  value = azurerm_kubernetes_cluster.aks.identity[0].principal_id
}

output "name" {
  value = azurerm_kubernetes_cluster.aks.name
}

output "resource_group_name" {
  value = azurerm_kubernetes_cluster.aks.resource_group_name
}
