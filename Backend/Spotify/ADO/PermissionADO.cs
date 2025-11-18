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
}
