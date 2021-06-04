FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Checkout/Checkout.Application/Checkout.Application.csproj", "src/Checkout/Checkout.Application/"]
COPY ["src/Shared/Shared.Messages/Shared.Messages.csproj", "src/Shared/Shared.Messages/"]
COPY ["src/Shared/Shared.Core/Shared.Core.csproj", "src/Shared/Shared.Core/"]
COPY ["src/Checkout/Checkout.Api/Checkout.Api.csproj", "src/Checkout/Checkout.Api/"]
RUN dotnet restore "src/Checkout/Checkout.Api/Checkout.Api.csproj"
COPY . .
WORKDIR "/src/src/Checkout/Checkout.Api"
RUN dotnet build "Checkout.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Checkout.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Checkout.Api.dll"]
