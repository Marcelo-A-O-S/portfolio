FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore AuthService.API/AuthService.API.csproj
RUN dotnet publish AuthService.API/AuthService.API.csproj -c Release -o /app/publish
WORKDIR /app
COPY --from=build /app/publish .
RUN apt-get update && apt install -y curl
ENV ASPNETCORE_URLS=http://+:5001
EXPOSE 5001
ENTRYPOINT [ "dotnet","AuthService.API.dll" ]