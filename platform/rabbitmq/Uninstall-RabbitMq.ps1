param(
    [Parameter(Mandatory = $true)]
    [string] $RabbitMqNamespace,

    [Parameter(Mandatory = $true)]
    [string] $RabbitMqClusterName
)

$ErrorActionPreference = "Stop"

Write-Host "Checking if RabbitMQ cluster exists..."

$exists = kubectl get rabbitmqcluster $RabbitMqClusterName -n $RabbitMqNamespace --ignore-not-found

if (-not $exists) {
    Write-Host "Cluster does not exist. Skipping deletion."
    exit 0
}

# ----------------------------------------------------
# 1. DELETE RABBITMQ CLUSTER (CR)
# ----------------------------------------------------
Write-Host "Deleting RabbitMQ cluster CR..."

kubectl delete rabbitmqcluster $RabbitMqClusterName -n $RabbitMqNamespace --wait=true

# ----------------------------------------------------
# 2. WAIT UNTIL PODS ARE GONE
# ----------------------------------------------------
Write-Host "Waiting for RabbitMQ pods to terminate..."

while (kubectl get pods -n $RabbitMqNamespace -l app.kubernetes.io/name=rabbitmq --no-headers 2>$null) {
    Write-Host "Pods still terminating..."
    Start-Sleep 5
}

# ----------------------------------------------------
# 3. DELETE PVCs (VERY IMPORTANT)
# ----------------------------------------------------
Write-Host "Deleting PVCs..."

kubectl delete pvc -n $RabbitMqNamespace --all --wait=true

# ----------------------------------------------------
# 4. DELETE OPERATOR
# ----------------------------------------------------
Write-Host "Deleting RabbitMQ Cluster Operator..."

kubectl delete -f "https://github.com/rabbitmq/cluster-operator/releases/latest/download/cluster-operator.yml" --ignore-not-found

# ----------------------------------------------------
# 5. FORCE CLEANUP NAMESPACE (SAFE ONLY AFTER ABOVE)
# ----------------------------------------------------
Write-Host "Final namespace cleanup..."

kubectl delete namespace $RabbitMqNamespace --grace-period=0 --force

Write-Host "Cleanup completed."
