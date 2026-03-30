# Dockerfile para TaskFlow WebMvc
# Construcción multi-stage para publicar la app ASP.NET 10.0

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar sólo los archivos de proyecto necesarios para restaurar rápido
COPY ["src/TaskFlow.Application/TaskFlow.Application.csproj", "src/TaskFlow.Application/"]
COPY ["src/TaskFlow.Domain/TaskFlow.Domain.csproj", "src/TaskFlow.Domain/"]
COPY ["src/TaskFlow.Infrastructure/TaskFlow.Infrastructure.csproj", "src/TaskFlow.Infrastructure/"]
COPY ["src/TaskFlow.WebMvc/TaskFlow.WebMvc.csproj", "src/TaskFlow.WebMvc/"]

RUN dotnet restore "src/TaskFlow.WebMvc/TaskFlow.WebMvc.csproj"

RUN apt-get update && apt-get install -y curl ca-certificates gnupg2
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get install -y nodejs

COPY . .
WORKDIR "/src/src/TaskFlow.WebMvc"
RUN npm install
RUN npm run build:css
RUN dotnet publish "TaskFlow.WebMvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "TaskFlow.WebMvc.dll"]
