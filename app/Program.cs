using System.Security.Cryptography;
using app;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add CORS configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
            );
        });

        // Add controllers
        builder.Services.AddControllers();

        var app = builder.Build();

        app.Use(
            async (context, next) =>
            {
                if (!context.Request.Cookies.ContainsKey("ClientId"))
                {
                    string clientId = GenerateUniqueClientId();
                    context.Response.Cookies.Append(
                        "ClientId",
                        clientId,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddDays(30),
                        }
                    );
                    Console.WriteLine($"Generated new Client ID: {clientId}");
                }
                else
                {
                    string existingClientId = context.Request.Cookies["ClientId"];
                    Console.WriteLine($"Existing Client ID: {existingClientId}");
                }

                await next();
            }
        );

        // Use CORS policy before other middleware
        app.UseCors("AllowAll");

        // Use routing and map controllers
        app.UseRouting();
        app.MapControllers();

        // Run the application
        app.Run();
    }

    public static string GenerateUniqueClientId()
    {
        var rng = RandomNumberGenerator.Create();
        var id = new byte[16];
        rng.GetBytes(id);
        return Convert.ToBase64String(id);
    }
}
