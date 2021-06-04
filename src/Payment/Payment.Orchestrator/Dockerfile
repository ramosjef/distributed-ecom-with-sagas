FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Payment/Payment.Orchestrator/Payment.Orchestrator.csproj", "src/Payment/Payment.Orchestrator/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "src/Shared/Shared.Messages/"]
COPY ["src/Shared/Shared.Core/Shared.Core.csproj", "src/Shared/Shared.Core/"]
RUN dotnet restore "src/Payment/Payment.Orchestrator/Payment.Orchestrator.csproj"
COPY . .
WORKDIR "/src/src/Payment/Payment.Orchestrator"
RUN dotnet build "Payment.Orchestrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.Orchestrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Orchestrator.dll"]