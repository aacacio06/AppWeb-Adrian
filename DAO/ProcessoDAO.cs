using AppWeb_Adriann.Configs;
using AppWeb_Adriann.Model;
using MySql.Data.MySqlClient;

namespace AppWeb_Adriann.DAO;

public class ProcessoDAO
{
    private readonly Conexao _conexao;

    public ProcessoDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<Processo> Listar()
    {
        var processos = new List<Processo>();
        using var connection = _conexao.GetConnection();
        using var command = new MySqlCommand(
            "SELECT id_pro, numero_pro, data_pro, interessado_pro, assunto_pro, descricao_pro, situacao_pro FROM processos;",
            connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            processos.Add(new Processo
            {
                Id = reader.GetInt32("id_pro"),
                Numero = DAOHelper.GetString(reader, "numero_pro"),
                Data = DAOHelper.GetDateTime(reader, "data_pro"),
                Interessado = DAOHelper.GetString(reader, "interessado_pro"),
                Assunto = DAOHelper.GetString(reader, "assunto_pro"),
                Descricao = DAOHelper.GetString(reader, "descricao_pro"),
                Situacao = DAOHelper.GetString(reader, "situacao_pro")
            });
        }

        return processos;
    }

    public void Inserir(Processo processo)
    {
        using var connection = _conexao.GetConnection();
        using var command = new MySqlCommand(
            """
            INSERT INTO processos
                (numero_pro, data_pro, interessado_pro, assunto_pro, descricao_pro, situacao_pro)
            VALUES
                (@numero, @data, @interessado, @assunto, @descricao, @situacao);
            """,
            connection);

        command.Parameters.AddWithValue("@numero", processo.Numero.Trim());
        command.Parameters.AddWithValue("@data", processo.Data?.Date ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@interessado", processo.Interessado.Trim());
        command.Parameters.AddWithValue("@assunto", processo.Assunto.Trim());
        command.Parameters.AddWithValue("@descricao", processo.Descricao.Trim());
        command.Parameters.AddWithValue("@situacao", processo.Situacao.Trim());

        command.ExecuteNonQuery();
    }
}
