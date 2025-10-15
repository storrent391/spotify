using Microsoft.Data.SqlClient;
using static System.Console;
using Spotify.Services;
using Spotify.Model;

namespace Spotify.Repository;

class SongPlaylistADO
{
   
    public static void Insert(DatabaseConnection dbConn, SongPlaylist songPlaylist)    // El mètode ha de passar a ser static
    {

        dbConn.Open();

        string sql = @"INSERT INTO SongPlaylist (Id, Song_Id, Playlist_Id)
                        VALUES (@Id, @Song_Id , @Playlist_Id)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", songPlaylist.Id);
        cmd.Parameters.AddWithValue("@Song_Id", songPlaylist.Song_Id);
        cmd.Parameters.AddWithValue("@Playlist_Id", songPlaylist.Playlist_Id);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static void Update(DatabaseConnection dbConn, SongPlaylist songPlaylist)
    {
        dbConn.Open();

        string sql =@"UPDATE SongPlaylist
                       SET Song_Id = @Song_Id,
                           Playlist_Id = @Playlist_Id,
                       WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", songPlaylist.Id);
        cmd.Parameters.AddWithValue("@Song_Id", songPlaylist.Song_Id);
        cmd.Parameters.AddWithValue("@Playlist_Id", songPlaylist.Playlist_Id);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static List<SongPlaylist> GetAll(DatabaseConnection dbConn)
    {
        List<SongPlaylist> songPlaylist = new();

        dbConn.Open();
        string sql = "SELECT Id, Song_Id, Playlist_Id FROM SongPlaylist";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            songPlaylist.Add(new SongPlaylist
            {
                Id = reader.GetGuid(0),
                Song_Id = reader.GetGuid(1),
                Playlist_Id = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return songPlaylist;
    }

    public static SongPlaylist? GetById(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Song_Id, Playlist_Id FROM SongPlaylist WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        using SqlDataReader reader = cmd.ExecuteReader();
        SongPlaylist? songPlaylist = null;    

        if (reader.Read())
        {
            songPlaylist = new SongPlaylist
            {
                Id = reader.GetGuid(0),
                Song_Id = reader.GetGuid(1),
                Playlist_Id = reader.GetGuid(2)
            };
        }

        dbConn.Close();
        return songPlaylist;
    }

    public static bool Delete(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Playlist WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

}