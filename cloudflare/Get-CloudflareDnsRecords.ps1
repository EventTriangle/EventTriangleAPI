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

# Create a hashtable to hold the records
$dnsRecords = @{}

# Loop through the result and populate the hashtable
$json.result | ForEach-Object {
    # Use the DNS record id as the key and the name as the value
    $dnsRecords[$_.id] = $_.name
}

# Return the hashtable
return $dnsRecords
