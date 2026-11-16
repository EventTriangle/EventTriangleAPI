# Authorization Helm chart

Run the install commands from the EventTriangleAPI repository root with a
sibling checkout of `fluxcd-repository`. Use a namespace that contains the
Secrets referenced by `secretEnv`. Set `imagePullSecrets` to names of existing
registry Secrets when the image requires authentication. Keep credentials in
Secrets and public application settings in `env`.

Authorization serves the Angular frontend, handles the Entra callback, and
proxies `/sender` and `/consumer` to their internal Services. If the Service
names or ports change, update the proxy addresses and `GrpcChannelAddresses`
under `env` in the GitOps repository's `values-authorization.yaml`. `frontend.baseUrl` defaults
to the current origin (`/`).

Supply the PostgreSQL connection string for Authorization, the Redis password
and the Entra client secret through `secretEnv`. Set the Redis endpoint with
`env.RedisUrl`.
Public Entra settings can override the application configuration using
`env.AzureAd__TenantId`, `env.AzureAd__ClientId` and other `AzureAd__*` keys.

The dev example requires Traefik, the `auth-body-limit` Middleware from the
AKS overlay, and the namespaced cert-manager Issuer `eventtriangle-letsencrypt`.
The main Ingress requests `eventtriangle-tls`. The exact `/consumer/notify`
route uses the certificate Traefik loads for the same host. The dev host
must resolve to Traefik for cert-manager HTTP-01 issuance and renewal.

## Install and upgrade

Use the dev example in an environment where its resource names are available.

```sh
helm upgrade --install authorization charts/authorization \
  --namespace event-triangle-dev \
  -f ../fluxcd-repository/overlays/aks/eventtriangle/values-authorization.yaml
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
`helm history authorization -n event-triangle-dev` and run
`helm rollback authorization REVISION -n event-triangle-dev`. Helm rollback does not
restore external Secret contents or revert database changes.
