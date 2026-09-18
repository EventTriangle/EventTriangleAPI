#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "Helm Installation"
echo "================================================================================"

# Check whether Helm is already installed
if command -v helm >/dev/null 2>&1; then
    echo "[INFO] Helm is already installed."
    echo "[INFO] Path: $(command -v helm)"
    echo "[INFO] Version:"
    helm version
    exit 0
fi

echo "[INFO] Helm is not installed."
echo "[INFO] Installing required packages..."

sudo apt-get update

sudo apt-get install -y \
    ca-certificates \
    curl \
    gnupg

echo "================================================================================"
echo "[INFO] Configuring Helm signing key..."
echo "================================================================================"

sudo mkdir -p /etc/apt/keyrings

curl -fsSL https://packages.buildkite.com/helm-linux/helm-debian/gpgkey \
    | sudo gpg --dearmor --yes \
        -o /etc/apt/keyrings/helm.gpg

sudo chmod a+r /etc/apt/keyrings/helm.gpg

echo "================================================================================"
echo "[INFO] Configuring Helm APT repository..."
echo "================================================================================"

echo \
    "deb [signed-by=/etc/apt/keyrings/helm.gpg] https://packages.buildkite.com/helm-linux/helm-debian/any/ any main" \
    | sudo tee /etc/apt/sources.list.d/helm-stable-debian.list > /dev/null

echo "================================================================================"
echo "[INFO] Installing Helm..."
echo "================================================================================"

sudo apt-get update
sudo apt-get install -y helm

echo "================================================================================"
echo "[INFO] Verifying installation..."
echo "================================================================================"

if ! command -v helm >/dev/null 2>&1; then
    echo "[ERROR] Helm installation failed."
    exit 1
fi

echo "[INFO] Path: $(command -v helm)"
echo "[INFO] Version:"

helm version

echo "================================================================================"
echo "[INFO] Helm installation completed successfully."
echo "================================================================================"
