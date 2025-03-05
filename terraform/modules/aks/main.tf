resource "azurerm_kubernetes_cluster" "aks" {
  name                = var.aks_name
  kubernetes_version  = var.kubernetes_version
  location            = var.resource_group_location
  resource_group_name = var.resource_group_name
  dns_prefix          = var.aks_name

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

  dynamic "oms_agent" {
    for_each = var.should_deploy_log_analytics ? [1] : []
    content {
      log_analytics_workspace_id      = var.log_analytics_workspace_id
      msi_auth_for_monitoring_enabled = true
    }
  }

  lifecycle {
    ignore_changes = [
      monitor_metrics
    ]
  }
}
