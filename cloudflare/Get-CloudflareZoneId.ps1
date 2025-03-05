param (
    [Parameter(Mandatory = $true)]
    [string]$ApiToken,

    [Parameter(Mandatory = $true)]
    [string]$ZoneName
)

$ErrorActionPreference = "Stop"

$url = "https://api.cloudflare.com/client/v4/zones"

# Perform the API request
$response = $( curl $url -H "Authorization: Bearer $ApiToken" -H "Content-Type: application/json" )

# Parse the JSON response
$json = $response | ConvertFrom-Json

# Filter the result for the zone named 'razumovsky.me'
$zone = $json.result | Where-Object { $_.name -eq "$ZoneName" }

# Output the Zone ID
if ($zone)
{
    return $zone.id
}
else
{
    Write-Output "Zone '$ZoneName' not found."
}



