using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using Middleware.JwtHelper;
using RepositoryLayer.Context;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Starting up the application");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Setup NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Retrieve connection string
    var connectionString = builder.Configuration.GetConnectionString("GreetingAppDB");

    if (string.IsNullOrEmpty(connectionString))
    {
        logger.Error("Connection string 'GreetingAppDB' not found in appsettings.json.");
        throw new InvalidOperationException("Connection string 'GreetingAppDB' not found.");
    }

    logger.Info("Database connection string loaded successfully.");

    // Register services
    builder.Services.AddControllers();

    // Register DbContext
    builder.Services.AddDbContext<GreetingAppContext>(options =>
        options.UseSqlServer(connectionString));

    // Register Business & Repository Layer
    builder.Services.AddScoped<IGreetingBL, GreetingBL>();
    builder.Services.AddScoped<IGreetingRL, GreetingRL>();
    builder.Services.AddScoped<IUserBL, UserBL>();
    builder.Services.AddScoped<IUserRL, UserRL>();
    // Register SMTP Middleware

    builder.Services.AddScoped<Middleware.Email.SMTP>();

    // ? Register JwtTokenHelper as Singleton (Best practice)
    builder.Services.AddSingleton<JwtTokenHelper>();

    // Configure Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Registration API", Version = "v1" });
    });

    var app = builder.Build();

    // Enable Swagger UI
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Registration API v1"));
    }

    // ? Middleware Configuration
    app.UseHttpsRedirection();

    // ? Authentication should come before Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    logger.Info("Application started successfully.");
    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Application startup failed.");
}
finally
{
    LogManager.Shutdown();
}