using Microsoft.Data.SqlClient;
using static System.Console;
using Spotify.Services;
using Spotify.Model;

namespace Spotify.Repository;

class PlaylistADO
{
    public static void Insert(DatabaseConnection dbConn, Playlist playlist)    // El mètode ha de passar a ser static
    {
        dbConn.Open();

        string sql = @"INSERT INTO Playlist (Id, Name,User_ID)
                        VALUES (@Id, @Name, @User_ID)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlist.Id);
        cmd.Parameters.AddWithValue("@Name", playlist.Name);
        cmd.Parameters.AddWithValue("@User_ID", playlist.User_ID);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }
    public static void Update(DatabaseConnection dbConn, Playlist playlist)
    {
        dbConn.Open();

        string sql = @"UPDATE Playlist SET Name = @Name, User_ID = @User_ID WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlist.Id);
        cmd.Parameters.AddWithValue("@Name", playlist.Name);
        cmd.Parameters.AddWithValue("@User_ID", playlist.User_ID);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }
    public static List<DataSong> GetAll(DatabaseConnection dbConn)
    {
        List<DataSong> dataSong = new();

        dbConn.Open();
        string sql = "SELECT sp.ID, sp.Song_ID, sp.Playlist_ID, s.Name, p.Name FROM SongPlaylist as sp INNER JOIN Songs as s on s.ID = sp.Song_ID INNER JOIN Playlist as p on p.ID = sp.Playlist_ID ";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            dataSong.Add(new DataSong
            {
                ID = reader.GetGuid(0),
                Song_ID = reader.GetGuid(1),
                Playlist_ID = reader.GetGuid(2),
                Song_name = reader.GetString(3),
                Playlist_name = reader.GetString(4),
            });
        }

        dbConn.Close();
        return dataSong;
    }

    public static Playlist? GetById(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();
        string sql = "SELECT ID, Name ,User_ID FROM Playlist WHERE ID = @ID";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Playlist? playlist = null;

        if (reader.Read())
        {
            playlist = new Playlist
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                User_ID = reader.GetGuid(2)
            };
        }

        dbConn.Close();
        return playlist;
    }
    public static bool Delete(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Playlist WHERE ID = @ID";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@ID", Id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}