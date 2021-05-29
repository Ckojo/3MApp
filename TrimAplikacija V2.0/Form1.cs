using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrimAplikacija_V2._0.DataAccess;
using TrimAplikacija_V2._0.Helpers;

namespace TrimAplikacija_V2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            UpdateCompanyGridView();
            AddButtonsToLayoutPanel();
        }

        private void btnAddEmp_Click(object sender, System.EventArgs e)
        {
            var employeeDictionary = new Dictionary<string, string>();
            employeeDictionary["FirstName"] = txtFirstName.Text;
            employeeDictionary["LastName"] = txtLastName.Text;
            employeeDictionary["IdNumber"] = txtIdNumber.Text;
            employeeDictionary["UniqueNumber"] = txtUniqueNumber.Text;
            employeeDictionary["CompanyId"] = txtCompany.Text;

            var isSuccess = EmployeeHelper.InsertEmployee(employeeDictionary);

            if(isSuccess)
            {
                MessageBox.Show("Zaposleni je uspesno zaveden", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Doslo je do greske. Pokusajte kasnije.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            var companyDictionary = new Dictionary<string, string>();
            companyDictionary["companyName"] = txtAddCompanyName.Text;
            companyDictionary["companyCity"] = txtAddCompanyCity.Text;
            companyDictionary["companyEmail"] = txtAddCompanyEmail.Text;
            companyDictionary["companyTIN"] = txtAddTIN.Text;
            companyDictionary["companyTotalDebt"] = txtAddTotalDebtCompany.Text;

            var isSuccess = CompanyHelper.InsertCompany(companyDictionary);

            if (isSuccess)
            {
                MessageBox.Show("Nova firma je uspesno zavedena.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Doslo je do greske. Pokusajte ponovo kasnije.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            foreach (var textbox in panel3.Controls)
            {
                if (textbox is TextBox box) box.Text = string.Empty;
            }

            UpdateCompanyGridView();
            AddButtonsToLayoutPanel();
        }

        private void companiesBindingSource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var data = companiesBindingSource.Rows[e.RowIndex];

                CompanyUpdate companyUpdate = new CompanyUpdate();
                companyUpdate.StartPosition = FormStartPosition.CenterScreen;
                companyUpdate.txtUpdateCompanyId.Text = data.Cells["CompanyId"].Value.ToString();
                companyUpdate.txtUpdateCompanyName.Text = data.Cells["CompanyName"].Value.ToString();
                companyUpdate.txtUpdateCompanyCity.Text = data.Cells["City"].Value.ToString(); 
                companyUpdate.txtUpdateCompanyEmail.Text = data.Cells["Email"].Value.ToString();
                companyUpdate.txtUpdateTIN.Text = data.Cells["TIN"].Value.ToString();
                companyUpdate.ShowDialog();

                UpdateCompanyGridView();
            }
        }

        private void employeesBindingSource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                var data = employeesBindingSource.Rows[e.RowIndex];

                EmployeeUpdate employeeUpdate = new EmployeeUpdate();
                employeeUpdate.txtEditEmployeeId.Text = data.Cells["EmployeeId"].Value.ToString();
                employeeUpdate.txtEditEmployeeFirstName.Text = data.Cells["FirstName"].Value.ToString();
                employeeUpdate.txtEditEmployeeLastName.Text = data.Cells["LastName"].Value.ToString();
                employeeUpdate.txtEditEmployeeIdNumber.Text = data.Cells["IdNumber"].Value.ToString();
                employeeUpdate.txtEditEmployeeUniqueNumber.Text = data.Cells["UniqueNumber"].Value.ToString();
                employeeUpdate.ShowDialog();
            }
        }

        private void UpdateEmployeeGridView()
        {
            using (DataContext data = new DataContext())
            {
                employeesBindingSource.DataSource = data.Employees.ToList();
            }
        }

        private void UpdateCompanyGridView()
        {
            using (DataContext data = new DataContext())
            {
                companiesBindingSource.DataSource = data.Companies.ToList();
                companiesHelperBindingSource.DataSource = data.Companies.ToList();
            }
        }

        private void AddButtonsToLayoutPanel()
        {
            flowLayoutPanel1.Controls.Clear();

            var companies = CompanyHelper.GetCompanies();

            foreach (var company in companies)
            {
                Button companyButton = new Button();
                companyButton.Text = company.Name;
                companyButton.Size = new Size(flowLayoutPanel1.ClientSize.Width - 5, 23);
                companyButton.Font = new System.Drawing.Font("Segoe UI", 8);
                companyButton.Padding = new Padding(0);
                companyButton.BackColor = Color.White;
                companyButton.FlatStyle = FlatStyle.Popup;
                companyButton.Click += new EventHandler(Company_Click);
                companyButton.Anchor = AnchorStyles.None;
                companyButton.BringToFront();

                flowLayoutPanel1.Controls.Add(companyButton);
            }
        }

        private void Company_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var selectedCompany = CompanyHelper.GetCompanyByName(btn.Text)[0];
            var employees = EmployeeHelper.GetEmployeesByCompany(selectedCompany);

            txtCompanyID.Text = selectedCompany.CompanyId.ToString();
            txtCompanyName.Text = selectedCompany.Name;
            txtCompanyCity.Text = selectedCompany.City;
            txtTIN.Text = selectedCompany.TIN;
            txtTotalDebt.Text = selectedCompany.TotalDebt.ToString();

            employeesBindingSource.DataSource = employees.ToList();
        }
    }
}