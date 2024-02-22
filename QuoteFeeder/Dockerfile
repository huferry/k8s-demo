FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["QuoteFeeder.csproj", "./"]
RUN dotnet restore "QuoteFeeder.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "QuoteFeeder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuoteFeeder.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuoteFeeder.dll"]
