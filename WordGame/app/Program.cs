// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

string clientId;
//HttpContext context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

//void BakeCookie()
app.Use(async (context, next) =>
{
    clientId = GenerateUniqueClientId();
    context.Response.Cookies.Append("ClientID", clientId, new CookieOptions
    {
        HttpOnly = true,
        Secure = false,
        SameSite = SameSiteMode.Strict,
        MaxAge = TimeSpan.MaxValue
    });
    await next();
});
    

static string GenerateUniqueClientId()
    {
        var rng = RandomNumberGenerator.Create();
        var id = new byte[16];
        rng.GetBytes(id);
        return Convert.ToBase64String(id);
    }
