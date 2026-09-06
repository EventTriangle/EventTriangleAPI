#!/bin/bash

set -euo pipefail

SCRIPT_FILE="$(realpath "${BASH_SOURCE[0]}")"
SCRIPT_DIR="$(dirname "$SCRIPT_FILE")"

echo "Script file      : $SCRIPT_FILE"
echo "Script directory : $SCRIPT_DIR"

WORKING_DIRECTORY="$(realpath "$SCRIPT_DIR/../src/consumer")"
SHARED_CONTEXT="$(realpath "$SCRIPT_DIR/../src/shared")"
DOCKER_FILE="$WORKING_DIRECTORY/Dockerfile"

SEM_VER="${1:-1.0.0-local}"

VERSION_TAG="acrsharedd01.azurecr.io/consumer-service:$SEM_VER"
LATEST_TAG="acrsharedd01.azurecr.io/consumer-service:latest"
CACHE_IMAGE_TAG="acrsharedd01.azurecr.io/consumer-service:buildcache"

$SCRIPT_DIR/docker-build.sh \
    --working-directory "$WORKING_DIRECTORY" \
    --docker-file "$DOCKER_FILE" \
    --version-tag "$VERSION_TAG" \
    --latest-tag "$LATEST_TAG" \
    --cache-image-tag "$CACHE_IMAGE_TAG" \
    --shared-context "$SHARED_CONTEXT"
