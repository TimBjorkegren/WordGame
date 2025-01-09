namespace app;
using Npgsql;
using Microsoft.AspNetCore.Mvc;

public class LobbyController : ControllerBase{

[HttpPost("/generate-invite")]
public IInviteHandler GenerateInvite()
{
    var invitecode = GenerateLobbyCode();

    SaveLobbyCodeToDatabase(inviteCode, "player1_client");
    return Ok(new {invite_code = inviteCode})
}

private string GenerateLobbyCode(){

    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    string invitecode = ShuffleString(chars, 6);
    console.WriteLine(invitecode)
}

public static string ShuffleString(string chars, int length){

    random = new Random();

    return new string(
        chars.OrderBy(c => random.Next())
        .Take(length)
        .ToArray()
    )
}

private void SaveLobbyCodeToDatabase(string inviteCode, string LobbyCreatorID){
    
    DatabaseConnect _dbConnect = new DatabaseConnect();
        
    var SqlQuery = "INSERT INTO lobbys (invite_code, player1_client) VALUES (@inviteCode, @LobbyCreatorID)";
    using var cmd = new npgsqlcommand(SqlQuery, cmd);
    cmd.parameters.AddWithValue("inviteCode", inviteCode);
    cmd.parameters.AddWithValue("LobbyCreatorID", LobbyCreatorID);
    cmd.ExecuteNonQuery();
}
}