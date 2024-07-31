# K8s Terraform Automation

## Purpose

I want to create and re-create AKS clusters with minimal effort, for example simply changing prefix.
This implies process like

- Generate service principal using Azure CLI
- Assign owner role to service principal (required to get AKS cluster managed identity and assign AcrPull role)
- Update service connection in AzureDevOps
- Update infra name prefix in AzureDevOps library
- Run Terraform create pipeline

In addition, terraform pipeline creates and configures AKS cluster

- Creates namespace
- Configures secret values for applications deployments
- Installs Postgres database deployments with LoadBalancer service
- Installs RabbitMQ HELM release with LoadBalancer service
- Installs Ingress HELM release