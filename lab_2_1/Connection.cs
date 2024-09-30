using Microsoft.Data.SqlClient;

public class DatabaseConnection
{
    private string _connectionString;
    public DatabaseConnection()
    {
        _connectionString = "Data Source=(localdb)\\olx;Initial Catalog=lab_2_1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}