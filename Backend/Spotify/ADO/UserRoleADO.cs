using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Spotify.Model;
using Spotify.DTO;

namespace Spotify.ADO
{
    public class UserRoleADO
    {
        private readonly string _connectionString;

        public UserRoleADO(string connectionString)
        {
            _connectionString = connectionString;
        }

        
        public void Insert(UserRoleDTO dto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO UserRoles (ID, Role_Code, User_ID)
                    VALUES (@ID, @Role_Code, @User_ID)
                ";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ID", dto.Id);
                cmd.Parameters.AddWithValue("@Role_Code", dto.Role_Code);
                cmd.Parameters.AddWithValue("@User_ID", dto.User_Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        public List<UserRoleDTO> GetAll()
        {
            List<UserRoleDTO> list = new List<UserRoleDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT ID, Role_Code, User_ID FROM UserRoles";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new UserRoleDTO
                        {
                            Id = reader.GetGuid(0),
                            Role_Code = reader.GetInt32(1),
                            User_Id = reader.GetGuid(2)
                        });
                    }
                }
            }

            return list;
        }

        // -------------------------------
        // READ BY ID
        // -------------------------------
        public UserRoleDTO? GetById(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ID, Role_Code, User_ID
                    FROM UserRoles
                    WHERE ID = @ID
                ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserRoleDTO
                        {
                            Id = reader.GetGuid(0),
                            Role_Code = reader.GetInt32(1),
                            User_Id = reader.GetGuid(2)
                        };
                    }
                }
            }

            return null;
        }

        // -------------------------------
        // UPDATE
        // -------------------------------
        public void Update(UserRoleDTO dto)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE UserRoles
                    SET Role_Code = @Role_Code,
                        User_ID = @User_ID
                    WHERE ID = @ID
                ";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ID", dto.Id);
                cmd.Parameters.AddWithValue("@Role_Code", dto.Role_Code);
                cmd.Parameters.AddWithValue("@User_ID", dto.User_Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        public void Delete(Guid id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM UserRoles WHERE ID = @ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
