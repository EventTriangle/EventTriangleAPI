#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

find "${SCRIPT_DIR}" -type f -name "*.sh" -exec chmod +x {} +
