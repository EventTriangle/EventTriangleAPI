#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "Terraform Installation"
echo "================================================================================"

# Check whether Terraform is already installed
if command -v terraform >/dev/null 2>&1; then
    echo "[INFO] Terraform is already installed."
    echo "[INFO] Path: $(command -v terraform)"
    echo "[INFO] Version:"
    terraform version
    exit 0
fi

echo "[INFO] Terraform is not installed."
echo "[INFO] Installing required packages..."

sudo apt-get update

sudo apt-get install -y \
    ca-certificates \
    curl \
    gnupg \
    lsb-release

echo "================================================================================"
echo "[INFO] Configuring HashiCorp signing key..."
echo "================================================================================"

sudo mkdir -p /etc/apt/keyrings

curl -fsSL https://apt.releases.hashicorp.com/gpg \
    | sudo gpg --dearmor --yes \
        -o /etc/apt/keyrings/hashicorp-archive-keyring.gpg

sudo chmod a+r /etc/apt/keyrings/hashicorp-archive-keyring.gpg

echo "================================================================================"
echo "[INFO] Configuring HashiCorp APT repository..."
echo "================================================================================"

UBUNTU_CODENAME="$(lsb_release -cs)"

echo "[INFO] Ubuntu codename: ${UBUNTU_CODENAME}"

echo \
    "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/hashicorp-archive-keyring.gpg] https://apt.releases.hashicorp.com ${UBUNTU_CODENAME} main" \
    | sudo tee /etc/apt/sources.list.d/hashicorp.list > /dev/null

echo "================================================================================"
echo "[INFO] Installing Terraform..."
echo "================================================================================"

sudo apt-get update
sudo apt-get install -y terraform

echo "================================================================================"
echo "[INFO] Terraform installation completed."
echo "================================================================================"

echo "[INFO] Path: $(command -v terraform)"
echo "[INFO] Version:"

terraform version

echo "================================================================================"
