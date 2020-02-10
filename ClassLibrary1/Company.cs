using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
