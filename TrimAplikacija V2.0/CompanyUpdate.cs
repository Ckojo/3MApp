using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrimAplikacija_V2._0.Helpers;

namespace TrimAplikacija_V2._0
{
    public partial class CompanyUpdate : Form
    {
        public CompanyUpdate()
        {
            InitializeComponent();
        }

        private void btnUpdateCompany_Click(object sender, EventArgs e)
        {
            var updatedCompanyDictionary = new Dictionary<string, string>();
            updatedCompanyDictionary["companyId"] = txtUpdateCompanyId.Text;
            updatedCompanyDictionary["companyName"] = txtUpdateCompanyName.Text;
            updatedCompanyDictionary["companyCity"] = txtUpdateCompanyCity.Text;
            updatedCompanyDictionary["companyEmail"] = txtUpdateCompanyEmail.Text;
            updatedCompanyDictionary["companyTIN"] = txtUpdateTIN.Text;

            var isSuccess = CompanyHelper.UpdateCompany(int.Parse(updatedCompanyDictionary["companyId"]), updatedCompanyDictionary);

            if (isSuccess)
            {
                MessageBox.Show("Firma je uspesno azurirana.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Doslo je do greske. Pokusajte ponovo kasnije.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }

        private void btnDeleteCompany_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Da li ste sigurni da zelite da izvrsite ovu radnju?", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if(dialogResult.Equals(DialogResult.OK))
            {
                var isSuccess = CompanyHelper.DeleteCompany(int.Parse(txtUpdateCompanyId.Text));
                
                if(isSuccess)
                {
                    MessageBox.Show("Firma je uspesno izbrisana.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Doslo je do greske. Pokusajte ponovo kasnije", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Close();
            }
        }
    }
}
