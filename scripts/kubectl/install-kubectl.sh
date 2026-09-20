#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "kubectl Installation"
echo "================================================================================"

# Check whether kubectl is already installed
if command -v kubectl >/dev/null 2>&1; then
    echo "[INFO] kubectl is already installed."
    echo "[INFO] Path: $(command -v kubectl)"
    echo "[INFO] Version:"
    kubectl version --client
    exit 0
fi

echo "[INFO] kubectl is not installed."
echo "[INFO] Installing required packages..."

sudo apt-get update
sudo apt-get install -y \
    ca-certificates \
    curl \
    gnupg

echo "================================================================================"
echo "[INFO] Configuring Kubernetes APT repository..."
echo "================================================================================"

# Kubernetes repository version
KUBERNETES_VERSION="v1.35"

echo "[INFO] Kubernetes repository: ${KUBERNETES_VERSION}"

sudo mkdir -p /etc/apt/keyrings

curl -fsSL \
    "https://pkgs.k8s.io/core:/stable:/${KUBERNETES_VERSION}/deb/Release.key" \
    | sudo gpg --dearmor --yes \
        -o /etc/apt/keyrings/kubernetes-apt-keyring.gpg

echo \
    "deb [signed-by=/etc/apt/keyrings/kubernetes-apt-keyring.gpg] https://pkgs.k8s.io/core:/stable:/${KUBERNETES_VERSION}/deb/ /" \
    | sudo tee /etc/apt/sources.list.d/kubernetes.list > /dev/null

echo "================================================================================"
echo "[INFO] Installing kubectl..."
echo "================================================================================"

sudo apt-get update
sudo apt-get install -y kubectl

echo "================================================================================"
echo "[INFO] Installation completed."
echo "================================================================================"

echo "[INFO] Path: $(command -v kubectl)"
echo "[INFO] Version:"

kubectl version --client

echo "================================================================================"