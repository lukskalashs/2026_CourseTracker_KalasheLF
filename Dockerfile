# Stage 1: Build the Angular client
FROM node:20-alpine AS client-build
WORKDIR /src/client
COPY client/package*.json ./
RUN npm ci
COPY client/ ./
RUN npx ng build --configuration production
 
# Stage 2: Build and publish the .NET API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS api-build
WORKDIR /src
COPY api/ ./api/
WORKDIR /src/api
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish
 
# Stage 3: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=api-build /app/publish ./
# Copy the built Angular files into the API's wwwroot
COPY --from=client-build /src/client/dist/client/browser ./wwwroot
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "API.dll"]