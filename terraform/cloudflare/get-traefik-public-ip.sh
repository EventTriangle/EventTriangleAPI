#!/usr/bin/env bash

set -euo pipefail

namespace="${1:-traefik}"
service_name="${2:-traefik}"
fallback_ip="${3:-10.10.190.1}"
require_live_ip="${4:-false}"

public_ip="$(
  kubectl get service "$service_name" \
    --namespace "$namespace" \
    --output jsonpath='{.status.loadBalancer.ingress[0].ip}' \
    2>/dev/null || true
)"

if [[ -z "$public_ip" ]]; then
  if [[ "${require_live_ip,,}" == "true" ]]; then
    echo "Traefik service $namespace/$service_name has no public IP; refusing DNS apply." >&2
    exit 1
  fi

  public_ip="$fallback_ip"
  echo "##vso[task.logissue type=warning]Traefik public IP is unavailable; using test value $fallback_ip for this plan."
fi

"$(dirname "$0")/validate-traefik-endpoint.sh" "$public_ip"
echo "##vso[task.setvariable variable=traefikPublicIp]$public_ip"
