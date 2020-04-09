using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using static System.IO.Path;
using static System.IO.Directory;
using static System.Environment;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DGVPrinterHelper;
using System.Diagnostics;
using RawPrint;

namespace TrimAplikacija_V2._0
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        SqlDataAdapter sqlDataAdapter;
        SqlDataReader sqlDataReader;
        DataTable dataTable;
        UI ui = new UI();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateCompanyDataDG2();
            ui.LoadButtons(flowLayoutPanel1, flowLayoutPanel2, Company_Click);
            PopulateEmployees3();
        }

        string GetConnectionString()
        {
            string sqlConnectionString = @"Data Source =.\SQLEXPRESS; Initial Catalog = TrimDb; Integrated Security = True";
            return sqlConnectionString;
        }

        /// <summary>
        /// Populates data about employees in datagridview3
        /// </summary>
        void PopulateEmployees3()
        {
            string querry = $"SELECT * FROM dbo.zaposleni";

            using(sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter(querry, sqlConnection);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                employeesDataGridView3.AutoGenerateColumns = false;
                employeesDataGridView3.DataSource = dataTable;
            }
        }

        /// <summary>
        /// // Populates data about employees in DataGridView
        /// </summary>
        void PopulateEmployeeData(int i, DataGridView dgvName)
        {
            string query =
                $"SELECT " +
                $"id_zaposlen, " +
                $"ime, " +
                $"prezime, " +
                $"licna_karta, " +
                $"poziv_na_broj, " +
                $"convert(varchar, datum_kupovine, 106) AS datum_kupovine, " +
                $"broj_rata," +
                $"iznos_kupovine," +
                $"ROUND(iznos_rate, 2) AS iznos_rate," +
                $"ukupno_duga," +
                $"uplaceno," +
                $"preostali_dug, " +
                $"firma " +
                $"FROM " +
                $"dbo.zaposleni " +
                $"WHERE firma = {i}";

            //string query = $"SELECT * FROM dbo.zaposleni WHERE firma = {i};";

            // -----  Populates data about employee in a table -----  //
            using (sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                if(dgvName.Name == "employeesDataGridView")
                {
                    employeesDataGridView.AutoGenerateColumns = false;
                    employeesDataGridView.DataSource = dataTable;
                } else if(dgvName.Name == "employeesDataGridView2")
                {
                    employeesDataGridView2.AutoGenerateColumns = false;
                    employeesDataGridView2.DataSource = dataTable;
                }
            }
        }

        /// <summary>
        /// Populates data about a comapny in the first DataGridView
        /// </summary>
        void PopulateCompanyData(string companyName)
        {
            // ----- Populating data about a company -----  //
            using (sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand($"SELECT * FROM dbo.firme WHERE naziv = '{companyName}'", sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    txtCompanyID.Text = sqlDataReader.GetValue(0).ToString();
                    txtCompanyName.Text = sqlDataReader.GetValue(1).ToString();
                    txtHeadQuarter.Text = sqlDataReader.GetValue(2).ToString();
                    txtPIB.Text = sqlDataReader.GetValue(3).ToString();
                    txtTotalDebt.Text = sqlDataReader.GetValue(4).ToString();
                }
            }
        }

        /// <summary>
        /// Populates data about a comapny in the second DataGridView
        /// </summary>
        void PopulateCompanyDataDG2()
        {
            // ----- Populating data about a company -----  //
            using (sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.firme", sqlConnection);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                // Populates data in first datagridview (Tab page - 'Unos firme')
                firmeDataGridView.AutoGenerateColumns = false;
                firmeDataGridView.DataSource = dataTable;

                // Populates data in second datagridview (Tab page - 'Zaposleni - Unos')
                firmeDataGridView2.AutoGenerateColumns = false;
                firmeDataGridView2.DataSource = dataTable;
            }
        }

        private double CompanyDebt()
        {
            double sum = 0;
            for (int i = 0; i < employeesDataGridView.Rows.Count; i++)
            {
                if (employeesDataGridView.Rows[i].Cells[11].Value == null)
                {
                    sum = sum + double.Parse(employeesDataGridView.Rows[i].Cells[11].Value.ToString());
                }
                else if (employeesDataGridView.Rows[i].Cells[11].Value.ToString() != string.Empty)
                {
                    sum = sum + double.Parse(employeesDataGridView.Rows[i].Cells[11].Value.ToString());
                }
            }
            return sum;
        }

        void UpdateTotalDebt(string debt)
        {
            if (employeesDataGridView.CurrentCell == employeesDataGridView.CurrentRow.Cells["txt_preostali_dug"])
            {
                string sqlCmd = $"UPDATE dbo.firme SET ukupno_duga = {float.Parse(debt)} WHERE id_firme = {Convert.ToInt32(txtCompanyID.Text)};";
                sqlCommand = new SqlCommand(sqlCmd, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            } 
        }

        void UpdateTotalDebt2(string debt)
        {
            using(sqlConnection = Connection.AddConnection())
            {
                string sqlCmd = $"UPDATE dbo.firme SET ukupni_dug = {float.Parse(debt)} WHERE id_firme = {Convert.ToInt32(txtCompanyID.Text)};";
                sqlCommand = new SqlCommand(sqlCmd, sqlConnection);
                sqlCommand.ExecuteNonQuery();
            }
        }

        private void Company_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            PopulateCompanyData(clickedButton.Text);
            //Company.PopulateCompanyData(clickedButton.Text, txtCompanyID.Text, txtCompanyName.Text, txtHeadQuarter.Text, txtPIB.Text, txtTotalDebt.Text);
            
            if(txtCompanyID.Text != string.Empty)
            {
                if(clickedButton.Name.Contains("2"))
                    PopulateEmployeeData(Convert.ToInt32(txtCompanyID.Text), employeesDataGridView2);
                else
                    PopulateEmployeeData(Convert.ToInt32(txtCompanyID.Text), employeesDataGridView);
            }

            string totalDebt = CompanyDebt().ToString();
            txtTotalDebt.Text = totalDebt;
            UpdateTotalDebt(totalDebt);
        }

        void AnnuityPaymentDate(int id)
        {
            string querry = $"INSERT INTO dbo.datum_uplate(id_z) VALUES({id});";
            using(sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                if(sqlConnection.State == ConnectionState.Open)
                {
                    sqlCommand = new SqlCommand(querry, sqlConnection);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // Adds and updates users
        private void employeesDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (employeesDataGridView.CurrentRow != null)
            {
                DataGridViewRow gridViewRow = employeesDataGridView.CurrentRow;
                int nRowIndex = employeesDataGridView.Rows.Count - 2;
                if (gridViewRow.Cells["txt_firma"].Value != DBNull.Value)
                {
                    using (sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();
                        sqlCommand = new SqlCommand("EmployeeAddOrEdit", sqlConnection);
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        double annuity = double.Parse(gridViewRow.Cells["txt_iznos_kupovine"].Value.ToString()) / double.Parse(gridViewRow.Cells["txt_broj_rata"].Value.ToString());
                        double leftDebt = double.Parse(gridViewRow.Cells["txt_iznos_kupovine"].Value.ToString()) - double.Parse(gridViewRow.Cells["txt_uplaceno"].Value.ToString());

                        if (gridViewRow.Cells["id_zaposlen"].Value == DBNull.Value) // insert
                        {
                            sqlCommand.Parameters.AddWithValue("@id_zaposlen", 0);
                        }
                        else // update
                        sqlCommand.Parameters.AddWithValue("@id_zaposlen", Convert.ToString(gridViewRow.Cells["id_zaposlen"].Value));
                        sqlCommand.Parameters.AddWithValue("@ime", gridViewRow.Cells["txt_ime"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_ime"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@prezime", gridViewRow.Cells["txt_prezime"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_prezime"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@licna_karta", gridViewRow.Cells["txt_licna_karta"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_licna_karta"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@poziv_na_broj", gridViewRow.Cells["txt_poziv_na_broj"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_poziv_na_broj"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@datum_kupovine", gridViewRow.Cells["txt_datum_kupovine"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_datum_kupovine"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@broj_rata", gridViewRow.Cells["txt_broj_rata"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_broj_rata"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@iznos_kupovine", gridViewRow.Cells["txt_iznos_kupovine"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_iznos_kupovine"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@iznos_rate", gridViewRow.Cells["txt_iznos_rate"].Value == DBNull.Value ? annuity.ToString() : annuity.ToString());
                        sqlCommand.Parameters.AddWithValue("@ukupno_duga", gridViewRow.Cells["txt_ukupno_duga"].Value == DBNull.Value ? gridViewRow.Cells["txt_iznos_kupovine"].Value.ToString() : gridViewRow.Cells["txt_iznos_kupovine"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@uplaceno", gridViewRow.Cells["txt_uplaceno"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_uplaceno"].Value.ToString());
                        sqlCommand.Parameters.AddWithValue("@preostali_dug", gridViewRow.Cells["txt_preostali_dug"].Value == DBNull.Value ? leftDebt.ToString() : leftDebt.ToString());
                        sqlCommand.Parameters.AddWithValue("@firma", gridViewRow.Cells["txt_firma"].Value == DBNull.Value ? "" : gridViewRow.Cells["txt_firma"].Value.ToString());
                        sqlCommand.ExecuteNonQuery();

                        PopulateEmployeeData(Convert.ToInt32(txtCompanyID.Text), employeesDataGridView);
                        string totalDebt = CompanyDebt().ToString();
                        txtTotalDebt.Text = totalDebt;
                        UpdateTotalDebt(totalDebt);
                    }
                }
            }
        }

        // This method deletes users from "DatumUplate" table in Db, once we delete a certain user.
        public void DeleteUsersFromPurchaseDate()
        {
            sqlConnection = Connection.AddConnection();
            int currentRow = Convert.ToInt32(employeesDataGridView.CurrentRow.Cells["id_zaposlen"].Value);
            using (sqlConnection)
            {
                try
                {
                    sqlCommand = new SqlCommand("DeleteFromDatumUplate", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@id_z", currentRow);
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Delets users
        private void employeesDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DeleteUsersFromPurchaseDate();
            Employee employee = new Employee();
            employee.DeleteEmployee(employeesDataGridView);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var ui = new UI();
            Company firma = new Company($"{txtCompanyName2.Text}", $"{txtHeadQuarter2.Text}", $"{txtPIB2.Text}");
            if(txtCompanyName2.Text == string.Empty || txtHeadQuarter2.Text == string.Empty || txtPIB2.Text == string.Empty)
            {
                MessageBox.Show("Molimo, popunite sva polja!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (firma.AddCompany())
            {
                MessageBox.Show("Firma je uspešno unesena!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Greška pri unosu podataka. Pokušajte ponovo!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            PopulateCompanyDataDG2();
            ui.CreateButton(txtCompanyName2.Text, flowLayoutPanel1, Company_Click);
            ui.CreateButton(txtCompanyName2.Text, flowLayoutPanel2, Company_Click);

            foreach (Control c in panel3.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    ((TextBox)c).Text = string.Empty;
                }
            }
        }

        private void firmeDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var ui = new UI();
            if (firmeDataGridView.CurrentRow.Cells["id_firme"].Value != DBNull.Value)
            {
                if (MessageBox.Show("Da li ste sigurni da želite da obrišete firmu?", "Company delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using(sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        int companyID = int.Parse(firmeDataGridView.CurrentRow.Cells["id_firme"].Value.ToString());
                        sqlConnection.Open();
                        sqlCommand = new SqlCommand($"DELETE FROM dbo.zaposleni WHERE firma = {companyID}", sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }
                    using (sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();
                        sqlCommand = new SqlCommand("CompanyDeleteByID", sqlConnection);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@id_firme", Convert.ToInt32(firmeDataGridView.CurrentRow.Cells["id_firme"].Value));
                        sqlCommand.ExecuteNonQuery();
                    }

                    ui.UIRemoveButtons(flowLayoutPanel1, firmeDataGridView);
                    ui.UIRemoveButtons(flowLayoutPanel2, firmeDataGridView);
                }
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }

        private void employeesDataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = employeesDataGridView2.Rows[e.RowIndex];
                List<TextBox> textBoxes = new List<TextBox>();
                List<DateTimePicker> dateTimePickers = new List<DateTimePicker>();
                int currentIdIndex = int.Parse(row.Cells["id_zaposlen_2"].Value.ToString());
                string querry = $"SELECT * FROM dbo.datum_uplate WHERE id_z = {currentIdIndex}";
                sqlConnection = Connection.AddConnection();
                using(sqlConnection)
                {
                    sqlCommand = new SqlCommand(querry, sqlConnection);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    while(sqlDataReader.Read())
                    {
                        txtRata1.Text = sqlDataReader.GetSqlDouble(1).ToString();
                        dtpRata1.Value = sqlDataReader.GetDateTime(2);
                        txtRata2.Text = sqlDataReader.GetSqlDouble(3).ToString();
                        dtpRata2.Value = sqlDataReader.GetDateTime(4);
                        txtRata3.Text = sqlDataReader.GetSqlDouble(5).ToString();
                        dtpRata3.Value = sqlDataReader.GetDateTime(6);
                        txtRata4.Text = sqlDataReader.GetSqlDouble(7).ToString();
                        dtpRata4.Value = sqlDataReader.GetDateTime(8);
                        txtRata5.Text = sqlDataReader.GetSqlDouble(9).ToString();
                        dtpRata5.Value = sqlDataReader.GetDateTime(10);
                        txtRata6.Text = sqlDataReader.GetSqlDouble(11).ToString();
                        dtpRata6.Value = sqlDataReader.GetDateTime(12);
                        txtRata7.Text = sqlDataReader.GetSqlDouble(13).ToString();
                        dtpRata7.Value = sqlDataReader.GetDateTime(14);
                        txtRata8.Text = sqlDataReader.GetSqlDouble(15).ToString();
                        dtpRata8.Value = sqlDataReader.GetDateTime(16);
                        txtRata9.Text = sqlDataReader.GetSqlDouble(17).ToString();
                        dtpRata9.Value = sqlDataReader.GetDateTime(18);
                        txtRata10.Text = sqlDataReader.GetSqlDouble(19).ToString();
                        dtpRata10.Value = sqlDataReader.GetDateTime(20);
                        txtRata11.Text = sqlDataReader.GetSqlDouble(21).ToString();
                        dtpRata11.Value = sqlDataReader.GetDateTime(22);
                        txtRata12.Text = sqlDataReader.GetSqlDouble(23).ToString();
                        dtpRata12.Value = sqlDataReader.GetDateTime(24);
                        txtRata13.Text = sqlDataReader.GetSqlDouble(25).ToString();
                        dtpRata13.Value = sqlDataReader.GetDateTime(26);
                        txtRata14.Text = sqlDataReader.GetSqlDouble(27).ToString();
                        dtpRata14.Value = sqlDataReader.GetDateTime(28);
                        txtRata15.Text = sqlDataReader.GetSqlDouble(29).ToString();
                        dtpRata15.Value = sqlDataReader.GetDateTime(30);
                        txtRata16.Text = sqlDataReader.GetSqlDouble(31).ToString();
                        dtpRata16.Value = sqlDataReader.GetDateTime(32);
                        txtRata17.Text = sqlDataReader.GetSqlDouble(33).ToString();
                        dtpRata17.Value = sqlDataReader.GetDateTime(34);
                        txtRata18.Text = sqlDataReader.GetSqlDouble(35).ToString();
                        dtpRata18.Value = sqlDataReader.GetDateTime(36);
                        txtRata19.Text = sqlDataReader.GetSqlDouble(37).ToString();
                        dtpRata19.Value = sqlDataReader.GetDateTime(38);
                        txtRata20.Text = sqlDataReader.GetSqlDouble(39).ToString();
                        dtpRata20.Value = sqlDataReader.GetDateTime(40);
                    }
                }

                lblEmployeeInfo.Text = $"{row.Cells["ime_2"].Value.ToString()} {row.Cells["prezime_2"].Value.ToString()}";
            }
        }

        private void btnAddAnnuityDate_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            double totalPaid = employee.CountTotalPaid(tabPage3);
            int currentEmployee = int.Parse(employeesDataGridView2.CurrentRow.Cells["id_zaposlen_2"].Value.ToString());
            double purchaseAmount = double.Parse(employeesDataGridView2.CurrentRow.Cells["iznos_kupovine_2"].Value.ToString());


            foreach (DataGridViewRow row in employeesDataGridView.Rows)
            {
                if (row.Cells["id_zaposlen"].Value.ToString() == employeesDataGridView2.CurrentRow.Cells["id_zaposlen_2"].Value.ToString())
                {
                    MessageBox.Show("WORKS");
                    using (sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();
                        string query = $"UPDATE dbo.zaposleni SET uplaceno = {totalPaid} WHERE id_zaposlen = {currentEmployee}";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }

                    using (sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        sqlConnection.Open();
                        string query = $"UPDATE dbo.zaposleni SET preostali_dug = {purchaseAmount - totalPaid} WHERE id_zaposlen = {currentEmployee}";
                        sqlCommand = new SqlCommand(query, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }

                    PopulateEmployeeData(int.Parse(txtCompanyID.Text), employeesDataGridView2);
                    PopulateEmployeeData(int.Parse(txtCompanyID.Text), employeesDataGridView);


                    string totalDebt = CompanyDebt().ToString();
                    txtTotalDebt.Text = totalDebt;
                    UpdateTotalDebt2(totalDebt);
                }
            }


            string querry = $"UPDATE dbo.datum_uplate " +
                $"SET " +
                $"rata_1 = {txtRata1.Text}, " +
                $"rata_1_datum = '{dtpRata1.Value}', " +
                $"rata_2 = {txtRata2.Text}, " +
                $"rata_2_datum = '{dtpRata2.Value}', " +
                $"rata_3 = {txtRata3.Text}, " +
                $"rata_3_datum = '{dtpRata3.Value}', " +
                $"rata_4 = {txtRata4.Text}, " +
                $"rata_4_datum = '{dtpRata4.Value}', " +
                $"rata_5 = {txtRata5.Text}, " +
                $"rata_5_datum = '{dtpRata5.Value}', " +
                $"rata_6 = {txtRata6.Text}, " +
                $"rata_6_datum = '{dtpRata6.Value}', " +
                $"rata_7 = {txtRata7.Text}, " +
                $"rata_7_datum = '{dtpRata7.Value}', " +
                $"rata_8 = {txtRata8.Text}, " +
                $"rata_8_datum = '{dtpRata8.Value}', " +
                $"rata_9 = {txtRata9.Text}, " +
                $"rata_9_datum = '{dtpRata9.Value}', " +
                $"rata_10 = {txtRata10.Text}, " +
                $"rata_10_datum = '{dtpRata10.Value}', " +
                $"rata_11 = {txtRata11.Text}, " +
                $"rata_11_datum = '{dtpRata11.Value}', " +
                $"rata_12 = {txtRata12.Text}, " +
                $"rata_12_datum = '{dtpRata12.Value}', " +
                $"rata_13 = {txtRata13.Text}, " +
                $"rata_13_datum = '{dtpRata13.Value}', " +
                $"rata_14 = {txtRata14.Text}, " +
                $"rata_14_datum = '{dtpRata14.Value}', " +
                $"rata_15 = {txtRata15.Text}, " +
                $"rata_15_datum = '{dtpRata15.Value}', " +
                $"rata_16 = {txtRata16.Text}, " +
                $"rata_16_datum = '{dtpRata16.Value}', " +
                $"rata_17 = {txtRata17.Text}, " +
                $"rata_17_datum = '{dtpRata17.Value}', " +
                $"rata_18 = {txtRata18.Text}, " +
                $"rata_18_datum = '{dtpRata18.Value}', " +
                $"rata_19 = {txtRata19.Text}, " +
                $"rata_19_datum = '{dtpRata19.Value}', " +
                $"rata_20 = {txtRata20.Text}, " +
                $"rata_20_datum = '{dtpRata20.Value}' " +
                $"WHERE id_z = {currentEmployee}";

            sqlConnection = Connection.AddConnection();
            using (sqlConnection)
            {
                sqlCommand = new SqlCommand(querry, sqlConnection);
                if (sqlCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Podaci uspešno uneseni!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string querry = $"SELECT " +
                $"id_zaposlen, " +
                $"ime, " +
                $"prezime, " +
                $"licna_karta, " +
                $"poziv_na_broj, " +
                $"convert(varchar, datum_kupovine, 106) as datum_kupovine, " +
                $"broj_rata, " +
                $"iznos_kupovine, " +
                $"ROUND(iznos_rate, 2) AS iznos_rate, " +
                $"ukupno_duga, " +
                $"uplaceno, " +
                $"preostali_dug, " +
                $"firma " +
                $"FROM dbo.zaposleni " +
                $"WHERE datum_kupovine > '{dtpDateOne.Value}' AND datum_kupovine <= '{dtpDateTwo.Value}' AND firma = {txtCompanyID.Text};";
            using(sqlConnection = new SqlConnection(GetConnectionString()))
            {
                if(txtCompanyID.Text != string.Empty)
                {
                    sqlConnection.Open();
                    sqlDataAdapter = new SqlDataAdapter(querry, sqlConnection);
                    dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);

                    employeesDataGridView.AutoGenerateColumns = false;
                    employeesDataGridView.DataSource = dataTable;
                }
                else
                    MessageBox.Show("Firma nije izabrana! \nIzaberite firmu pre provere obračunskog perioda!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            string totalDebt = CompanyDebt().ToString();
            txtTotalDebt.Text = totalDebt;
            UpdateTotalDebt(totalDebt);
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            PopulateEmployeeData(Convert.ToInt32(txtCompanyID.Text), employeesDataGridView);

            string totalDebt = CompanyDebt().ToString();
            txtTotalDebt.Text = totalDebt;
            UpdateTotalDebt(totalDebt);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            lblTime.Text = DateTime.Now.ToString("T");

            lblDate2.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            lblTime2.Text = DateTime.Now.ToString("T");
        }


        // Adds an employee to the database
        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            int nRowIndex = employeesDataGridView3.Rows.Count - 1;

            Employee employee = new Employee(txtName.Text, txtLastName.Text, txtIdentityCard.Text, txtReferenceNumber.Text, dtpPurchaseDate.Value,
                float.Parse(txtAnnuityNumber.Text), float.Parse(txtPurchaseAmount.Text), 
                float.Parse(txtAnnuityAmount.Text), float.Parse(txtTotalDebtt.Text), float.Parse(txtPaid.Text), float.Parse(txtLeftDebt.Text), int.Parse(txtCompany.Text));
            employee.AddEmployee();


            PopulateEmployees3();
            AnnuityPaymentDate(int.Parse(employeesDataGridView3.Rows[nRowIndex].Cells["id_zaposlen2"].Value.ToString()));

            foreach (Control c in panel1.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    ((TextBox)c).Text = "";
                }
            }

            /*txtSearchComp.Text = "Unesite ime firme";
            txtCompany.Text = "ID";*/
        }

        void ExportToPDF(DataGridView dataGridView, string fileName)
        {
            if (dataGridView.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL);


                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Output.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count);
                            pdfTable.DefaultCell.Padding = 6;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            float[] widths = new float[] { 0f, 75f, 75f, 85f, 100f, 100f, 50f, 65f, 65f, 0f, 0f, 0f, 0f };
                            pdfTable.SetWidths(widths);

                            foreach (DataGridViewColumn column in dataGridView.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, font));
                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(new Phrase(cell.Value.ToString(), font));
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 30f, 30f, 30f, 30f);
                                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();

                                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("logo.png");
                                image.ScalePercent(10);
                                pdfDoc.Add(image);

                                Paragraph pLeft = new Paragraph($"\n__________\nFirma: {txtCompanyName.Text}\nMesto: {txtHeadQuarter.Text}\n" +
                                    $"PIB: {txtPIB.Text}\n\n", font);
                                Paragraph pRight = new Paragraph($"Obračunski period:\n{dtpDateOne.Value.ToString("dd. MM. yyyy.")} - {dtpDateTwo.Value.ToString("dd. MM. yyyy.")}\n\n", font);

                                string sellerInformations = $"\n__________\nTRIM DOO\nMaršala Tita 56, 21460, Vrbas\n(021)/794-355\nPIB: 100639492\nŽiro račun: 275-0000220029609-95\ntrimsports@yahoo.com";
                                var sellerDocument = new Paragraph(sellerInformations, font);

                                pdfDoc.Add(sellerDocument);
                                pdfDoc.Add(pLeft);
                                pdfDoc.Add(pRight);
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Podaci uspešno konvertovani.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            ExportToPDF(employeesDataGridView, "test");
        }

        private void firmeDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Company company = new Company();
            company.EditCompany(firmeDataGridView);
        }

        private void txtPurchaseAmount_TextChanged(object sender, EventArgs e)
        {
            if(txtAnnuityNumber.Text != string.Empty && float.TryParse(txtPurchaseAmount.Text, out float purchaseAmount))
                txtAnnuityAmount.Text = ((float.Parse(txtPurchaseAmount.Text) / (float.Parse(txtAnnuityNumber.Text))).ToString());

            txtTotalDebtt.Text = txtPurchaseAmount.Text;
        }

        private void txtAnnuityNumber_TextChanged(object sender, EventArgs e)
        {
            if (txtPurchaseAmount.Text == string.Empty)
            {
                txtPurchaseAmount.Text = "Unesite iznos kupovine!";
            }
        }

        private void txtPaid_TextChanged(object sender, EventArgs e)
        {
            if(!(float.TryParse(txtPurchaseAmount.Text, out _)))
            {
                txtPurchaseAmount.Text = "Unesite iznos kupovine";
            }

            if(txtPaid.Text != string.Empty && (float.TryParse(txtPurchaseAmount.Text, out _)))
                txtLeftDebt.Text = ((float.Parse(txtPurchaseAmount.Text) - float.Parse(txtPaid.Text))).ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            /*UI ui = new UI();
            ui.PrintDocument(employeesDataGridView);*/
            string path = Combine(GetFolderPath(SpecialFolder.Desktop), "Output.pdf");
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                Verb = "print",
                FileName = path
            };
            p.Start();
        }

        private void btnSearchEmployee_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(employeesDataGridView2);
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.Show();
        }

        private void btnSearchCompany_Click(object sender, EventArgs e)
        {
            // ----- Populating data about a company -----  //
            /*using (sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.firme WHERE naziv LIKE '%{txtSearchCompany.Text}%'", sqlConnection);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                // Populates data in second datagridview (Tab page - 'Zaposleni - Unos')
                firmeDataGridView2.AutoGenerateColumns = false;
                firmeDataGridView2.DataSource = dataTable;

                txtSearchCompany.Text = "";
            }*/
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            PopulateCompanyDataDG2();
        }

        void SearchCompText()
        {
            // ----- Populating data about a company -----  //
            using (sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM dbo.firme WHERE naziv LIKE '%{txtSearchComp.Text}%'", sqlConnection);
                dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                // Populates data in second datagridview (Tab page - 'Zaposleni - Unos')
                firmeDataGridView2.AutoGenerateColumns = false;
                firmeDataGridView2.DataSource = dataTable;
            }
        }

        private void txtSearchComp_TextChanged(object sender, EventArgs e)
        {
            SearchCompText();
        }

        private void txtCompany_TextChanged(object sender, EventArgs e)
        {
            if (txtCompany.Text == string.Empty)
            {
                txtCompany.Text = "ID";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PopulateCompanyDataDG2();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UI.SearchButton(flowLayoutPanel1, txtSearch);
        }

        private void btnShowAll_Click_1(object sender, EventArgs e)
        {
            UI.ShowAllButtons(flowLayoutPanel1);
        }

        private void btnSearchCom_Click(object sender, EventArgs e)
        {
            UI.SearchButton(flowLayoutPanel2, txtSearchCo);
        }

        private void btnGetAllCom_Click(object sender, EventArgs e)
        {
            UI.ShowAllButtons(flowLayoutPanel2);
        }

        private void txtSearchCo_Enter(object sender, EventArgs e)
        {
            if(txtSearchCo.Text == "Pretraga firme...")
            {
                txtSearchCo.Text = string.Empty;
                txtSearchCo.ForeColor = Color.Black;
            }
        }

        private void txtSearchCo_Leave(object sender, EventArgs e)
        {
            if (txtSearchCo.Text == string.Empty)
            {
                txtSearchCo.Text = "Pretraga firme...";
                txtSearchCo.ForeColor = Color.DarkGray;
            }
        }
    }
}