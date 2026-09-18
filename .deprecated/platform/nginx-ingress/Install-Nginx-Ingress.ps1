param(
    [Parameter(Position = 0, Mandatory = $true)]
    [string] $HelmReleaseName,
    [Parameter(Position = 1, Mandatory = $true)]
    [string] $Namespace
)

$listReleases = $( helm list -n $Namespace )
$releaseExists = $listReleases -match $HelmReleaseName

if ($releaseExists)
{
    Write-Output "Release $HelmReleaseName already exists in namespace $Namespace. Skipping..."
    return
}

Write-Output "Release $HelmReleaseName does not exist in namespace $Namespace. Installing..."

helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm upgrade --install $HelmReleaseName ingress-nginx `
    --repo https://kubernetes.github.io/ingress-nginx `
    --namespace $Namespace `
    --set controller.service.externalTrafficPolicy=Local `
    --create-namespace

# example call: 
# .\nginx-ingress\Install-Nginx-Ingress.ps1 -HelmReleaseName "nginx-ingress-dev" -Namespace "ing-dev"
