# Commands

## Connect to AKS

- `az aks get-credentials --resource-group rg-aks-terraform-d01 --name my-aks-cluster-d01 --subscription f32f6566-8fa0-4198-9c91-a3b8ac69e89a`
- `az aks get-credentials --resource-group $(terraform output -raw rg_name) --name $(terraform output -raw aks_name) --subscription $(terraform output -raw subscription)`
