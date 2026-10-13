resource "azuredevops_serviceendpoint_github" "github" {
  project_id            = var.project_id
  service_endpoint_name = "EventTriangle"

  description = ""
}

resource "azuredevops_serviceendpoint_dockerregistry" "docker_hub" {
  project_id            = var.project_id
  service_endpoint_name = "Docker_Hub_Connection"
  description           = ""
  registry_type         = "DockerHub"
  docker_registry       = "https://index.docker.io/v1/"
  docker_username       = var.docker_hub_username
  docker_password       = var.docker_hub_token
}

resource "azuredevops_serviceendpoint_dockerregistry" "azure_acr" {
  project_id            = var.project_id
  service_endpoint_name = "Azure_ACR_Connection"
  description           = "Service connection to push images to ACR"
  registry_type         = "Others"
  docker_registry       = var.azure_acr_registry_url
  docker_username       = var.azure_acr_username
  docker_email          = var.azure_acr_email
  docker_password       = var.azure_acr_password
}

resource "azuredevops_serviceendpoint_sonarcloud" "sonarcloud" {
  project_id            = var.project_id
  service_endpoint_name = "SonarCloud_Service_Connection"
  description           = ""
  token                 = var.sonarcloud_token
}

resource "azuredevops_serviceendpoint_azurerm" "azure" {
  project_id                             = var.project_id
  service_endpoint_name                  = "Azure_Connection"
  description                            = ""
  service_endpoint_authentication_scheme = "ServicePrincipal"
  azurerm_spn_tenantid                   = var.azure_tenant_id
  azurerm_subscription_id                = var.azure_subscription_id
  azurerm_subscription_name              = var.azure_subscription_name

  credentials {
    serviceprincipalid  = var.azure_client_id
    serviceprincipalkey = var.azure_client_secret
  }
}
