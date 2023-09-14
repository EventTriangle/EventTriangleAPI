FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81

ARG FRONT_API_URL

FROM node:18.15.0-alpine AS angularBuild
WORKDIR /angular
COPY ["authorization/EventTriangle.Client/package.json", "EventTriangle.Client/"]
COPY ["authorization/EventTriangle.Client/package-lock.json", "EventTriangle.Client/"]
WORKDIR "EventTriangle.Client"
RUN npm ci
WORKDIR /angular
COPY ["authorization/EventTriangle.Client", "EventTriangle.Client/"]
RUN npm install -g @angular/cli@15.2.7
WORKDIR "EventTriangle.Client"
RUN sed -i "s|https://localhost:7000/|$FRONT_API_URL|" ./src/assets/config/config.json
RUN ng build --output-path "dist/client"

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["authorization/EventTriangleAPI.Authorization.BusinessLogic/EventTriangleAPI.Authorization.BusinessLogic.csproj", "authorization/EventTriangleAPI.Authorization.BusinessLogic/"]
COPY ["authorization/EventTriangleAPI.Authorization.Domain/EventTriangleAPI.Authorization.Domain.csproj", "authorization/EventTriangleAPI.Authorization.Domain/"]
COPY ["authorization/EventTriangleAPI.Authorization.Persistence/EventTriangleAPI.Authorization.Persistence.csproj", "authorization/EventTriangleAPI.Authorization.Persistence/"]
COPY ["authorization/EventTriangleAPI.Authorization.Presentation/EventTriangleAPI.Authorization.Presentation.csproj", "authorization/EventTriangleAPI.Authorization.Presentation/"]
COPY ["authorization/EventTriangleAPI.Authorization.UnitTests/EventTriangleAPI.Authorization.UnitTests.csproj", "authorization/EventTriangleAPI.Authorization.UnitTests/"]
COPY ["shared/EventTriangleAPI.Shared.Application/EventTriangleAPI.Shared.Application.csproj", "shared/EventTriangleAPI.Shared.Application/"]
COPY ["shared/EventTriangleAPI.Shared.DTO/EventTriangleAPI.Shared.DTO.csproj", "shared/EventTriangleAPI.Shared.DTO/"]
RUN dotnet restore "authorization/EventTriangleAPI.Authorization.Presentation/EventTriangleAPI.Authorization.Presentation.csproj"
COPY . .
WORKDIR "authorization/EventTriangleAPI.Authorization.Presentation"
RUN dotnet publish "EventTriangleAPI.Authorization.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-cache

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=angularBuild "/angular/EventTriangle.Client/dist/client" wwwroot
ENTRYPOINT ["dotnet", "EventTriangleAPI.Authorization.Presentation.dll"]
