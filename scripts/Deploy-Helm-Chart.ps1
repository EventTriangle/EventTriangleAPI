param (
    [string]$HelmChartsFolder = "E:\RiderProjects\02_DOTNET_PROJECTS\EventTriangleAPI\helm",
    [string]$ChartName = "auth-service-chart",
    [string]$Version,
    [string]$Namespace = "event-triangle",
    [bool]$UseAcr = $true,
    [string]$AcrRegistryUrl = "azuredevopsacrd01.azurecr.io"
)

# Check if version is empty
if ([string]::IsNullOrEmpty($Version)) {
    Write-Output "Version is empty, setting up to latest"
    $Version = "latest"
}

$ChartMap = @{ }
$ChartMap["auth-service-chart"] = "auth-service"
$ChartMap["consumer-service-chart"] = "consumer-service"
$ChartMap["sender-service-chart"] = "sender-service"

$Image = $ChartMap[$ChartName]

Write-Output "Version: $Version"
Write-Output "Deploying Helm chart..."

# Determine image repository
if ($UseAcr) {
    $ImageRepository = "$AcrRegistryUrl/$Image"
} else {
    $ImageRepository = "kaminome/$Image"
}

Write-Output "Image repository: $ImageRepository"

# Execute Helm upgrade/install command
helm upgrade --install $ChartName "$HelmChartsFolder/$ChartName" `
    --values "$HelmChartsFolder/$ChartName/values.yaml" `
    --set image.tag="$Version" `
    --set image.repository="$ImageRepository" `
    --namespace "$Namespace"
