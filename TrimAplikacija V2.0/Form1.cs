using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TrimAplikacija_V2._0
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        SqlDataAdapter sqlDataAdapter;
        SqlDataReader sqlDataReader;
        DataTable dataTable;
        
        public Form1()
        {
            InitializeComponent();
        }

        string GetConnectionString()
        {
            string sqlConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TrimDb;Integrated Security=True";
            return sqlConnectionString;
        }

        void CreateButton(string companyName, FlowLayoutPanel layoutPanel)
        {
            string noSpaceName = companyName.Replace(" ", string.Empty);
            Button button = new Button();
            button.Location = new Point(22, 250);
            button.Name = $"btn{noSpaceName}";
            
            if(layoutPanel.Name == "flowLayoutPanel1")
                button.Name = $"btn{noSpaceName}";
            else if(layoutPanel.Name == "flowLayoutPanel2")
                button.Name = $"btn{noSpaceName}2";

            button.Text = $"{companyName}";
            button.Size = new Size(layoutPanel.ClientSize.Width - 5, 23);
            button.Font = new Font("Segoe UI", 8);
            button.Padding = new Padding(0);
            button.BackColor = Color.White;
            button.FlatStyle = FlatStyle.Popup;
            button.Click += new EventHandler(Company_Click);
            button.Anchor = AnchorStyles.None;
            button.BringToFront();

            layoutPanel.Controls.Add(button);
        }

        void LoadButtons()
        {
            using (sqlConnection = new SqlConnection(GetConnectionString()))
            {
                sqlConnection.Open();
                string querry = "SELECT dbo.firme.id_firme, dbo.firme.naziv FROM dbo.firme;";
                sqlCommand = new SqlCommand(querry, sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    string[] companies = new[] { sqlDataReader["naziv"].ToString() };

                    // Populates button in first flow layout panel ( In tab 'Zaposleni - Detalji')
                    foreach (var company in companies)
                    {
                        CreateButton(companyName: company, layoutPanel: flowLayoutPanel1);
                    }

                    // Populates button in second flow layout panel ( In tab 'Datumi uplate')
                    foreach (var company in companies)
                    {
                        CreateButton(companyName: company, layoutPanel: flowLayoutPanel2);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateCompanyDataDG2();
            LoadButtons();
            PopulateEmployees3();
            //LoadLabelsTab3();
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
            for (int i = 0; i < employeesDataGridView.Rows.Count - 1; i++)
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

        private void Company_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            PopulateCompanyData(clickedButton.Text);

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

        // Delets users
        private void employeesDataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (employeesDataGridView.CurrentRow.Cells["id_zaposlen"].Value != DBNull.Value)
            {
                if (MessageBox.Show("Da li ste sigurni da želite da obrišete firmu?", "Company delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using(sqlConnection = new SqlConnection(GetConnectionString()))
                    {
                        try
                        {
                            sqlConnection.Open();
                            sqlCommand = new SqlCommand("EmployeeDeleteByID", sqlConnection);
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@id_zaposlen", Convert.ToInt32(employeesDataGridView.CurrentRow.Cells["id_zaposlen"].Value));
                            sqlCommand.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
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
            CreateButton(txtCompanyName2.Text, flowLayoutPanel1);
            CreateButton(txtCompanyName2.Text, flowLayoutPanel2);

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

                    UIRemoveButtons(flowLayoutPanel1);
                    UIRemoveButtons(flowLayoutPanel2);
                }
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }

        void UIRemoveButtons(FlowLayoutPanel layoutPanel)
        {
            foreach (Control c in layoutPanel.Controls)
            {
                if (c.GetType() == typeof(Button))
                {
                    if (((Button)c).Text == firmeDataGridView.CurrentRow.Cells["naziv"].Value.ToString())
                    {
                        layoutPanel.Controls.Remove(c);
                    }
                }
            }
        }

        void CreateLabel(int i, int x, int y)
        {
            Label label = new Label();
            label.Text = $"Rata {i}";
            label.Name = $"lblRata{i}";
            label.Location = new Point(x, y);
            label.Font = new Font("Segoe UI", 12);

            tabPage3.Controls.Add(label);
        }

        private void employeesDataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = employeesDataGridView2.Rows[e.RowIndex];
                int currentIdIndex = int.Parse(row.Cells["id_zaposlen_2"].Value.ToString());
                string querry = $"SELECT * FROM dbo.datum_uplate WHERE id_z = {currentIdIndex}";
                using(sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    sqlConnection.Open();
                    sqlCommand = new SqlCommand(querry, sqlConnection);
                    sqlDataReader = sqlCommand.ExecuteReader();

                    while (sqlDataReader.Read())
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
            using(sqlConnection = new SqlConnection(GetConnectionString()))
            {
                string querry = $"UPDATE dbo.datum_uplate SET ";
                List<TextBox> textBoxes = new List<TextBox>();
                List<DateTimePicker> dateTimePickers = new List<DateTimePicker>();
                foreach (Control c in tabPage3.Controls)
                {
                    if (c.GetType() == typeof(TextBox) && c.Name.Contains("txtRata"))
                        textBoxes.Add((TextBox)c);
                }

                foreach (Control c in tabPage3.Controls)
                {
                    if (c.GetType() == typeof(DateTimePicker) && c.Name.Contains("dtpRata"))
                    {
                        dateTimePickers.Add((DateTimePicker)c);
                    }
                }

                textBoxes.Reverse();
                dateTimePickers.Reverse();

                if(textBoxes.Count == dateTimePickers.Count)
                {
                    for(int i = 1; i <= textBoxes.Count; i++)
                    {
                        if(i == 20)
                        {
                            querry += $"rata_{i} = {float.Parse(textBoxes[i - 1].Text)}, " +
                                $"rata_{i}_datum = '{dateTimePickers[i - 1].Value}' " +
                                $"WHERE id_z = {int.Parse(employeesDataGridView2.CurrentRow.Cells["id_zaposlen_2"].Value.ToString())}";
                        }
                        else
                        {
                            querry += $"rata_{i} = {float.Parse(textBoxes[i - 1].Text)}, " +
                            $"rata_{i}_datum = '{dateTimePickers[i - 1].Value}', ";
                        }
                    }
                }

                sqlConnection.Open();
                sqlCommand = new SqlCommand(querry, sqlConnection);
                if(sqlCommand.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Podaci uspešno uneseni", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            PopulateEmployeeData(Convert.ToInt32(txtCompanyID.Text), employeesDataGridView);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            lblTime.Text = DateTime.Now.ToString("T");
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
                    ((TextBox)c).Text = string.Empty;
                }
            }
        }
    }
}