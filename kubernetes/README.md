# K8s Deployment notes

## Deployment order

- Deploy cluster using terraform: https://github.com/kolosovpetro/azure-aks-terraform
- Install ingress controller using HELM: [Link](./helm-install-nginx-ingress)
- Deploy namespace resource: `kubectl apply -f .\namespace\`
- Deploy pgsql database: `kubectl apply -f .\pgsql-deployment-cluster-ip\`
- Deploy RabbitMQ using HELM: [Link](./helm-install-rabbit-mq)
- Deploy Secrets: `kubectl apply -f .\secrets\`