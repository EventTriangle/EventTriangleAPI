resource "azuredevops_variable_group" "cloudflare_api_key" {
  project_id   = var.project_id
  name         = "Cloudflare_API_Key"
  description  = "Cloudflare API Key"
  allow_access = true

  variable {
    name         = "cloudflare-api-key"
    is_secret    = true
    secret_value = var.cloudflare_api_key
  }

  variable {
    name  = "cloudflare-zone-name"
    value = "razumovsky.me"
  }
}
