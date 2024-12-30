# Guía de despliegue

# Requisitos previos

1. Tener instalado:
   - Docker y Docker Compose.
2. Configurar las variables de entorno:
   - DOTNET_ENVIRONMENT
   - DB_CONNECTION_STRING
   - JWT_SECRET
   - JWT_ISSUER
   - JWT_AUDIENCE
   - JWT_AccessTokenExpirationMinutes
   - JWT_RefreshTokenExpirationDays

# Archivos necesarios

- Dockerfile
- docker-compose.yml

# Pasos para desplegar

1. Construir y ejecutar los Contenedores desde la raíz del proyecto:
   docker-compose up --build
2. Verificar el despliegue
   - Acceder a la API en http://localhost:5000/swagger para verificar los endpoints (cambiar por la url del servidor donde se realice el despliegue).
