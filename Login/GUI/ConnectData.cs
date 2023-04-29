using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.GUI
{
    class ConnectData
    {
        private static OracleConnection _connection;
        private static string _connectionString;

        static ConnectData()
        {
            _connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.238.1)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));";
            _connection = new OracleConnection(_connectionString);
        }

        public static bool OpenConnection(string username, string password)
        {
            string connectionStringWithCredentials = _connectionString + $"User Id={username};Password={password};DBA Privilege=SYSDBA;";
            _connection.ConnectionString = connectionStringWithCredentials;

            try
            {
                _connection.Open();
                return true;
            }
            catch (OracleException)
            {
                return false;
            }
        }

        public static void CloseConnection()
        {
            _connection.Close();
        }

        public static OracleDataReader ExecuteQuery(string query)
        {
            OracleCommand command = new OracleCommand(query, _connection);
            return command.ExecuteReader();
        }

        public static void ExecuteNonQuery(string query)
        {
            OracleCommand command = new OracleCommand(query, _connection);
            command.ExecuteNonQuery();
        }
    }
}
