# Connect to AKS

- `az aks get-credentials --resource-group rg-aks-terraform-d01 --name my-aks-cluster-d01 --subscription f32f6566-8fa0-4198-9c91-a3b8ac69e89a`
- `az aks get-credentials --resource-group $(terraform output -raw rg_name) --name $(terraform output -raw aks_name) --subscription $(terraform output -raw subscription)`

# Manifest deployment order

- kubectl apply -f .\auth-deployment-cluster-ip\
- kubectl apply -f .\consumer-deployment-cluster-ip\
- kubectl apply -f .\sender-deployment-cluster-ip\
- kubectl apply -f .\certificate-issuer-letsencrypt\
- kubectl apply -f .\auth-https-nginx-ingress\
- kubectl apply -f .\certificate\ -n "event-triangle"

# Delete all resources

- kubectl delete -f .\auth-deployment-cluster-ip\
- kubectl delete -f .\consumer-deployment-cluster-ip\
- kubectl delete -f .\sender-deployment-cluster-ip\
- kubectl delete -f .\certificate-issuer-letsencrypt\
- kubectl delete -f .\auth-https-nginx-ingress\
- kubectl delete -f .\certificate\ -n "event-triangle"
