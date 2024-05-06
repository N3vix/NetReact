FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NetReact.MessagingWorker/NetReact.MessagingWorker.csproj", "NetReact.MessagingWorker/"]
COPY ["NetReact.MessageBroker/NetReact.MessageBroker.csproj", "NetReact.MessageBroker/"]
COPY ["Models/Models.csproj", "Models/"]
RUN dotnet restore "NetReact.MessagingWorker/NetReact.MessagingWorker.csproj"
COPY . .
WORKDIR "/src/NetReact.MessagingWorker"
RUN dotnet build "NetReact.MessagingWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NetReact.MessagingWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetReact.MessagingWorker.dll"]
