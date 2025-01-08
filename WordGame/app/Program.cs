// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;

string clientId;
HttpContent context;
var app = WebApplication.CreateBuilder(args);

public void BakeCookie()
{
    clientId = GenerateUniqueClientId();
    context.Response.C
}

static string GenerateUniqueClientId()
{
    var rng = RandomNumberGenerator.Create();
    var id = new byte[16];
    rng.GetBytes(id);
    return Convert.ToBase64String(id);
}