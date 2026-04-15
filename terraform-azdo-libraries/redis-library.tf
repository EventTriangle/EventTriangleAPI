resource "azuredevops_variable_group" "redis_secrets" {
  project_id   = var.project_id
  name         = "Redis_Credentials"
  description  = "Redis_Credentials"
  allow_access = true

  variable {
    name         = "library-redis-password"
    is_secret    = true
    secret_value = var.redis_password
  }

  variable {
    name  = "library-redis-url"
    value = ""
  }
}
