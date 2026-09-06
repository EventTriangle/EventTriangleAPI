#######################################################################################################
# BASE RUNTIME
#######################################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

EXPOSE 8080

#######################################################################################################
# DOTNET RESTORE
#######################################################################################################
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore

WORKDIR /src

# Copy only project files first for better restore caching.
COPY EventTriangleAPI.Consumer.Application/*.csproj \
     consumer/EventTriangleAPI.Consumer.Application/

COPY EventTriangleAPI.Consumer.BusinessLogic/*.csproj \
     consumer/EventTriangleAPI.Consumer.BusinessLogic/

COPY EventTriangleAPI.Consumer.Domain/*.csproj \
     consumer/EventTriangleAPI.Consumer.Domain/

COPY EventTriangleAPI.Consumer.Persistence/*.csproj \
     consumer/EventTriangleAPI.Consumer.Persistence/

COPY EventTriangleAPI.Consumer.Presentation/*.csproj \
     consumer/EventTriangleAPI.Consumer.Presentation/

# Shared projects come from the named BuildKit context "shared".
COPY --from=shared EventTriangleAPI.Shared.Application/*.csproj \
     shared/EventTriangleAPI.Shared.Application/

COPY --from=shared EventTriangleAPI.Shared.DTO/*.csproj \
     shared/EventTriangleAPI.Shared.DTO/

RUN dotnet restore \
    consumer/EventTriangleAPI.Consumer.Presentation/EventTriangleAPI.Consumer.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS publish

# Main build context is src/consumer.
COPY . consumer/

# Shared source comes from the separate named build context.
COPY --from=shared . shared/

WORKDIR /src/consumer/EventTriangleAPI.Consumer.Presentation

ARG VERSION

RUN dotnet publish \
    EventTriangleAPI.Consumer.Presentation.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

#######################################################################################################
# FINAL IMAGE
#######################################################################################################
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "EventTriangleAPI.Consumer.Presentation.dll"]
