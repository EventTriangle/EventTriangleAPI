param (
    [Parameter(Mandatory = $true)]
    [string]$ApiToken,

    [Parameter(Mandatory = $true)]
    [string]$DnsName,

    [Parameter(Mandatory = $true)]
    [string]$ZoneId,

    [Parameter(Mandatory = $true)]
    [string]$RecordId,

    [Parameter(Mandatory = $true)]
    [string]$IpAddress
)

$url = "https://api.cloudflare.com/client/v4/zones/$ZoneId/dns_records/$RecordId"

$body = @{
    comment = "Sent from Powershell $( $( Get-Date ).DateTime )"
    content = $IpAddress
    name = $DnsName
    proxied = $false
    settings = @{
        ipv4_only = $false
        ipv6_only = $false
    }
    ttl = 1
    type = "A"
} | ConvertTo-Json -Depth 4

# Perform the API request
$response = curl $url `
    -X PATCH `
    -H "Authorization: Bearer $ApiToken" `
    -H "Content-Type: application/json" `
    -d $body

$responseJson = $response | ConvertFrom-Json

# Check the response
if ($responseJson.success -eq $true)
{
    Write-Host "DNS record updated successfully."
    Write-Host "Response: $response"
}
else
{
    Write-Host "Failed to update DNS record."
    Write-Host "Response: $( $response )"
}

#.\Update-CloudflareDnsRecord.ps1 -ApiToken $env:CLOUDFLARE_API_KEY `
#	-DnsName "auth-eventtriangle.razumovsky.me" `
#	-ZoneId $zoneId `
#	-RecordId "98b014141c8d4bae0db9800617c04076" `
#	-IpAddress "172.205.36.169"
