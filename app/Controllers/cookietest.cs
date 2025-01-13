using Microsoft.AspNetCore.Mvc;
/*
[ApiController]
[Route("[controller]")]
public class CookieController : ControllerBase
{
    [HttpGet("check-client-id")]
    public IActionResult CheckClientId()
    {
        if (Request.Cookies.TryGetValue("ClientId", out var clientId))
        {
            return Ok(new(clientId = "clientId"));
        }
        else
        {
            return NotFound("Ingen Client ID-cookie hittades.");
        }
    }
} */