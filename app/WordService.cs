using Npgsql;

namespace app
{
    public class WordService
    {
        private readonly DatabaseConnect _databaseConnect;

        public WordService()
        {
            _databaseConnect = new DatabaseConnect(); 
        }

        public string GetRandomWord()
        {
            using (var conn = _databaseConnect.GetConnection())
            {
                var cmd = new NpgsqlCommand("SELECT c1 FROM english_dictionary.dictionary WHERE LENGTH(c1) = 10 ORDER BY RANDOM() LIMIT 1;", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(0); // Return the random word
                    }
                    Console.WriteLine($"Random word: {reader.GetString(0)}");
                }
            }

            throw new Exception("No words with at least 10 characters found in the database.");
        }
    }
} 