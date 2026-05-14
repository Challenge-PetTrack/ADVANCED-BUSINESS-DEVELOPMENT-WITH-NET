FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Recommenda.Domain/Recommenda.Domain.csproj", "Recommenda.Domain/"]
RUN dotnet restore "Recommenda.Domain/Recommenda.Domain.csproj"
COPY . .
WORKDIR "/src/Recommenda.Domain"
RUN dotnet build "./Recommenda.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Recommenda.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Recommenda.Domain.dll"]
