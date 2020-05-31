#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8081/tcp
ENV ASPNETCORE_URLS http://*:8081
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://0.0.0.0:8081/api/Board || exit 1
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Battleship.Game/Battleship.Game.csproj", "Battleship.Game/"]
RUN dotnet restore "Battleship.Game/Battleship.Game.csproj"
COPY . .
WORKDIR "/src/Battleship.Game"
RUN dotnet build "Battleship.Game.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.Game.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleship.Game.dll"]