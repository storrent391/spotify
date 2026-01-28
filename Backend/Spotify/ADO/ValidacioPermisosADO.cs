using Microsoft.Data.SqlClient;
using Spotify.Services;
using Spotify.Model;
using Spotify.Encryption;
using Spotify.Repository.Validators;
namespace Spotify.Repository;

public class ValidacioPermisosADO
{
    public static List<string> GetPermsById(DatabaseConnection dbConn, Guid id)
    {
        List<string>perms = new List<string>();

        dbConn.Open();

        string sql = @"SELECT p.Code FROM Users u 
                        INNER JOIN UserRoles ur ON u.ID = ur.User_ID 
                        INNER JOIN RolesPermissions rp ON ur.Role_ID = rp.Role_ID
                        INNER JOIN Permissions p ON p.ID = rp.Permission_ID 
                        WHERE u.ID = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
            perms.Add(reader.GetString(0));
        
        dbConn.Close();
        return perms;
    }

}
