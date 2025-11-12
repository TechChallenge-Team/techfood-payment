ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:8.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:8.0

FROM ${DOTNET_RUNTIME} AS base
RUN apt-get update && \
    apt-get install wget -y
WORKDIR /app
EXPOSE 8080

FROM ${DOTNET_SDK} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TechFood.Api/TechFood.Api.csproj", "TechFood.Api/"]
COPY ["src/TechFood.Application/TechFood.Application.csproj", "TechFood.Application/"]
COPY ["src/TechFood.Contracts/TechFood.Contracts.csproj", "TechFood.Contracts/"]
COPY ["src/TechFood.Domain/TechFood.Domain.csproj", "TechFood.Domain/"]
COPY ["src/TechFood.Infra/TechFood.Infra.csproj", "TechFood.Infra/"]
RUN dotnet restore "./TechFood.Api/TechFood.Api.csproj"
COPY /src .
WORKDIR TechFood.Api
RUN dotnet build "./TechFood.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechFood.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechFood.Api.dll"]
