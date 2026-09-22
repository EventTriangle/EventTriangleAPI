output "id" {
  description = "AKS cluster resource ID."
  value       = azurerm_kubernetes_cluster.aks.id
}

output "name" {
  description = "AKS cluster name."
  value       = azurerm_kubernetes_cluster.aks.name
}

output "resource_group_name" {
  description = "AKS resource group name."
  value       = azurerm_kubernetes_cluster.aks.resource_group_name
}

output "node_resource_group_name" {
  description = "AKS-managed node resource group name."
  value       = azurerm_kubernetes_cluster.aks.node_resource_group
}

output "principal_id" {
  description = "Object ID of the AKS control-plane managed identity."
  value       = azurerm_kubernetes_cluster.aks.identity[0].principal_id
}

output "kubelet_identity_principal_id" {
  description = "Object ID of the AKS kubelet managed identity."
  value       = azurerm_kubernetes_cluster.aks.kubelet_identity[0].object_id
}
