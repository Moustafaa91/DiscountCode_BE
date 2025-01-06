# Discount Code Service (SignalR Backend)

This is a C# backend application for managing discount codes, built using **ASP.NET Core** and **SignalR**. The backend exposes a WebSocket hub that allows real-time interaction for generating, using, and retrieving discount codes.

A corresponding client application is available at [SignalR Console Client](https://github.com/Moustafaa91/DiscountCode_ConsoleApp).

## Features
- **GenerateCodes**: Create unique discount codes with a specified count and length.
- **UseCode**: Mark a discount code as used.
- **GetUsedCodes**: Retrieve a list of used discount codes.
- **GetUnusedCodes**: Retrieve a list of unused discount codes.
- **Ping**: Test the connectivity to the WebSocket hub.

## Technologies Used
- **C#** (.NET)
- **SignalR** for real-time WebSocket communication
- **MongoDB** for data storage
- **Docker** (optional) for deployment

## Prerequisites
- **.NET SDK**: Install the latest version from [here](https://dotnet.microsoft.com/download).
- **MongoDB**: Either host MongoDB locally or use a cloud service like [MongoDB Atlas](https://www.mongodb.com/cloud/atlas).

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/Moustafaa91/DiscountCode_BE.git
   cd DiscountCode_BE
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Set up environment variables:
   Create a `.env` file in the project root with the following:
   ```env
   MONGODB_CONNECTION_STRING=<Your MongoDB Connection String>
   MONGODB_DATABASE_NAME=DiscountCodeDB
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

## API Reference
The backend exposes the following WebSocket endpoints via SignalR:
- **Ping**: Tests connectivity.
- **GenerateCodes(count, length)**: Generates discount codes with the specified count and length.
- **UseCode(code)**: Marks a discount code as used.
- **GetUsedCodes()**: Retrieves a list of used codes.
- **GetUnusedCodes()**: Retrieves a list of unused codes.

The WebSocket hub is hosted at:
[https://discountcode-be.onrender.com/discountHub](https://discountcode-be.onrender.com/discountHub)

## Packages Used
This project uses the following NuGet packages:
- `Microsoft.AspNetCore.SignalR`: Enables SignalR hub functionality.
- `Microsoft.AspNetCore.SignalR.Client`: Allows communication with SignalR hubs.
- `MongoDB.Driver`: MongoDB .NET Driver for database operations.
- `dotenv.net`: For managing environment variables.
- `Newtonsoft.Json`: For JSON serialization.

## Deployment
### Using Docker
Build and run the application using Docker:
1. Create a `Dockerfile` in the project root:
   ```dockerfile
   FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
   WORKDIR /app
   COPY . .
   RUN dotnet restore
   RUN dotnet publish -c Release -o out

   FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
   WORKDIR /app
   COPY --from=build /app/out .
   ENTRYPOINT ["dotnet", "DiscountCodeService.dll"]
   ```

2. Build and run the Docker image:
   ```bash
   docker build -t discountcode-backend .
   docker run -p 5000:5000 discountcode-backend
   ```

### Hosting on Render
The application is currently hosted on Render:
[https://discountcode-be.onrender.com](https://discountcode-be.onrender.com)

## Client Application
A console-based client application to interact with this backend is available at:
[SignalR Console Client](https://github.com/Moustafaa91/DiscountCode_ConsoleApp)

## Author
Moustafa A. - [GitHub Profile](https://github.com/Moustafaa91)

