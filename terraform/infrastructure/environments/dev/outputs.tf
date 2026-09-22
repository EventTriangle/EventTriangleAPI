output "rg_name" {
  description = "AKS resource group name."
  value       = azurerm_resource_group.public.name
}

output "aks_name" {
  description = "AKS cluster name."
  value       = module.aks.name
}

output "aks_id" {
  description = "AKS cluster resource ID."
  value       = module.aks.id
}

output "aks_node_resource_group_name" {
  description = "AKS-managed node resource group name."
  value       = module.aks.node_resource_group_name
}

output "aks_identity_principal_id" {
  description = "Object ID of the AKS control-plane managed identity."
  value       = module.aks.principal_id
}

output "aks_kubelet_identity_principal_id" {
  description = "Object ID of the AKS kubelet managed identity."
  value       = module.aks.kubelet_identity_principal_id
}

output "acr_id" {
  description = "Resource ID of the existing ACR granted to AKS."
  value       = module.configure_acr_access.acr_id
}

output "acr_pull_role_assignment_ids" {
  description = "AcrPull role assignment IDs for the AKS control-plane and kubelet identities."
  value = {
    control_plane = module.configure_acr_access.control_plane_role_assignment_id
    kubelet       = module.configure_acr_access.kubelet_role_assignment_id
  }
}

output "subscription" {
  description = "Azure subscription ID used by the provider."
  value       = data.azurerm_client_config.current.subscription_id
}

output "tenant_id" {
  description = "Azure tenant ID used by the provider."
  value       = data.azurerm_client_config.current.tenant_id
}

output "cluster_connect_command" {
  description = "Azure CLI command for merging the AKS credentials into the current kubeconfig."
  value       = "az aks get-credentials --resource-group ${azurerm_resource_group.public.name} --name ${module.aks.name} --subscription ${data.azurerm_client_config.current.subscription_id} --overwrite-existing"
}
