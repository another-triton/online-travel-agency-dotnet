#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RezLive.Adapter.Api/RezLive.Adapter.Api.csproj", "RezLive.Adapter.Api/"]
RUN dotnet restore "./RezLive.Adapter.Api/RezLive.Adapter.Api.csproj"
COPY . .
WORKDIR "/src/RezLive.Adapter.Api"
RUN dotnet build "./RezLive.Adapter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RezLive.Adapter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RezLive.Adapter.Api.dll"]