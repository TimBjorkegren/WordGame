namespace app;

using Npgsql;

public class InviteValidator
{
    private readonly DatabaseConnect _dbConnect;

    public InviteValidator()
    {
        _dbConnect = new DatabaseConnect();
    }

    public bool ExactMatchCode(string inviteCode)
    {
        try
        {
            var sqlQuery =
                "SELECT COUNT(*) FROM english_dictionary.lobbys WHERE invite_code = @inviteCode";
            using var conn = _dbConnect.GetConnection();
            using var cmd = new NpgsqlCommand(sqlQuery, conn);
            cmd.Parameters.AddWithValue("inviteCode", inviteCode);

            var count = (long)cmd.ExecuteScalar();
            return count > 0;
        }
        catch (Exception ex)
        {
            throw;
        }
        ;
    }
};