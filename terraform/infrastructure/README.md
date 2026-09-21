# Azure infrastructure Terraform

The development root is `environments/dev`. It creates the AKS resource group
and cluster, then grants the cluster control-plane and kubelet identities
`AcrPull` on the existing Azure Container Registry.

Application images are moving to Docker Hub, but ACR access remains by design:
the existing registry can still host internal, migration, or rollback images and
the requested pull permission must remain available to AKS.

Log Analytics, Azure Managed Prometheus, and Azure Managed Grafana are not part
of this root. Platform observability can be introduced later through the GitOps
repository when its ownership and requirements are defined.

## State and migration

The development root retains the existing backend association:

- storage account: `tfstatestorage011`
- container: `tfstatecontainer01`
- key: `azure.tfstate`

Copy `environments/dev/backend.hcl.example` to an ignored local
`environments/dev/backend.hcl`, or provide the backend values through pipeline
parameters. Supply the SAS token only through a protected environment variable
or secret pipeline variable.

Moving the root does not change the retained Terraform resource addresses:

- `azurerm_resource_group.public`
- `module.aks.azurerm_kubernetes_cluster.aks`
- `module.configure_acr_access.azurerm_role_assignment.role_acrpull`
- `module.configure_acr_access.azurerm_role_assignment.role_acrpull_kubelet`

No `terraform state mv` is required for those resources. The monitoring module
addresses were intentionally removed and would be destroyed if they exist in a
non-empty state. Review every plan before applying it, especially for an
unexpected AKS replacement.

At the time of this refactor, the configured `azure.tfstate` contained no
managed resources and the subscription contained no AKS cluster. The existing
`acrsharedd01` registry in `rg-acr-d01` is read as a data source and is not
created or destroyed here.

## Local workflow

Authenticate first with `az login`, then export the backend variables used by
`terraform/terraform-init.sh`:

```bash
export TF_BACKEND_STORAGE_ACCOUNT_NAME="tfstatestorage011"
export TF_BACKEND_CONTAINER_NAME="tfstatecontainer01"
export TF_BACKEND_KEY="azure.tfstate"
export TF_BACKEND_SAS_TOKEN="<protected value>"
```

From the `terraform` directory, use the maintained wrappers:

```bash
./terraform-init.sh
./terraform-fmt.sh
./terraform-validate.sh
./terraform-plan.sh
./terraform-apply.sh
```

The checked-in dependency lock file must be updated deliberately with
`terraform init -upgrade` and reviewed in the same change as a provider upgrade.
