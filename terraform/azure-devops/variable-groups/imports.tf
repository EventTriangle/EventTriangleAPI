locals {
  pipeline_import_ids = {
    pr_validation_auth     = 23
    pr_validation_sender   = 24
    pr_validation_consumer = 25
    build_auth             = 26
    build_consumer         = 27
    build_sender           = 28
    terraform_create       = 32
    terraform_destroy      = 33
  }
}

import {
  for_each = local.pipeline_import_ids

  to = azuredevops_build_definition.pipeline[each.key]
  id = "${var.project_id}/${each.value}"
}

import {
  to = azuredevops_serviceendpoint_github.github
  id = "${var.project_id}/00d968fc-300a-44de-b20a-026b3555785b"
}

import {
  to = azuredevops_serviceendpoint_dockerregistry.docker_hub
  id = "${var.project_id}/4310e77d-d641-4f84-9751-999b5b55b8d4"
}

import {
  to = azuredevops_serviceendpoint_dockerregistry.azure_acr
  id = "${var.project_id}/fbc5ab6c-e38a-4c08-9b7f-ae4220ee733a"
}

import {
  to = azuredevops_serviceendpoint_sonarcloud.sonarcloud
  id = "${var.project_id}/993a42ad-5193-4df4-9e9c-b29332e94e4f"
}

import {
  to = azuredevops_serviceendpoint_azurerm.azure
  id = "${var.project_id}/3b7f602e-7ba3-4476-a138-d54923b9da29"
}
