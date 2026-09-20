#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "jq Installation"
echo "================================================================================"

# Check whether jq is already installed
if command -v jq >/dev/null 2>&1; then
    echo "[INFO] jq is already installed."
    echo "[INFO] Path: $(command -v jq)"
    echo "[INFO] Version:"
    jq --version
    exit 0
fi

echo "[INFO] jq is not installed."
echo "[INFO] Updating APT package index..."

sudo apt-get update

echo "================================================================================"
echo "[INFO] Installing jq..."
echo "================================================================================"

sudo apt-get install -y jq

echo "================================================================================"
echo "[INFO] Verifying installation..."
echo "================================================================================"

if ! command -v jq >/dev/null 2>&1; then
    echo "[ERROR] jq installation failed."
    exit 1
fi

echo "[INFO] Path: $(command -v jq)"
echo "[INFO] Version:"

jq --version

echo "================================================================================"
echo "[INFO] jq installation completed successfully."
echo "================================================================================"
