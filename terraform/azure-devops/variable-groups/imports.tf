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
