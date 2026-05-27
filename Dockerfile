# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution and project files
COPY ToDo.slnx global.json ./
COPY src/ToDo.Api/ToDo.Api.csproj src/ToDo.Api/
COPY tests/ToDo.Tests/ToDo.Tests.csproj tests/ToDo.Tests/

# Restore dependencies
RUN dotnet restore ToDo.slnx

# Copy the rest of the source code
COPY src/ToDo.Api/ src/ToDo.Api/
COPY tests/ToDo.Tests/ tests/ToDo.Tests/

# Build and run tests to ensure image health
WORKDIR /app/tests/ToDo.Tests
RUN dotnet test --no-restore

# Publish the API
WORKDIR /app/src/ToDo.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ToDo.Api.dll"]
