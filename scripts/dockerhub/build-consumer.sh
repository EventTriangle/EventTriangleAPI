#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPOSITORY_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"
VERSION="${1:-1.0.0-local}"
PUSH_IMAGE="${2:-false}"
IMAGE="docker.io/petrokolosov/consumer-service"

bash "$SCRIPT_DIR/../docker-build.sh" \
    --working-directory "$REPOSITORY_ROOT/src/consumer" \
    --docker-file "$REPOSITORY_ROOT/src/consumer/Dockerfile" \
    --version-tag "$IMAGE:$VERSION" \
    --latest-tag "$IMAGE:latest" \
    --cache-image-tag "$IMAGE:buildcache" \
    --shared-context "$REPOSITORY_ROOT/src/shared" \
    --push "$PUSH_IMAGE"
