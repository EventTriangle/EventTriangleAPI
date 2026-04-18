resource "azuredevops_variable_group" "tfvars_transform" {
  project_id   = var.project_id
  name         = "Terraform_Auto_Tfvars_Json_Transform"
  description  = "Terraform_Auto_Tfvars_Json_Transform"
  allow_access = true

  variable {
    name         = "client_secret"
    is_secret    = true
    secret_value = var.azure_rm_client_secret
  }

  variable {
    name  = "should_deploy_log_analytics"
    value = "false"
  }

  variable {
    name  = "should_deploy_prometheus"
    value = "false"
  }
}
