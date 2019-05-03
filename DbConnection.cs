using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace LogReg.Connectors // In your controllers where you want to use all this stuff, put "using [[THIS SAME NAMESPACE]]" at the top.
{
    public class DbConnector
    {
        static string server = "localhost";
        static string db = "deliverthething"; //Change to your schema name
        static string port = "3306"; //Potentially 8889
        static string user = "root";
        static string pass = "root";
        internal static IDbConnection Connection {
            get {
                return new MySqlConnection($"Server={server};Port={port};Database={db};UserID={user};Password={pass};SslMode=None");
            }
        }
        
        //This method runs a query and stores the response in a list of dictionary records
        public static List<Dictionary<string, object>> Query(string queryString)
        {
            using(IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                command.CommandText = queryString;
                dbConnection.Open();
                var result = new List<Dictionary<string, object>>();
                using(IDataReader rdr = command.ExecuteReader())
                {
                    while(rdr.Read())
                    {
                        var dict = new Dictionary<string, object>();
                        for( int i = 0; i < rdr.FieldCount; i++ ) {
                            dict.Add(rdr.GetName(i), rdr.GetValue(i));
                        }
                        result.Add(dict);
                    }
                    return result;
                                    }
                }
            }
        }
        //This method run a query and returns no values
        public static void Execute(string queryString)
        {
            using (IDbConnection dbConnection = Connection)
            {
                using(IDbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = queryString;
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}