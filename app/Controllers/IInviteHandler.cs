namespace app;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

public class LobbyController : ControllerBase
{
    [HttpPost("/generate-invite")]
    public IActionResult GenerateInvite()
    {
        var inviteCode = GenerateLobbyCode();
        var clientId = GetClientId();

        SaveLobbyCodeToDatabase(inviteCode, clientId);
        return Ok(new { invite_code = inviteCode, client_id = clientId });
    }

    private string GetClientId()
    {
        if (Request.Cookies.TryGetValue("ClientId", out var clientId))
        {
            return clientId;
        }
        else
        {
            var newClientId = Program.GenerateUniqueClientId();
            Response.Cookies.Append(
                "ClientId",
                newClientId,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                }
            );
            return newClientId;
        }
    }

    private string GenerateLobbyCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return ShuffleString(chars, 6);
    }

    private string ShuffleString(string chars, int length)
    {
        var random = new Random();
        return new string(chars.OrderBy(c => random.Next()).Take(length).ToArray());
    }

    private void SaveLobbyCodeToDatabase(string inviteCode, string clientId)
    {
        var _dbConnect = new DatabaseConnect();
        var sqlQuery =
            "INSERT INTO english_dictionary.lobbys (invite_code, player1_client) VALUES (@inviteCode, @clientId)";
        using var conn = _dbConnect.GetConnection();
        using var cmd = new NpgsqlCommand(sqlQuery, conn);
        cmd.Parameters.AddWithValue("inviteCode", inviteCode);
        cmd.Parameters.AddWithValue("clientId", clientId); // Fixed parameter name
        cmd.ExecuteNonQuery();
    }
}
