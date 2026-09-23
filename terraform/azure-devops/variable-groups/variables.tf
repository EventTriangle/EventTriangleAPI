variable "organization_url" {
  description = "Azure DevOps organization URL used by the provider."
  type        = string
  default     = "https://dev.azure.com/EventTriangle"
}

variable "project_id" {
  description = "Azure DevOps project ID that owns the variable groups."
  type        = string
  default     = "29327428-805a-440b-9d16-fcf0ac20edb2"
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

variable "cloudflare_api_token" {
  description = "Cloudflare API token consumed by the Cloudflare DNS pipeline."
  type        = string
  sensitive   = true
}
