using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors;
using app;


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

        // Använd routing och mappar controllers
        app.UseRouting();
        app.MapControllers();

        // Kör applikationen
        app.Run();
    }

    
    public static void BakeCookie()
    {
        string clientId = GenerateUniqueClientId();
        Console.WriteLine($"Generated Client ID: {clientId}");
    }

    private static string GenerateUniqueClientId()
    {
        var rng = RandomNumberGenerator.Create();
        var id = new byte[16];
        rng.GetBytes(id);
        return Convert.ToBase64String(id);
    }
}