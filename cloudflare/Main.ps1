param (
    [Parameter(Mandatory = $true)]
    [string]$ApiToken,

    [Parameter(Mandatory = $true)]
    [string]$ZoneName
)

# Set error handling and preferences
$ErrorActionPreference = "Stop"

Write-Host "Starting Cloudflare DNS Records Update Script..."

# Step 1: Get Zone ID
Write-Host "Fetching Zone ID for Zone Name: $ZoneName..."
$zoneId = $(./Get-CloudflareZoneId.ps1 -ApiToken $ApiToken -ZoneName $ZoneName)

if (-not $zoneId) {
    Write-Error "Failed to fetch Zone ID. Exiting script."
    exit 1
}

Write-Host "Zone ID Retrieved: $zoneId"

# Step 2: Get DNS Records
Write-Host "Fetching existing DNS records for Zone ID: $zoneId..."
$dnsRecords = $(.\Get-CloudflareDnsRecords.ps1 -ApiToken $ApiToken -ZoneId "$zoneId")

if (-not $dnsRecords -or -not ($dnsRecords -is [hashtable])) {
    Write-Error "Failed to fetch DNS records or records are not in the expected format. Exiting script."
    exit 1
}

Write-Host "DNS Records Retrieved: $($dnsRecords.Count) records found."

# Step 3: Get New DNS Entries
Write-Host "Fetching new DNS entries to update..."
$newDnsEntries = $(.\Get-NewDnsEntries.ps1)

if (-not $newDnsEntries -or -not ($newDnsEntries -is [hashtable])) {
    Write-Error "Failed to fetch new DNS entries or entries are not in the expected format. Exiting script."
    exit 1
}

Write-Host "New DNS Entries Retrieved: $($newDnsEntries.Count) entries to process."

# Step 4: Process Each New DNS Entry
Write-Host "Starting to process new DNS entries..."
foreach ($entry in $newDnsEntries.GetEnumerator()) {
    $dnsName = $entry.Name
    $ipAddress = $entry.Value

    Write-Host "`nProcessing Entry: $dnsName => $ipAddress"

    # Check if the DNS name exists in the current DNS records
    if ($dnsRecords.ContainsKey($dnsName)) {
        # Get the record ID for the existing DNS record
        $recordId = $dnsRecords[$dnsName]

        Write-Host "Found existing DNS record for $dnsName. Record ID: $recordId"
        Write-Host "Updating DNS record for $dnsName with IP Address: $ipAddress"

        # Update the DNS record
        try {
            .\Update-CloudflareDnsRecord.ps1 -ApiToken $ApiToken `
                -DnsName $dnsName `
                -ZoneId $zoneId `
                -RecordId $recordId `
                -IpAddress $ipAddress

            Write-Host "Successfully updated DNS record for $dnsName => $ipAddress" -ForegroundColor Green
        } catch {
            Write-Error "Failed to update DNS record for $dnsName. Error: $_"
        }
    } else {
        Write-Host "DNS name $dnsName does not exist in Cloudflare. Skipping..." -ForegroundColor Red
    }
}

# Final Step: Script Completion
Write-Host "`nDNS records update process completed successfully!" -ForegroundColor Green
