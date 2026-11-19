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
COPY FluxPayAPI.Consumer.Application/*.csproj \
     consumer/FluxPayAPI.Consumer.Application/

COPY FluxPayAPI.Consumer.BusinessLogic/*.csproj \
     consumer/FluxPayAPI.Consumer.BusinessLogic/

COPY FluxPayAPI.Consumer.Domain/*.csproj \
     consumer/FluxPayAPI.Consumer.Domain/

COPY FluxPayAPI.Consumer.Persistence/*.csproj \
     consumer/FluxPayAPI.Consumer.Persistence/

COPY FluxPayAPI.Consumer.Presentation/*.csproj \
     consumer/FluxPayAPI.Consumer.Presentation/

# Shared projects come from the named BuildKit context "shared".
COPY --from=shared FluxPayAPI.Shared.Application/*.csproj \
     shared/FluxPayAPI.Shared.Application/

COPY --from=shared FluxPayAPI.Shared.DTO/*.csproj \
     shared/FluxPayAPI.Shared.DTO/

RUN dotnet restore \
    consumer/FluxPayAPI.Consumer.Presentation/FluxPayAPI.Consumer.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS publish

# Main build context is src/consumer.
COPY . consumer/

# Shared source comes from the separate named build context.
COPY --from=shared . shared/

WORKDIR /src/consumer/FluxPayAPI.Consumer.Presentation

RUN dotnet publish \
    FluxPayAPI.Consumer.Presentation.csproj \
    -c Release \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false

#######################################################################################################
# FINAL IMAGE
#######################################################################################################
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "FluxPayAPI.Consumer.Presentation.dll"]
