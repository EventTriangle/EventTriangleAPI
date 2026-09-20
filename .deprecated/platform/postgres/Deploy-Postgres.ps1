param(
    [Parameter(Position = 0, Mandatory = $true)]
    [string] $HelmReleaseName,
    [Parameter(Position = 1, Mandatory = $true)]
    [string] $Namespace
)

# Set error handling and preferences
$ErrorActionPreference = "Stop"

Write-Host "Changing directory to PSScriptRoot ..."

Set-Location $PSScriptRoot

$listReleases = $( helm list -n $Namespace )
$releaseExists = $listReleases -match $HelmReleaseName

if ($releaseExists)
{
    Write-Output "Release $HelmReleaseName already exists in namespace $Namespace. Skipping..."
    return
}

Write-Output "Release $HelmReleaseName does not exist in namespace $Namespace. Installing..."

helm upgrade --install pgsql-dev .\pgsql-helm\ --values .\pgsql-helm\values.yaml --namespace pgsql-dev --create-namespace

# .\Deploy-Postgres.ps1 -HelmReleaseName "pgsql-dev" -Namespace "pgsql-dev"
