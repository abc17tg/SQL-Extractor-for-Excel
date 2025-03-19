using System.IO;
using Newtonsoft.Json;

namespace SQL_Extractor_for_Excel.Scripts
{
    public class SqlConn
    {
        public readonly string Name;
        public readonly string UserName;
        public readonly string Password;
        public readonly string Link;
        public readonly string Port;
        public readonly SqlServerManager.ServerType Type;

        public string ConnectionString(int timeout = -1)
        {
            switch (Type)
            {
                case SqlServerManager.ServerType.SqlServer:
                    if (!string.IsNullOrEmpty(Link) && !string.IsNullOrEmpty(Port) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(UserName))
                        return $"Data Source=\"{Link},{Port}\";User ID=\"{UserName}\";Password=\"{m_password}\";{(timeout > 0 ? $"Connect Timeout={timeout};" : string.Empty)}";
                    break;
                case SqlServerManager.ServerType.Oracle:
                    if (!string.IsNullOrEmpty(Link) && !string.IsNullOrEmpty(Port) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Name))
                        return $"DATA SOURCE=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={Link})(PORT={Port})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={Name})));USER ID={UserName};PASSWORD={m_password};{(timeout > 0 ? $"Connection Timeout={timeout};" : string.Empty)}";
                    break;
                default:
                    break;
            }
            return null;

        }

        private string m_password => AesEncryption.DecryptString(Password);
        private SqlConn() { }

        public SqlConn(string name, string userName, string password, string link, string port, SqlServerManager.ServerType type, bool encrypt = false)
        {
            Name = name;
            UserName = userName;
            if (encrypt)
                Password = AesEncryption.EncryptString(password);
            else
                Password = password;
            Link = link;
            Port = port;
            Type = type;
        }

        public SqlConn Clone() 
        {
            return new SqlConn(Name, UserName, Password, Link, Port, Type, false); 
        }

        public static bool SaveSqlConn(SqlConn sqlConn)
        {
            string json = JsonConvert.SerializeObject(sqlConn, Newtonsoft.Json.Formatting.Indented);
            string result = Microsoft.VisualBasic.Interaction.InputBox("Name your server connection", "Name your server connection", "", 0, 0);
            if (string.IsNullOrWhiteSpace(result))
                return false;
            try
            {
                switch (sqlConn.Type)
                {
                    case SqlServerManager.ServerType.SqlServer:
                        File.WriteAllText(Path.Combine(FileManager.SqlServerQueriesPath, $"{result}.json"), json);
                        break;
                    case SqlServerManager.ServerType.Oracle:
                        File.WriteAllText(Path.Combine(FileManager.OracleQueriesPath, $"{result}.json"), json);
                        break;
                    default:
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static SqlConn LoadSqlConn(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SqlConn>(json);
        }

        public bool Test()
        {
            switch (Type)
            {
                case SqlServerManager.ServerType.SqlServer:
                    return SqlServerManager.TestConnectionSqlServer(ConnectionString());
                case SqlServerManager.ServerType.Oracle:
                    return SqlServerManager.TestConnectionOracle(ConnectionString());
                default:
                    return false;
            }
        }
    }
}
