resource "azuredevops_variable_group" "postgres_settings" {
  project_id   = var.project_id
  name         = "Postgres_Settings"
  description  = "Postgres_Settings"
  allow_access = true

  variable {
    name  = "POSTGRES_DB"
    value = "postgres"
  }

  variable {
    name         = "POSTGRES_PASSWORD"
    is_secret    = true
    secret_value = var.postgres_password
  }

  variable {
    name  = "POSTGRES_USER"
    value = "postgres"
  }
}
