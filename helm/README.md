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

- helm install event-triangle-auth .\auth-service-chart\ --values .\auth-service-chart\values.yaml
- helm install event-triangle-consumer .\consumer-service-chart\ --values .\consumer-service-chart\values.yaml
- helm install event-triangle-sender .\sender-service-chart\ --values .\sender-service-chart\values.yaml

## HELM Upgrade

- helm upgrade event-triangle-auth .\auth-service-chart\ --values .\auth-service-chart\values.yaml
- helm upgrade event-triangle-consumer .\consumer-service-chart\ --values .\consumer-service-chart\values.yaml
- helm upgrade event-triangle-sender .\sender-service-chart\ --values .\sender-service-chart\values.yaml

## HELM Uninstall

- helm uninstall event-triangle-auth -n event-triangle
- helm uninstall event-triangle-consumer -n event-triangle
- helm uninstall event-triangle-sender -n event-triangle
