namespace app;
using Npgsql;

public class DatebaseConnect
{
    private String _connectionString;
    private NpgsqlDataSource _connection;

    private NpgsqlDataSource Connection()
    {
        return _connection;
    }
    public DatebaseConnect()
    {
        _connectionString = "Host=localhost;Username=postgres;Password=thedAnne@3223;Database=postgres";
        _connection = NpgsqlDataSource.Create(_connectionString);
    }

    public NpgsqlConnection GetConnection()
    {
        var conn = new NpgsqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}