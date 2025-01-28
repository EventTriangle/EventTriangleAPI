locals {
  resource_group_name = "${var.resource_group_name}-${var.prefix}"
  aks_name            = "${var.cluster_name}-${var.prefix}"
  acr_name            = "${var.acr_name}${var.prefix}"
  prometheus_name     = "prometheus-aks-${var.prefix}"
  grafana_name        = "grafana-aks-${var.prefix}"
  workspace_name      = "loganalytics-${var.prefix}"
}
