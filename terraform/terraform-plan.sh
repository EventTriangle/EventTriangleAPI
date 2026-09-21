#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

terraform -chdir="${SCRIPT_DIR}/infrastructure/environments/dev" plan -out main.tfplan
