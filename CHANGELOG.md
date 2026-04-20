# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning v2.0.0](https://semver.org/spec/v2.0.0.html).

## [1.x.x] - In progress

### Changed

- Add dotnet install step to CI/CD
- Upgrade .NET version to 10
- Organize ports for apps, containers, services
- Run sonar scan only on auth service CI, skip other services

## [1.0.0] - 18-Apr-2026

### Changed

- Terraform tech debt
- HELM charts
- Merge gitignore files
- Update gitversion tasks
- HELM deployment pipelines
- Terraform infrastructure provision azure pipelines
- Cloudflare DNS automation using PowerShell
- Move ACR to separate resource group
- K8s Rollout deploy pipeline deprecated
- Merge two docker builds templates for docker build and push
- Add integration tests project to auth service
- Create a separate powershell script for build and tag docker images
- Move configure AKS cluster to separate pipeline
- Cloudflare DNS automation using PowerShell in Azure pipelines
- Configure CD with AKS and HELM
- Helm deploy powershell script
- Move ingress and certificates to HELM chart
- Install Prometheus Grafana Alert manager using HELM
- Configure ingress for Prometheus Grafana Alert manager
- Fix encoding
- Merge plan and plan-destroy terraform pipelines
- Configure HPA for services
- Fix the error Unable to unprotect the message.State.
- ArgoCD initial config
- ArgoCD application manifest
- Add nuget.config
- Delete ArgoCD configs
- Add Renovate dashboard link to README
- Update GitVersion tasks in Azure DevOps
- Add SonarCloud scan to Azure DevOps PR validation
- Add separate build pipelines in Azure DevOps
- Split pipelines by folders: pr validation, build, infrastructure
- Update Cloudflare step: add only DNS record for ingress
- Add deployed project URLs to README
- Platform: Change PostgeSQL to be cluster IP
- Disable terraform log info
- Add AKS node pool name
- Platform updates
- Azure DevOps library terraform
- Wrap platform postgres deployment to powershell script
- Update verify encoding script
- Rename platform pipeline
- Run SonarCloud scan only if source branch is main (add if condition)
- For Docker CI/CD build: add one more tag with short commit sha
- Add build pipelines trigger on tag
- Docker script add logs
