#!/usr/bin/env bash

set -euo pipefail

public_ip="${1:-}"

if [[ -z "$public_ip" ]]; then
  echo "Traefik public IP must not be empty." >&2
  exit 1
fi

validation_error="$({ python3 - "$public_ip" <<'PY'
import ipaddress
import sys

value = sys.argv[1]

try:
    address = ipaddress.ip_address(value)
except ValueError:
    raise SystemExit("Traefik public IP is not a valid IPv4 address.")

if address.version != 4:
    raise SystemExit("Traefik public IP must be an IPv4 address.")
PY
} 2>&1)" || {
  echo "$validation_error" >&2
  exit 1
}

echo "Traefik public IP is valid."
