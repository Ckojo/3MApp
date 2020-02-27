using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Windows.Forms;

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

        public void EditCompany(DataGridView gridView)
        {
            SqlConnection sqlConnection = Connection.AddConnection();
            if(gridView.CurrentRow != null)
            {
                DataGridViewRow viewRow = gridView.CurrentRow;
                if(viewRow.Cells["id_firme"].Value != DBNull.Value)
                {
                    using(sqlConnection)
                    {
                        SqlCommand sqlCommand = new SqlCommand("CompanyEdit", sqlConnection);
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        if (viewRow.Cells["id_firme"].Value != DBNull.Value)
                        {
                            sqlCommand.Parameters.AddWithValue("@id_firme", Convert.ToString(viewRow.Cells["id_firme"].Value));
                            sqlCommand.Parameters.AddWithValue("@naziv", viewRow.Cells["naziv"].Value == DBNull.Value ? "" : viewRow.Cells["naziv"].Value);
                            sqlCommand.Parameters.AddWithValue("@mesto", viewRow.Cells["mesto"].Value == DBNull.Value ? "" : viewRow.Cells["mesto"].Value);
                            sqlCommand.Parameters.AddWithValue("@pib", viewRow.Cells["pib"].Value == DBNull.Value ? "" : viewRow.Cells["pib"].Value);
                            sqlCommand.Parameters.AddWithValue("@ukupni_dug", viewRow.Cells["ukupni_dug"].Value == DBNull.Value ? "" : viewRow.Cells["ukupni_dug"].Value);
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
