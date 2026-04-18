param(
    [Parameter(Mandatory = $true)]
    [string] $Namespace,
    [Parameter(Mandatory = $true)]
    [string] $HelmRelease,
    [Parameter(Mandatory = $true)]
    [string] $Password
)

$listReleases = $( helm list -n $Namespace )
$releaseExists = $listReleases -match $HelmRelease

if ($releaseExists)
{
    Write-Output "Release $HelmRelease already exists in namespace $Namespace. Skipping..."
    exit 0
}

Write-Host "Release $HelmRelease does not exist. Installing it ..."

helm repo add bitnami https://charts.bitnami.com/bitnami

helm repo update

helm upgrade --install $HelmRelease bitnami/redis `
--namespace $Namespace `
--set auth.enabled=true `
--set auth.password="$Password" `
--create-namespace

# .\redis\Install-Redis.ps1 -Namespace "redis-dev" -HelmRelease "redis-server-dev" -Password "qwerty12345"
