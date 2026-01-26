using Microsoft.Data.SqlClient;
using static System.Console;
using Spotify.Services;
using Spotify.Model;

namespace Spotify.Repository;

class PlaylistADO
{
   
    public static void Insert(DatabaseConnection dbConn,Playlist playlist)    // El mètode ha de passar a ser static
    {
        dbConn.Open();

        string sql = @"INSERT INTO Playlist (Id, Name)
                        VALUES (@Id, @Name)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlist.Id);
        cmd.Parameters.AddWithValue("@Name", playlist.Name);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static void Update(DatabaseConnection dbConn, Playlist playlist)
    {
        dbConn.Open(); 

        string sql =@"UPDATE Playlist
                       SET Name = @Name,
                       WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlist.Id);
        cmd.Parameters.AddWithValue("@Name", playlist.Name);
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
        string sql = "SELECT ID, Name  FROM Playlist WHERE ID = @ID";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Playlist? playlist = null;    // Si no inicialitzem la variable => no existeix en el return!

        if (reader.Read())
        {
            playlist = new Playlist
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1)
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