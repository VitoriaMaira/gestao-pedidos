FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["src/LojaPedidos.Web/LojaPedidos.Web.csproj", "src/LojaPedidos.Web/"]

RUN dotnet restore "src/LojaPedidos.Web/LojaPedidos.Web.csproj"

COPY . .

ARG API_BASE_URL=http://localhost:8080/

RUN dotnet publish "src/LojaPedidos.Web/LojaPedidos.Web.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

RUN sed -i \
    "s|http://localhost:8080/|${API_BASE_URL}|g" \
    /app/publish/wwwroot/appsettings.json

FROM nginx:1.27-alpine AS final

COPY src/LojaPedidos.Web/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]