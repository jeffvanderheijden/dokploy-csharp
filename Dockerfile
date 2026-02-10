# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./

# Dokploy/Traefik proxy verwacht vaak dat je app op een vaste poort luistert.
# We defaulten naar 8080, maar Program.cs pakt ook PORT uit env als die gezet wordt.
ENV PORT=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "DokployApp.dll"]
