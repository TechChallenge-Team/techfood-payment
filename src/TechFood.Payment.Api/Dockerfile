ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:8.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:8.0

FROM ${DOTNET_RUNTIME} AS base
RUN apt-get update && \
    apt-get install wget -y
WORKDIR /app
EXPOSE 8080

FROM ${DOTNET_SDK} AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_TOKEN
ARG GITHUB_ACTOR
WORKDIR /src
COPY ["TechFood.Payment.Api/TechFood.Payment.Api.csproj", "TechFood.Payment.Api/"]
COPY ["TechFood.Payment.Application/TechFood.Payment.Application.csproj", "TechFood.Payment.Application/"]
COPY ["TechFood.Payment.Contracts/TechFood.Payment.Contracts.csproj", "TechFood.Payment.Contracts/"]
COPY ["TechFood.Payment.Domain/TechFood.Payment.Domain.csproj", "TechFood.Payment.Domain/"]
COPY ["TechFood.Payment.Infra/TechFood.Payment.Infra.csproj", "TechFood.Payment.Infra/"]
COPY ["TechFood.Payment.Common/TechFood.Payment.Common.csproj", "TechFood.Payment.Common/"]
RUN dotnet nuget add source --username "${GITHUB_ACTOR}" --password "${GITHUB_TOKEN}" --store-password-in-clear-text --name github "https://nuget.pkg.github.com/TechChallenge-Team/index.json"
RUN dotnet restore "./TechFood.Payment.Api/TechFood.Payment.Api.csproj"
COPY . .
WORKDIR /src/TechFood.Payment.Api
RUN dotnet build "./TechFood.Payment.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechFood.Payment.Api.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechFood.Payment.Api.dll"]
