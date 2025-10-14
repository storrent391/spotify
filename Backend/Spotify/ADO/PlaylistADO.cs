using Microsoft.Data.SqlClient;
using static System.Console;
using dbdemo.Services;
using dbdemo.Model;

namespace dbdemo.Repository;

class PlaylistADO
{
   
    public static void Insert(DatabaseConnection dbConn,Product product)    // El mètode ha de passar a ser static
    {

        dbConn.Open();

        string sql = @"INSERT INTO Playlist (Id, Code, Name, Price)
                        VALUES (@Id, @Code, @Name, @Price)";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", product.Id);
        cmd.Parameters.AddWithValue("@Code", product.Code);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);

        int rows = cmd.ExecuteNonQuery();
        Console.WriteLine($"{rows} fila inserida.");
        dbConn.Close();
    }

    public static void Update(DatabaseConnection dbConn, Product product)
    {
        dbConn.Open();

        string sql = @"UPDATE Products
                    SET Code = @Code,
                        Name = @Name,
                        Price = @Price,
                        Image = @Image
                    WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", product.Id);
        cmd.Parameters.AddWithValue("@Code", product.Code);
        cmd.Parameters.AddWithValue("@Name", product.Name);
        cmd.Parameters.AddWithValue("@Price", product.Price);
        cmd.Parameters.AddWithValue("@Image", product.ImagePath);

        int rows = cmd.ExecuteNonQuery();

        Console.WriteLine($"{rows} fila actualitzada.");
       
        dbConn.Close();
    }

    public static List<Product> GetAll(DatabaseConnection dbConn)
    {
        List<Product> products = new();

        dbConn.Open();
        string sql = "SELECT Id, Code, Name, Price FROM Products";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Price = reader.GetDecimal(3)
            });
        }

        dbConn.Close();
        return products;
    }

    public static Product? GetById(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();
        string sql = "SELECT Id, Code, Name, Price FROM Products WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();
        Product? product = null;    // Si no inicialitzem la variable => no existeix en el return!

        if (reader.Read())
        {
            product = new Product
            {
                Id = reader.GetGuid(0),
                Code = reader.GetString(1),
                Name = reader.GetString(2),
                Price = reader.GetDecimal(3)
            };
        }

        dbConn.Close();
        return product;
    }

    public static bool Delete(DatabaseConnection dbConn, Guid id)
    {
        dbConn.Open();

        string sql = @"DELETE FROM Products WHERE Id = @Id";

        using SqlCommand cmd = new SqlCommand(sql, dbConn.sqlConnection);
        cmd.Parameters.AddWithValue("@Id", id);

        int rows = cmd.ExecuteNonQuery();

        dbConn.Close();

        return rows > 0;
    }

}