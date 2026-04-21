#######################################################################################################
# BASE RUNTIME
#######################################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

#######################################################################################################
# ANGULAR BUILD (CACHE OPTIMIZED)
#######################################################################################################
FROM node:18.15.0-alpine AS angular

WORKDIR /app/client

# 1. dependency layer (VERY STABLE)
COPY authorization/EventTriangle.Client/package.json ./
COPY authorization/EventTriangle.Client/package-lock.json ./
COPY authorization/EventTriangle.Client/angular.json ./
COPY authorization/EventTriangle.Client/tsconfig*.json ./

RUN npm ci

# 2. source layer (changes often)
COPY authorization/EventTriangle.Client/src ./src
COPY authorization/EventTriangle.Client/src/assets ./assets
COPY authorization/EventTriangle.Client/src/favicon.png ./src/favicon.png

RUN npm run build -- --configuration production --output-path dist/client

#######################################################################################################
# DOTNET RESTORE LAYER (MOST IMPORTANT CACHE LAYER)
#######################################################################################################
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

# ONLY csproj files first (MAXIMUM CACHE HIT RATE)
WORKDIR /src

COPY authorization/EventTriangleAPI.Authorization.BusinessLogic/*.csproj authorization/EventTriangleAPI.Authorization.BusinessLogic/
COPY authorization/EventTriangleAPI.Authorization.Domain/*.csproj authorization/EventTriangleAPI.Authorization.Domain/
COPY authorization/EventTriangleAPI.Authorization.Persistence/*.csproj authorization/EventTriangleAPI.Authorization.Persistence/
COPY authorization/EventTriangleAPI.Authorization.Presentation/*.csproj authorization/EventTriangleAPI.Authorization.Presentation/

COPY shared/EventTriangleAPI.Shared.Application/*.csproj shared/EventTriangleAPI.Shared.Application/
COPY shared/EventTriangleAPI.Shared.DTO/*.csproj shared/EventTriangleAPI.Shared.DTO/

RUN dotnet restore authorization/EventTriangleAPI.Authorization.Presentation/EventTriangleAPI.Authorization.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS build

# NOW source code (this invalidates only build, not restore)
COPY authorization/ authorization/
COPY shared/ shared/

WORKDIR /src/authorization/EventTriangleAPI.Authorization.Presentation

ARG VERSION

RUN dotnet publish -c Release \
    -p:Version=$VERSION \
    -o /app/publish \
    /p:UseAppHost=false

#######################################################################################################
# FINAL IMAGE
#######################################################################################################
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY --from=angular /app/client/dist/client wwwroot

ENTRYPOINT ["dotnet", "EventTriangleAPI.Authorization.Presentation.dll"]
