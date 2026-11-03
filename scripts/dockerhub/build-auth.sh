#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPOSITORY_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"
VERSION="${1:?Usage: $0 <version> (publishes to Docker Hub)}"
IMAGE="docker.io/petrokolosov/auth-service"

bash "$SCRIPT_DIR/../docker-build.sh" \
    --working-directory "$REPOSITORY_ROOT/src/authorization" \
    --docker-file "$REPOSITORY_ROOT/src/authorization/Dockerfile" \
    --version-tag "$IMAGE:$VERSION" \
    --latest-tag "$IMAGE:latest" \
    --cache-image-tag "$IMAGE:buildcache" \
    --shared-context "$REPOSITORY_ROOT/src/shared"
