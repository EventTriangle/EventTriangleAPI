#!/bin/sh

set -eu

# Parameters (used only if the corresponding env var is not set)
WORKING_DIRECTORY="${WORKING_DIRECTORY:-${1:-}}"
VERSION_TAG="${VERSION_TAG:-${2:-}}"
LATEST_TAG="${VERSION_TAG:-${2:-}}"
CACHE_IMAGE_TAG="${CACHE_IMAGE:-${3:-}}"

if [[ -z "$WORKING_DIRECTORY" || -z "$VERSION_TAG" ]]; then
    echo "Usage:"
    echo "  $0 <working_directory> <version_tag> [http_proxy] [http_host] [https_proxy] [https_host] [no_proxy] [cache_image]"
    exit 1
fi

echo "Building image: ${VERSION_TAG}"

docker buildx build \
    --progress=plain \
    --cache-to type=inline \
    --cache-from type=registry,ref="${CACHE_IMAGE}" \
    --tag "${VERSION_TAG}" \
    --tag "${CACHE_IMAGE}" \
    --file "${WORKING_DIRECTORY}/Dockerfile" \
    --push \
    "${WORKING_DIRECTORY}"
