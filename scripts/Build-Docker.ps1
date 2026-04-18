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
    [string]$CommitSha,

    [Parameter(Mandatory = $true)]
    [string]$WorkingDirectory
)

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
Write-Output "DOCKERHUB_GIT_VERSION_IMAGE: $GIT_VERSION_IMAGE"
Write-Output "DOCKERHUB_GIT_LATEST_VERSION_IMAGE: $LATEST_VERSION_IMAGE"
Write-Output "DOCKERHUB_SHA_VERSION_IMAGE: $SHA_TAG"

Write-Output "ACR_GIT_VERSION_IMAGE: $ACR_GIT_VERSION_IMAGE"
Write-Output "ACR_LATEST_VERSION_IMAGE: $ACR_LATEST_VERSION_IMAGE"
Write-Output "ACR_SHA_IMAGE: $ACR_SHA_TAG"

# Build the Docker image
docker build --build-arg FRONT_API_URL="$DockerBuildParameterUrl" `
             --build-arg VERSION="$gitVersion" `
             -t "$GIT_VERSION_IMAGE" `
             -f "$DockerfilePath" .

# Tag the images
docker tag "$GIT_VERSION_IMAGE" "$LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_GIT_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$SHA_TAG"
docker tag "$GIT_VERSION_IMAGE" "$ACR_SHA_TAG"

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
