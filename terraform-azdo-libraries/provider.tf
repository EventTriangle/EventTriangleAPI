provider "azuredevops" {
  org_service_url = "https://dev.azure.com/EventTriangle"
  client_secret   = file("${path.module}/azdo-pat-token.txt")
}
