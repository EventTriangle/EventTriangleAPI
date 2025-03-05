# K8s Deployment notes

## Deployment order

After Terraform pipeline is run, the following steps are required to deploy applications

- Deploy Auth service
- Deploy Consumer service
- Deploy Sender service
- Deploy Certificate issuer (LetsEncrypt)
- Deploy Certificate
- Deploy HTTPS Ingress controller for Auth service
- Update DNS records for Auth service

At this point TLS certificate should be issued and whole project must be accessible via HTTPS
at https://auth.eventtriangle.razumovsky.me/app/transactions