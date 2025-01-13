using System.Security.Cryptography;
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

        // Lägg till CORS-konfiguration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                policy =>
                {
                    policy
                        .WithOrigins("http://127.0.0.1:5500")
                        .AllowAnyMethod() // Tillåt alla HTTP-metoder (GET, POST, etc.)
                        .AllowAnyHeader() // Tillåt alla headers
                        .AllowCredentials(); // Tillåt credentials
                }
            );
        });

        // Lägg till controllers
        builder.Services.AddControllers();

        var app = builder.Build();

        // Använd CORS-policy innan andra middleware
        app.UseCors("AllowAll");

        // Middleware för att hantera cookies
        app.Use(
            async (context, next) =>
            {
                // Kontrollera om cookien "ClientId" redan finns
                if (!context.Request.Cookies.ContainsKey("ClientId"))
                {
                    // Om cookien inte finns, generera en ny och sätt den
                    string clientId = GenerateUniqueClientId();
                    context.Response.Cookies.Append(
                        "ClientId",
                        clientId,
                        new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTimeOffset.UtcNow.AddDays(30), // Sätt 30 dagars utgångstid
                        }
                    );

                    Console.WriteLine($"Generated new Client ID: {clientId}");
                }

                await next();
            }
        );

        // Använd routing och mappar controllers
        app.UseRouting();
        app.MapControllers();

        // Kör applikationen
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
