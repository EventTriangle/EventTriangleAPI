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
FROM node:24.16.0-alpine AS angular

WORKDIR /app/client

# Install dependencies before copying Angular configuration and source.
COPY FluxPay.Client/package.json ./
COPY FluxPay.Client/package-lock.json ./
RUN npm install --global --ignore-scripts npm@11.16.0 \
    && npm ci --ignore-scripts

COPY FluxPay.Client/angular.json ./
COPY FluxPay.Client/tsconfig*.json ./

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
     authorization/FluxPayAPI.Authorization.BusinessLogic/

COPY FluxPayAPI.Authorization.Domain/*.csproj \
     authorization/FluxPayAPI.Authorization.Domain/

COPY FluxPayAPI.Authorization.Persistence/*.csproj \
     authorization/FluxPayAPI.Authorization.Persistence/

COPY FluxPayAPI.Authorization.Presentation/*.csproj \
     authorization/FluxPayAPI.Authorization.Presentation/

# Shared projects come from the additional BuildKit build context named "shared".
COPY --from=shared FluxPayAPI.Shared.Application/*.csproj \
     shared/FluxPayAPI.Shared.Application/

COPY --from=shared FluxPayAPI.Shared.DTO/*.csproj \
     shared/FluxPayAPI.Shared.DTO/

RUN dotnet restore \
    authorization/FluxPayAPI.Authorization.Presentation/FluxPayAPI.Authorization.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS build

# Keep Angular changes out of the .NET build cache key.
COPY FluxPayAPI.Authorization.BusinessLogic/ authorization/FluxPayAPI.Authorization.BusinessLogic/
COPY FluxPayAPI.Authorization.Domain/ authorization/FluxPayAPI.Authorization.Domain/
COPY FluxPayAPI.Authorization.Persistence/ authorization/FluxPayAPI.Authorization.Persistence/
COPY FluxPayAPI.Authorization.Presentation/ authorization/FluxPayAPI.Authorization.Presentation/

# Copy shared source from the separately supplied BuildKit context.
COPY --from=shared . shared/

WORKDIR /src/authorization/FluxPayAPI.Authorization.Presentation

RUN dotnet publish \
    -c Release \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false

#######################################################################################################
# FINAL IMAGE
#######################################################################################################
FROM base AS final

WORKDIR /app

COPY --from=build /app/publish ./
COPY --from=angular /app/client/dist/client/browser ./wwwroot

USER $APP_UID

ENTRYPOINT ["dotnet", "FluxPayAPI.Authorization.Presentation.dll"]
