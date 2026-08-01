FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/LojaPedidos.Api/LojaPedidos.Api.csproj", "src/LojaPedidos.Api/"]
COPY ["src/LojaPedidos.Application/LojaPedidos.Application.csproj", "src/LojaPedidos.Application/"]
COPY ["src/LojaPedidos.Domain/LojaPedidos.Domain.csproj", "src/LojaPedidos.Domain/"]
COPY ["src/LojaPedidos.Infrastructure/LojaPedidos.Infrastructure.csproj", "src/LojaPedidos.Infrastructure/"]
COPY ["src/LojaPedidos.ServiceDefaults/LojaPedidos.ServiceDefaults.csproj", "src/LojaPedidos.ServiceDefaults/"]

RUN dotnet restore "src/LojaPedidos.Api/LojaPedidos.Api.csproj"

COPY . .
RUN dotnet publish "src/LojaPedidos.Api/LojaPedidos.Api.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID
ENTRYPOINT ["dotnet", "LojaPedidos.Api.dll"]
