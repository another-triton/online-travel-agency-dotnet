#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RezLive.Simulator.Api/RezLive.Simulator.Api.csproj", "RezLive.Simulator.Api/"]
COPY ["RezLive.Simulator.Domain/RezLive.Simulator.Domain.csproj", "RezLive.Simulator.Domain/"]
RUN dotnet restore "./RezLive.Simulator.Api/RezLive.Simulator.Api.csproj"
COPY . .
WORKDIR "/src/RezLive.Simulator.Api"
RUN dotnet build "./RezLive.Simulator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RezLive.Simulator.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RezLive.Simulator.Api.dll"]