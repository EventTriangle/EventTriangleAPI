# TASK-10: archived platform migration

Implementation is prepared for review in the API and GitOps repositories. This document
records the archive inventory and the interface to the maintained GitOps
configuration. It does not authorize deployment or demonstrate runtime readiness.

Target: [EventTriangle/fluxcd-repository](https://github.com/EventTriangle/fluxcd-repository).
Base branch: `master` (this repository has no `main`). Existing entry point:
`clusters/aks`, extended in place with settings under `overlays/aks`.
Full operations guide in the target repository:
`docs/aks-platform.md`.

Review branches are `task-10-platform-fluxcd-migration` in both repositories.
Runtime acceptance remains a separate step; published manifests do not establish
cluster readiness.

## What changes

The PowerShell installers are replaced with Flux configuration, not rewritten
as another sequence of imperative installers. Dev deployments use fixed
chart/image versions, explicit dependencies, external Secrets and CSI-backed
storage. The archive is retained as a reference; it is not a deployment entry point.

The existing cert-manager, RabbitMQ Cluster Operator, Redis Enterprise Operator
and ingress definitions are reused. PostgreSQL uses the official CloudNativePG
operator Helm chart. Platform workloads are declared as operator resources:
`Cluster`, `RabbitmqCluster`, `RedisEnterpriseCluster` and `RedisEnterpriseDatabase`.
No custom platform Helm charts are maintained. Application charts belong to TASK-12.

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
  Public issuer/TLS routing belongs to TASK-11.

### Namespaces, secrets and bootstrap

- `namespace/Create-Namespace.ps1` →
  `clusters/aks/01_namespaces.yml`: preserve `cert-manager`, `rabbit-mq-operator`,
  `ingress-nginx`, `redis-operator`; add `pgsql-dev`, `rabbitmq-dev`,
  `event-triangle-dev` and `cnpg-system`. The root is the sole Namespace owner.
- `secrets/connection-secrets.yaml` → externally provisioned platform Secrets
  and the protected application Secret delivery contract for TASK-12. Provide
  credentials before service reconciliation; each service checks its required
  Secret during readiness. Names and keys are documented in the GitOps operations
  guide. Do not copy archived values.
- `fluxcd/flux-install.yaml` → retire archived controller installation;
  Flux Operator bootstrap belongs to TASK-03/TASK-16. Installing a second Flux
  distribution from this archive is not part of TASK-10.
- `fluxcd/git-repository.yaml` → bootstrap contract: `GitRepository/flux-system`
  in `flux-system`, private/read credential as required, selected repository
  ref and root path `./clusters/aks`.
  Matching root `Kustomization/flux-system` is supplied by bootstrap.

TASK-15's minimum foundation uses the existing root, extended dependency graph,
namespace and secret conventions. Repository visibility, access provisioning
and full application release configuration remain TASK-15 responsibilities.
Having local Git read access is not proof that Flux has repository access.

### Ingress, monitoring and helpers

- `nginx-ingress/Install-Nginx-Ingress.ps1` → inventory retained, replacement
  deferred to TASK-11. The existing AKS NGINX release remains unchanged;
  no second ingress is installed.
  TASK-11 must preserve the required host/path routing and supply a Traefik
  LoadBalancer address to the Cloudflare pipeline.
- `prometheus-grafana/Example.ps1` → archived; do not restore monitoring in
  TASK-10. This is consistent with TASK-04 removing the Azure monitoring stack.
  A future requirement needs its own explicit installation/ownership decision.
- `prometheus-grafana/monitoring-http-nginx-ingress.yaml` → retired from the
  active platform: it depends on the old NGINX/monitoring setup. Revisit only
  with a future monitoring requirement and the ingress selected in TASK-11.
- `scripts/wait-deployments.ps1` → Flux readiness dependencies and
  bounded `kubectl wait` checks of actual resource conditions instead of testing
  whether names appear in command output.
- `scripts/print-ip.ps1` → retire public database/queue IP discovery; the
  platform services are internal. Traefik IP discovery remains TASK-11/TASK-03.

## Connection contract for application charts

- PostgreSQL host: `pgsql-dev-rw.pgsql-dev.svc.cluster.local:5432`.
  `DatabaseConnectionString` selects AuthorizationDb, SenderDb or ConsumerDb.
- RabbitMQ host: `rabbitmq-cluster.rabbitmq-dev.svc.cluster.local:5672`, vhost
  `/`; supply matching `RabbitMqUsername` and `RabbitMqPassword`.
- Redis endpoint: `event-redis.redis-operator.svc.cluster.local:6379`;
  Authorization uses `RedisUrl` and `RedisPassword`.

Deliver application Secrets into each application's namespace separately.
Kubernetes Secret references cannot cross namespaces. TASK-12 must override the
archived/current application defaults with these endpoints; this task does not
change application source or create application charts.

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

Existing NGINX and Redis Enterprise Operator installations stay in their original
namespaces. The Redis cluster and database use the existing operator in
`redis-operator`; no second operator is installed. NGINX replacement remains TASK-11.
Suspend obsolete custom HelmReleases before changing a live Git ref, then retire
them explicitly after data migration; pruning protection can leave them running.
Transfer any existing Helm-owned RabbitmqCluster to direct Flux management only
after reviewing ownership and release cleanup.

Git reverts restore configuration; they do not undo database data migrations.
Rollback may require a tested backup and compatible image. Explicit deletion
protection and orphaning prevent routine Git pruning from becoming data cleanup,
but they do not replace backups or a reviewed teardown.

## Validation and status

From the Flux repository:

```bash
kubectl kustomize clusters/aks
for overlay in overlays/aks/*; do
    kubectl kustomize "$overlay" > /dev/null || exit 1
done
```

Render external Helm charts at the pinned versions and validate custom resources
against the matching operator CRDs. The root build does not render child
reconciliations or Helm charts. Check namespace ownership, dependencies,
credential references and actual readiness conditions.

Runtime acceptance requires valid cluster access, bootstrap and Secrets, then
Flux reconciliation, authenticated PostgreSQL/Redis operations, cross-namespace
application connectivity, AMQP publish/consume, restart persistence and a second
reconciliation. Redis acceptance also includes a failover/reconnection check.

No cluster apply or data migration was performed while preparing these changes.
Runtime acceptance remains pending. The inventory covers all 22 tracked archive
files; the GitOps operations guide describes migration, retention and validation.
