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
    }
}
