## ArgoCD project

- https://github.com/argoproj/argo-cd

## Install ArgoCD CLI (Windows)

- choco install argocd-cli -y
- argocd version

## Install and configure ArgoCD

- kubectl create namespace argocd
- kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/v2.14.7/manifests/install.yaml
- kubectl patch svc argocd-server -n argocd --type merge --patch '{"spec": {"type": "LoadBalancer"}}'
- https://argocd-et.razumovsky.me/
- $Password = kubectl get secret argocd-initial-admin-secret -n argocd -o jsonpath="{.data.password}" | ForEach-Object { [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($_)) }
- argocd login argocd-et.razumovsky.me  --username admin --password $Password --insecure
- argocd repo add "https://github.com/EventTriangle/EventTriangleAPI.git"
