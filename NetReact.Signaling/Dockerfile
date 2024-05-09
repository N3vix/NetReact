FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NetReact.Signaling/NetReact.Signaling.csproj", "NetReact.Signaling/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["NetReact.ServiceSetup/NetReact.ServiceSetup.csproj", "NetReact.ServiceSetup/"]
RUN dotnet restore "NetReact.Signaling/NetReact.Signaling.csproj"
COPY . .
WORKDIR "/src/NetReact.Signaling"
RUN dotnet build "NetReact.Signaling.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NetReact.Signaling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetReact.Signaling.dll"]
