FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 8083/tcp
ENV ASPNETCORE_URLS http://*:8083
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://0.0.0.0:80/api/ScoreBoard || exit 1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build
WORKDIR /src
COPY ["Battleship.Scoreboard/Battleship.Scoreboard.csproj", "Battleship.Scoreboard/"]
RUN dotnet restore "Battleship.Scoreboard/Battleship.Scoreboard.csproj"
COPY . .
WORKDIR "/src/Battleship.Scoreboard"
RUN dotnet build "Battleship.Scoreboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.Scoreboard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleship.Scoreboard.dll"]