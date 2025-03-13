# Zobacz https://aka.ms/customizecontainer, aby dowiedzieć się, jak dostosować kontener debugowania i jak program Visual Studio używa tego pliku Dockerfile do kompilowania obrazów w celu szybszego debugowania.

# Ten etap jest używany podczas uruchamiania z programu VS w trybie szybkim (domyślnie dla konfiguracji debugowania)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 2303


# Ten etap służy do kompilowania projektu usługi
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ScrumMaster.Tasks/ScrumMaster.Tasks.csproj", "ScrumMaster.Tasks/"]
COPY ["ScrumMaster.Tasks.Infrastructure/ScrumMaster.Tasks.Infrastructure.csproj", "ScrumMaster.Tasks.Infrastructure/"]
COPY ["ScrumMaster.Tasks.Core/ScrumMaster.Tasks.Core.csproj", "ScrumMaster.Tasks.Core/"]
RUN dotnet restore "./ScrumMaster.Tasks/ScrumMaster.Tasks.csproj"
COPY . .
WORKDIR "/src/ScrumMaster.Tasks"
RUN dotnet build "./ScrumMaster.Tasks.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Ten etap służy do publikowania projektu usługi do skopiowania do etapu końcowego
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ScrumMaster.Tasks.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Ten etap jest używany w środowisku produkcyjnym lub w przypadku uruchamiania z programu VS w trybie regularnym (domyślnie, gdy nie jest używana konfiguracja debugowania)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScrumMaster.Tasks.dll"]