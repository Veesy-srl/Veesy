FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Veesy.WorkerService/Veesy.WorkerService.csproj", "Veesy.WorkerService/"]
RUN dotnet restore "Veesy.WorkerService/Veesy.WorkerService.csproj"
COPY . .
WORKDIR "/src/Veesy.WorkerService"
RUN dotnet build "Veesy.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Veesy.WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Veesy.WorkerService.dll"]
