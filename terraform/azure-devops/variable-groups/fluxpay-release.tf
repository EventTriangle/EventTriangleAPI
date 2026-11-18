variable "fluxpay_ghcr_user" {
  type        = string
  description = "GitHub account that owns the token used to publish FluxPay Helm charts."
}

variable "fluxpay_ghcr_token" {
  type        = string
  sensitive   = true
  description = "Token with write access to FluxPay OCI chart packages."
}

variable "fluxpay_gitops_repository" {
  type        = string
  description = "HTTPS URL of the GitHub fluxcd-repository used by Flux."
  default     = "https://github.com/FluxPayProject/fluxcd-repository.git"
}

variable "fluxpay_gitops_token" {
  type        = string
  sensitive   = true
  description = "GitHub credential with contents write access to the GitOps repository."
}

resource "azuredevops_variable_group" "fluxpay_ghcr_publisher" {
  project_id   = var.project_id
  name         = "FluxPay_GHCR_Publisher"
  description  = "Credentials for publishing FluxPay Helm charts to GHCR."
  allow_access = false

  variable {
    name  = "ghcr-user"
    value = var.fluxpay_ghcr_user
  }

  variable {
    name         = "ghcr-token"
    is_secret    = true
    secret_value = var.fluxpay_ghcr_token
  }
}

resource "azuredevops_variable_group" "fluxpay_gitops_writer" {
  project_id   = var.project_id
  name         = "FluxPay_GitOps_Writer"
  description  = "Credentials for committing published versions to the GitOps repository."
  allow_access = false

  variable {
    name  = "gitops-repository"
    value = var.fluxpay_gitops_repository
  }

  variable {
    name         = "gitops-token"
    is_secret    = true
    secret_value = var.fluxpay_gitops_token
  }
}
