# TASK-10: archived platform migration

Implementation is prepared for local review in two repositories. This document
records the archive inventory and the interface to the maintained GitOps
configuration. It does not authorize deployment or demonstrate runtime readiness.

Target: [EventTriangle/fluxcd-repository](https://github.com/EventTriangle/fluxcd-repository).
Base branch: `master` (this repository has no `main`). Existing entry point:
`clusters/aks`, extended in place with settings under `overlays/aks`.
Full operations guide in the target repository:
`docs/aks-platform.md`.

Local review branches are `task-10-platform-fluxcd-migration` in both checkouts.
The EventTriangleAPI branch starts from `task-09-docker-hub-migration`; the Flux
branch starts from `origin/master`. Do not push or deploy until review is complete.
Future commits/PRs must identify the matching revision in the other repository;
no cross-repository change is made atomic by matching branch names.

## What changes

The PowerShell installers are replaced with Flux configuration, not rewritten
as another sequence of imperative installers. Dev deployments use fixed
chart/image versions, explicit dependencies, external Secrets and CSI-backed
storage. The archive is retained as a reference; it is not a deployment entry point.

The Flux repository already contains cert-manager and RabbitMQ Cluster Operator.
Those installations are reused. New local platform charts install PostgreSQL
and Redis, and define the operator-managed RabbitMQ server via Helm. The
operator itself remains Kustomize-managed to avoid two competing installations.

## Complete archive inventory

All paths below are relative to `.deprecated/platform/`. Destination paths are
relative to the Flux repository unless explicitly stated otherwise.

### PostgreSQL

- `postgres/Deploy-Postgres.ps1` → `base/postgresql/release.yml` and
  `overlays/aks/postgresql/values.yml`. Old hard-coded command is retired.
- `postgres/pgsql-helm/Chart.yaml` → `charts/postgresql/Chart.yaml`, version
  `0.1.0`. This is a new maintained dev chart, not a claim that the archive chart
  can be upgraded in place without a migration.
- `postgres/pgsql-helm/values.yaml` → dev values and the external
  `pgsql-dev/postgresql-auth` Secret. Archived credential values are not copied.
- `postgres/pgsql-helm/templates/deployment.yaml` →
  `charts/postgresql/templates/statefulset.yaml`; replace `postgres:latest`
  with digest-pinned PostgreSQL 15.19 and correct the image data mount.
- `postgres/pgsql-helm/templates/service.yaml` →
  `charts/postgresql/templates/service.yaml`, internal ClusterIP and headless
  services. New DNS contract is listed below.
- `postgres/pgsql-helm/templates/pv.yaml` → retired static node `hostPath` PV;
  AKS CSI dynamically provisions storage.
- `postgres/pgsql-helm/templates/pv-claim.yaml` → StatefulSet
  `volumeClaimTemplates`, 8Gi RWO `managed-csi`, PVC retention `Retain`.
- `postgres/pgsql-helm/templates/secrets.yaml` → external Secret references;
  optionally SOPS-encrypted manifests in `overlays/aks/secrets`.
- `postgres/pgsql-helm/.helmignore` → archive-only chart packaging helper;
  the small maintained platform chart is sourced from the selected Git revision
  and does not publish archive build artifacts.

The chart initializes `AuthorizationDb`, `SenderDb` and `ConsumerDb` only on a
new volume. EF schema migration ownership stays with the application deployment.
Existing data must be backed up and restored or explicitly adopted after review.

### RabbitMQ

- `rabbitmq/Install-RabbitMq.ps1` → reuse
  `base/rabbit-mq-operator-crds/cluster-operator.yml` (operator 2.22.5), selected
  through `overlays/aks/rabbit-mq-operator-crds`, after cert-manager is ready.
  Preserve its existing `rabbit-mq-operator` namespace and correct certificate
  DNS names and webhook CA references for that namespace. The
  server is installed by `base/rabbitmq/release.yml` using the
  `charts/rabbitmq-cluster` chart, version `0.1.0`, digest-pinned RabbitMQ 4.3.4,
  one replica, external credential Secret and 8Gi CSI storage. Readiness is
  bounded and checks the actual RabbitmqCluster, not just the operator.
- `rabbitmq/Uninstall-RabbitMq.ps1` → retired destructive cleanup. It deletes
  all namespace PVCs; do not use it for migration or rollback. Controlled
  teardown, retained resources and ownership are documented in the dev guide.

### Redis and certificates

- `redis/Install-Redis.ps1` → `base/redis/release.yml`,
  `overlays/aks/redis/values.yml` and `charts/redis` version `0.1.0`,
  digest-pinned Redis 7.4.11, external password Secret and 2Gi CSI storage.
  Replace the old Bitnami installation deliberately; the existing Enterprise
  operator is not a substitute for ordinary Redis. AOF persistence and
  `noeviction` preserve Authorization's Data Protection keys.
- `cert-manager/Install-Cert-Manager.ps1` → reuse
  `base/cert-manager-crds`, with `overlays/aks/cert-manager-crds` pinning the existing
  v1.21.1 chart digest and bounded Helm actions. The Helm chart owns CRDs;
  separate archived `latest` CRD installation is retired. Internal RabbitMQ
  webhook certificates require no public DNS. Public issuer/TLS routing is TASK-11.

### Namespaces, secrets and bootstrap

- `namespace/Create-Namespace.ps1` →
  `clusters/aks/01_namespaces.yml`: preserve `cert-manager`, `rabbit-mq-operator`,
  `ingress-nginx`, `redis-operator`; add `pgsql-dev`, `rabbitmq-dev`, `redis-dev`,
  `event-triangle-dev`. The root is the sole Namespace owner.
- `secrets/connection-secrets.yaml` → platform secret contract in
  `overlays/aks/secrets/contract.yml` and the protected application Secret
  delivery contract for TASK-12. Do not copy values from this archive.
- `fluxcd/flux-install.yaml` → retire archived controller installation;
  Flux Operator bootstrap belongs to TASK-03/TASK-16. Installing a second Flux
  distribution from this archive is not part of TASK-10.
- `fluxcd/git-repository.yaml` → bootstrap contract: `GitRepository/flux-system`
  in `flux-system`, private/read credential as required, selected repository
  ref, source artifact including `charts/`, root path `./clusters/aks`.
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

- PostgreSQL host: `pgsql-dev.pgsql-dev.svc.cluster.local:5432`.
  `DatabaseConnectionString` selects AuthorizationDb, SenderDb or ConsumerDb.
- RabbitMQ host: `rabbitmq-cluster.rabbitmq-dev.svc.cluster.local:5672`, vhost
  `/`; supply matching `RabbitMqUsername` and `RabbitMqPassword`.
- Redis endpoint: `event-redis-master.redis-dev.svc.cluster.local:6379`;
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
PostgreSQL/Redis layout changes usually need a separate restore and application
cutover. Do not force resource takeover or delete PVCs to resolve conflicts.

Existing NGINX and Redis Enterprise Operator resources remain in their original
AKS reconciliations. Enterprise Redis is not reused as the ordinary Redis
instance: removing the existing operator requires inventorying its consumers and
a separate controlled retirement decision. NGINX replacement remains TASK-11.

Git reverts restore configuration; they do not undo database data migrations.
Rollback may require a tested backup and compatible image. Explicit deletion
protection and orphaning prevent routine Git pruning from becoming data cleanup,
but they do not replace backups or a reviewed teardown.

## Validation and status

From the Flux repository:

```bash
kubectl kustomize clusters/aks
helm lint charts/postgresql charts/redis charts/rabbitmq-cluster --strict
```

Render each dev overlay separately and use `helm template` with the corresponding
release values. Review dependencies, ownership and Secret references. The root
build alone does not render child reconciliations or Helm charts. After deliberate
deployment, check Flux readiness and use bounded `kubectl wait` commands.
Acceptance also requires credential-key checks, authenticated service operations,
cross-namespace application connectivity, an AMQP publish/consume test, restart
persistence checks with disposable data and a second Flux reconciliation.

On 2026-10-23 the current `my-aks-cluster-d01` kubeconfig endpoint did not resolve
in DNS. No assumption is made about whether resources/data still exist. No
cluster apply, push, bootstrap, destructive migration or runtime acceptance was
performed while preparing these local changes. Complete runtime acceptance only
after review, valid cluster access and the required bootstrap/Secrets.

The archive mapping covers all 22 tracked source files. See the Flux guide for
the manual validation procedure and the explicit runtime limitations.

The expanded AKS configuration passed local review: all eight overlays and six
Helm releases rendered; schemas passed for 104 standard resources, 21 custom
resources and 7 CRDs. Namespace ownership, RabbitMQ webhook references and the
dependency graph were checked. Existing NGINX/Redis Enterprise definitions are
unchanged. Live readiness and persistence acceptance remain pending.
