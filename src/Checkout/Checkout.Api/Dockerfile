FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Checkout/Checkout.Api/Checkout.Api.csproj", "src/Checkout/Checkout.Api/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "src/Shared/Shared.Messages/"]
COPY ["src/Checkout/Checkout.Application/Checkout.Application.csproj", "src/Checkout/Checkout.Application/"]
COPY ["src/Checkout/Checkout.Domain/Checkout.Domain.csproj", "src/Checkout/Checkout.Domain/"]
COPY ["src/Checkout/Checkout.Infrastructure/Checkout.Infrastructure.csproj", "src/Checkout/Checkout.Infrastructure/"]
RUN dotnet restore "src/Checkout/Checkout.Api/Checkout.Api.csproj"
COPY . .
WORKDIR "/src/src/Checkout/Checkout.Api"
RUN dotnet build "Checkout.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.Api.dll"]