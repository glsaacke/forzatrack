using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;

namespace api.core.middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER = "X-Api-Key";
        private readonly string _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["API_KEY"];

        }

        public async Task Invoke(HttpContext context)
        {

            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/api/health") ||
                context.Request.Path.StartsWithSegments("/api/User/AuthenticateUser") ||
                context.Request.Path.StartsWithSegments("/api/User/CreateUser")){
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey) || extractedApiKey != _apiKey)
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized: Invalid API Key");
                return;
            }

            await _next(context);
        }
    }
}