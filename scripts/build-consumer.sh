#!/bin/sh

set -eu

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "Script directory: $SCRIPT_DIR"

WORKING_DIRECTORY="$(realpath "$SCRIPT_DIR/../src/consumer")"
SHARED_CONTEXT="$(realpath "$SCRIPT_DIR/../src/shared")"
DOCKER_FILE="$WORKING_DIRECTORY/Dockerfile"

SEM_VER="${1:-1.0.0-local}"

VERSION_TAG="acrsharedd01.azurecr.io/consumer-service:$SEM_VER"
LATEST_TAG="acrsharedd01.azurecr.io/consumer-service:latest"
CACHE_IMAGE_TAG="acrsharedd01.azurecr.io/consumer-service:buildcache"

./docker-build.sh \
    --working-directory "$WORKING_DIRECTORY" \
    --docker-file "$DOCKER_FILE" \
    --version-tag "$VERSION_TAG" \
    --latest-tag "$LATEST_TAG" \
    --cache-image-tag "$CACHE_IMAGE_TAG" \
    --shared-context "$SHARED_CONTEXT"
