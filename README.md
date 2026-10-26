<p align="center">
  <img src="./img/logo.png" width="150" height="150"  alt="Mango Messenger Logo"/>
</p>

<div align="center">
  <h1>EventTriangleAPI</h1>
</div>

Event Triangle is a project that leverages the power of event sourcing using
.NET and Angular frameworks to manage and track currency transactions. 
This project employs a cutting-edge approach to data storage and retrieval, capturing each state-changing event 
in the system to create a comprehensive and reliable transaction history. 
With a focus on real-time updates and seamless integration between the backend and frontend, 
EventTriangleAPI offers realtime communication using SignalR.

- **Video**: https://www.youtube.com/watch?v=9Deyui0g1RY

## Required Software

- **.NET SDK 8.0.301 or later:** https://dotnet.microsoft.com/en-us/download
- **NVM for windows:** https://github.com/coreybutler/nvm-windows
- **Angular CLI:** `15.2.6`
- **NodeJS:** `18.15.0`
- **NPM:** `9.5.0`
- **Docker:** https://docs.docker.com/get-docker/
- **IDE**: Visual Studio 2022 or JetBrains Rider

## How to run in Debug mode

### Build Angular client

- Install NVM: `choco install nvm -y`
- Install NodeJS `18.15.0` using NVM & PowerShell as Administrator: `nvm install 18.15.0`
- Use NodeJS `18.15.0` using NVM via PowerShell as Administrator: `nvm use 18.15.0`
- Check NodeJS installed properly (should be `18.15.0`): `node -v`
- Check NPM installed properly (should be `9.5.0`): `npm -v`
- Go to the project folder: `cd src/authorization/EventTriangle.Client`
- Restore node modules: `npm ci`
- Install Angular CLI globally: `npm install -g @angular/cli@15.2.6`
- Open PowerShell as Administrator and type: `Set-ExecutionPolicy -ExecutionPolicy RemoteSigned`
- Check that Angular CLI installed properly: `ng version`
- Build project for development using Angular CLI: `ng build`
- Run angular client: `ng serve`

### Run required containers

- `docker run --name "event-auth-pgsql-db" -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres:latest`
- `docker run --name=rabbit1 -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=guest -e RABBITMQ_DEFAULT_PASS=guest -d rabbitmq:3-management`

### Run .NET Services in order

1. `Authorization service`
2. `Sender service`
3. `Consumer service`

## Infrastructure diagram

Setup: [Azure infrastructure](terraform/infrastructure/README.md),
[Azure DevOps](terraform/azure-devops/variable-groups/README.md),
[Cloudflare DNS](terraform/cloudflare/README.md).

![./img/infrastructure_diagram.png](./img/infrastructure_diagram.png)

## Technology stack

- **SDK:** `.NET 8`
- **Frameworks:** `ASP .NET Core`, `Angular 15`
- **Persistence:**
    - Database: `PostgreSQL 15`
    - ORM: `Entity Framework Core`
- **Authorization:** `Azure AD`
- **Presentation:**
    - API Documentation: `OpenAPI (Swagger)`
    - Realtime Communication: `SignalR`
    - Frontend Development: `Angular 15`
    - Microservices communication: `RabbitMQ`, `GRPC`
- **Unit and Integration Testing:** `XUnit`, `FluentAssertions`
- **Containerization:** `Docker`, `k8s`
- **Continuous Integration:** `Azure Pipelines`, `GitHub Actions`
- **Continuous Deployment:** `Azure Pipelines`, `GitHub Actions`, `Azure DevOps`, `Azure K8s Service`
- **Programming languages:** `C#`, `SQL`, `TypeScript`
- **Tools & IDE:** `Visual Studio`, `Rider`, `VS Code`, `WebStorm`, `SMSS`, `Postman`

## Useful links

### Deployments

- https://auth-eventtriangle.razumovsky.me/app/transactions
- https://auth-eventtriangle.razumovsky.me/swagger/index.html

### Management

- Docker: https://hub.docker.com/u/petrokolosov
- Azure DevOps: https://dev.azure.com/EventTriangle/EventTriangleAPI
- Renovate: https://developer.mend.io/github/EventTriangle/EventTriangleAPI
- SonarCloud: https://sonarcloud.io/organizations/event-triangle/projects

