param (
    [Parameter(Mandatory = $true)]
    [string]$DockerRegistryUrl,

    [Parameter(Mandatory = $true)]
    [string]$ImageRepository,

    [Parameter(Mandatory = $true)]
    [string]$AcrRegistryUrl,

    [Parameter(Mandatory = $true)]
    [string]$DockerBuildParameterUrl,

    [Parameter(Mandatory = $true)]
    [string]$DockerfilePath,

    [Parameter(Mandatory = $true)]
    [string]$GitVersion,

    [Parameter(Mandatory = $true)]
    [string]$WorkingDirectory
)

Write-Output "Changing directory to $WorkingDirectory"
Set-Location $WorkingDirectory

# Define image tags
$GIT_VERSION_IMAGE = "$DockerRegistryUrl/$ImageRepository`:$gitVersion"
$LATEST_VERSION_IMAGE = "$DockerRegistryUrl/$ImageRepository`:latest"
$ACR_GIT_VERSION_IMAGE = "$AcrRegistryUrl/$ImageRepository`:$gitVersion"
$ACR_LATEST_VERSION_IMAGE = "$AcrRegistryUrl/$ImageRepository`:latest"

# Output image tags
Write-Output "DOCKERHUB_GIT_VERSION_IMAGE: $GIT_VERSION_IMAGE"
Write-Output "DOCKERHUB_GIT_LATEST_VERSION_IMAGE: $LATEST_VERSION_IMAGE"
Write-Output "ACR_GIT_VERSION_IMAGE: $ACR_GIT_VERSION_IMAGE"
Write-Output "ACR_LATEST_VERSION_IMAGE: $ACR_LATEST_VERSION_IMAGE"

# Build the Docker image
docker build --build-arg FRONT_API_URL="$DockerBuildParameterUrl" `
             --build-arg VERSION="$gitVersion" `
             -t "$GIT_VERSION_IMAGE" `
             -f "$DockerfilePath" .

# Tag the images
docker tag "$GIT_VERSION_IMAGE" "$LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_GIT_VERSION_IMAGE"

# List images
docker image ls

# EXAMPLE CALL
#.\Build-Docker.ps1 `
#	-DockerRegistryUrl "docker.io/kaminome"`
#	-ImageRepository "auth-service" `
#	-AcrRegistryUrl "azuredevopsacrd01.azurecr.io" `
#	-DockerBuildParameterUrl "https://auth-eventtriangle.razumovsky.me/" `
#	-DockerfilePath "E:\RiderProjects\02_DOTNET_PROJECTS\EventTriangleAPI\src\authorization\Dockerfile" `
#	-GitVersion "1.0.0" `
#	-WorkingDirectory "E:\RiderProjects\02_DOTNET_PROJECTS\EventTriangleAPI\src"
