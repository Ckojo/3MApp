using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace TrimAplikacija_V2._0
{
    public class Connection
    {
        
        public static SqlConnection AddConnection()
        {
            string sqlConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TrimDb;Integrated Security=True";
            SqlConnection connection = new SqlConnection(sqlConnectionString);
            connection.Open();
            return connection;
        }
        public static bool Command(string commandTxt)
        {
            SqlCommand sqlCommand = new SqlCommand(commandTxt, AddConnection());
            if(sqlCommand.ExecuteNonQuery() == 1) 
                return true; 
            else 
                return false; 

        }

        public static List<string> SearchQuery(string query)
        {
            SqlConnection sqlConnection = Connection.AddConnection();
            List<string> results = new List<string>();
            using (sqlConnection)
            {
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    results.Add(sqlDataReader.GetString(0));
                }
            }

            return results;
        }
    }
}
