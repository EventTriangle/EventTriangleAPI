param (
    [Parameter(Mandatory = $true)]
    [string]$DockerRegistryUrl,

    [Parameter(Mandatory = $true)]
    [string]$ImageRepository,

    [Parameter(Mandatory = $true)]
    [string]$AcrRegistryUrl,

    [Parameter(Mandatory = $true)]
    [string]$DockerfilePath,

    [Parameter(Mandatory = $true)]
    [string]$GitVersion,

    [Parameter(Mandatory = $true)]
    [string]$CommitSha,

    [Parameter(Mandatory = $true)]
    [string]$WorkingDirectory
)

$ErrorActionPreference = "Stop"

Write-Host "================================================================================"

docker --version

Write-Host "================================================================================"

# Variable to use modern Docker BuildKit
$env:DOCKER_BUILDKIT = "1"

$InitDirectory = Get-Location

Write-Output "Changing directory to $WorkingDirectory"
Set-Location $WorkingDirectory

# Define image tags
$GIT_VERSION_IMAGE = "$DockerRegistryUrl/$ImageRepository`:$GitVersion"
$LATEST_VERSION_IMAGE = "$DockerRegistryUrl/$ImageRepository`:latest"
$SHA_TAG = "$DockerRegistryUrl/$ImageRepository`:$GitVersion-$CommitSha"

$ACR_GIT_VERSION_IMAGE = "$AcrRegistryUrl/$ImageRepository`:$GitVersion"
$ACR_LATEST_VERSION_IMAGE = "$AcrRegistryUrl/$ImageRepository`:latest"
$ACR_SHA_TAG = "$AcrRegistryUrl/$ImageRepository`:$GitVersion-$CommitSha"

# Output image tags

Write-Host "================================================================================"

Write-Output "DOCKERHUB_GIT_VERSION_IMAGE: $GIT_VERSION_IMAGE"
Write-Output "DOCKERHUB_GIT_LATEST_VERSION_IMAGE: $LATEST_VERSION_IMAGE"
Write-Output "DOCKERHUB_SHA_VERSION_IMAGE: $SHA_TAG"

Write-Output "ACR_GIT_VERSION_IMAGE: $ACR_GIT_VERSION_IMAGE"
Write-Output "ACR_LATEST_VERSION_IMAGE: $ACR_LATEST_VERSION_IMAGE"
Write-Output "ACR_SHA_IMAGE: $ACR_SHA_TAG"

Write-Host "================================================================================"

# Try to pull cache image
docker pull "$LATEST_VERSION_IMAGE" 2>$null

$sw = [System.Diagnostics.Stopwatch]::StartNew()

# Build the Docker image
docker buildx build --load `
    --build-arg VERSION="$gitVersion" `
    --cache-from "type=registry,ref=$LATEST_VERSION_IMAGE" `
    --cache-to "type=inline" `
    -f "$DockerfilePath" .

$sw.Stop()
Write-Host "Docker build occupied: $($sw.Elapsed.TotalSeconds)"

# Tag the images
docker tag "$GIT_VERSION_IMAGE" "$LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_GIT_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$SHA_TAG"
docker tag "$GIT_VERSION_IMAGE" "$ACR_SHA_TAG"

# List images
docker image ls

Write-Host "Set location back to $InitDirectory..."

Set-Location $InitDirectory

# EXAMPLE CALL
#.\Build-Docker.ps1 `
#	-DockerRegistryUrl "docker.io/kaminome"`
#	-ImageRepository "auth-service" `
#	-AcrRegistryUrl "azuredevopsacrd01.azurecr.io" `
#	-DockerfilePath "E:\RiderProjects\02_DOTNET_PROJECTS\EventTriangleAPI\src\authorization\Dockerfile" `
#	-GitVersion "1.0.0" `
#	-CommitSha "8e33ce9" `
#	-WorkingDirectory "E:\RiderProjects\02_DOTNET_PROJECTS\EventTriangleAPI\src"
