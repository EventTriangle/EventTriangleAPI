helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
kubectl create namespace monitoring
helm install prometheus prometheus-community/kube-prometheus-stack -n monitoring --set grafana.service.type=ClusterIP --set prometheus.service.type=ClusterIP
kubectl get svc -n monitoring
kubectl --namespace monitoring get secret prometheus-grafana -o jsonpath="{.data.admin-password}" | ForEach-Object { [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($_)) }

# Port mapping for grafana
$POD_NAME = kubectl --namespace monitoring get pod -l "app.kubernetes.io/name=grafana,app.kubernetes.io/instance=prometheus" -o name
kubectl --namespace monitoring port-forward $POD_NAME 3000

# Port mapping for prometheus master
kubectl --namespace monitoring port-forward svc/prometheus-kube-prometheus-prometheus 9090:9090

# Port mapping for k8s metrics endpoint
kubectl --namespace monitoring port-forward svc/prometheus-kube-state-metrics 8080:8080

