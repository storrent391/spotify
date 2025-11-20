 using Microsoft.Data.SqlClient;
 using Spotify.Model;
 using Spotify.Services;


namespace Spotify.Repository;

class RolePermissionADO
{
    public static List<RolePermission> GetAll(DatabaseConnection dbConn)
    {
        List<RolePermission> rolePermissions = new();

        dbConn.Open();
        string sql = "SELECT ID,Permission_Code,Role_Code FROM RolesPermissions";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            rolePermissions.Add(new RolePermission
            {
                Id = reader.GetGuid(0),
                Permission_Code = reader.GetString(1),
                Role_Code = reader.GetString(2),
            });
        }
        dbConn.Close();
        return rolePermissions;
    }

    public static List<RolePermission> GetByCode(DatabaseConnection dbConn, string Role_Code)
{
    List<RolePermission> rolePermissions = new();

    dbConn.Open();
    string sql = "SELECT ID, Permission_Code, Role_Code FROM RolesPermissions WHERE Role_Code = @Role_Code;";

    using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
    cmd.Parameters.AddWithValue("@Role_Code", Role_Code);

    using SqlDataReader reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        rolePermissions.Add(new RolePermission
        {
            Id = reader.GetGuid(0),
            Permission_Code = reader.GetString(1),
            Role_Code = reader.GetString(2)
        });
    }

    dbConn.Close();
    return rolePermissions;
}




}




// POST Roles/{Id}/Permissions
// DELETE Roles/{Id}/Permissions
// GET Roles/{Id}/Permissions
//
//
//
