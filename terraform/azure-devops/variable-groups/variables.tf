variable "organization_url" {
  description = "Azure DevOps organization URL used by the provider."
  type        = string
  default     = "https://dev.azure.com/EventTriangle"
}

variable "project_id" {
  description = "Azure DevOps project ID that owns the managed resources."
  type        = string
  default     = "29327428-805a-440b-9d16-fcf0ac20edb2"
}

variable "github_repository_id" {
  description = "GitHub repository in OWNER/REPOSITORY form used by all YAML pipelines."
  type        = string
  default     = "EventTriangle/EventTriangleAPI"
}

variable "pipeline_default_branch" {
  description = "Fully qualified default branch stored on Azure DevOps pipeline definitions."
  type        = string
  default     = "refs/heads/main"

  validation {
    condition     = startswith(var.pipeline_default_branch, "refs/heads/")
    error_message = "pipeline_default_branch must use the refs/heads/<branch> form."
  }
}

variable "backend_storage_account_name" {
  description = "Azure Storage account used by the infrastructure Terraform backend."
  type        = string
  default     = "tfstatestorage011"
}

variable "backend_container_name" {
  description = "Azure Blob container used by the infrastructure Terraform backend."
  type        = string
  default     = "tfstatecontainer01"
}

variable "infrastructure_state_key" {
  description = "Blob key used by the Azure infrastructure Terraform root."
  type        = string
  default     = "azure.tfstate"
}

variable "backend_sas_token" {
  description = "SAS token exposed to infrastructure pipelines as a protected variable."
  type        = string
  sensitive   = true
}

variable "azure_client_id" {
  description = "Azure service-principal client ID consumed by infrastructure pipelines."
  type        = string
  default     = "ab0a5dc1-ee52-4574-96e0-469f237928a6"
}

variable "azure_client_secret" {
  description = "Azure service-principal client secret consumed by infrastructure pipelines."
  type        = string
  sensitive   = true
}

variable "azure_subscription_id" {
  description = "Azure subscription ID consumed by infrastructure pipelines."
  type        = string
  default     = "1b08b9a2-ac6d-4b86-8a2f-8fef552c8371"
}

variable "azure_tenant_id" {
  description = "Microsoft Entra tenant ID consumed by infrastructure pipelines."
  type        = string
  default     = "b40a105f-0643-4922-8e60-10fc1abf9c4b"
}

variable "azure_subscription_name" {
  description = "Azure subscription name used by the Azure Resource Manager service connection."
  type        = string
  default     = "Mitra"
}

variable "docker_hub_username" {
  description = "Docker Hub account used by the existing Docker Hub service connection."
  type        = string
  default     = "petrokolosov"
}

variable "docker_hub_token" {
  description = "Docker Hub access token supplied through TF_VAR_docker_hub_token."
  type        = string
  sensitive   = true
}

variable "azure_acr_registry_url" {
  description = "Azure Container Registry URL used by Azure_ACR_Connection."
  type        = string
  default     = "https://acrsharedd01.azurecr.io"
}

variable "azure_acr_username" {
  description = "Registry username used by the existing Azure ACR service connection."
  type        = string
  default     = "acrsharedd01"
}

variable "azure_acr_email" {
  description = "Email stored on the existing Azure ACR service connection."
  type        = string
  default     = "kolosovp94@gmail.com"
}

variable "azure_acr_password" {
  description = "Registry password supplied through TF_VAR_azure_acr_password."
  type        = string
  sensitive   = true
}

variable "manage_sonarcloud" {
  description = "Import and manage SonarCloud once a working token is available."
  type        = bool
  default     = false
}

variable "sonarcloud_token" {
  description = "SonarCloud token supplied through TF_VAR_sonarcloud_token."
  type        = string
  sensitive   = true
  default     = null
}

variable "cloudflare_api_key" {
  description = "Cloudflare API key consumed by the Cloudflare DNS pipeline."
  type        = string
  sensitive   = true
}
