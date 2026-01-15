using Microsoft.Data.SqlClient;
using Spotify.Services;
using Spotify.Model;
using Spotify.Encryption;
using Spotify.Repository.Validators;
namespace Spotify.Repository;

public class UserADO
{


    public static void Insert(DatabaseConnection dbConn, User user)
{
    
    UserValidator validator = new UserValidator(dbConn);
    validator.ValidateUserDoesNotExist(user.Name);

    
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


    public static User? GetByName(DatabaseConnection dbConn, string name)
    {
        dbConn.Open();

        string sql = @"
            SELECT Id, Name, Password, Salt
            FROM Users
            WHERE Name = @Name
        ";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Name", name);

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

    public static List<DataUser> GetAll(DatabaseConnection dbConn)
    {
        List<DataUser> dataUser = new();
        dbConn.Open();

        string sql = "SELECT pu.ID, pu.User_ID, pu.Playlist_ID, u.Name, p.Name FROM UserPlaylist as pu INNER JOIN Users as u on u.ID = pu.User_ID INNER JOIN Playlist as p on p.ID = pu.Playlist_ID";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            dataUser.Add(new DataUser
            {
                ID = reader.GetGuid(0),
                User_ID = reader.GetGuid(1),
                Playlist_ID = reader.GetGuid(2),
                User_name = reader.GetString(3),
                Playlist_name = reader.GetString(4)
            });
        }

        dbConn.Close();
        return dataUser;
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
        
        UserValidator validator = new UserValidator(dbConn);

        validator.ValidateUserDoesNotExist(user.Name);
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
