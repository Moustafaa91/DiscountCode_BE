using DiscountCodeService.Data;
using DiscountCodeService.Hubs;
using DiscountCodeService.Services;
using DiscountCodeService.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
builder.Services.AddSingleton<MongoDBContext>(provider =>
{
    var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
    var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");

    if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
    {
        throw new InvalidOperationException("MongoDB configuration is missing.");
    }

    return new MongoDBContext(connectionString, databaseName);
});

builder.Services.AddSingleton<Logger>(provider =>
{
    var context = provider.GetService<MongoDBContext>();
    return new Logger(context.Database);
});

builder.Services.AddSingleton<DiscountService>(provider =>
{
    var context = provider.GetService<MongoDBContext>();
    var logger = provider.GetService<Logger>();
    return new DiscountService(context.Database, logger);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific", builder =>
    {
        builder.WithOrigins(
            "http://localhost:3000" 
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});


// Add SignalR
builder.Services.AddSignalR();


var app = builder.Build();

app.MapGet("/", () => "Welcome to the Discount Code Service!");


app.UseCors("AllowAll");

// Map the SignalR Hub
app.MapHub<DiscountHub>("/discountHub");

// Start the app
app.Run();
