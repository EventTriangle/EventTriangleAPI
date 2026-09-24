output "dns_record" {
  description = "Managed application DNS record."
  value = {
    id      = cloudflare_dns_record.application.id
    name    = cloudflare_dns_record.application.name
    type    = cloudflare_dns_record.application.type
    content = cloudflare_dns_record.application.content
  }
}

output "application_url" {
  description = "HTTPS URL for the managed application hostname."
  value       = "https://${cloudflare_dns_record.application.name}"
}
