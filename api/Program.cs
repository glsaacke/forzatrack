using api.core.services.UserService;
using api.core.services.CarService;
using api.core.services.RecordService;
using api.core.services.BuildService;
using api.core.middleware;
using Microsoft.OpenApi.Models;


var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

System.Console.WriteLine("TESTING CONSOLE OUTPUT");

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-Api-Key", // The name of the header
        Type = SecuritySchemeType.ApiKey,
        Description = "API Key required to access this API"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey" // This matches the definition name above
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers(); // <-- Add this line to register controllers.

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IRecordService, RecordService>();
builder.Services.AddScoped<IRecordRepository, RecordRepository>();
builder.Services.AddScoped<IBuildService, BuildService>();
builder.Services.AddScoped<IBuildRepository, BuildRepository>();
builder.Services.AddLogging();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod());
});


var app = builder.Build();

var config =
    new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

// Map controllers (this is required for endpoint routing).
app.MapControllers();  // <-- This line maps the controller endpoints.

app.Run();
