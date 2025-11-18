using Microsoft.Data.SqlClient;
using Spotify.Model;
using Spotify.Services;


namespace Spotify.Repository;

class RoleADO
{
    public static List<Role> GetAll(DatabaseConnection dbConn)
    {
        List<Role> roles = new();

        dbConn.Open();
        string sql = "SELECT ID, Code, Name FROM Roles";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            roles.Add(new Role
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
            });
        }
        dbConn.Close();
        return roles;
    }
}
