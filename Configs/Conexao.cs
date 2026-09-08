using MySql.Data.MySqlClient;

namespace AppWeb_Adriann.Configs;

public class Conexao
{
    private readonly string _connectionString;

    public Conexao(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection") ?? string.Empty;
    }

    public MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
