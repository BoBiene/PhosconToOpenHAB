#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PhosconToOpenHAB/PhosconToOpenHAB.csproj", "PhosconToOpenHAB/"]
RUN dotnet restore "PhosconToOpenHAB/PhosconToOpenHAB.csproj"
COPY . .
WORKDIR "/src/PhosconToOpenHAB"
RUN dotnet build "PhosconToOpenHAB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhosconToOpenHAB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhosconToOpenHAB.dll"]