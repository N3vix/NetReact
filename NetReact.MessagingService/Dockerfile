FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NetReact.MessagingService/NetReact.MessagingService.csproj", "NetReact.MessagingService/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["NetReact.ServiceSetup/NetReact.ServiceSetup.csproj", "NetReact.ServiceSetup/"]
RUN dotnet restore "NetReact.MessagingService/NetReact.MessagingService.csproj"
COPY . .
WORKDIR "/src/NetReact.MessagingService"
RUN dotnet build "NetReact.MessagingService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NetReact.MessagingService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetReact.MessagingService.dll"]
