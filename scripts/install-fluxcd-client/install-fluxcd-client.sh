#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "Flux CD CLI Installation"
echo "================================================================================"

# Check whether Flux CLI is already installed
if command -v flux >/dev/null 2>&1; then
    echo "[INFO] Flux CLI is already installed."
    echo "[INFO] Path: $(command -v flux)"
    echo "[INFO] Version:"
    flux --version
    exit 0
fi

echo "[INFO] Flux CLI is not installed."
echo "[INFO] Installing required packages..."

sudo apt-get update

sudo apt-get install -y \
    ca-certificates \
    curl

echo "================================================================================"
echo "[INFO] Installing Flux CD CLI..."
echo "================================================================================"

curl -s https://fluxcd.io/install.sh | sudo bash

echo "================================================================================"
echo "[INFO] Verifying installation..."
echo "================================================================================"

if ! command -v flux >/dev/null 2>&1; then
    echo "[ERROR] Flux CLI installation failed."
    exit 1
fi

echo "[INFO] Path: $(command -v flux)"
echo "[INFO] Version:"

flux --version

echo "================================================================================"
echo "[INFO] Flux CD CLI installation completed successfully."
echo "================================================================================"
