#######################################################################################################
# BASE RUNTIME
#######################################################################################################
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

WORKDIR /app

EXPOSE 8080
EXPOSE 8081

#######################################################################################################
# DOTNET RESTORE
#######################################################################################################
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore

WORKDIR /src

# Copy only project files first.
# This keeps the restore layer cached while source files change.
COPY FluxPayAPI.Sender.Application/*.csproj \
     sender/FluxPayAPI.Sender.Application/

COPY FluxPayAPI.Sender.BusinessLogic/*.csproj \
     sender/FluxPayAPI.Sender.BusinessLogic/

COPY FluxPayAPI.Sender.Domain/*.csproj \
     sender/FluxPayAPI.Sender.Domain/

COPY FluxPayAPI.Sender.Persistence/*.csproj \
     sender/FluxPayAPI.Sender.Persistence/

COPY FluxPayAPI.Sender.Presentation/*.csproj \
     sender/FluxPayAPI.Sender.Presentation/

# Shared projects come from the named BuildKit context "shared".
COPY --from=shared FluxPayAPI.Shared.Application/*.csproj \
     shared/FluxPayAPI.Shared.Application/

COPY --from=shared FluxPayAPI.Shared.DTO/*.csproj \
     shared/FluxPayAPI.Shared.DTO/

RUN dotnet restore \
    sender/FluxPayAPI.Sender.Presentation/FluxPayAPI.Sender.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS publish

# Main build context is src/sender.
COPY . sender/

# Shared source comes from the separate BuildKit context.
COPY --from=shared . shared/

WORKDIR /src/sender/FluxPayAPI.Sender.Presentation

ARG VERSION

RUN dotnet publish \
    FluxPayAPI.Sender.Presentation.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

#######################################################################################################
# FINAL IMAGE
#######################################################################################################
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "FluxPayAPI.Sender.Presentation.dll"]
