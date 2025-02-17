az aks get-credentials --resource-group $(terraform output -raw rg_name) --name $(terraform output -raw aks_name) --subscription $(terraform output -raw subscription)

helm repo add bitnami https://charts.bitnami.com/bitnami

helm repo update

helm install event-rabbitmq bitnami/rabbitmq `
  --namespace event-triangle `
  --set auth.username=guest `
  --set auth.password=guest `
  --set service.type=LoadBalancer

helm upgrade event-rabbitmq bitnami/rabbitmq `
  --set auth.username=guest `
  --set auth.password=guest

helm uninstall event-rabbitmq

kubectl get endpoints

kubectl describe service "event-rabbitmq"
