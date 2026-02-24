FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore Gateway.API/Gateway.API.csproj
RUN dotnet publish Gateway.API/Gateway.API.csproj -c Release -o /app/publish
FROM mcr.microsoft/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && apt install -y curl
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT [ "dotnet","Gateway.API.dll" ]