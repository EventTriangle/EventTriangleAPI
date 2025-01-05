# Commands

## Connect to AKS

- `az aks get-credentials --resource-group $(terraform output -raw rg_name) --name $(terraform output -raw aks_name) --subscription $(terraform output -raw subscription)`
