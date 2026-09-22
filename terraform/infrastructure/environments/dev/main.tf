data "azurerm_client_config" "current" {}

locals {
  resource_group_name          = "${var.resource_group_name}-${var.prefix}"
  aks_node_resource_group_name = "${var.resource_group_name}-node-${var.prefix}"
  aks_name                     = "${var.cluster_name}-${var.prefix}"
}

resource "azurerm_resource_group" "public" {
  name     = local.resource_group_name
  location = var.resource_group_location
}

module "aks" {
  source = "../../modules/aks"

  aks_name                     = local.aks_name
  aks_node_resource_group_name = local.aks_node_resource_group_name
  default_node_pool_type       = var.default_node_pool_type
  default_node_pool_vm_size    = var.default_node_pool_vm_size
  kubernetes_version           = var.kubernetes_version
  resource_group_location      = azurerm_resource_group.public.location
  resource_group_name          = azurerm_resource_group.public.name
  system_node_count            = var.system_node_count
}

module "configure_acr_access" {
  source = "../../modules/acr-access"

  acr_name                          = var.acr_name
  acr_resource_group_name           = var.acr_resource_group_name
  aks_identity_principal_id         = module.aks.principal_id
  aks_kubelet_identity_principal_id = module.aks.kubelet_identity_principal_id
}
