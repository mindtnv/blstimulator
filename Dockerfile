FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./src/BLStimulator/BLStimulator.csproj", "./BLStimulator/"]
COPY ["./src/BLStimulator.Contracts/BLStimulator.Contracts.csproj", "./BLStimulator.Contracts/"]
RUN dotnet nuget add source https://baget.dev.gbms.site/v3/index.json -n baget
RUN dotnet restore "./BLStimulator/BLStimulator.csproj"
RUN dotnet restore "./BLStimulator.Contracts/BLStimulator.Contracts.csproj"
COPY ["./src/BLStimulator/", "./BLStimulator/"]
COPY ["./src/BLStimulator.Contracts/", "./BLStimulator.Contracts/"]
WORKDIR "./BLStimulator"
RUN dotnet build "BLStimulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BLStimulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BLStimulator.dll"]
