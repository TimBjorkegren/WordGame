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
                        return reader.GetString(0);
                    }
                    Console.WriteLine($"Random word: {reader.GetString(0)}");
                }
            }

            throw new Exception("No words with at least 10 characters found in the database.");
        }

        public bool ValidateWord(string word)
        {
            // Check if the word is null or empty
            if (string.IsNullOrEmpty(word))
            {
               Console.WriteLine("Received empty or null word for validation.");
                return false;
            }
            string formattedWord = char.ToUpper(word[0]) + word.Substring(1).ToLower();

            using var conn = _databaseConnect.GetConnection();
            using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM english_dictionary.dictionary WHERE c1 = @word", conn);
    
            cmd.Parameters.AddWithValue("word", formattedWord);

            var count = (long)cmd.ExecuteScalar();
            bool isValid = count > 0;

            Console.WriteLine($"Validating word: {formattedWord} - Exists: {isValid}");
            return isValid;
        }


    }
} 