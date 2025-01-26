# Tato fáze se používá pøi spuštìní z VS v rychlém režimu (výchozí pro Debug konfiguraci)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Tato fáze se používá pro sestavení projektu
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Zkopírujeme csproj a obnovíme závislosti
COPY ["Tasker.Web.csproj", "./"]
RUN dotnet restore "./Tasker.Web.csproj"

# Zkopírujeme celý projekt
COPY . .

# Nastavení správného pracovního adresáøe pro build
WORKDIR /src

# Kompilace aplikace
RUN dotnet build "./Tasker.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Tato fáze slouží k publikování projektu pro finální fázi
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tasker.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Finální fáze pro produkci
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tasker.Web.dll"]
