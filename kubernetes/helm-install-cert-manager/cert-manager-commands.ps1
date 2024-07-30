helm repo add jetstack https://charts.jetstack.io
helm repo update

kubectl apply --validate=false -f https://github.com/jetstack/cert-manager/releases/download/v1.12.0/cert-manager.crds.yaml
helm install cert-manager jetstack/cert-manager --namespace cert-manager --create-namespace
helm install cert-manager jetstack/cert-manager --namespace event-triangle