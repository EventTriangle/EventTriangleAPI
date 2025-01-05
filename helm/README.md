# Commands

## HELM Create

- `helm create auth-service-chart`
- `helm create sender-service-chart`
- `helm create consumer-service-chart`

## HELM Lint

- `helm lint .\helm\auth-service-chart\`
- `helm lint .\helm\sender-service-chart\`
- `helm lint .\helm\consumer-service-chart\`
- `helm template event-triangle-auth .\helm\auth-service-chart\ --values .\helm\auth-service-chart\values.yaml`

## HELM Install

- `helm install event-triangle-auth .\helm\auth-service-chart\ --values .\helm\auth-service-chart\values.yaml`
- `helm install event-triangle-consumer .\helm\consumer-service-chart\ --values .\helm\consumer-service-chart\values.yaml`
