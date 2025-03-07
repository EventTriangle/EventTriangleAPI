helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update

# Simple install command
helm upgrade --install event-redis bitnami/redis --namespace event-triangle

# Set password as a parameter
helm upgrade --install event-redis bitnami/redis `
    --namespace event-triangle `
    --set auth.enabled=true `
    --set auth.password="MySecureRedisPassword"

# Get the base64 encoded password from the Kubernetes secret
$encodedPassword = kubectl get secret --namespace event-triangle event-redis -o jsonpath="{.data.redis-password}"

# Decode the base64 string
$decodedPassword = [System.Text.Encoding]::UTF8.GetString([Convert]::FromBase64String($encodedPassword))

# Set the decoded password into the variable
$REDIS_PASSWORD = $decodedPassword
