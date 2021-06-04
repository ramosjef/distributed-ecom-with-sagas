FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Boundaries/Payments/Payment.Worker/Payment.Worker.csproj", "src/Boundaries/Payments/Payment.Worker/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "src/Shared/Shared.Messages/"]
RUN dotnet restore "src/Boundaries/Payments/Payment.Worker/Payment.Worker.csproj"
COPY . .
WORKDIR "/src/src/Boundaries/Payments/Payment.Worker"
RUN dotnet build "Payment.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Worker.dll"]