using MySql.Data.MySqlClient;

namespace Management
{
    public class DBContent
    {
        public static string ConnectSting = System.Configuration.ConfigurationManager.ConnectionStrings["MySQL"].ToString();
        public static MySqlConnection mySql = new MySqlConnection(ConnectSting);

    }
}
