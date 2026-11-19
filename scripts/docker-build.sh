#!/bin/bash

set -euo pipefail

usage() {
    echo "Usage:"
    echo "  $0 \\"
    echo "    --working-directory <path> \\"
    echo "    --docker-file <path> \\"
    echo "    --version-tag <tag> \\"
    echo "    --latest-tag <tag> \\"
    echo "    --cache-image-tag <tag> \\"
    echo "    --shared-context <path>"
}

# ==============================================================================
# Parameters
# ==============================================================================

WORKING_DIRECTORY=""
DOCKER_FILE=""
VERSION_TAG=""
LATEST_TAG=""
CACHE_IMAGE_TAG=""
SHARED_CONTEXT=""

while [ "$#" -gt 0 ]; do
    case "$1" in
        --working-directory)
            WORKING_DIRECTORY="${2:-}"
            shift 2
            ;;
        --docker-file)
            DOCKER_FILE="${2:-}"
            shift 2
            ;;
        --version-tag)
            VERSION_TAG="${2:-}"
            shift 2
            ;;
        --latest-tag)
            LATEST_TAG="${2:-}"
            shift 2
            ;;
        --cache-image-tag)
            CACHE_IMAGE_TAG="${2:-}"
            shift 2
            ;;
        --shared-context)
            SHARED_CONTEXT="${2:-}"
            shift 2
            ;;
        --help|-h)
            usage
            exit 0
            ;;
        *)
            echo "[ERROR] Unknown parameter: $1"
            echo
            usage
            exit 1
            ;;
    esac
done

# ==============================================================================
# Validation
# ==============================================================================

if [ -z "$WORKING_DIRECTORY" ]; then
    echo "[ERROR] --working-directory must not be empty."
    exit 1
fi

if [ -z "$DOCKER_FILE" ]; then
    echo "[ERROR] --docker-file must not be empty."
    exit 1
fi

if [ -z "$VERSION_TAG" ]; then
    echo "[ERROR] --version-tag must not be empty."
    exit 1
fi

if [ -z "$LATEST_TAG" ]; then
    echo "[ERROR] --latest-tag must not be empty."
    exit 1
fi

if [ -z "$CACHE_IMAGE_TAG" ]; then
    echo "[ERROR] --cache-image-tag must not be empty."
    exit 1
fi

if [ ! -d "$WORKING_DIRECTORY" ]; then
    echo "[ERROR] Working directory does not exist: $WORKING_DIRECTORY"
    exit 1
fi

if [ ! -f "$DOCKER_FILE" ]; then
    echo "[ERROR] Dockerfile does not exist: $DOCKER_FILE"
    exit 1
fi

if [ -z "$SHARED_CONTEXT" ]; then
    echo "[ERROR] --shared-context must not be empty."
    exit 1
fi

if [ ! -d "$SHARED_CONTEXT" ]; then
    echo "[ERROR] Shared context directory does not exist: $SHARED_CONTEXT"
    exit 1
fi

# ==============================================================================
# Configuration
# ==============================================================================

DEFAULT_CACHE_IMAGE_TAG="$CACHE_IMAGE_TAG"
if [[ -n "${DOCKER_CACHE_TAG:-}" ]]; then
    CACHE_IMAGE_TAG="${CACHE_IMAGE_TAG%:*}:$DOCKER_CACHE_TAG"
fi

echo "============================================================"
echo "Docker Build Configuration"
echo "============================================================"
echo "Working directory : $WORKING_DIRECTORY"
echo "Dockerfile        : $DOCKER_FILE"
echo "Version tag       : $VERSION_TAG"
echo "Latest tag        : $LATEST_TAG"
echo "Cache image tag   : $CACHE_IMAGE_TAG"
echo "Shared context    : $SHARED_CONTEXT"
echo "DOCKER_BUILDKIT   : ${DOCKER_BUILDKIT:-<not set>}"
echo "============================================================"

# ==============================================================================
# Build
# ==============================================================================

echo "Building image: $VERSION_TAG"

build_args=(--tag "$VERSION_TAG" --cache-from "type=registry,ref=$CACHE_IMAGE_TAG")
if [[ "$CACHE_IMAGE_TAG" != "$DEFAULT_CACHE_IMAGE_TAG" ]]; then
    build_args+=(--cache-from "type=registry,ref=$DEFAULT_CACHE_IMAGE_TAG")
fi
if [[ "${DOCKER_PUBLISH_LATEST:-true}" == true ]]; then
    build_args+=(--tag "$LATEST_TAG")
fi
if [[ -n "${DOCKER_METADATA_FILE:-}" ]]; then
    build_args+=(--metadata-file "$DOCKER_METADATA_FILE")
fi

# Reuse the daemon's BuildKit when it supports registry cache exports.
if [[ "$(docker info --format '{{json .DriverStatus}}')" == *io.containerd.snapshotter.v1* ]]; then
    build_args+=(--builder "$(docker context show)")
else
    builder="$(docker buildx create --driver docker-container)"
    trap 'docker buildx rm "$builder" >/dev/null' EXIT
    build_args+=(--builder "$builder")
fi

docker buildx build \
    --progress=plain \
    --cache-to "type=registry,ref=$CACHE_IMAGE_TAG,mode=max" \
    --build-context shared="${SHARED_CONTEXT}" \
    "${build_args[@]}" \
    --file "${DOCKER_FILE}" \
    --push \
    "${WORKING_DIRECTORY}"

echo "Build completed successfully."
