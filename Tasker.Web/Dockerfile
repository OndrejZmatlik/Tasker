
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS publish

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY Tasker.Web.csproj ./

RUN dotnet restore Tasker.Web.csproj -r linux-musl-arm64

COPY . .

RUN dotnet publish Tasker.Web.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    -r linux-musl-arm64 \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

USER root

RUN apk add --no-cache \
    icu-libs \
    icu-data-full \
    krb5-libs \
    tzdata

WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    TZ=Europe/Prague \
    LANG=cs_CZ.UTF-8 \
    LC_ALL=cs_CZ.UTF-8

EXPOSE 8080

COPY --chown=$APP_UID:$APP_UID --from=publish /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "Tasker.Web.dll"]
