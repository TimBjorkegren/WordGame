namespace app;
using Npgsql;

[HttpPost("/generate-invite")]
public IInviteHandler GenerateInvite()
{
    var invitecode = GenerateLobbyCode();

    SaveLobbyCodeToDatabase(invitecode, LobbyCreatorID: "player1_client");
    
}

private string GenerateLobbyCode(){

    var random = new Random();
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

private void SaveLobbyCodeToDatabase(string InviteCode, string LobbyCreatorID){
    
    DatebaseConnect _dbConnect = new DatebaseConnect();
        
    var SqlQuery = "INSERT INTO lobbys (invite_code, LobbyCreatorID) VALUES (@invite_code, @LobbyCreatorID)";
    using var cmd = new npgsqlcommand(SqlQuery, cmd);
    cmd.parameters.AddWithValue("InviteCode", invite_code);
    cmd.parameters.AddWithValue("LobbyCreatorID", LobbyCreatorID);
    cmd.ExecuteNonQuery();
}