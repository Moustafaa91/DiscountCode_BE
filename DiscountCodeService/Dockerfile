# Use the official .NET 8 SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY . ./
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o out

# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "DiscountCodeService.dll"]
