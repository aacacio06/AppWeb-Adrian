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
        var erros = new List<Exception>();

        foreach (var connectionString in GetConnectionStrings())
        {
            try
            {
                return AbrirEInicializar(connectionString);
            }
            catch (Exception exception)
            {
                erros.Add(new InvalidOperationException(
                    $"Falha na conexão {Resumo(connectionString)}: {exception.Message}",
                    exception));
            }
        }

        throw new InvalidOperationException(
            "Não foi possível conectar ao MySQL. Verifique se o MySQL está iniciado, " +
            "se a porta é 3306 ou 3360 e se o usuário root possui a senha configurada.",
            new AggregateException(erros));
    }

    private IEnumerable<string> GetConnectionStrings()
    {
        var configuracao = new MySqlConnectionStringBuilder(_connectionString);
        if (string.IsNullOrWhiteSpace(configuracao.Server)) configuracao.Server = "localhost";
        if (string.IsNullOrWhiteSpace(configuracao.UserID)) configuracao.UserID = "root";

        var portas = new[] { configuracao.Port, 3360u, 3306u }.Distinct();
        var bancos = new[] { configuracao.Database, "pds_app_web", "app_web_bd" }
            .Where(database => !string.IsNullOrWhiteSpace(database))
            .Distinct(StringComparer.OrdinalIgnoreCase);

        foreach (var porta in portas)
        {
            foreach (var banco in bancos)
            {
                var tentativa = new MySqlConnectionStringBuilder(configuracao.ConnectionString)
                {
                    Port = porta,
                    Database = banco
                };

                yield return tentativa.ConnectionString;
            }
        }
    }

    private static MySqlConnection AbrirEInicializar(string connectionString)
    {
        var configuracao = new MySqlConnectionStringBuilder(connectionString);
        var connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            GarantirTabelaProcessos(connection);
            return connection;
        }
        catch
        {
            connection.Dispose();
        }

        using var servidor = new MySqlConnection(new MySqlConnectionStringBuilder(configuracao.ConnectionString)
        {
            Database = string.Empty
        }.ConnectionString);
        servidor.Open();

        using (var criarBanco = servidor.CreateCommand())
        {
            var banco = configuracao.Database.Replace("`", "``", StringComparison.Ordinal);
            criarBanco.CommandText = $"CREATE DATABASE IF NOT EXISTS `{banco}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
            criarBanco.ExecuteNonQuery();
        }

        var retry = new MySqlConnection(connectionString);
        retry.Open();
        GarantirTabelaProcessos(retry);
        return retry;
    }

    private static void GarantirTabelaProcessos(MySqlConnection connection)
    {
        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS processos (
                id_pro INT NOT NULL AUTO_INCREMENT,
                numero_pro VARCHAR(200) NOT NULL,
                data_pro DATE NULL,
                interessado_pro VARCHAR(200) NOT NULL,
                assunto_pro VARCHAR(200) NOT NULL,
                descricao_pro TEXT NULL,
                situacao_pro VARCHAR(50) NOT NULL,
                PRIMARY KEY (id_pro)
            ) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
            """;
        command.ExecuteNonQuery();
    }

    private static string Resumo(string connectionString)
    {
        var configuracao = new MySqlConnectionStringBuilder(connectionString);
        return $"{configuracao.Server}:{configuracao.Port}/{configuracao.Database}";
    }
}
