FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["consumer/EventTriangleAPI.Consumer.Application/EventTriangleAPI.Consumer.Application.csproj", "consumer/EventTriangleAPI.Consumer.Application/"]
COPY ["consumer/EventTriangleAPI.Consumer.BusinessLogic/EventTriangleAPI.Consumer.BusinessLogic.csproj", "consumer/EventTriangleAPI.Consumer.BusinessLogic/"]
COPY ["consumer/EventTriangleAPI.Consumer.Domain/EventTriangleAPI.Consumer.Domain.csproj", "consumer/EventTriangleAPI.Consumer.Domain/"]
COPY ["consumer/EventTriangleAPI.Consumer.Persistence/EventTriangleAPI.Consumer.Persistence.csproj", "consumer/EventTriangleAPI.Consumer.Persistence/"]
COPY ["consumer/EventTriangleAPI.Consumer.Presentation/EventTriangleAPI.Consumer.Presentation.csproj", "consumer/EventTriangleAPI.Consumer.Presentation/"]
COPY ["consumer/EventTriangleAPI.Consumer.UnitTests/EventTriangleAPI.Consumer.UnitTests.csproj", "consumer/EventTriangleAPI.Consumer.UnitTests/"]
COPY ["shared/EventTriangleAPI.Shared.Application/EventTriangleAPI.Shared.Application.csproj", "shared/EventTriangleAPI.Shared.Application/"]
COPY ["shared/EventTriangleAPI.Shared.DTO/EventTriangleAPI.Shared.DTO.csproj", "shared/EventTriangleAPI.Shared.DTO/"]
RUN dotnet restore "consumer/EventTriangleAPI.Consumer.Presentation/EventTriangleAPI.Consumer.Presentation.csproj"
COPY . .
WORKDIR "consumer/EventTriangleAPI.Consumer.Presentation"
ARG VERSION
RUN dotnet publish "EventTriangleAPI.Consumer.Presentation.csproj" -c Release -p:Version=$VERSION -o /app/publish /p:UseAppHost=false --no-cache

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventTriangleAPI.Consumer.Presentation.dll"]
