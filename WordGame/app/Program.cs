using System.Security.Cryptography;
using app;
using Npgsql;

string clientId;
HttpContent context;
var app = WebApplication.CreateBuilder(args);

void BakeCookie()
{
    clientId = GenerateUniqueClientId();
    context.Response.C;
    Console.WriteLine(clientId);
}

static string GenerateUniqueClientId()
{
    var rng = RandomNumberGenerator.Create();
    var id = new byte[16];
    rng.GetBytes(id);
    return Convert.ToBase64String(id);
}