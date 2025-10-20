using Microsoft.Data.SqlClient;
using Spotify.Model;
using Spotify.Services;

namespace Spotify.Repository
{
    public class MediaADO
    {
        public static void Insert(DatabaseConnection dbConn, Media media)
        {
            dbConn.Open();

            string sql = @"INSERT INTO Media (ID, URL, Song_ID)
                           VALUES (@Id, @Url, @Song_Id)";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", media.Id);
            cmd.Parameters.AddWithValue("@Url", media.Url);
            cmd.Parameters.AddWithValue("@Song_Id", media.Song_Id);

            cmd.ExecuteNonQuery();
            dbConn.Close();
        }

        public static List<Media> GetAll(DatabaseConnection dbConn)
        {
            List<Media> list = new();
            dbConn.Open();

            string sql = "SELECT ID, URL, Song_ID FROM Media";
            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Media
                {
                    Id = reader.GetGuid(0),
                    Song_Id = reader.GetGuid(1),
                    Url = reader.GetString(2),

                });
            }

            dbConn.Close();
            return list;
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
        public static List<Media> GetBySongId(DatabaseConnection dbConn, Guid Song_Id)
        {
            List<Media> list = new();
            dbConn.Open();

            string sql = "SELECT ID, URL, Song_ID FROM Media WHERE Song_ID = @SongId";
            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Song_Id", Song_Id);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Media
                {
                    Id = reader.GetGuid(0),
                    Song_Id = reader.GetGuid(1),
                    Url = reader.GetString(2),

                });
            }

            dbConn.Close();
            return list;
        }

        // public static Media? GetById(DatabaseConnection dbConn, Guid id)
        // {
        //     dbConn.Open();

        //     string sql = "SELECT ID, URL, Song_ID FROM Media WHERE ID = @Id";
        //     using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        //     cmd.Parameters.AddWithValue("@Id", id);

        //     using SqlDataReader reader = cmd.ExecuteReader();
        //     Media? media = null;

        //     if (reader.Read())
        //     {
        //         media = new Media
        //         {
        //             Id = reader.GetGuid(0),
        //             Song_Id = reader.GetGuid(1),
        //             Url = reader.GetString(2),

        //         };
        //     }

        //     dbConn.Close();
        //     return media;
        // }

        public static void Update(DatabaseConnection dbConn, Media media)
        {
            dbConn.Open();

            string sql = @"UPDATE Media
                           SET URL = @Url,
                               Song_ID = @Song_Id
                           WHERE ID = @Id";

            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", media.Id);
            cmd.Parameters.AddWithValue("@Url", media.Url);
            cmd.Parameters.AddWithValue("@Song_Id", media.Song_Id);

            cmd.ExecuteNonQuery();
            dbConn.Close();
        }

        public static bool Delete(DatabaseConnection dbConn, Guid id)
        {
            dbConn.Open();

            string sql = @"DELETE FROM Media WHERE ID = @Id";
            using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();
            dbConn.Close();

            return rows > 0;
        }
    }
}
