FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 8081/tcp
ENV ASPNETCORE_URLS http://*:8081
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://0.0.0.0:8081/api/Player || exit 1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build
WORKDIR /src
COPY ["Battleship.Player.csproj", "Battleship.Player/"]
RUN dotnet restore "Battleship.Player/Battleship.Player.csproj"
COPY . ./Battleship.Player
WORKDIR "/src/Battleship.Player"
RUN dotnet build "Battleship.Player.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.Player.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleship.Player.dll"]