# AKS platform migration

This guide maps the archived installers and manifests to the maintained GitOps
configuration and describes how to migrate existing workloads and data.

Flux reads `clusters/aks` from the GitOps repository's `master` branch and applies
the environment settings under `overlays/aks`. See the
[AKS operations guide](https://github.com/EventTriangle/fluxcd-repository/blob/master/docs/aks-platform.md)
for the maintained configuration and deployment checks.

## Platform configuration

Flux reconciles the platform from Git. Dev deployments use pinned chart and image
versions, explicit dependencies, external Secrets and CSI-backed storage. The
archived PowerShell installers and manifests serve as a migration reference.

The platform uses cert-manager, RabbitMQ Cluster Operator and Redis Enterprise
Operator. PostgreSQL uses the official CloudNativePG operator Helm chart. Flux
declares platform workloads as operator resources: `Cluster`, `RabbitmqCluster`,
`RedisEnterpriseCluster` and `RedisEnterpriseDatabase`.

## Complete archive inventory

All paths below are relative to `.deprecated/platform/`. Destination paths are
relative to the Flux repository unless explicitly stated otherwise.

### PostgreSQL

- `postgres/Deploy-Postgres.ps1` → `base/postgresql-operator/release.yml`
  (official CloudNativePG chart 0.29.1, operator 1.30.1),
  `base/postgresql/cluster.yml` and `overlays/aks/postgresql/values.yml`.
- `postgres/pgsql-helm/Chart.yaml` → retired custom chart; the operator owns
  PostgreSQL installation and lifecycle.
- `postgres/pgsql-helm/values.yaml` → cluster settings and the external
  `pgsql-dev/postgresql-auth` Secret. Credentials are not copied from the archive.
- `postgres/pgsql-helm/templates/deployment.yaml` → CloudNativePG `Cluster`,
  one instance, PostgreSQL 15.19-system-trixie pinned by digest. The operator
  creates database pods and handles their lifecycle.
- `postgres/pgsql-helm/templates/service.yaml` → operator-created Services;
  applications use `pgsql-dev-rw` in `pgsql-dev`.
- `postgres/pgsql-helm/templates/pv.yaml` → retired static node `hostPath` PV;
  AKS CSI dynamically provisions storage.
- `postgres/pgsql-helm/templates/pv-claim.yaml` → `Cluster.spec.storage`,
  8Gi `managed-csi`; PVC lifecycle belongs to CloudNativePG.
- `postgres/pgsql-helm/templates/secrets.yaml` → external basic-auth Secret
  with username `eventtriangle`; optionally SOPS-encrypted manifests.
- `postgres/pgsql-helm/.helmignore` → archive-only packaging helper; no local
  PostgreSQL chart is packaged.

Bootstrap creates `AuthorizationDb`, `SenderDb` and `ConsumerDb` on a new cluster.
EF schema migration ownership stays with application deployment. Existing data
requires a tested restore or supported migration; existing StatefulSet PVCs are
not adopted automatically.

### RabbitMQ

- `rabbitmq/Install-RabbitMq.ps1` → reuse
  `base/rabbit-mq-operator-crds/cluster-operator.yml` (operator 2.22.5), through
  `overlays/aks/rabbit-mq-operator-crds`, after cert-manager. Preserve namespace
  `rabbit-mq-operator` and patch certificate DNS names/webhook CA references.
  `base/rabbitmq/cluster.yml` directly declares a `RabbitmqCluster` with
  digest-pinned RabbitMQ 4.3.4, one replica, external credential Secret and 8Gi
  CSI storage. No wrapper Helm chart is required. Readiness checks
  `AllReplicasReady` and `ReconcileSuccess` on the server resource.
- `rabbitmq/Uninstall-RabbitMq.ps1` → retired destructive cleanup. It deletes
  namespace PVCs; do not use it for migration or rollback.

### Redis and certificates

- `redis/Install-Redis.ps1` → reuse the official Redis Enterprise Operator chart
  8.2.0-13. `base/redis-cluster/cluster.yml` and its AKS overlay declare a
  three-node `RedisEnterpriseCluster` using matching Enterprise 8.2.0-46.13.
  Each node requests 2 CPU, 4Gi RAM and a 12Gi CSI volume (36Gi storage total).
  `base/redis/database.yml` declares one `RedisEnterpriseDatabase` in the
  operator's `redis-operator` namespace: 128MB, one primary shard, replication,
  `aofEverySecond`, `noeviction`, and external `redis-auth` password Secret.
  The database waits for cluster readiness. A single internal endpoint serves
  Authorization. Enterprise licensing is required after the default trial.
- `cert-manager/Install-Cert-Manager.ps1` → reuse `base/cert-manager-crds`, with
  the AKS overlay pinning the v1.21.1 chart digest and bounded Helm actions.
  The chart owns CRDs; the separate archived `latest` CRD installation is retired.
  The [application deployment guide](https://github.com/EventTriangle/fluxcd-repository/blob/master/docs/applications.md)
  describes the public issuer and TLS configuration.

### Namespaces, secrets and bootstrap

- `namespace/Create-Namespace.ps1` →
  `clusters/aks/01_namespaces.yml` declares namespaces for the controllers and
  workloads. The root Kustomization owns these namespaces.
- `secrets/connection-secrets.yaml` → externally provisioned platform Secrets
  and SOPS-encrypted application Secrets in the dev overlay. Provide
  credentials before service reconciliation; each service checks its required
  Secret during readiness. Names and keys are documented in the GitOps operations
  guide. Do not copy archived values.
- `fluxcd/flux-install.yaml` → Ansible bootstraps Flux Operator and its managed
  Flux instance.
- `fluxcd/git-repository.yaml` → bootstrap contract: `GitRepository/flux-system`
  in `flux-system`, private/read credential as required, selected repository
  ref and root path `./clusters/aks`.
  Matching root `Kustomization/flux-system` is supplied by bootstrap.

For a private GitOps repository, provision a read credential for the Flux
GitRepository and verify that the source becomes Ready.

### Ingress, monitoring and helpers

- `nginx-ingress/Install-Nginx-Ingress.ps1` → Flux manages the ingress controller.
  See the [application deployment guide](https://github.com/EventTriangle/fluxcd-repository/blob/master/docs/applications.md)
  for routing, TLS and DNS.
- `prometheus-grafana/Example.ps1` → archived. The current platform configuration
  omits the standalone Prometheus and Grafana installation.
- `prometheus-grafana/monitoring-http-nginx-ingress.yaml` → retired from the
  active platform because it depends on the archived monitoring setup.
- `scripts/wait-deployments.ps1` → Flux readiness dependencies and
  bounded `kubectl wait` checks of actual resource conditions instead of testing
  whether names appear in command output.
- `scripts/print-ip.ps1` → retired. Applications reach the databases and broker
  through internal Kubernetes Services.

## Connection contract for application charts

- PostgreSQL host: `pgsql-dev-rw.pgsql-dev.svc.cluster.local:5432`.
  `DatabaseConnectionString` selects AuthorizationDb, SenderDb or ConsumerDb.
- RabbitMQ host: `rabbitmq-cluster.rabbitmq-dev.svc.cluster.local:5672`, vhost
  `/`; supply matching `RabbitMqUsername` and `RabbitMqPassword`.
- Redis endpoint: `event-redis.redis-operator.svc.cluster.local:6379`;
  Authorization uses `RedisUrl` and `RedisPassword`.

Configure these endpoints in the application deployment and deliver Secrets
into the same namespace as the workloads that reference them.

## Migration, data and ownership gates

Use the Flux repository's detailed dev guide before any apply. Inventory actual
releases, controller ownership and data first. A fresh cluster and migration of
existing state are different procedures. Back up and test restore of all
databases, durable RabbitMQ messages and Redis authentication keys. Never mount
an old data directory into a different database major without a tested upgrade.

The existing `clusters/aks` root path and cert-manager/operator reconciliation
names are preserved. Before exposing an updated ref to a live cluster, suspend
affected existing children; reconcile the root namespace protections and
controller pruning settings first, then resume. The RabbitMQ operator overlay
stops declaring a duplicate Namespace: this must transfer inventory ownership
without pruning the root-owned namespace. See the AKS guide for the sequence.
Retain the actual Helm release/storage namespace when adopting a compatible release.
CloudNativePG/Redis Enterprise layout changes need a supported import or restore
and application cutover. Do not force resource takeover or delete PVCs to resolve conflicts.

The Redis cluster and database use the existing operator in `redis-operator`.
Suspend obsolete custom HelmReleases before changing a live Git ref, then retire
them explicitly after data migration; pruning protection can leave them running.
Transfer any existing Helm-owned RabbitmqCluster to direct Flux management only
after reviewing ownership and release cleanup.

Git reverts restore configuration; they do not undo database data migrations.
Rollback may require a tested backup and compatible image. Explicit deletion
protection and orphaning prevent routine Git pruning from becoming data cleanup,
but they do not replace backups or a reviewed teardown.

## Validation

Dev runtime acceptance passed across two AKS test runs. The recorded checks
covered HTTPS access to the application through ingress-nginx, authenticated
PostgreSQL and Redis operations, RabbitMQ publish/consume, data retention after
pod recreation, and repeat Flux reconciliation.

For local validation of future changes, run from the Flux repository:

```bash
for manifest in overlays/aks/*/kustomization.yml; do
    kubectl kustomize "${manifest%/*}" > /dev/null || exit 1
done
```

Render external Helm charts at the pinned versions and validate custom resources
against the matching operator CRDs. The root build does not render child
reconciliations or Helm charts. Check namespace ownership, dependencies,
credential references and actual readiness conditions.

Use the [AKS operations guide](https://github.com/EventTriangle/fluxcd-repository/blob/master/docs/aks-platform.md#validation)
to repeat runtime checks, including Redis failover and client reconnection.
