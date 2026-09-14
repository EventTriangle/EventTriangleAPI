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
COPY EventTriangleAPI.Sender.Application/*.csproj \
     sender/EventTriangleAPI.Sender.Application/

COPY EventTriangleAPI.Sender.BusinessLogic/*.csproj \
     sender/EventTriangleAPI.Sender.BusinessLogic/

COPY EventTriangleAPI.Sender.Domain/*.csproj \
     sender/EventTriangleAPI.Sender.Domain/

COPY EventTriangleAPI.Sender.Persistence/*.csproj \
     sender/EventTriangleAPI.Sender.Persistence/

COPY EventTriangleAPI.Sender.Presentation/*.csproj \
     sender/EventTriangleAPI.Sender.Presentation/

# Shared projects come from the named BuildKit context "shared".
COPY --from=shared EventTriangleAPI.Shared.Application/*.csproj \
     shared/EventTriangleAPI.Shared.Application/

COPY --from=shared EventTriangleAPI.Shared.DTO/*.csproj \
     shared/EventTriangleAPI.Shared.DTO/

RUN dotnet restore \
    sender/EventTriangleAPI.Sender.Presentation/EventTriangleAPI.Sender.Presentation.csproj

#######################################################################################################
# DOTNET BUILD + PUBLISH
#######################################################################################################
FROM restore AS publish

# Main build context is src/sender.
COPY . sender/

# Shared source comes from the separate BuildKit context.
COPY --from=shared . shared/

WORKDIR /src/sender/EventTriangleAPI.Sender.Presentation

ARG VERSION

RUN dotnet publish \
    EventTriangleAPI.Sender.Presentation.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

#######################################################################################################
# FINAL IMAGE
#######################################################################################################
FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish ./

ENTRYPOINT ["dotnet", "EventTriangleAPI.Sender.Presentation.dll"]
