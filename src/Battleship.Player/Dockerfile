#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
# Expose ports
EXPOSE 8082/tcp
ENV ASPNETCORE_URLS http://*:8082
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://0.0.0.0:8082/api/Player || exit 1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Battleship.Player/Battleship.Player.csproj", "Battleship.Player/"]
COPY ["Battleship.Microservices.Core/Battleship.Microservices.Core.csproj", "Battleship.Microservices.Core/"]
RUN dotnet restore "Battleship.Player/Battleship.Player.csproj"
COPY . .
WORKDIR "/src/Battleship.Player"
RUN dotnet build "Battleship.Player.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.Player.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleship.Player.dll"]