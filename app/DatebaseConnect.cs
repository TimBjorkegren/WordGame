namespace app;
using Npgsql;

public class DatebaseConnect
{
    private String _connectionString;

    public DatebaseConnect()
    {
        _connectionString = "Host=localhost;Username=postgres;Password=thedAnne@3223;Database=postgres";
        
    }

    public NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}