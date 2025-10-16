using Microsoft.Data.SqlClient;
using Spotify.Model;
using Spotify.Services;


namespace Spotify.Repository;

class SongADO
{
    public static void Insert(DatabaseConnection dbConn, Song song)
    {
        dbConn.Open();

        string sql = @"INSERT INTO Songs (Id, Name)
                        VALUES (@Id, @Name)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@ID", song.Id);
        cmd.Parameters.AddWithValue("@Name", song.Name);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }
    public static List<Song> GetAll(DatabaseConnection dbConn)
    {
        List<Song> songs = new();

        dbConn.Open();
        string sql = "SELECT Id, Name FROM Songs";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            songs.Add(new Song
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
            });
        }
        dbConn.Close();
        return songs;
    }

    public static Song? GetById(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Name FROM Songs WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Song? song = null;

        if (reader.Read())
        {
            song = new Song
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

        string sql = @"UPDATE Songs 
                        SET Id = @Id,
                        Name = @Name
                        WHERE Id = @Id";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
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

        string sql = @"DELETE FROM Songs WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}
