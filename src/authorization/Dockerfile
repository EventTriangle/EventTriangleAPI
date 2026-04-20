FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

WORKDIR /app

EXPOSE 8080

#######################################################################################################
#                                          BUILD ANGULAR
#######################################################################################################

FROM node:18.15.0-alpine AS angularBuild

WORKDIR /angular/EventTriangle.Client

COPY authorization/EventTriangle.Client/package.json ./
COPY authorization/EventTriangle.Client/package-lock.json ./
COPY authorization/EventTriangle.Client/angular.json ./
COPY authorization/EventTriangle.Client/tsconfig*.json ./

RUN npm ci

COPY authorization/EventTriangle.Client/src ./src

RUN npm run build -- --output-path dist/client

#######################################################################################################
#                                          BUILD DOTNET
#######################################################################################################

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS publish

WORKDIR /src

COPY ["authorization/EventTriangleAPI.Authorization.BusinessLogic/EventTriangleAPI.Authorization.BusinessLogic.csproj", "authorization/EventTriangleAPI.Authorization.BusinessLogic/"]
COPY ["authorization/EventTriangleAPI.Authorization.Domain/EventTriangleAPI.Authorization.Domain.csproj", "authorization/EventTriangleAPI.Authorization.Domain/"]
COPY ["authorization/EventTriangleAPI.Authorization.Persistence/EventTriangleAPI.Authorization.Persistence.csproj", "authorization/EventTriangleAPI.Authorization.Persistence/"]
COPY ["authorization/EventTriangleAPI.Authorization.Presentation/EventTriangleAPI.Authorization.Presentation.csproj", "authorization/EventTriangleAPI.Authorization.Presentation/"]
COPY ["shared/EventTriangleAPI.Shared.Application/EventTriangleAPI.Shared.Application.csproj", "shared/EventTriangleAPI.Shared.Application/"]
COPY ["shared/EventTriangleAPI.Shared.DTO/EventTriangleAPI.Shared.DTO.csproj", "shared/EventTriangleAPI.Shared.DTO/"]

RUN dotnet restore "authorization/EventTriangleAPI.Authorization.Presentation/EventTriangleAPI.Authorization.Presentation.csproj"

COPY . .

WORKDIR "authorization/EventTriangleAPI.Authorization.Presentation"

ARG VERSION

RUN dotnet publish "EventTriangleAPI.Authorization.Presentation.csproj" -c Release -p:Version=$VERSION -o /app/publish /p:UseAppHost=false

#######################################################################################################
#                                          CREATE FINAL ARTIFACT
#######################################################################################################

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=angularBuild "/angular/EventTriangle.Client/dist/client" wwwroot
ENTRYPOINT ["dotnet", "EventTriangleAPI.Authorization.Presentation.dll"]
