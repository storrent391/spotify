using Microsoft.Data.SqlClient;
using Spotify.Services;

namespace Spotify.RepositoryUser;

public class UserADO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Password { get; set; }
    public string Salt { get; set; }

    public static void Insert(DatabaseConnection dbConn, UserADO user)
{
    dbConn.Open();
    string sql = @"INSERT INTO Users (Id, Name, Password, Salt)
                   VALUES (@Id, @Name, @Password, @Salt)";
    using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
    cmd.Parameters.AddWithValue("@Id", product.Id);
    cmd.Parameters.AddWithValue("@Name", product.Name);
    cmd.Parameters.AddWithValue("@Password", product.Password);
    cmd.Parameters.AddWithValue("@Salt", product.Salt);
    cmd.ExecuteNonQuery();
    dbConn.Close();
}


    public static List<UserADO> GetAll(DatabaseConnection dbConn)
    {
        List<UserADO> list = new();
        dbConn.Open();

        string sql = "SELECT Id, Name, Password, Salt FROM Users";
        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new UserADO
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

    public static UserADO? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Name, Password, Salt FROM Users WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        ProductADO? product = null;

        if (reader.Read())
        {
            product = new ProductADO
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Password = reader.GetString(2),
                Salt = reader.GetString(3)
            };
        }

        dbConn.Close();
        return product;
    }

    public static void Update(DatabaseConnection dbConn, ProductADO product)
    {
        dbConn.Open();

        string sql = @"UPDATE Users
                       SET Name = @Name,
                           Passowrd = @Password,
                           Salt = @Salt
                       WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", product.Id);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Password", product.Password);
        cmd.Parameters.AddWithValue("@Salt", product.Salt);

        cmd.ExecuteNonQuery();
        dbConn.Close();
    }

    public static bool Delete(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Products WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();
        dbConn.Close();

        return rows > 0;
    }
}
