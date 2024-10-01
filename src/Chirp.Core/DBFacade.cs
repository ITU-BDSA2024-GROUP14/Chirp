using Microsoft.Data.Sqlite;

namespace Chirp.Core;

public class DBFacade
{
    public IDatabase db { get; }

    public DBFacade(IDatabase database)
    {
        db = database;
    }


    /// <summary>
    /// Gets cheeps from the database
    /// </summary>
    /// <param name="authorUsername">Optional: filter by author username</param>
    /// <param name="limit">Optional: limit results by N. If left null or below 0, all cheeps will be returned.</param>
    /// <returns>Collection of Cheeps</returns>
    public IEnumerable<Cheep> GetCheeps(string? authorUsername = null, int? limit = null)
    {
        using var connection = new SqliteConnection(db.ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
                              SELECT user.username, message.text, message.pub_date 
                              FROM message 
                              INNER JOIN user ON message.author_id = user.user_id
                              """;
        if (authorUsername != null)
        {
            command.CommandText += " WHERE user.username = @Username";
            command.Parameters.AddWithValue("@Username", authorUsername);
        }

        if (limit != null)
        {
            command.CommandText += " LIMIT @Limit";
            command.Parameters.AddWithValue("@Limit", limit);
        }

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            yield return new Cheep(reader.GetString(0), reader.GetString(1), reader.GetInt64(2));
        }
    }
}