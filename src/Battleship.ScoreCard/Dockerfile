FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 8083/tcp
ENV ASPNETCORE_URLS http://*:8083
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://172.19.0.7:8083/api/ScoreCard || exit 1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build
WORKDIR /src
COPY ["Battleship.ScoreCard/Battleship.ScoreCard.csproj", "Battleship.ScoreCard/"]
RUN dotnet restore "Battleship.ScoreCard/Battleship.ScoreCard.csproj"
COPY . .
WORKDIR "/src/Battleship.ScoreCard"
RUN dotnet build "Battleship.ScoreCard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.ScoreCard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY /Battleship.ScoreCard/wait-for-it.sh wait-for-it.sh
RUN chmod +x wait-for-it.sh