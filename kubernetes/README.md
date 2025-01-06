# Manifest deployment order

- kubectl apply -f .\auth-deployment-cluster-ip\
- kubectl apply -f .\consumer-deployment-cluster-ip\
- kubectl apply -f .\sender-deployment-cluster-ip\
- kubectl apply -f .\certificate-issuer-letsencrypt\
- kubectl apply -f .\auth-https-nginx-ingress\
- kubectl apply -f .\certificate\ -n "event-triangle"

# Delete all resources

- kubectl delete -f .\auth-deployment-cluster-ip\
- kubectl delete -f .\consumer-deployment-cluster-ip\
- kubectl delete -f .\sender-deployment-cluster-ip\
- kubectl delete -f .\certificate-issuer-letsencrypt\
- kubectl delete -f .\auth-https-nginx-ingress\
- kubectl delete -f .\certificate\ -n "event-triangle"
