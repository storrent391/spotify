using Microsoft.Data.SqlClient;
using Spotify.Services;
using Spotify.Model;
using Spotify.Encryption;
namespace Spotify.Repository;

public class UserADO
{


    public static void Insert(DatabaseConnection dbConn, User user)
{
    PasswordEncryption.ConvertPassword(user);
    dbConn.Open();
    string sql = @"INSERT INTO Users (Id, Name, Password, Salt)
                   VALUES (@Id, @Name, @Password, @Salt)";
    using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
    cmd.Parameters.AddWithValue("@Id", user.Id);
    cmd.Parameters.AddWithValue("@Name", user.Name);
    cmd.Parameters.AddWithValue("@Password", user.Password);
    cmd.Parameters.AddWithValue("@Salt", user.Salt);
    cmd.ExecuteNonQuery();
    
    dbConn.Close();
}


    public static List<User> GetAll(DatabaseConnection dbConn)
    {
        List<User> list = new();
        dbConn.Open();

        string sql = "SELECT Id, Name, Password, Salt FROM Users";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new User
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Password = reader.GetString(2),
                Salt = reader.GetString(3)
            });
        }

        dbConn.Close();
        return list;
    }

    public static User? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Name, Password, Salt FROM Users WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        User? user = null;

        if (reader.Read())
        {
                user = new User
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Password = reader.GetString(2),
                Salt = reader.GetString(3)
            };
        }

        dbConn.Close();
        return user;
    }

    public static void Update(DatabaseConnection dbConn, User user)
    {
        dbConn.Open();

        string sql = @"UPDATE Users
                       SET Name = @Name,
                           Password = @Password,
                           Salt = @Salt
                       WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", user.Id);
        cmd.Parameters.AddWithValue("@Name", user.Name);
        cmd.Parameters.AddWithValue("@Password", user.Password);
        cmd.Parameters.AddWithValue("@Salt", user.Salt);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static bool Delete(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Users WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();
        dbConn.Close();

        return rows > 0;
    }
}
