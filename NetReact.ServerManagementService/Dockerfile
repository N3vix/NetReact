FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NetReact.ServerManagementService/NetReact.ServerManagementService.csproj", "NetReact.ServerManagementService/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["NetReact.ServiceSetup/NetReact.ServiceSetup.csproj", "NetReact.ServiceSetup/"]
RUN dotnet restore "NetReact.ServerManagementService/NetReact.ServerManagementService.csproj"
COPY . .
WORKDIR "/src/NetReact.ServerManagementService"
RUN dotnet build "NetReact.ServerManagementService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NetReact.ServerManagementService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetReact.ServerManagementService.dll"]
