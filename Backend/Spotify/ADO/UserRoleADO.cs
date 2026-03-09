using Microsoft.Data.SqlClient;
using Spotify.Model;
using Spotify.Services;


namespace Spotify.Repository;

class UserRoleADO
{
    //GET Tots els roles
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
    //Get Tots els Roles de un usuari
    public static List<UserRole> GetUserRolesById(DatabaseConnection dbConn, Guid User_ID)
    {
        List<UserRole> userRoles = new();

        dbConn.Open();
        string sql = "SELECT ID, Role_ID , User_ID FROM UserRoles WHERE User_ID = @User_ID;";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@User_ID", User_ID);

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            userRoles.Add(new UserRole
            {
                Id = reader.GetGuid(0),
                Role_ID = reader.GetGuid(1),
                User_Id = reader.GetGuid(2)

            });
        }

        dbConn.Close();
        return userRoles;
    }

    //insertar un Rol a un usuari
    public static void Insert(DatabaseConnection dbConn, UserRole userRole)
    {
        dbConn.Open();

        string sql = @"INSERT INTO UserRoles (ID, Role_ID, User_ID)
                   VALUES (@ID, @Role_ID, @User_ID)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@ID", userRole.Id);
        cmd.Parameters.AddWithValue("@Role_ID", userRole.Role_ID);
        cmd.Parameters.AddWithValue("@User_ID", userRole.User_Id);


        cmd.ExecuteNonQuery();
        dbConn.Close();
    }
    public static bool Delete(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM UserRoles WHERE ID = @ID";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@ID", Id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

}




// POST Roles/{Id}/Permissions
// DELETE Roles/{Id}/Permissions
// GET Roles/{Id}/Permissions
//
//
//
