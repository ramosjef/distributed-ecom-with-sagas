FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Checkout/Checkout.Orchestrator/Checkout.Orchestrator.csproj", "src/Checkout/Checkout.Orchestrator/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "src/Shared/Shared.Messages/"]
COPY ["src/Checkout/Checkout.Application/Checkout.Application.csproj", "src/Checkout/Checkout.Application/"]
COPY ["src/Checkout/Checkout.Domain/Checkout.Domain.csproj", "src/Checkout/Checkout.Domain/"]
COPY ["src/Checkout/Checkout.Infrastructure/Checkout.Infrastructure.csproj", "src/Checkout/Checkout.Infrastructure/"]
RUN dotnet restore "src/Checkout/Checkout.Orchestrator/Checkout.Orchestrator.csproj"
COPY . .
WORKDIR "/src/src/Checkout/Checkout.Orchestrator"
RUN dotnet build "Checkout.Orchestrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.Orchestrator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.Orchestrator.dll"]