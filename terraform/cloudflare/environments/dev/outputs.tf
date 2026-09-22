output "zone_id" {
  description = "Cloudflare zone ID discovered from zone_name."
  value       = local.selected_zone_id
}

output "dns_records" {
  description = "Managed application DNS records and their Traefik target."
  value = {
    for key, record in cloudflare_dns_record.application : key => {
      id      = record.id
      name    = record.name
      type    = record.type
      content = record.content
      ttl     = record.ttl
      proxied = record.proxied
    }
  }
}

output "application_urls" {
  description = "HTTPS URLs for the managed application hostnames."
  value       = [for hostname in values(local.fqdn_by_hostname) : "https://${hostname}"]
}
