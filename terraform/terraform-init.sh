#!/usr/bin/env bash

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TERRAFORM_ROOT="${SCRIPT_DIR}/infrastructure/environments/dev"

: "${TF_BACKEND_STORAGE_ACCOUNT_NAME:?Set TF_BACKEND_STORAGE_ACCOUNT_NAME}"
: "${TF_BACKEND_CONTAINER_NAME:?Set TF_BACKEND_CONTAINER_NAME}"
: "${TF_BACKEND_KEY:?Set TF_BACKEND_KEY}"
: "${TF_BACKEND_SAS_TOKEN:?Set TF_BACKEND_SAS_TOKEN}"

terraform -chdir="${TERRAFORM_ROOT}" init \
  -reconfigure \
  -backend-config="storage_account_name=${TF_BACKEND_STORAGE_ACCOUNT_NAME}" \
  -backend-config="container_name=${TF_BACKEND_CONTAINER_NAME}" \
  -backend-config="key=${TF_BACKEND_KEY}" \
  -backend-config="sas_token=${TF_BACKEND_SAS_TOKEN}"
