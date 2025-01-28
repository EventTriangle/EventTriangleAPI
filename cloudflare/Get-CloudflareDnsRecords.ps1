param (
    [Parameter(Mandatory = $true)]
    [string]$ApiToken,

    [Parameter(Mandatory = $true)]
    [string]$ZoneId
)

$url = "https://api.cloudflare.com/client/v4/zones/$ZoneId/dns_records"

# Perform the API request
$response = $( curl $url -H "Authorization: Bearer $ApiToken" -H "Content-Type: application/json" )

# Parse the JSON response
$json = $response | ConvertFrom-Json

$dnsRecords = $json.result | ForEach-Object {
    [PSCustomObject]@{
        dnsRecordId = $_.id
        Name = $_.name
    }
}

return $dnsRecords
