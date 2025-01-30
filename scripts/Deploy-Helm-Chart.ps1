param (
    [string]$helmChartsFolder = "E:\RiderProjects\02_DOTNET_PROJECTS\EventTriangleAPI\helm",
    [string]$chartName = "auth-service-chart",
    [string]$version,
    [string]$namespace = "event-triangle",
    [bool]$useAcr = $true,
    [string]$acrRegistryUrl = "azuredevopsacrd01.azurecr.io"
)

# Check if version is empty
if ( [string]::IsNullOrEmpty($version))
{
    Write-Output "Version is empty, setting up to latest"
    $version = "latest"
}

$chartMap = @{ }
$chartMap["auth-service-chart"] = "auth-service"
$chartMap["consumer-service-chart"] = "consumer-service"
$chartMap["sender-service-chart"] = "sender-service"

$image = $chartMap[$chartName]

Write-Output "Version: $version"
Write-Output "Deploying Helm chart..."

# Determine image repository
if ($useAcr)
{
    $imageRepository = "$acrRegistryUrl/$image"
}
else
{
    $imageRepository = "kaminome/$image"
}

# Execute Helm upgrade/install command
helm upgrade --install $chartName "$helmChartsFolder/$chartName" `
    --values "$helmChartsFolder/$chartName/values.yaml" `
    --set image.tag="$version" `
    --set image.repository="$imageRepository" `
    --namespace "$namespace"
