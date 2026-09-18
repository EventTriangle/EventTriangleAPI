
$ingressService = $( kubectl get service "nginx-ingress-dev-ingress-nginx-controller" -n "ing-dev" -o json ) | ConvertFrom-Json
$ingressPublicIp = $ingressService.status.loadBalancer.ingress[0].ip

$dnsRecords = @{}

$dnsRecords["auth-eventtriangle.razumovsky.me"] = $ingressPublicIp

return $dnsRecords
