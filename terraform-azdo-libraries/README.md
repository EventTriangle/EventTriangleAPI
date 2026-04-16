- Export libraries API call: `curl -u :{PAT} "https://dev.azure.com/{organization}/{project}/_apis/distributedtask/variablegroups?api-version=7.0"`
- Get project ID from: https://dev.azure.com/EventTriangle/_apis/projects?api-version=5.0-preview.3
- Generate Azure DevOps PAT with FULL permissions
- Copy AZDO PAT to azdo-pat-token.txt
- Fill the JSON default values terraform.auto.tfvars.json:
  ```json
  {
    "cloudflare_api_key": "",
    "azure_rm_client_secret": "",
    "entra_id_auth_secret": "",
    "postgres_password": "",
    "sas_token": "",
    "redis_password": ""
  }

  ```
- Login to Azure CLI in browser as user (yes it matters): az login --use-device-code
- terraform plan -out main.tfplan -lock=false
- terraform apply -lock=false "main.tfplan"
