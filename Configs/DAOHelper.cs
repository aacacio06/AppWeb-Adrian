using MySql.Data.MySqlClient;

namespace AppWeb_Adriann.Configs;

public static class DAOHelper
{
    public static string GetString(MySqlDataReader reader, string columnName)
    {
        return reader.IsDBNull(reader.GetOrdinal(columnName))
            ? string.Empty
            : reader.GetString(columnName);
    }

    public static DateTime? GetDateTime(MySqlDataReader reader, string columnName)
    {
        return reader.IsDBNull(reader.GetOrdinal(columnName))
            ? null
            : reader.GetDateTime(columnName);
    }
}
