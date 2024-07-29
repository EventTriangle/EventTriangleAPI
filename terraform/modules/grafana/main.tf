resource "azurerm_dashboard_grafana" "grafana" {
  name                              = var.grafana_name
  resource_group_name               = var.resource_group_name
  location                          = var.resource_group_location
  api_key_enabled                   = true
  deterministic_outbound_ip_enabled = true
  public_network_access_enabled     = true
  sku                               = "Standard"
  zone_redundancy_enabled           = true

  azure_monitor_workspace_integrations {
    resource_id = var.prometheus_id
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_role_assignment" "role_grafana_admin" {
  scope                = azurerm_dashboard_grafana.grafana.id
  role_definition_name = "Grafana Admin"
  principal_id         = var.grafana_admin_object_id
}

resource "azurerm_role_assignment" "role_monitoring_data_reader" {
  scope                = var.prometheus_id
  role_definition_name = "Monitoring Data Reader"
  principal_id         = azurerm_dashboard_grafana.grafana.identity.0.principal_id
}
