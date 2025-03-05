using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);

// Retrieve connection string
var connectionString = builder.Configuration.GetConnectionString("GreetingAppDB");

Console.WriteLine($"Connection String: {connectionString}"); // Debugging output

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'GreetingAppDB' not found in appsettings.json.");
}

// Register services
builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<GreetingAppContext>(options =>
    options.UseSqlServer(connectionString));




//Logger
builder.Logging.ClearProviders();//clearing all the pre defined outputs of logger
builder.Logging.AddConsole();//logs to the console
builder.Logging.AddDebug();//loogs to the debug console window

builder.Services.AddControllers();
//Adding services of business layer
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
//Adding services of repository layer
builder.Services.AddScoped<IGreetingRL, GreetingRL>();

//Add swagger to the container
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//using Swagger
    app.UseSwaggerUI();// responsible for colorfullness
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
