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

Write-Output "Helm repo add steps ..."
helm repo add jetstack https://charts.jetstack.io
helm repo update

Write-Output "Applying cert-manager custom resource definitions (CRDs) ..."
kubectl apply --validate=false -f https://github.com/jetstack/cert-manager/releases/latest/download/cert-manager.crds.yaml

Write-Output "Installing cert-manager helm chart ..."
helm upgrade --install $HelmReleaseName jetstack/cert-manager --namespace $Namespace --create-namespace

# example call: 
# .\cert-manager\Install-Cert-Manager.ps1 -HelmReleaseName "cert-manager" -Namespace "cert-manager-dev"
