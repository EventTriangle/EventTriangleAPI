output "acr_id" {
  description = "Resource ID of the existing Azure Container Registry."
  value       = data.azurerm_container_registry.acr.id
}

output "control_plane_role_assignment_id" {
  description = "AcrPull role assignment ID for the AKS control-plane identity."
  value       = azurerm_role_assignment.role_acrpull.id
}

output "kubelet_role_assignment_id" {
  description = "AcrPull role assignment ID for the AKS kubelet identity."
  value       = azurerm_role_assignment.role_acrpull_kubelet.id
}
