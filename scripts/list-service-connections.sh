#!/usr/bin/env bash
set -euo pipefail

usage() {
    cat <<'EOF'
Usage: bash scripts/list-service-connections.sh PROJECT_ID [ORGANIZATION_URL]

List service-connection inventory as JSON for an Azure DevOps project UUID.
Organization defaults to https://dev.azure.com/EventTriangle.

Prerequisites:
  Azure CLI with the azure-devops extension:
    az extension add --name azure-devops
  Authenticate with az login, az devops login, or AZURE_DEVOPS_EXT_PAT.
  Your identity needs permission to read the project's service connections.
  For PAT authentication, use a token for the target organization with
  Service Connections (Read) scope, owned by a user with project access:
    az devops login --organization https://dev.azure.com/EventTriangle

Example:
  bash scripts/list-service-connections.sh 29327428-805a-440b-9d16-fcf0ac20edb2

Output includes IDs, names, types, authentication schemes, readiness, and
sharing status. Credentials and authorization parameters are excluded.
EOF
}

if [[ "${1:-}" == '--help' || "${1:-}" == '-h' ]]; then
    usage
    exit 0
fi
if (( $# < 1 || $# > 2 )); then
    usage >&2
    exit 2
fi

project_id="$1"
organization_url="${2:-https://dev.azure.com/EventTriangle}"
if [[ ! "$project_id" =~ ^[[:xdigit:]]{8}-[[:xdigit:]]{4}-[[:xdigit:]]{4}-[[:xdigit:]]{4}-[[:xdigit:]]{12}$ ]]; then
    echo 'Error: PROJECT_ID must be a UUID.' >&2
    exit 2
fi
if [[ ! "$organization_url" =~ ^https://dev\.azure\.com/[a-zA-Z0-9_-]+/?$ ]]; then
    echo 'Error: ORGANIZATION_URL must have the form https://dev.azure.com/ORGANIZATION.' >&2
    exit 2
fi
if ! command -v az >/dev/null 2>&1; then
    echo 'Error: Azure CLI is required.' >&2
    exit 1
fi
if ! az extension show --name azure-devops --only-show-errors >/dev/null 2>&1; then
    echo 'Error: Install the extension with: az extension add --name azure-devops' >&2
    exit 1
fi

if az devops service-endpoint list \
    --organization "$organization_url" \
    --project "$project_id" \
    --detect false \
    --query '[].{id:id,name:name,type:type,authenticationScheme:authorization.scheme,isReady:isReady,isShared:isShared}' \
    --output json \
    --only-show-errors; then
    exit 0
else
    query_status=$?
    cat >&2 <<EOF

Service-connection query failed for project $project_id in $organization_url.
If the error is TF400813 or HTTP 401/403:
  1. Confirm this organization contains the project and your account can open it.
  2. Authenticate as a user with project and service-connection read access:
       az devops login --organization $organization_url
     Enter a valid PAT for this organization with Service Connections (Read).
  3. Check whether AZURE_DEVOPS_EXT_PAT is set to an expired or wrong-account
     token. Unset it if you intend to use the interactive login credentials.
  4. If access is still denied, ask a project administrator to check your
     project membership and service-connection Reader permissions.
Azure subscription access alone does not grant Azure DevOps project access.

Use:

unset AZURE_DEVOPS_EXT_PAT
az devops login --organization https://dev.azure.com/EventTriangle
# Enter the PAT when prompted.

./list-service-connections.sh "29327428-805a-440b-9d16-fcf0ac20edb2"
EOF
    exit "$query_status"
fi
