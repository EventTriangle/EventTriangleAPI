# K8s Deployment notes

## Deployment order

- Deploy cluster using terraform: https://github.com/kolosovpetro/azure-aks-terraform
- Install ingress controller using HELM: [Link](./install-nginx-ingress-helm)
- Deploy namespace resource: `kubectl apply -f namespace.yaml`