using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;


public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args);
        BakeCookie();
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