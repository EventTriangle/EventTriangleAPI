curl https://api.cloudflare.com/client/v4/zones -H "X-Auth-Email: $env:CLOUDFLARE_EMAIL" -H "X-Auth-Key: $env:CLOUDFLARE_API_KEY"

curl https://api.cloudflare.com/client/v4/zones -H "Authorization: Bearer $env:CLOUDFLARE_API_KEY" -H "Content-Type: application/json"

curl https://api.cloudflare.com/client/v4/user/tokens/verify -H "Authorization: Bearer $env:CLOUDFLARE_API_KEY" -H "Content-Type: application/json"

curl https://api.cloudflare.com/client/v4/zones/d8bdf4c7860b59eddfd9fcc7bf864b47/dns_records `
    -H "Authorization: Bearer $env:CLOUDFLARE_API_KEY" -H "Content-Type: application/json"
