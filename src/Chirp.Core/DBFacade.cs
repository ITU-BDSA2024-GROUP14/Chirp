using Chirp.Core.DataTransferObjects;
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
    public IEnumerable<CheepDTO> GetCheeps(int page, string? authorUsername = null)
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

        var rowsToSkip = (page - 1) * 32;

        command.CommandText += " ORDER BY message.pub_date DESC";
        command.CommandText += " LIMIT 32 OFFSET " + rowsToSkip;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            CheepDTO cheep = new CheepDTO();
            cheep.Author = reader.GetString(0);
            cheep.Text = reader.GetString(1);
            cheep.Timestamp = reader.GetInt64(2);
            yield return cheep;
        }
    }
}