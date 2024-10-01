using Chirp.Core;
using Microsoft.Data.Sqlite;

namespace TestHelpers;

public class TestDatabase : IDatabase
{
    public string ConnectionString => "Data Source=InMemorySample;Mode=Memory;Cache=Shared";

    public void EnsureCreated()
    {
    }

    public void Reset()
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
}