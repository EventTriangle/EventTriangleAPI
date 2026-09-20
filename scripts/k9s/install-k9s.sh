#!/usr/bin/env bash

set -euo pipefail

echo "================================================================================"
echo "K9s Installation"
echo "================================================================================"

# Check whether k9s is already installed
if command -v k9s >/dev/null 2>&1; then
    echo "[INFO] k9s is already installed."
    echo "[INFO] Path: $(command -v k9s)"
    echo "[INFO] Version:"
    k9s version
    exit 0
fi

echo "[INFO] k9s is not installed."
echo "[INFO] Detecting system architecture..."

ARCH="$(dpkg --print-architecture)"

echo "[INFO] Architecture: ${ARCH}"

case "${ARCH}" in
    amd64)
        K9S_ARCH="amd64"
        ;;
    arm64)
        K9S_ARCH="arm64"
        ;;
    *)
        echo "[ERROR] Unsupported architecture: ${ARCH}"
        exit 1
        ;;
esac

echo "[INFO] Getting latest k9s release..."

K9S_VERSION="$(
    curl -fsSL https://api.github.com/repos/derailed/k9s/releases/latest |
        grep '"tag_name":' |
        head -n 1 |
        cut -d '"' -f 4
)"

if [[ -z "${K9S_VERSION}" ]]; then
    echo "[ERROR] Could not determine latest k9s version."
    exit 1
fi

echo "[INFO] Latest version: ${K9S_VERSION}"

PACKAGE="k9s_linux_${K9S_ARCH}.deb"
DOWNLOAD_URL="https://github.com/derailed/k9s/releases/download/${K9S_VERSION}/${PACKAGE}"
TEMP_FILE="/tmp/${PACKAGE}"

echo "[INFO] Package:      ${PACKAGE}"
echo "[INFO] Download URL: ${DOWNLOAD_URL}"
echo "[INFO] Temporary:    ${TEMP_FILE}"

echo "================================================================================"
echo "[INFO] Downloading k9s..."
echo "================================================================================"

curl -fL "${DOWNLOAD_URL}" -o "${TEMP_FILE}"

echo "================================================================================"
echo "[INFO] Installing k9s..."
echo "================================================================================"

sudo apt install -y "${TEMP_FILE}"

echo "[INFO] Removing temporary package..."

rm -f "${TEMP_FILE}"

echo "================================================================================"
echo "[INFO] Installation completed."
echo "================================================================================"

echo "[INFO] Path: $(command -v k9s)"
echo "[INFO] Version:"

k9s version

echo "================================================================================"