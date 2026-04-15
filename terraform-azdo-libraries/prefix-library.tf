resource "azuredevops_variable_group" "prefix_library" {
  project_id   = var.project_id
  name         = "Prefix_Library"
  description  = "Prefix_Library"
  allow_access = true

  variable {
    name  = "library-prefix"
    value = "d01"
  }
}
