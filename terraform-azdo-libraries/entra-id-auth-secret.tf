resource "azuredevops_variable_group" "entra_id_auth_secret" {
  project_id   = var.project_id
  name         = "Entra_ID_Auth_Secret"
  description  = "Entra_ID_Auth_Secret"
  allow_access = true

  variable {
    name         = "library-entra-id-auth-secret"
    is_secret    = true
    secret_value = var.entra_id_auth_secret
  }
}
