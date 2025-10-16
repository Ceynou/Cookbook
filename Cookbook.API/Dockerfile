FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Cookbook.API/Cookbook.API.csproj", "Cookbook.API/"]
COPY ["Cookbook.Infrastructure/Cookbook.Infrastructure.csproj", "Cookbook.Infrastructure/"]
COPY ["Cookbook.SharedData/Cookbook.SharedData.csproj", "Cookbook.SharedData/"]
COPY ["Cookbook.Core/Cookbook.Core.csproj", "Cookbook.Core/"]
RUN dotnet restore "Cookbook.API/Cookbook.API.csproj"
COPY . .
WORKDIR "/src/Cookbook.API"
RUN dotnet build "./Cookbook.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Cookbook.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cookbook.API.dll"]
