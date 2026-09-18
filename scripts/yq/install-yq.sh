#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "yq Installation"
echo "================================================================================"

# Check whether yq is already installed
if command -v yq >/dev/null 2>&1; then
    echo "[INFO] yq is already installed."
    echo "[INFO] Path: $(command -v yq)"
    echo "[INFO] Version:"
    yq --version
    exit 0
fi

echo "[INFO] yq is not installed."
echo "[INFO] Installing required packages..."

sudo apt-get update

sudo apt-get install -y \
    ca-certificates \
    curl

echo "================================================================================"
echo "[INFO] Detecting architecture..."
echo "================================================================================"

ARCH="$(dpkg --print-architecture)"

echo "[INFO] Architecture: ${ARCH}"

case "${ARCH}" in
    amd64)
        YQ_ARCH="amd64"
        ;;
    arm64)
        YQ_ARCH="arm64"
        ;;
    *)
        echo "[ERROR] Unsupported architecture: ${ARCH}"
        exit 1
        ;;
esac

YQ_URL="https://github.com/mikefarah/yq/releases/latest/download/yq_linux_${YQ_ARCH}"
YQ_PATH="/usr/local/bin/yq"

echo "[INFO] Download URL: ${YQ_URL}"
echo "[INFO] Install path: ${YQ_PATH}"

echo "================================================================================"
echo "[INFO] Downloading yq..."
echo "================================================================================"

sudo curl \
    -fL \
    "${YQ_URL}" \
    -o "${YQ_PATH}"

echo "[INFO] Setting executable permissions..."

sudo chmod 755 "${YQ_PATH}"

echo "================================================================================"
echo "[INFO] Verifying installation..."
echo "================================================================================"

if ! command -v yq >/dev/null 2>&1; then
    echo "[ERROR] yq installation failed."
    exit 1
fi

echo "[INFO] Path: $(command -v yq)"
echo "[INFO] Version:"

yq --version

echo "================================================================================"
echo "[INFO] yq installation completed successfully."
echo "================================================================================"
