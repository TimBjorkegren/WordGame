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
        
        try
        {
            var wordService = new WordService();
            string randomWord = wordService.GetRandomWord();
            Console.WriteLine($"Random Word: {randomWord}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
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