
$ingressService = $( kubectl get service "event-ingress-ingress-nginx-controller" -n "event-triangle" -o json ) | ConvertFrom-Json
$ingressPublicIp = $ingressService.status.loadBalancer.ingress[0].ip

$dnsRecords = @{}

$dnsRecords["auth-eventtriangle.razumovsky.me"] = $ingressPublicIp

return $dnsRecords
