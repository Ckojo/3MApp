using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlClient;

namespace TrimAplikacija_V2._0
{
    public class Company
    {
        public string CompanyName { get; set; }
        public string HeadQuarter { get; set; }
        public string Pib { get; set; }

        public Company()
        {
            CompanyName = "Undefined";
            HeadQuarter = "Undefined";
            Pib = "Undefined";
        }

        public Company(string companyName, string headQuarter, string pib)
        {
            this.CompanyName = companyName;
            this.HeadQuarter = headQuarter;
            this.Pib = pib;
        }

        public bool AddCompany()
        {
            using (Connection.AddConnection())
            {
                string commandText = $"INSERT INTO dbo.firme(naziv, mesto, pib) VALUES('{this.CompanyName}','{this.HeadQuarter}', '{this.Pib}');";
                if (Connection.Command(commandText))
                    return true;
                else
                    return false;
            }
        }

        // Zakomentarisana dok se ne pronadje resenje
        /*public static void PopulateCompanyData(string companyName, string companyID, string name, string headQuarter, string pib, string totalDebt)
        {
            // ----- Populating data about a company -----  //
            SqlConnection connection = Connection.AddConnection();
            using (connection)
            {
                SqlCommand sqlCommand = new SqlCommand($"SELECT * FROM dbo.firme WHERE naziv = '{companyName}'", connection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    companyID = sqlDataReader.GetValue(0).ToString();
                    name = sqlDataReader.GetValue(1).ToString();
                    headQuarter = sqlDataReader.GetValue(2).ToString();
                    pib = sqlDataReader.GetValue(3).ToString();
                    totalDebt = sqlDataReader.GetValue(4).ToString();
                }
            }
        }*/
    }
}
