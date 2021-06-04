FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Vendor/Vendor.Orchestrator/Vendor.Orchestrator.csproj", "src/Vendor/Vendor.Orchestrator/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "src/Shared/Shared.Messages/"]
COPY ["src/Shared/Shared.Core/Shared.Core.csproj", "src/Shared/Shared.Core/"]
RUN dotnet restore "src/Vendor/Vendor.Orchestrator/Vendor.Orchestrator.csproj"
COPY . .
WORKDIR "/src/src/Vendor/Vendor.Orchestrator"
RUN dotnet build "Vendor.Orchestrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vendor.Orchestrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vendor.Orchestrator.dll"]