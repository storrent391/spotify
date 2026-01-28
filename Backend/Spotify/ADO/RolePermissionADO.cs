 using Microsoft.Data.SqlClient;
 using Spotify.Model;
 using Spotify.Services;


namespace Spotify.Repository;

class RolePermissionADO
{
    //GET
    public static List<RolePermission> GetAll(DatabaseConnection dbConn)
    {
        List<RolePermission> rolePermissions = new();

        dbConn.Open();
        string sql = "SELECT ID,Permission_ID,Role_ID FROM RolesPermissions";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            rolePermissions.Add(new RolePermission
            {
                Id = reader.GetGuid(0),
                Permission_ID = reader.GetGuid(1),
                Role_ID = reader.GetGuid(2),
            });
        }
        dbConn.Close();
        return rolePermissions;
    }

    public static List<RolePermission> GetById(DatabaseConnection dbConn, Guid Role_ID)
    {
        List<RolePermission> rolePermissions = new();

        dbConn.Open();
        string sql = "SELECT ID, Permission_ID, Role_ID FROM RolesPermissions WHERE Role_ID = @Role_ID;";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Role_ID", Role_ID);

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            rolePermissions.Add(new RolePermission
            {
                Id = reader.GetGuid(0),
                Permission_ID = reader.GetGuid(1),
                Role_ID = reader.GetGuid(2)
            });
        }

        dbConn.Close();
        return rolePermissions;
    }

   public static void Insert(DatabaseConnection dbConn, RolePermission rolePermission)
{
    dbConn.Open();

    string sql = @"INSERT INTO RolesPermissions (ID, Permission_ID, Role_ID)
                   VALUES (@ID, @Permission_ID, @Role_ID)";

    using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
    cmd.Parameters.AddWithValue("@ID", rolePermission.Id);
    cmd.Parameters.AddWithValue("@Permission_ID", rolePermission.Permission_ID);
    cmd.Parameters.AddWithValue("@Role_ID", rolePermission.Role_ID);

    cmd.ExecuteNonQuery();
    dbConn.Close();
}


}




// POST Roles/{Id}/Permissions
// DELETE Roles/{Id}/Permissions
// GET Roles/{Id}/Permissions
//
//
//
