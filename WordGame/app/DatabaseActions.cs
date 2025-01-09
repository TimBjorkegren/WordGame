using Npgsql;

namespace app;

public class DatabaseActions
{
    private DatebaseConnect _datebaseConnect = new();
    private NpgsqlDataSource db;
    private string clientId;


    async Task<string> CreateLobby<T>()
    {
        await using var cmd = db.CreateCommand("INSERT INTO Lobbies (player_1_client) VALUES ($1)");
        cmd.Parameters.AddWithValue(clientId);
        await cmd.ExecuteReaderAsync();
        return clientId;
    }
}