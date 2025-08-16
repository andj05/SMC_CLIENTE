using MySql.Data.MySqlClient;

namespace SMC_CLIENTE.Data
{
    public class DatabaseConnection
    {
        private static string connectionString = "Server=localhost;Database=consultorio_medico;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
