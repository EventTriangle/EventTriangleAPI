FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 433

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY ["sender/EventTriangleAPI.Sender.Application/EventTriangleAPI.Sender.Application.csproj", "sender/EventTriangleAPI.Sender.Application/"]
COPY ["sender/EventTriangleAPI.Sender.BusinessLogic/EventTriangleAPI.Sender.BusinessLogic.csproj", "sender/EventTriangleAPI.Sender.BusinessLogic/"]
COPY ["sender/EventTriangleAPI.Sender.Domain/EventTriangleAPI.Sender.Domain.csproj", "sender/EventTriangleAPI.Sender.Domain/"]
COPY ["sender/EventTriangleAPI.Sender.Persistence/EventTriangleAPI.Sender.Persistence.csproj", "sender/EventTriangleAPI.Sender.Persistence/"]
COPY ["sender/EventTriangleAPI.Sender.Presentation/EventTriangleAPI.Sender.Presentation.csproj", "sender/EventTriangleAPI.Sender.Presentation/"]
COPY ["sender/EventTriangleAPI.Sender.UnitTests/EventTriangleAPI.Sender.UnitTests.csproj", "sender/EventTriangleAPI.Sender.UnitTests/"]
COPY ["shared/EventTriangleAPI.Shared.Application/EventTriangleAPI.Shared.Application.csproj", "shared/EventTriangleAPI.Shared.Application/"]
COPY ["shared/EventTriangleAPI.Shared.DTO/EventTriangleAPI.Shared.DTO.csproj", "shared/EventTriangleAPI.Shared.DTO/"]
RUN dotnet restore "sender/EventTriangleAPI.Sender.Presentation/EventTriangleAPI.Sender.Presentation.csproj"
COPY . .
WORKDIR "sender/EventTriangleAPI.Sender.Presentation"
RUN dotnet publish "EventTriangleAPI.Sender.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventTriangleAPI.Sender.Presentation.dll"]
