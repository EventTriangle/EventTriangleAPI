#######################################################################################################
# BASE RUNTIME
#######################################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

EXPOSE 8080

ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

#######################################################################################################
# ANGULAR BUILD
#######################################################################################################
FROM node:18.15.0-alpine AS angular

WORKDIR /app/client

# Copy dependency/configuration files first.
# npm ci is cached until one of these files changes.
COPY FluxPay.Client/package.json ./
COPY FluxPay.Client/package-lock.json ./
COPY FluxPay.Client/angular.json ./
COPY FluxPay.Client/tsconfig*.json ./

RUN npm ci

# Copy application source only after dependencies are installed.
COPY FluxPay.Client/src ./src

RUN npm run build -- --configuration production --output-path dist/client

#######################################################################################################
# DOTNET RESTORE
#######################################################################################################
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore

WORKDIR /src

# Copy only project files first.
# This keeps the restore layer cached while only source code changes.
COPY FluxPayAPI.Authorization.BusinessLogic/*.csproj \
     FluxPayAPI.Authorization.BusinessLogic/

COPY FluxPayAPI.Authorization.Domain/*.csproj \
     FluxPayAPI.Authorization.Domain/

COPY FluxPayAPI.Authorization.Persistence/*.csproj \
     FluxPayAPI.Authorization.Persistence/

COPY FluxPayAPI.Authorization.Presentation/*.csproj \
     FluxPayAPI.Authorization.Presentation/

# Shared projects come from the additional BuildKit build context named "shared".
COPY --from=shared FluxPayAPI.Shared.Application/*.csproj \
     shared/FluxPayAPI.Shared.Application/

COPY --from=shared FluxPayAPI.Shared.DTO/*.csproj \
     shared/FluxPayAPI.Shared.DTO/

RUN dotnet restore \
    FluxPayAPI.Authorization.Presentation/FluxPayAPI.Authorization.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS build

# Main build context is src/authorization.
COPY . authorization/

# Copy shared source from the separately supplied BuildKit context.
COPY --from=shared . shared/

WORKDIR /src/authorization/FluxPayAPI.Authorization.Presentation

ARG VERSION

RUN dotnet publish \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

#######################################################################################################
# FINAL IMAGE
#######################################################################################################
FROM base AS final

WORKDIR /app

COPY --from=build /app/publish ./
COPY --from=angular /app/client/dist/client ./wwwroot

ENTRYPOINT ["dotnet", "FluxPayAPI.Authorization.Presentation.dll"]
