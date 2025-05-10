FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Digital.Wallet.Transaction.Consumer/Digital.Wallet.Transaction.Consumer.csproj", "Digital.Wallet.Transaction.Consumer/"]
RUN dotnet restore "./Digital.Wallet.Transaction.Consumer/Digital.Wallet.Transaction.Consumer.csproj"
COPY . .
WORKDIR "/src/Digital.Wallet.Transaction.Consumer"
RUN dotnet build "./Digital.Wallet.Transaction.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Digital.Wallet.Transaction.Consumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Digital.Wallet.Transaction.Consumer.dll"]