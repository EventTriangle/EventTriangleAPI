# Service connections

`service-connections.tf` creates connections from protected inputs; IDs come
from Terraform resources, not hard-coded imports.

- `EventTriangle`: GitHub PAT authentication, used by all nine managed pipelines.
- `Azure_ACR_Connection`: registry credentials, used by the three Build and
  three PR Validation pipelines.
- `Docker_Hub_Connection`: Docker Hub username/token; no active pipeline consumer.
- `Azure_Connection`: Azure service-principal credentials. Maintained infrastructure
  pipelines currently use the Azure credential variable group instead.
- `SonarCloud_Service_Connection`: optional token authentication, used only by
  Build Auth when analysis is enabled; disabled by default.

List actual connections from the repository root without exporting secrets:

```bash
bash scripts/list-service-connections.sh \
  29327428-805a-440b-9d16-fcf0ac20edb2 \
  https://dev.azure.com/EventTriangle
```

After apply, `terraform output service_connections` shows managed IDs and names.
Do not treat an endpoint's `Ready` flag as proof that its credentials work.

See [README](README.md) for inputs and [maintenance](DEVELOPER_ACCESS.md) for
replacement and recovery.
