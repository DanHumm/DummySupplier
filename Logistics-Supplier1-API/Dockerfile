FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Logistics-Supplier1-API/Logistics-Supplier1-API.csproj", "Logistics-Supplier1-API/"]
RUN dotnet restore "Logistics-Supplier1-API/Logistics-Supplier1-API.csproj"
COPY . .
WORKDIR "/src/Logistics-Supplier1-API"
RUN dotnet build "Logistics-Supplier1-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Logistics-Supplier1-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/data 
COPY ./Logistics-Supplier1-API/https /app/https 

ENV PFX_PASSWORDD=$PFX_PASSWORDD
ENV ConnectionStrings__Default=$DB_CONN_STRING
ENV JWT_SECRET_KEY=$JWT_SERCRET_KEY
ENV JWT_SECRET_ISSUER=$JWT_SERCRET_ISSUER
ENV Kestrel__Certificates__Default__Password=$Kestrel__Certificates__Default__Password

ENTRYPOINT ["dotnet", "Logistics-Supplier1-API.dll"]
