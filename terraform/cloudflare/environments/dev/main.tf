data "cloudflare_zone" "selected" {
  filter = {
    name = var.zone_name
  }
}

locals {
  normalized_hostnames = {
    for hostname in var.hostnames : hostname => trimsuffix(lower(hostname), ".")
  }

  fqdn_by_hostname = {
    for key, hostname in local.normalized_hostnames : key => (
      hostname == var.zone_name || endswith(hostname, ".${var.zone_name}")
      ? hostname
      : "${hostname}.${var.zone_name}"
    )
  }

  endpoint_is_ipv4 = can(cidrhost("${var.traefik_endpoint}/32", 0)) && !strcontains(var.traefik_endpoint, ":")
  endpoint_is_ipv6 = can(cidrhost("${var.traefik_endpoint}/128", 0)) && strcontains(var.traefik_endpoint, ":")
  detected_record_type = (
    local.endpoint_is_ipv4 ? "A" :
    local.endpoint_is_ipv6 ? "AAAA" :
    "CNAME"
  )
  effective_record_type = var.record_type == "AUTO" ? local.detected_record_type : var.record_type
  selected_zone_id      = data.cloudflare_zone.selected.id
}

resource "cloudflare_dns_record" "application" {
  for_each = local.fqdn_by_hostname

  zone_id = local.selected_zone_id
  name    = each.value
  type    = local.effective_record_type
  content = trimsuffix(var.traefik_endpoint, ".")
  ttl     = var.ttl
  proxied = var.proxied
  comment = "Managed by Terraform (${var.environment})"
}
