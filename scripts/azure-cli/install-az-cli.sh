#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "Azure CLI Installation"
echo "================================================================================"

# Check whether Azure CLI is already installed
if command -v az >/dev/null 2>&1; then
    echo "[INFO] Azure CLI is already installed."
    echo "[INFO] Path: $(command -v az)"
    echo "[INFO] Version:"
    az version
    exit 0
fi

echo "[INFO] Azure CLI is not installed."
echo "[INFO] Installing required packages..."

sudo apt-get update

sudo apt-get install -y \
    ca-certificates \
    curl \
    apt-transport-https \
    lsb-release \
    gnupg

echo "================================================================================"
echo "[INFO] Configuring Microsoft signing key..."
echo "================================================================================"

sudo mkdir -p /etc/apt/keyrings

curl -sLS https://packages.microsoft.com/keys/microsoft.asc \
    | gpg --dearmor \
    | sudo tee /etc/apt/keyrings/microsoft.gpg > /dev/null

sudo chmod go+r /etc/apt/keyrings/microsoft.gpg

echo "================================================================================"
echo "[INFO] Configuring Azure CLI APT repository..."
echo "================================================================================"

AZ_DIST="noble"

echo "[INFO] Ubuntu distribution: ${AZ_DIST}"

echo \
    "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/microsoft.gpg] https://packages.microsoft.com/repos/azure-cli/ ${AZ_DIST} main" \
    | sudo tee /etc/apt/sources.list.d/azure-cli.list > /dev/null

echo "================================================================================"
echo "[INFO] Installing Azure CLI..."
echo "================================================================================"

sudo apt-get update
sudo apt-get install -y azure-cli

echo "================================================================================"
echo "[INFO] Azure CLI installation completed."
echo "================================================================================"

echo "[INFO] Path: $(command -v az)"
echo "[INFO] Version:"

az version

echo "================================================================================"
