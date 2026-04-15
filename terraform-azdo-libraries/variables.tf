variable "cloudflare_api_key" {
  sensitive = true
}

variable "azure_rm_client_secret" {
  sensitive = true
}

variable "entra_id_auth_secret" {
  sensitive = true
}

variable "postgres_password" {
  sensitive = true
}

variable "sas_token" {
  sensitive = true
}

variable "redis_password" {
  sensitive = true
}

variable "project_id" {
  default = "29327428-805a-440b-9d16-fcf0ac20edb2"
}
