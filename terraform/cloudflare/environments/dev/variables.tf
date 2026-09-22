variable "environment" {
  description = "Environment name included in record metadata."
  type        = string
  default     = "dev"
}

variable "zone_name" {
  description = "Existing Cloudflare DNS zone to discover by name."
  type        = string
  default     = "razumovsky.me"

  validation {
    condition     = var.zone_name == lower(trimsuffix(var.zone_name, ".")) && !strcontains(var.zone_name, "/")
    error_message = "zone_name must be a lowercase DNS zone without a trailing dot or path."
  }
}

variable "hostnames" {
  description = "Relative or fully-qualified application hostnames that point to Traefik."
  type        = set(string)
  default     = ["auth-eventtriangle"]

  validation {
    condition = length(var.hostnames) > 0 && alltrue([
      for hostname in var.hostnames :
      hostname != "" &&
      !strcontains(hostname, "://") &&
      !strcontains(hostname, "/") &&
      !strcontains(hostname, " ")
    ])
    error_message = "Each hostname must be a non-empty relative name or FQDN without a scheme, path, or whitespace."
  }
}

variable "traefik_endpoint" {
  description = "External IPv4, IPv6, or DNS hostname published by the Traefik LoadBalancer."
  type        = string

  validation {
    condition = (
      var.traefik_endpoint != "" &&
      !strcontains(var.traefik_endpoint, "://") &&
      !strcontains(var.traefik_endpoint, "/") &&
      !strcontains(var.traefik_endpoint, " ") &&
      (
        can(cidrhost("${var.traefik_endpoint}/32", 0)) ||
        can(cidrhost("${var.traefik_endpoint}/128", 0)) ||
        can(regex("^([A-Za-z0-9]([A-Za-z0-9-]*[A-Za-z0-9])?\\.)+[A-Za-z]([A-Za-z0-9-]*[A-Za-z0-9])?\\.?$", var.traefik_endpoint))
      )
    )
    error_message = "traefik_endpoint must be an IPv4 address, IPv6 address, or DNS hostname without a scheme or path."
  }

  validation {
    condition = (
      var.record_type == "AUTO" ||
      (var.record_type == "A" && can(cidrhost("${var.traefik_endpoint}/32", 0)) && !strcontains(var.traefik_endpoint, ":")) ||
      (var.record_type == "AAAA" && can(cidrhost("${var.traefik_endpoint}/128", 0)) && strcontains(var.traefik_endpoint, ":")) ||
      (var.record_type == "CNAME" && can(regex("^([A-Za-z0-9]([A-Za-z0-9-]*[A-Za-z0-9])?\\.)+[A-Za-z]([A-Za-z0-9-]*[A-Za-z0-9])?\\.?$", var.traefik_endpoint)))
    )
    error_message = "record_type must match the supplied Traefik endpoint."
  }
}

variable "record_type" {
  description = "DNS type to create. AUTO infers A, AAAA, or CNAME from traefik_endpoint."
  type        = string
  default     = "AUTO"

  validation {
    condition     = contains(["AUTO", "A", "AAAA", "CNAME"], var.record_type)
    error_message = "record_type must be AUTO, A, AAAA, or CNAME."
  }
}

variable "ttl" {
  description = "Cloudflare DNS TTL in seconds; 1 means automatic."
  type        = number
  default     = 1

  validation {
    condition     = var.ttl == 1 || (var.ttl >= 60 && var.ttl <= 86400)
    error_message = "ttl must be 1 (automatic) or between 60 and 86400 seconds."
  }

  validation {
    condition     = !var.proxied || var.ttl == 1
    error_message = "ttl must be 1 (automatic) when proxied is enabled."
  }
}

variable "proxied" {
  description = "Whether Cloudflare proxies traffic instead of serving DNS only."
  type        = bool
  default     = false
}
