FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview5-alpine3.9 as base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview5-alpine3.9 AS build
WORKDIR /src
COPY ["src/api/SchoolApi.csproj", "api/"]
RUN dotnet restore "api/SchoolApi.csproj"
COPY src/api api/
WORKDIR "/src/api"
RUN dotnet build "SchoolApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SchoolApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SchoolApi.dll"]