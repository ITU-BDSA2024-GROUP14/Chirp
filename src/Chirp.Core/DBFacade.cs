using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Chirp.Core;

public class DBFacade(IConfigurationRoot configuration)
{
    private readonly string _path = configuration["CHIRPDBPATH"] ?? Path.GetTempPath();
    private string ConnectionString => $"Data Source={_path}";

    public void EnsureCreated()
    {
        // check if the database exists, if not call reset
        if (!File.Exists(_path))
        {
            Reset();
        }
    }

    private void Reset()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        // call the SQL command to create the table

        command.CommandText = """
                              drop table if exists user;
                              create table user (
                                user_id integer primary key autoincrement,
                                username string not null,
                                email string not null,
                                pw_hash string not null
                              );

                              drop table if exists message;
                              create table message (
                                message_id integer primary key autoincrement,
                                author_id integer not null,
                                text string not null,
                                pub_date integer
                              );
                              """;
        
        command.ExecuteNonQuery();
    }
    
    public void ExecuteQuery(string query)
    {
        
    }
}