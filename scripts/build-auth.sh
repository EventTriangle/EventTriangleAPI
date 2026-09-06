#!/bin/sh

set -eu

WORKING_DIRECTORY="$(realpath "../src/authorization")"
DOCKER_FILE="$WORKING_DIRECTORY/Dockerfile"
VERSION_TAG="acrsharedd01.azurecr.io/auth-service:1.0.0-local"
LATEST_TAG="acrsharedd01.azurecr.io/auth-service:latest"
CACHE_IMAGE_TAG="acrsharedd01.azurecr.io/auth-service:buildcache"

./docker-build.sh \
    --working-directory "$WORKING_DIRECTORY" \
    --docker-file "$DOCKER_FILE" \
    --version-tag "$VERSION_TAG" \
    --latest-tag "$LATEST_TAG" \
    --cache-image-tag "$CACHE_IMAGE_TAG"
