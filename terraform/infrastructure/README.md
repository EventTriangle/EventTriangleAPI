# Azure infrastructure Terraform

The dev root `environments/dev` creates the AKS resource group and cluster, then
grants control-plane and kubelet identities `AcrPull` on existing registry
`acrsharedd01` in `rg-acr-d01`. The registry is not created or destroyed here.

ACR pull access remains available for internal/rollback images alongside the
Docker Hub migration. Log Analytics, Prometheus and Grafana are not managed here.

## Run locally

Authenticate with `az login`. From the repository root:

```bash
cd terraform
export TF_BACKEND_STORAGE_ACCOUNT_NAME="tfstatestorage011"
export TF_BACKEND_CONTAINER_NAME="tfstatecontainer01"
export TF_BACKEND_KEY="azure.tfstate"
export TF_BACKEND_SAS_TOKEN="<backend SAS token>"
./terraform-init.sh
./terraform-fmt.sh
./terraform-validate.sh
./terraform-plan.sh
```

Review the plan, then apply:

```bash
./terraform-apply.sh
```

Environment defaults are in `environments/dev/variables.tf`. Override them with
`TF_VAR_*` inputs when needed. Keep backend credentials and saved plans out of Git.

## State

The backend remains `tfstatestorage011/tfstatecontainer01/azure.tfstate`.
Retained resource addresses did not change during the directory move; no
`terraform state mv` is needed. Removed monitoring resources will be destroyed
if they still exist in state. Check for unintended AKS replacement before apply.

Use `terraform init -upgrade` only for a deliberate provider upgrade and review
the dependency lock-file change.
