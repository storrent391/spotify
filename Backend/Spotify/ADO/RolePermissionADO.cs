using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Spotify.DTO;

namespace Spotify.ADO
{
    public class RolePermissionADO
    {
        private readonly string _connectionString;

        public RolePermissionADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Insert(RolePermissionDTO dto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO RolesPermissions (ID, Permission_Code, Role_Code)
                    VALUES (@ID, @Permission_Code, @Role_Code)
                ";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ID", dto.Id);
                cmd.Parameters.AddWithValue("@Permission_Code", dto.Permission_Code);
                cmd.Parameters.AddWithValue("@Role_Code", dto.Role_Code);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        public List<RolePermissionDTO> GetAll()
        {
            List<RolePermissionDTO> list = new List<RolePermissionDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT ID, Permission_Code, Role_Code FROM RolesPermissions";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new RolePermissionDTO
                        {
                            Id = reader.GetGuid(0),
                            Permission_Code = reader.GetString(1),
                            Role_Code = reader.GetString(2)
                        });
                    }
                }
            }

            return list;
        }

        
        public RolePermissionDTO? GetById(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ID, Permission_Code, Role_Code
                    FROM RolesPermissions
                    WHERE ID = @ID
                ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new RolePermissionDTO
                        {
                            Id = reader.GetGuid(0),
                            Permission_Code = reader.GetString(1),
                            Role_Code = reader.GetString(2)
                        };
                    }
                }
            }

            return null;
        }

    
        public void Update(RolePermissionDTO dto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE RolesPermissions
                    SET Permission_Code = @Permission_Code,
                        Role_Code = @Role_Code
                    WHERE ID = @ID
                ";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ID", dto.Id);
                cmd.Parameters.AddWithValue("@Permission_Code", dto.Permission_Code);
                cmd.Parameters.AddWithValue("@Role_Code", dto.Role_Code);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        public void Delete(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM RolesPermissions WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
