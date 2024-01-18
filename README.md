# EventTriangleAPI

[![Build And Test Auth API](https://github.com/EventTriangle/EventTriangleAPI/actions/workflows/auth-build-and-test.yml/badge.svg)](https://github.com/EventTriangle/EventTriangleAPI/actions/workflows/auth-build-and-test.yml)
[![Build And Test Sender API](https://github.com/EventTriangle/EventTriangleAPI/actions/workflows/sender-build-and-test.yml/badge.svg)](https://github.com/EventTriangle/EventTriangleAPI/actions/workflows/sender-build-and-test.yml)
[![Build And Test Consumer API](https://github.com/EventTriangle/EventTriangleAPI/actions/workflows/consumer-build-and-test.yml/badge.svg)](https://github.com/EventTriangle/EventTriangleAPI/actions/workflows/consumer-build-and-test.yml)

Repository that contains API for both event publisher and event consumer applications

## Screenshots

### Transactions

![./img/transactions.png](./img/transactions.png)

### Cards

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

## Required Software

- **.NET SDK 6.0.202 or later:** https://dotnet.microsoft.com/en-us/download
- **NVM for windows:** https://github.com/coreybutler/nvm-windows
- **Angular CLI:** `15.2.6`
- **NodeJS:** `18.15.0`
- **NPM:** `9.5.0`
- **Docker:** https://docs.docker.com/get-docker/
- **IDE**: Visual Studio 2022 or JetBrains Rider

## Useful links

- Docker images: https://hub.docker.com/u/kaminome
- Azure DevOps project: https://dev.azure.com/EventTriangle/EventTriangleAPI

## Run in Debug mode

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

### Run .NET Services

- `authorization`
- `sender`
- `consumer`

## Run Docker compose

### Windows

- `setx EVENT_TRIANGLE_AD_CLIENT_SECRET <YOUR_AD_SECRET>`
- `docker-compose up`

### Linux

- `export EVENT_TRIANGLE_AD_CLIENT_SECRET=<YOUR_AD_SECRET>`
- `docker compose up`

## To build docker images

From `src` folder run:

- `docker build --build-arg FRONT_API_URL="http://localhost:7000/" -t eventtriangle/auth:1.0 -f ./authorization/Dockerfile .`
- `docker build -t eventtriangle/consumer:1.0 -f ./consumer/Dockerfile . `
- `docker build -t eventtriangle/sender:1.0 -f ./sender/Dockerfile .`