namespace app;

[HttpPost("/generate-invite")]
public IInviteHandler GenerateInvite()
{
    
}

private string GenerateLobbyCode(){

    var random = new Random();
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    return new string Collections.shuffle(chars);
}

public static string ShuffleString(string chars, int length){

    random = new Random();

    return new string(
        chars.OrderBy(c => random.Next())
        .Take(length)
        .ToArray()
    )
}

private void SaveLobbyCodeToDatabase(){

}