using Microsoft.Data.SqlClient;
using Spotify.Model;
using Spotify.Services;


namespace Spotify.Repository;

class RoleADO
{
    //GET
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

    //POST
    public static void Insert(DatabaseConnection dbConn,Role role)
    {

        dbConn.Open();

        string sql = @"INSERT INTO Roles (Id, Code, Name)         
                        VALUES (@Id, @Code, @Name)";            

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", role.Id);
        cmd.Parameters.AddWithValue("@Code", role.Code);
        cmd.Parameters.AddWithValue("@Name", role.Name);

        cmd.ExecuteNonQuery();

        dbConn.Close();
    }

    //UPDATE
    public static void Update(DatabaseConnection dbConn, Role role)
    {
        dbConn.Open();

        string sql = @"UPDATE Roles 
                        SET
                        Id = @Id,
                        Code = @Code,
                        Name = @Name
                        WHERE Id = @Id";
                        


        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", role.Id);
        cmd.Parameters.AddWithValue("@Nom", role.Code);
        cmd.Parameters.AddWithValue("@Descripcio", role.Name);

        cmd.ExecuteNonQuery();
    
        dbConn.Close();
    }

    //DELETE
     public static bool Delete(DatabaseConnection dbConn, Guid Id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Roles WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", Id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

}
