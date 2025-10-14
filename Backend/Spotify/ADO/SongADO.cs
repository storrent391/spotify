using Microsoft.Data.SqlClient;
using static System.Console;
using Spotify.Model;
using Spotify.Services;
using System.Reflection.Metadata.Ecma335;
using System.Net.Http.Headers;
// using System.ComponentModel.DataAnnotations.Schema;
// using System.Net.Sockets;
// using System.Data.Common;
// using System.Reflection.Metadata.Ecma335;

namespace Spotify.Repository;

class SongADO
{
    public static void Insert(DatabaseConnection dbConn, Song song)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Song (ID, Name)
                        VALUES (@ID, @Name)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.slqConnection);
        cmd.Parameters.AddWithValue("@ID", Id);
        cmd.Parameters.AddWithValue("@Name", Name);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");

        dbConn.Close();
    }
    public static List<Song> GetAll(DatabaseConnection dbConn)
    {
        List<Song> songs = new();

        dbConn.Open();
        string sql = "SELECT Id, Name FROM Song";

        using SqlCommand cmd = SqlCommand(sql, dbConn.SqlConnectino);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            songs.Add(new Song
            {
                Id = reader.GetGuid(0),
                NamedArgumentsEncoder = reader.GetString(1),
            });
        }
        dbConn.Close();
        return songs;
    }

    public static Song? GetById(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Name FROM Song WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        using SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            SongADO = new Song
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
            };
        }

        dbConn.Close();
        return song;
    }

    public static void Update(DatabaseConnection dbConn, Song song)
    {
        dbConn.Open();

        string sql = @"UPDATE Song 
                        SET Id = @Id,
                        Name = @Name
                        WHERE Id = @Id";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.slqConnectoin);
        cmd.Parameters.AddWithValue("@Id", song.Id);
        cmd.Parameters.AddWithValue("@Name", song.Name);

        int rows = cmd.ExecuteNonQuery();

        // Console.WriteLine($"{rows} fila actualitzada.");

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static bool Delete(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Song WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.slqConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}
