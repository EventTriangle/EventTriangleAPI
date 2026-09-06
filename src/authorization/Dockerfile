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
COPY EventTriangle.Client/package.json ./
COPY EventTriangle.Client/package-lock.json ./
COPY EventTriangle.Client/angular.json ./
COPY EventTriangle.Client/tsconfig*.json ./

RUN npm ci

# Copy application source only after dependencies are installed.
COPY EventTriangle.Client/src ./src

RUN npm run build -- --configuration production --output-path dist/client

#######################################################################################################
# DOTNET RESTORE
#######################################################################################################
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore

WORKDIR /src

# Copy only project files first.
# This keeps the restore layer cached while only source code changes.
COPY EventTriangleAPI.Authorization.BusinessLogic/*.csproj \
     EventTriangleAPI.Authorization.BusinessLogic/

COPY EventTriangleAPI.Authorization.Domain/*.csproj \
     EventTriangleAPI.Authorization.Domain/

COPY EventTriangleAPI.Authorization.Persistence/*.csproj \
     EventTriangleAPI.Authorization.Persistence/

COPY EventTriangleAPI.Authorization.Presentation/*.csproj \
     EventTriangleAPI.Authorization.Presentation/

# Shared projects come from the additional BuildKit build context named "shared".
COPY --from=shared EventTriangleAPI.Shared.Application/*.csproj \
     shared/EventTriangleAPI.Shared.Application/

COPY --from=shared EventTriangleAPI.Shared.DTO/*.csproj \
     shared/EventTriangleAPI.Shared.DTO/

RUN dotnet restore \
    EventTriangleAPI.Authorization.Presentation/EventTriangleAPI.Authorization.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS build

# Main build context is src/authorization.
COPY . authorization/

# Copy shared source from the separately supplied BuildKit context.
COPY --from=shared . shared/

WORKDIR /src/authorization/EventTriangleAPI.Authorization.Presentation

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

ENTRYPOINT ["dotnet", "EventTriangleAPI.Authorization.Presentation.dll"]
