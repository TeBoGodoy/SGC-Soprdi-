using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
/// <summary>
/// Descripción breve de ConnectionManager
/// </summary>
namespace Conexion
{
    public class ConnectionManager
    {
        public static SqlConnection getSqlConnection()
        {
            return new SqlConnection(getConnectionString("SQLSERVER"));
        }
        public static String getConnectionString(String connectionName)
        {
            //return @"Data Source=SQL5020.Smarterasp.net;Initial Catalog=DB_9A4352_purahoja;User ID=DB_9A4352_purahoja_admin;Password=12345678abc" providerName="System.Data.SqlClient"";
            return @"Data Source=192.168.10.45;Initial Catalog=new_thx;User ID=sa;Password=Soprodi1234";
            //return @"Data Source=thinxsys.database.windows.net;Initial Catalog=DDB_9A4352_purahoja;User ID=thinxsys;Password=Sebastian.1984;";
        }
    }
}
