FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY Application/ ./Application/
COPY Infrastructure/ ./Infrastructure/
COPY Domain/ ./Domain/
COPY ApiWebPulso.Contracts/ ./ApiWebPulso.Contracts/
COPY ApiWebBase/ ./ApiWebBase/

RUN dotnet restore ApiWebBase/ApiWebPulso.csproj

RUN dotnet publish ApiWebBase/ApiWebPulso.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "ApiWebPulso.dll"]
