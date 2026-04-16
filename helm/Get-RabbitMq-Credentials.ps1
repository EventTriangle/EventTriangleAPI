$ErrorActionPreference = "Stop"

$InitialLocation = Get-Location

Write-Host "Setting location to PSScriptRoot ..."

Set-Location $PSScriptRoot

$Username = kubectl get secret rabbitmq-cluster-default-user -n rabbitmq-dev -o jsonpath="{.data.username}"

Write-Host "Successfully retrieved username value."

$Password = kubectl get secret rabbitmq-cluster-default-user -n rabbitmq-dev -o jsonpath="{.data.password}"

Write-Host "Successfully retrieved password value."

$UsernameDecode = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($Username))
$PasswordDecode = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($Password))

$Creds = @{}

$Creds["username"] = $UsernameDecode
$Creds["password"] = $PasswordDecode

Write-Host "Changing location to initial ..."

Set-Location $InitialLocation

return $Creds