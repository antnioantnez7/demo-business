# ------------------------------------------------------------------------------------------------
# Obtener la imagen oficial de docker hub https://hub.docker.com/r/microsoft/dotnet-sdk, 
# y lo renombra como build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Expone los puertos
EXPOSE 8080
# ------------------------------------------------------------------------------------------------
# Construye la imagen para release
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# ------------------------------------------------------------------------------------------------
# Copia los archivos del proyecto a la ruta del proyecto 
COPY ./*.csproj ./
# Verifica que el archivo del proyecto .csproj tenga todas las depndencias del proyecto
RUN dotnet restore 
# Copia el resto de los archivos del proyecto a la ruta del proyecto
COPY . .
# Copia el certificado a la ruta del proyecto
COPY ./certs/*.crt .
COPY ./certs/*.pem .
RUN dotnet tool update -g linux-dev-certs
#RUN dotnet dev-certs https --clean

RUN dotnet dev-certs https --trust
#RUN dotnet linux dev-certs install --no-deps

RUN dotnet build "banobras-bitacoras-business.csproj" -c $BUILD_CONFIGURATION -o /app/build
# ------------------------------------------------------------------------------------------------
# Obtiene lo de build y lo renombra como publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# Obtiene todos los artefactos que docker necesita para ejecutar la aplicación, toma las dlls
RUN dotnet publish "banobras-bitacoras-business.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# ------------------------------------------------------------------------------------------------
# Obtiene lo de base y lo renombra como finish
FROM base AS final
WORKDIR /app
# Toma el build que ahora se llama publish y lo pasa a la carpeta publish para publicar
COPY --from=publish /app/publish .
# Add certificates
#Copia el certificado terminacion .crt, y le agrega el nombre
#COPY --from=build /src/*.crt /etc/ca-certificates/banobras_gob_mx.crt
COPY --from=build /src/*.crt /etc/ssl/certs/banobras_gob_mx.crt
#COPY --from=build /src/*.pem /etc/ssl/certs/banobras_gob_mx.pem
COPY --from=build /src/*.pem /etc/ca-certificates/banobras_gob_mx.pem
RUN update-ca-certificates
ENTRYPOINT ["dotnet", "banobras-bitacoras-business.dll", "--urls:https://*:8080"]
# ------------------------------------------------------------------------------------------------


