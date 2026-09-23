moved {
  from = cloudflare_dns_record.application["auth-eventtriangle"]
  to   = cloudflare_dns_record.application
}

resource "cloudflare_dns_record" "application" {
  zone_id = var.cloudflare_zone_id
  name    = "auth-eventtriangle.razumovsky.me"
  type    = "A"
  content = var.traefik_public_ip
  ttl     = 1
  proxied = false
}
