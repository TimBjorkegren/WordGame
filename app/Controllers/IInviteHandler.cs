namespace app;
using Npgsql;
using Microsoft.AspNetCore.Mvc;

public class LobbyController : ControllerBase, IInviteHandler
{
    [HttpPost("/generate-invite")]
    public IActionResult GenerateInvite()
    {
        var inviteCode = GenerateLobbyCode();
        SaveLobbyCodeToDatabase(inviteCode, "player1_client");
        return Ok(new { invite_code = inviteCode });
    }

    public string GenerateInviteCode()
    {
        
        return GenerateLobbyCode();
    }

    private string GenerateLobbyCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string inviteCode = ShuffleString(chars, 6);
        Console.WriteLine(inviteCode); 
        return inviteCode;
    }

    public static string ShuffleString(string chars, int length)
    {
        var random = new Random();
        return new string(
            chars.OrderBy(c => random.Next())
            .Take(length)
            .ToArray()
        );
    }

    private void SaveLobbyCodeToDatabase(string inviteCode, string lobbyCreatorId)
    {
        DatabaseConnect _dbConnect = new DatabaseConnect();
        var sqlQuery = "INSERT INTO english_dictionary.lobbys (invite_code, player1_client) VALUES (@inviteCode, @LobbyCreatorID)";
        using var conn = _dbConnect.GetConnection();
        using var cmd = new NpgsqlCommand(sqlQuery, conn);
        cmd.Parameters.AddWithValue("inviteCode", inviteCode);
        cmd.Parameters.AddWithValue("LobbyCreatorID", lobbyCreatorId);
        cmd.ExecuteNonQuery();
    }
}