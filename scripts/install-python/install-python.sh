#!/usr/bin/env bash

set -euo pipefail

echo "========================================"
echo "Installing Python"
echo "========================================"

sudo apt-get update

sudo apt-get install -y \
    python3 \
    python3-pip \
    python3-venv \
    pipx

pipx ensurepath

# Make pipx binaries available immediately in this script
export PATH="$HOME/.local/bin:$PATH"

echo "========================================"
echo "Installed versions"
echo "========================================"

python3 --version
pip3 --version
pipx --version

echo "========================================"
echo "Python installation completed"
echo "========================================"