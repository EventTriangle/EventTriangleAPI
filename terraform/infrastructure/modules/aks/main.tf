resource "azurerm_kubernetes_cluster" "aks" {
  name                = var.aks_name
  kubernetes_version  = var.kubernetes_version
  location            = var.resource_group_location
  resource_group_name = var.resource_group_name
  dns_prefix          = var.aks_name
  node_resource_group = var.aks_node_resource_group_name

  default_node_pool {
    name                        = "systempool"
    node_count                  = var.system_node_count
    vm_size                     = var.default_node_pool_vm_size
    type                        = var.default_node_pool_type
    temporary_name_for_rotation = "rotationpool"

    upgrade_settings {
      drain_timeout_in_minutes      = 0
      max_surge                     = "10%"
      node_soak_duration_in_minutes = 0
    }
  }

  identity {
    type = "SystemAssigned"
  }

  network_profile {
    network_plugin = "azure"
    network_policy = "azure"
  }
}
