# Základní image pro produkci (pro bìh aplikace)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-bullseye-slim-arm64 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Stavební image pro SDK pro ARM64 architekturu
FROM mcr.microsoft.com/dotnet/sdk:9.0-bullseye-slim-arm64 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopíruj csproj a obnov závislosti
COPY ["Tasker.Web.csproj", "./"]
RUN dotnet restore "./Tasker.Web.csproj"

# Kopíruj zbytek kódu
COPY . .

# Pøepni pracovní adresáø a postav projekt
WORKDIR "/src/."
RUN dotnet build "./Tasker.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publikuj aplikaci pro produkci
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tasker.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Koncová fáze pro produkci
FROM base AS final
WORKDIR /app

# Kopíruj publikaèní soubory z fáze publish
COPY --from=publish /app/publish .

# Nastavení entrypointu pro bìh aplikace
ENTRYPOINT ["dotnet", "Tasker.Web.dll"]
