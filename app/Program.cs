using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

using app;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()  
                    .AllowAnyHeader()  
                    .AllowAnyMethod(); 
            });
        });

        builder.Services.AddSingleton<WordService>(); 

        var app = builder.Build();
        // Using CORS because frontend and backend are using different ports
        app.UseCors("AllowAll");

        BakeCookie();

        app.MapGet("/api/word/random", (WordService wordService) =>
        {
            try
            {
                string randomWord = wordService.GetRandomWord();
                return Results.Ok(new { word = randomWord });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapPost("/api/word/validate", async (WordService wordService, HttpContext httpContext) =>
        {
            try
            {
                // Parse the incoming JSON request
                var request = await httpContext.Request.ReadFromJsonAsync<WordRequest>();
                Console.WriteLine($"Received word: {request?.Word}");

                if (request == null || string.IsNullOrEmpty(request.Word))
                {
                    return Results.BadRequest(new { isValid = false, message = "Invalid word input." });
                }

                 // Validate the word using the WordService
                bool isValid = wordService.ValidateWord(request.Word);

                return Results.Ok(new { isValid });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });


        // Start the application
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