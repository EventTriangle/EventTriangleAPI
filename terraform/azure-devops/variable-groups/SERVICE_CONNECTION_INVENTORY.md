# Service connections

`service-connections.tf` creates connections from protected inputs; IDs come
from Terraform resources, not hard-coded imports.

- `EventTriangle`: GitHub PAT authentication, used by all nine managed pipelines.
- `Azure_ACR_Connection`: retained for rollback with the original ACR scripts;
  no maintained pipeline consumer after TASK-09 rollout.
- `Docker_Hub_Connection`: Docker Hub username/token, used by the three Build
  and three PR Validation pipelines. PR publication is limited to same-repository
  branches; fork triggers and secret sharing remain disabled.
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

TASK-09 changes pipeline registry permissions in code. Review and apply the
Terraform plan only after the matching YAML rollout: it adds any missing Docker
Hub grants for these six consumers and removes the old ACR grants without
deleting either connection. These updated permissions have not been applied
as part of the local PR fixes.

See [README](README.md) for inputs and [maintenance](DEVELOPER_ACCESS.md) for
replacement and recovery.
