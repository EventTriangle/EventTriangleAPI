param(
    [Parameter(Position = 0, Mandatory = $true)]
    [string] $HelmReleaseName,
    [Parameter(Position = 1, Mandatory = $true)]
    [string] $Namespace,
    [Parameter(Position = 2, Mandatory = $true)]
    [string] $RabbitMqUsername,
    [Parameter(Position = 3, Mandatory = $true)]
    [string] $RabbitMqPassword
)

$listReleases = $( helm list -n $Namespace )
$releaseExists = $listReleases -match $HelmReleaseName

if ($releaseExists)
{
    Write-Output "Release $HelmReleaseName already exists in namespace $Namespace. Skipping..."
    return
}

Write-Output "Release $HelmReleaseName does not exist in namespace $Namespace. Installing..."

helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm upgrade --install $HelmReleaseName bitnami/rabbitmq `
    --namespace $Namespace `
    --set auth.username=$RabbitMqUsername `
    --set auth.password=$RabbitMqPassword `
    --set service.type=LoadBalancer

# example call: 
# .\deploy-rabbitmq-helm.ps1 -HelmReleaseName "event-rabbitmq" -Namespace "event-triangle" -RabbitMqUsername "guest" -RabbitMqPassword "guest"