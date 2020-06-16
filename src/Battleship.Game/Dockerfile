FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 8082/tcp
ENV ASPNETCORE_URLS http://*:8082
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://0.0.0.0:8082/api/Board || exit 1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build
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
COPY /Battleship.Game/wait-for-it.sh wait-for-it.sh
RUN chmod +x wait-for-it.sh