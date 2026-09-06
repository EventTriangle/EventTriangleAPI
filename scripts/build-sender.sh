#!/bin/sh

set -eu

./build.sh \
    --working-directory "${CI_PROJECT_DIR}/2.346.1" \
    --docker-file "${CI_PROJECT_DIR}/2.346.1/Dockerfile" \
    --version-tag "${IMAGE}:2.346.1-33" \
    --latest-tag "${IMAGE}:latest" \
    --cache-image-tag "${IMAGE}:buildcache"
