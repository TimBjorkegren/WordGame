using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Lägg till CORS-konfiguration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()  // Tillåt alla ursprung (domän/port)
                      .AllowAnyMethod()  // Tillåt alla HTTP-metoder (GET, POST, etc.)
                      .AllowAnyHeader(); // Tillåt alla headers
            });
        });

        // Lägg till controllers
        builder.Services.AddControllers();

        var app = builder.Build();

        // Använd CORS-policy innan andra middleware
        app.UseCors("AllowAll");

        // Middleware för att hantera cookies
        app.Use(async (context, next) =>
        {
            // Kontrollera om cookien "ClientId" redan finns
            if (!context.Request.Cookies.ContainsKey("ClientId"))
            {
                // Om cookien inte finns, generera en ny och sätt den
                string clientId = GenerateUniqueClientId();
                context.Response.Cookies.Append("ClientId", clientId, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30) // Sätt 30 dagars utgångstid
                });

                Console.WriteLine($"Generated new Client ID: {clientId}");
            }
            else
            {
                // Cookie finns redan, hämta värdet för debugging (valfritt)
                string existingClientId = context.Request.Cookies["ClientId"];
                Console.WriteLine($"Existing Client ID: {existingClientId}");
            }

            await next();
        });

        // Använd routing och mappar controllers
        app.UseRouting();
        app.MapControllers();

        // Kör applikationen
        app.Run();
    }

    private static string GenerateUniqueClientId()
    {
        var rng = RandomNumberGenerator.Create();
        var id = new byte[16];
        rng.GetBytes(id);
        return Convert.ToBase64String(id);
    }
}
