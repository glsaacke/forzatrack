using api.core.services.UserService;
using api.core.services.CarService;
using api.core.services.RecordService;
using api.core.services.BuildService;
using api.core.middleware;
using api.core.data;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.Load();
string connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
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
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

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
        builder => builder
                          .SetIsOriginAllowed(origin =>
                              origin == "http://localhost:5173" ||
                              origin == "http://localhost:5174" ||
                              origin == "https://forzatrack.vercel.app" ||
                              (origin.StartsWith("https://forzatrack-") && origin.EndsWith(".vercel.app")))
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder => builder.SetIsOriginAllowed(_ => true) // Allow all origins
//                           .AllowAnyHeader()
//                           .AllowAnyMethod()
//                           .WithExposedHeaders("X-Api-Key") // Optional: expose API key in responses if needed
//                           .AllowCredentials()); // Optional: only if you're using cookies/auth
// });

var app = builder.Build();

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
// app.UseCors("AllowAll");

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/api/health", () => Results.Ok());

app.Run();
