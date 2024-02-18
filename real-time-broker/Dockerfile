FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["real-time-broker/RealTimeBroker.csproj", "."]
RUN dotnet restore "./RealTimeBroker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "RealTimeBroker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RealTimeBroker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeBroker.dll"]
