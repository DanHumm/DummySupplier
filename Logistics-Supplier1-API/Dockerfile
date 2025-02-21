 FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN chmod +w ./Logistics-Supplier1-API/Logistics-Supplier1-API/encrypted-supplier.db
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
COPY ./Logistics-Supplier1-API/encrypted-supplier.db /app/data

ARG PFX_PASSWORD
ARG JWT_SECRET_KEY
ARG JWT_SECRET_ISSUER
ARG DB_PASSWORD

ENV PFX_PASSWORD=${PFX_PASSWORD}
ENV JWT_SECRET_KEY=${JWT_SECRET_KEY}
ENV JWT_SECRET_ISSUER=${JWT_SECRET_ISSUER}
ENV DB_PASSWORD=${DB_PASSWORD}

EXPOSE 443

ENTRYPOINT ["dotnet", "Logistics-Supplier1-API.dll"]
