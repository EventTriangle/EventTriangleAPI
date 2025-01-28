# resource "azurerm_container_registry" "acr" {
#   name                = var.acr_name
#   resource_group_name = var.resource_group_name
#   location            = var.resource_group_location
#   sku                 = "Standard"
#   admin_enabled       = true
# }

data "azurerm_container_registry" "acr" {
  name                = var.acr_name
  resource_group_name = var.resource_group_name
}

resource "azurerm_role_assignment" "role_acrpull" {
  scope                            = data.azurerm_container_registry.acr.id
  role_definition_name             = "AcrPull"
  principal_id                     = var.aks_identity_principal_id
  skip_service_principal_aad_check = true

  depends_on = [data.azurerm_container_registry.acr]
}
