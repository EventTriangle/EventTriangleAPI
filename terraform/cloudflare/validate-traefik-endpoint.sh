#!/usr/bin/env bash

set -euo pipefail

endpoint="${1:-}"
requested_type="${2:-AUTO}"

if [[ -z "$endpoint" ]]; then
  echo "Traefik endpoint must not be empty." >&2
  exit 1
fi

if [[ "$endpoint" == *"://"* || "$endpoint" == */* || "$endpoint" =~ [[:space:]] ]]; then
  echo "Traefik endpoint must be an IP address or DNS hostname without a scheme, path, or whitespace." >&2
  exit 1
fi

case "$requested_type" in
  AUTO | A | AAAA | CNAME) ;;
  *)
    echo "Record type must be AUTO, A, AAAA, or CNAME." >&2
    exit 1
    ;;
esac

detected_type="$({ python3 - "$endpoint" <<'PY'
import ipaddress
import re
import sys

endpoint = sys.argv[1].rstrip(".")

try:
    address = ipaddress.ip_address(endpoint)
except ValueError:
    labels = endpoint.split(".")
    hostname_is_valid = (
        len(endpoint) <= 253
        and len(labels) >= 2
        and re.search(r"[A-Za-z]", labels[-1])
        and all(
            1 <= len(label) <= 63
            and re.fullmatch(r"[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?", label)
            for label in labels
        )
    )
    if not hostname_is_valid:
        raise SystemExit("Traefik endpoint is not a valid IP address or DNS hostname.")
    print("CNAME")
else:
    print("A" if address.version == 4 else "AAAA")
PY
} 2>&1)" || {
  echo "$detected_type" >&2
  exit 1
}

if [[ "$requested_type" != "AUTO" && "$requested_type" != "$detected_type" ]]; then
  echo "Record type $requested_type does not match endpoint type $detected_type." >&2
  exit 1
fi

echo "Traefik endpoint is valid ($detected_type)."
