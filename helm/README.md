# Commands

## HELM Create

- helm create auth-service-chart
- helm create consumer-service-chart
- helm create sender-service-chart

## HELM Lint

- helm lint .\auth-service-chart\
- helm lint .\consumer-service-chart\
- helm lint .\sender-service-chart\

## HELM Print YAMLs

- helm template event-triangle-auth .\auth-service-chart\ --values .\auth-service-chart\values.yaml
- helm template event-triangle-auth .\consumer-service-chart\ --values .\consumer-service-chart\values.yaml
- helm template event-triangle-auth .\sender-service-chart\ --values .\sender-service-chart\values.yaml
- helm template event-triangle-auth .\sender-service-chart\ --values .\sender-service-chart\values.yaml --set
  image.tag=new-tag

## HELM Install

- helm install auth-service-chart .\auth-service-chart\ --values .\auth-service-chart\values.yaml --namespace "event-triangle"
- helm install consumer-service-chart .\consumer-service-chart\ --values .\consumer-service-chart\values.yaml --namespace "event-triangle"
- helm install sender-service-chart .\sender-service-chart\ --values .\sender-service-chart\values.yaml --namespace "event-triangle"
- helm upgrade --install event-triangle-auth .\helm\auth-service-chart --values .\helm\auth-service-chart\values.yaml --set image.tag="0.1.3-helm.527" --namespace "event-triangle"

## HELM Upgrade

- helm upgrade event-triangle-auth .\auth-service-chart\ --values .\auth-service-chart\values.yaml
- helm upgrade event-triangle-consumer .\consumer-service-chart\ --values .\consumer-service-chart\values.yaml
- helm upgrade event-triangle-sender .\sender-service-chart\ --values .\sender-service-chart\values.yaml

## HELM Uninstall

- helm uninstall event-triangle-auth -n event-triangle
- helm uninstall event-triangle-consumer -n event-triangle
- helm uninstall event-triangle-sender -n event-triangle
