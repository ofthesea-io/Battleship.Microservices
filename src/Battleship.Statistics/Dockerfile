FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 8084/tcp
ENV ASPNETCORE_URLS http://*:8084
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://0.0.0.0:8084/api/Statistic || exit 1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build
WORKDIR /src
COPY ["Battleship.Statistics/Battleship.Statistics.csproj", "Battleship.Statistics/"]
RUN dotnet restore "Battleship.Statistics/Battleship.Statistics.csproj"
COPY . .
WORKDIR "/src/Battleship.Statistics"
RUN dotnet build "Battleship.Statistics.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.Statistics.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleship.Statistics.dll"]