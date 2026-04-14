$postgresService = $( kubectl get service "postgres-service" -n "event-triangle" -o json ) | ConvertFrom-Json
$postgresPublicIp = $postgresService.status.loadBalancer.ingress[0].ip

$rabbitService = $( kubectl get service "event-rabbitmq" -n "event-triangle" -o json ) | ConvertFrom-Json
$rabbitPublicIp = $rabbitService.status.loadBalancer.ingress[0].ip

$ingressService = $( kubectl get service "event-ingress-ingress-nginx-controller" -n "event-triangle" -o json ) | ConvertFrom-Json
$ingressPublicIp = $ingressService.status.loadBalancer.ingress[0].ip
$dnsRecords = @{}

$dnsRecords["auth-eventtriangle.razumovsky.me"] = $ingressPublicIp
$dnsRecords["rabbitmq-eventtriangle.razumovsky.me"] = $rabbitPublicIp
$dnsRecords["postgres-eventtriangle.razumovsky.me"] = $postgresPublicIp

return $dnsRecords
