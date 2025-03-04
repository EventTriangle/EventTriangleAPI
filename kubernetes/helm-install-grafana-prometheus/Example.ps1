helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
kubectl create namespace monitoring
helm install prometheus prometheus-community/kube-prometheus-stack -n monitoring --set grafana.service.type=ClusterIP --set prometheus.service.type=ClusterIP
kubectl get svc -n monitoring
kubectl --namespace monitoring get secret prometheus-grafana -o jsonpath="{.data.admin-password}" | ForEach-Object { [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($_)) }

# Port mapping for grafana
kubectl --namespace monitoring port-forward svc/prometheus-grafana 3000:80

# Port mapping for prometheus master
kubectl --namespace monitoring port-forward svc/prometheus-kube-prometheus-prometheus 9090:9090

# Port mapping for k8s metrics endpoint
kubectl --namespace monitoring port-forward svc/prometheus-kube-state-metrics 8080:8080

