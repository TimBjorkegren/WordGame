using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using app;


public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args);
        BakeCookie();
        
        var wordService = new WordService();  // Create an instance of WordService
        try
        {
            string randomWord = wordService.GetRandomWord();  // Get a random word
            Console.WriteLine($"Random word from WordService: {randomWord}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting random word: {ex.Message}");
        }
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