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

        string sql = @"INSERT INTO Playlist (Id, Name, User_Id)
                        VALUES (@Id, @Name , @User_Id)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlist.Id);
        cmd.Parameters.AddWithValue("@Name", playlist.Name);
        cmd.Parameters.AddWithValue("@User_Id", playlist.User_Id);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static void Update(DatabaseConnection dbConn, Playlist playlist)
    {
        dbConn.Open(); 

        string sql =@"UPDATE Playlist
                       SET Name = @Name,
                           User_Id = @User_Id,
                       WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", playlist.Id);
        cmd.Parameters.AddWithValue("@Name", playlist.Name);
        cmd.Parameters.AddWithValue("@User_Id", playlist.User_Id);
        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static List<Playlist> GetAll(DatabaseConnection dbConn)
    {
        List<Playlist> playlist = new();

        dbConn.Open();
        string sql = "SELECT Id, Name, User_Id FROM Playlist";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            playlist.Add(new Playlist
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                User_Id = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return playlist;
    }

    public static Playlist? GetById(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Name, User_Id FROM Playlist WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Playlist? playlist = null;    // Si no inicialitzem la variable => no existeix en el return!

        if (reader.Read())
        {
            playlist = new Playlist
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(2),
                User_Id = reader.GetGuid(3)
            };
        }

        dbConn.Close();
        return playlist;
    }

    public static bool Delete(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Playlist WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

}