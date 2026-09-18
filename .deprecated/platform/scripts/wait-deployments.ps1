param (
    [string]$Namespace,
    [string]$RabbitRelease,
    [string]$PostgresDeployment
)

Write-Output "Checking if helm releases are ready..."
Write-Output "Printing Helm releases..."
$helmReleases = $( helm list -n $Namespace )
Write-Output $helmReleases
$rabbitExists = $helmReleases -match $RabbitRelease

Write-Output "Checking deployment status..."
$deployments = $( kubectl get deployments -n $Namespace )
Write-Output $deployments
$postgresExists = $deployments -match $PostgresDeployment

if ($rabbitExists -and $postgresExists)
{
    Write-Output "All deployments are ready!"
    exit 0
}

Write-Output "Not all deployments are ready... Sleeping for 60 seconds..."
sleep 60

# example call:
# ./wait-deployments.ps1 -Namespace "event-triangle" -RabbitRelease "event-rabbitmq" -PostgresDeployment "postgresdb"
