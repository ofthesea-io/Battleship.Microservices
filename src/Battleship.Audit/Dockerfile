#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8086/tcp
ENV ASPNETCORE_URLS http://*:8086
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://0.0.0.0:8086/api/Board || exit 1

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Battleship.Audit/Battleship.Audit.csproj", "Battleship.Audit/"]
RUN dotnet restore "Battleship.Audit/Battleship.Audit.csproj"
COPY . .
WORKDIR "/src/Battleship.Audit"
RUN dotnet build "Battleship.Audit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleship.Audit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleship.Audit.dll"]