using Microsoft.Data.SqlClient;
using static System.Console;
using Spotify.Services;
using Spotify.Model;

namespace Spotify.Repository;

class PlaylistADO
{
   
    public static void Insert(DatabaseConnection dbConn,SongPlaylist songPlaylist)    // El mètode ha de passar a ser static
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
        string sql = "SELECT Id, Song_Id,Playlist_Id FROM SongPlaylist";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            playlist.Add(new SongPlaylist
            {
                Id = reader.GetGuid(0),
                Song_Id = reader.GetString(2),
                Playlist_Id = reader.GetGuid(3)
            });
        }

        dbConn.Close();
        return playlist;
    }

    public static Playlist? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Song_Id, Playlist_Id FROM Playlist WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Playlist? playlist = null;    // Si no inicialitzem la variable => no existeix en el return!

        if (reader.Read())
        {
            playlist = new Playlist
            {
                Id = reader.GetGuid(0),
                Song_Id = reader.GetString(2),
                Playlist_Id = reader.GetDecimal(3)
            };
        }

        dbConn.Close();
        return playlist;
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