## How to run Docker compose

### Windows

- `setx EVENT_TRIANGLE_AD_CLIENT_SECRET <YOUR_AD_SECRET>`
- `docker-compose up`

### Linux

- `export EVENT_TRIANGLE_AD_CLIENT_SECRET=<YOUR_AD_SECRET>`
- `docker compose up`

## How to build and publish docker images

Run from the repository root. Docker Desktop or another Docker daemon must be
running, with Buildx available. The scripts supply the shared source context
required by the Dockerfiles; no Dockerfile changes are needed. The shared
`scripts/docker-build.sh` builds and publishes with `docker buildx build --push`.
There is no boolean push argument or build-only mode. Supply a unique version
and authenticate to the registry before running a wrapper. Docker selects the
build platform; the Ubuntu CI agents build Linux AMD64 images.

The public Docker Hub repositories are `petrokolosov/auth-service`,
`petrokolosov/sender-service`, and `petrokolosov/consumer-service`. Public pull
access does not need a Kubernetes image pull secret. Private repositories would
require a read-only pull credential delivered through the GitOps secret
mechanism when deploying them.

### Publish to Docker Hub

Use a Docker Hub PAT belonging to `petrokolosov` with image write permission.
Keep it in a protected environment variable or password manager, never in
source code. For local publication, log in through standard input:

```bash
printf '%s' "$DOCKERHUB_TOKEN" | docker login docker.io --username petrokolosov --password-stdin
bash scripts/dockerhub/build-auth.sh 1.0.0-example
bash scripts/dockerhub/build-sender.sh 1.0.0-example
bash scripts/dockerhub/build-consumer.sh 1.0.0-example
docker logout docker.io
```

Run application tests first; these local shell commands do not run tests
themselves. Every wrapper publishes the version tag, `latest`, and `buildcache`.
The shared script imports registry cache from `buildcache` and exports inline
cache metadata with the published images. The original `scripts/build-*.sh`
remain available for ACR rollback and also publish using the same shared script.

### Pipeline behavior and deployment references

- Build and PR Validation pipelines run application checks before publication.
  Same-repository PRs may publish, including mutable latest/cache tags. The
  Docker publishing job skips fork PRs; managed definitions also disable fork
  triggers and secret sharing. Build CI triggers remain `main` and `v*` tags,
  while successful manual branch builds are allowed to publish.
- The shared template logs in with `Docker@2` and the registry service connection,
  then runs the maintained build script; its `buildx --push` performs publication.
- Terraform authorizes `Docker_Hub_Connection` for the three Build and three PR
  Validation pipelines. Apply the reviewed permission changes after the YAML
  rollout; retain the existing ACR permissions until that transition.
- CI tags include GitVersion SemVer and the unique Azure DevOps run ID, for
  example `1.0.1-104-1666`. Publication also produces a pipeline artifact with
  `published-image.json` containing the commit, version tag,
  digest, and immutable reference `docker.io/petrokolosov/auth-service@sha256:...`.
- Use the digest reference for deployment and rollback. `latest` and
  `buildcache` are mutable convenience tags, not deployment identifiers.

Docker Hub has immutable version tags enabled for all three repositories using
`^[0-9]+[.][0-9]+[.][0-9]+.*$`. A published version cannot be overwritten;
retry publication with a new pipeline run ID or a new local version tag.
`latest` and `buildcache` do not match this rule and remain mutable.

The pipeline reads the digest from Docker Hub after publishing and stores the
image reference in its artifact staging directory. Local build scripts do not
create metadata files. Terraform expects the same Docker Hub PAT through
`TF_VAR_docker_hub_token` when managing the service connection.

## Screenshots

### Transactions

![./img/transactions.png](./img/transactions.png)

### Credit Cards

![./img/cards.png](./img/cards.png)

### Deposit

![./img/deposit.png](./img/deposit.png)

### Contacts

![./img/contacts.png](./img/contacts.png)

### Support

![./img/support.png](./img/support.png)

### Tickets

![./img/tickets.png](./img/tickets.png)

### Users

![./img/users.png](./img/users.png)
