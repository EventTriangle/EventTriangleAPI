GIT_VERSION_IMAGE="${{ parameters.dockerRegistryUrl }}/${{ parameters.imageRepository }}:$(GitVersion.SemVer)"
LATEST_VERSION_IMAGE="${{ parameters.dockerRegistryUrl }}/${{ parameters.imageRepository }}:latest"
echo "GIT_VERSION_IMAGE: $GIT_VERSION_IMAGE"
echo "LATEST_VERSION_IMAGE: $LATEST_VERSION_IMAGE"

ACR_GIT_VERSION_IMAGE="${{ parameters.acrRegistryUrl }}/${{ parameters.imageRepository }}:$(GitVersion.SemVer)"
ACR_LATEST_VERSION_IMAGE="${{ parameters.acrRegistryUrl }}/${{ parameters.imageRepository }}:latest"
echo "ACR_GIT_VERSION_IMAGE: $ACR_GIT_VERSION_IMAGE"
echo "ACR_LATEST_VERSION_IMAGE: $ACR_LATEST_VERSION_IMAGE"

docker build --build-arg FRONT_API_URL="${{ parameters.dockerBuildParameterUrl }}" \
            --build-arg VERSION=$(GitVersion.SemVer) -t "$GIT_VERSION_IMAGE" \
            -f ${{ parameters.dockerfilePath }} .

docker tag "$GIT_VERSION_IMAGE" "$LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_LATEST_VERSION_IMAGE"
docker tag "$GIT_VERSION_IMAGE" "$ACR_GIT_VERSION_IMAGE"
