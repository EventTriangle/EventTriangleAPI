$postgresService = $( kubectl get service "postgres-service" -n "fluxpay" -o json ) | ConvertFrom-Json
$postgresPublicIp = $postgresService.status.loadBalancer.ingress[0].ip
Write-Output "Postgres Public IP: $postgresPublicIp"

$rabbitService = $( kubectl get service "event-rabbitmq" -n "fluxpay" -o json ) | ConvertFrom-Json
$rabbitPublicIp = $rabbitService.status.loadBalancer.ingress[0].ip
Write-Output "RabbitMQ Public IP: $rabbitPublicIp"

$ingressService = $( kubectl get service "event-ingress-ingress-nginx-controller" -n "fluxpay" -o json ) | ConvertFrom-Json
$ingressPublicIp = $ingressService.status.loadBalancer.ingress[0].ip
Write-Output "Ingress Public IP: $ingressPublicIp"
