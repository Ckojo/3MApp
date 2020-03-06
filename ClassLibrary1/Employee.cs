using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace TrimAplikacija_V2._0
{
    public class Employee 
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        SqlDataAdapter sqlDataAdapter;
        DataTable dataTable;

        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentityCard { get; set; }
        public string ReferenceNumber{ get; set; }
        public DateTime PurchaseDate { get; set; }
        public float AnnuityNumber { get; set; }
        public float PurchaseAmount { get; set; }
        public float AnnuityAmount { get; set; }
        public float TotalDebt { get; set; }
        public float Paid { get; set; }
        public float LeftDebt { get; set; }
        public int Company { get; set; }

        public Employee()
        {
            this.Name = "Undefined";
            this.LastName = "Undefined";
            this.IdentityCard = "00";
            this.ReferenceNumber = "00-00-00";
            this.PurchaseDate = new DateTime();
            this.AnnuityNumber = 0;
            this.PurchaseAmount = 0;
            this.AnnuityAmount = 0;
            this.TotalDebt = 0;
            this.Paid = 0;
            this.LeftDebt = 0;
            this.Company = 0;
        }

        public Employee(string name, string lastName, string identityCard, string referenceNumber, DateTime purchaseDate, float annuityNumber, float purchaseAmount, float annuityAmmount, float totalDebt, float paid, float leftDebt, int company)
        {
            this.Name = name;
            this.LastName = lastName;
            this.IdentityCard = identityCard;
            this.ReferenceNumber = referenceNumber;
            this.PurchaseDate = purchaseDate;
            this.AnnuityNumber = annuityNumber;
            this.PurchaseAmount = purchaseAmount;
            this.AnnuityAmount = annuityAmmount;
            this.TotalDebt = totalDebt;
            this.Paid = paid;
            this.LeftDebt = leftDebt;
            this.Company = company;
        }

        public void AddEmployee()
        {
            sqlConnection = Connection.AddConnection();
            using(sqlConnection)
            {
                string querry = $"" +
                    $"INSERT INTO dbo.zaposleni(ime, prezime, licna_karta, poziv_na_broj, datum_kupovine, broj_rata, iznos_kupovine, iznos_rate, ukupno_duga, uplaceno, preostali_dug, firma)" +
                    $"VALUES('{this.Name}', '{this.LastName}', '{this.IdentityCard}', '{this.ReferenceNumber}', '{this.PurchaseDate}', " +
                    $"{this.AnnuityNumber}, {this.PurchaseAmount}, {this.AnnuityAmount}, {this.TotalDebt}, {this.Paid}, {this.LeftDebt}, {this.Company});";
                sqlCommand = new SqlCommand(querry, sqlConnection);
                if(sqlCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Podaci su uspešno uneseni!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        public void DeleteEmployee(DataGridView gridView)
        {
            sqlConnection = Connection.AddConnection();
            if (gridView.CurrentRow.Cells["id_zaposlen"].Value != DBNull.Value)
            {
                using (sqlConnection)
                {
                    try
                    {
                        sqlCommand = new SqlCommand("EmployeeDeleteByID", sqlConnection);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@id_zaposlen", Convert.ToInt32(gridView.CurrentRow.Cells["id_zaposlen"].Value));
                        sqlCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        } 

        public double CountTotalPaid(TabPage tabPage)
        {
            double sum = 0;
            foreach (Control c in tabPage.Controls)
            {
                if(c.GetType() == typeof(TextBox))
                {
                    sum += double.Parse(c.Text);
                }
            }
            return sum;
        }
    }
}