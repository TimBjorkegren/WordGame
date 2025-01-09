namespace app;
using Npgsql;

public class DatabaseConnect
{
    private String _connectionString;

    public DatabaseConnect()
    {
        _connectionString = "Host=localhost;Username=postgres;Password=7942;Database=postgres";
        
    }

    public NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}