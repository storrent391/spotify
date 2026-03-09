using Microsoft.Data.SqlClient;
using Spotify.Model;
using Spotify.Services;


namespace Spotify.Repository;

class PermissionADO
{
    public static List<Permission> GetAll(DatabaseConnection dbConn)
    {
        List<Permission> permissions = new();

        dbConn.Open();
        string sql = "SELECT ID, Code, Name FROM Permissions";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            permissions.Add(new Permission
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
            });
        }
        dbConn.Close();
        return permissions;
    }

    //POST
    public static void Insert(DatabaseConnection dbConn, Permission permission)
    {

        dbConn.Open();

        string sql = @"INSERT INTO Permissions (Id, Code, Name)         
                        VALUES (@Id, @Code, @Name)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", permission.Id);
        cmd.Parameters.AddWithValue("@Code", permission.Code);
        cmd.Parameters.AddWithValue("@Name", permission.Name);

        cmd.ExecuteNonQuery();

        dbConn.Close();
    }

    public static bool Delete(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Permissions WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }
}
