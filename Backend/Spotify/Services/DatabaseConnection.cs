using Microsoft.Data.SqlClient;
using static System.Console;
using Spotify.Model;

namespace Spotify.Services;

public class DatabaseConnection
{
    private readonly string _connectionString;
    public SqlConnection? sqlConnection;
    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }
    public bool Open()
    {
        sqlConnection = new SqlConnection(_connectionString);

        try
        {
            sqlConnection.Open();
            return true;
        }
        catch (Exception ex)
        {
            WriteLine(ex);
            return false;
        }
    }
    public void Close()
    {
        sqlConnection?.Close();
    }
}
