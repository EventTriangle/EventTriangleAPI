param(
    [Parameter(Mandatory = $true)]
    [string] $RabbitMqNamespace,

    [Parameter(Mandatory = $true)]
    [string] $RabbitMqClusterName
)

$ErrorActionPreference = "Stop"

# ----------------------------------------------------
# 1. CHECK IF CLUSTER EXISTS
# ----------------------------------------------------
Write-Host "Checking if RabbitMQ cluster exists..."

$existing = kubectl get rabbitmqcluster $RabbitMqClusterName -n $RabbitMqNamespace --ignore-not-found

if ($existing) {
    Write-Host "RabbitMQ cluster already exists. Skipping installation."
    exit 0
}

# ----------------------------------------------------
# 2. INSTALL OPERATOR
# ----------------------------------------------------
Write-Host "Installing RabbitMQ Cluster Operator..."

kubectl apply -f "https://github.com/rabbitmq/cluster-operator/releases/latest/download/cluster-operator.yml"

# ----------------------------------------------------
# 3. WAIT FOR CRD
# ----------------------------------------------------
Write-Host "Waiting for CRDs..."

kubectl wait --for=condition=Established crd/rabbitmqclusters.rabbitmq.com --timeout=120s

# ----------------------------------------------------
# 4. WAIT FOR OPERATOR
# ----------------------------------------------------
Write-Host "Waiting for operator rollout..."

kubectl rollout status deployment/rabbitmq-cluster-operator -n rabbitmq-system --timeout=180s

# ----------------------------------------------------
# 5. CREATE NAMESPACE (SAFE / IDEMPOTENT)
# ----------------------------------------------------
Write-Host "Ensuring namespace exists..."

kubectl create namespace $RabbitMqNamespace --dry-run=client -o yaml | kubectl apply -f -

# ----------------------------------------------------
# 6. APPLY RABBITMQ CLUSTER
# ----------------------------------------------------
Write-Host "Creating RabbitMQ cluster..."

$yaml = @"
apiVersion: rabbitmq.com/v1beta1
kind: RabbitmqCluster
metadata:
  name: $RabbitMqClusterName
  namespace: $RabbitMqNamespace
spec:
  replicas: 1
"@

$yaml | kubectl apply -f -

# ----------------------------------------------------
# 7. WAIT FOR CLUSTER READINESS (IMPORTANT PART)
# ----------------------------------------------------
Write-Host "Waiting for RabbitMQ cluster to become Ready..."

do {
    $json = kubectl get rabbitmqcluster $RabbitMqClusterName -n $RabbitMqNamespace -o json | ConvertFrom-Json

    $conditions = $json.status.conditions

    $allReplicasReady = ($conditions | Where-Object { $_.type -eq "AllReplicasReady" }).status
    $reconcileSuccess  = ($conditions | Where-Object { $_.type -eq "ReconcileSuccess" }).status

    Write-Host "AllReplicasReady=$allReplicasReady ReconcileSuccess=$reconcileSuccess"

    $ready = ($allReplicasReady -eq "True" -and $reconcileSuccess -eq "True")

    if (-not $ready) {
        Start-Sleep 5
    }

} while (-not $ready)

Write-Host "RabbitMQ cluster is READY"

# ----------------------------------------------------
# 8. OPTIONAL: VERIFY PODS
# ----------------------------------------------------
Write-Host "Verifying pods..."

kubectl get pods -n $RabbitMqNamespace

# .\rabbitmq\Install-RabbitMq.ps1 -RabbitMqNamespace "rabbitmq-dev" -RabbitMqClusterName "rabbitmq-cluster"
