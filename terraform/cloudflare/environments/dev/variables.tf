variable "cloudflare_zone_id" {
  description = "Existing Cloudflare zone ID for razumovsky.me."
  type        = string
  default     = "d8bdf4c7860b59eddfd9fcc7bf864b47"
}

variable "traefik_public_ip" {
  description = "Public IPv4 address exposed by the Traefik LoadBalancer."
  type        = string
  default     = "10.10.190.1"

  validation {
    condition     = can(cidrhost("${var.traefik_public_ip}/32", 0)) && !strcontains(var.traefik_public_ip, ":")
    error_message = "traefik_public_ip must be a valid IPv4 address."
  }
}
