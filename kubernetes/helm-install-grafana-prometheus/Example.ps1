helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
kubectl create namespace monitoring
helm install prometheus prometheus-community/kube-prometheus-stack -n monitoring --set grafana.service.type=ClusterIP --set prometheus.service.type=ClusterIP
kubectl get svc -n monitoring

