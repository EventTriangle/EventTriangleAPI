# Consumer Helm chart

Run the install commands from the EventTriangleAPI repository root with a
sibling checkout of `fluxcd-repository`. Use a namespace that contains the
Secrets referenced by `secretEnv`. Set `imagePullSecrets` to names of existing
registry Secrets when the image requires authentication. Keep credentials in
Secrets and public application settings in `env`.

Consumer requires its PostgreSQL database and RabbitMQ. Set the broker hostname
and username with `env.RabbitMqHost` and `env.RabbitMqUsername`. The database
connection string and broker password come from `secretEnv`. Public Entra token
validation settings can be overridden with `env.AzureAd__TenantId`,
`env.AzureAd__ClientId` and `env.AzureAd__Scopes`.

The dev example leaves ingress disabled because Authorization proxies the
Consumer API and SignalR endpoint over the internal HTTP Service.

## Install and upgrade

Use the dev example in an environment where its resource names are available.

```sh
helm upgrade --install consumer charts/consumer \
  --namespace event-triangle-dev \
  -f ../fluxcd-repository/overlays/aks/eventtriangle/values-consumer.yaml
```

Choose the namespace with `--namespace` and use the same release name and
namespace for upgrades. Set `fullnameOverride` and `service.name` to override
the names derived from the release. Keep the Deployment name stable because
it is also the selector identity. Set `image.tag`, `image.digest`,
or both. To deploy by tag alone, clear the digest in the environment values.
To deploy by digest alone, clear the tag. Set a probe to `null` to disable it.
When changing its handler type, set the default handler to `null` and supply
the new handler. CPU autoscaling requires a metrics API and a CPU request.
Configure HTTP paths under `ingress.routes`. Each route can reference an
existing TLS Secret in the release namespace with `tlsSecretName`.

To restore a previous release revision, find it with
`helm history consumer -n event-triangle-dev` and run
`helm rollback consumer REVISION -n event-triangle-dev`. Helm rollback does not
restore external Secret contents or revert database changes.

## Dry runs

Preview the install or upgrade against your current Kubernetes context:

```sh
helm upgrade --install consumer charts/consumer \
  --namespace event-triangle-dev --dry-run=server \
  -f ../fluxcd-repository/overlays/aks/eventtriangle/values-consumer.yaml
```

Print the rendered YAML locally:

```sh
helm template consumer charts/consumer \
  --namespace event-triangle-dev \
  -f ../fluxcd-repository/overlays/aks/eventtriangle/values-consumer.yaml
```
