FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore CertificateService.API/CertificateService.API.csproj
RUN dotnet publish CertificateService.API/CertificateService.API.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && apt install -y curl && rm -rf /var/lib/apt/lists/*
ENV ASPNETCORE_URLS=http://+:5004
EXPOSE 5004
ENTRYPOINT [ "dotnet", "CertificateService.API.dll" ]