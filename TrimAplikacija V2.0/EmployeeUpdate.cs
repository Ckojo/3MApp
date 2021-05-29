using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrimAplikacija_V2._0.DataAccess;
using TrimAplikacija_V2._0.Helpers;

namespace TrimAplikacija_V2._0
{
    public partial class EmployeeUpdate : Form
    {
        public EmployeeUpdate()
        {
            InitializeComponent();
        }

        private void EmployeeUpdate_Load(object sender, EventArgs e)
        {
            UpdatePaymentsGridView();
        }

        private void btnAddNewPayment_Click(object sender, EventArgs e)
        {
            var paymentDictionary = new Dictionary<string, string>();
            paymentDictionary["purchaseDate"] = dtpEditEmployeePurchaseDate.Value.ToString();
            paymentDictionary["installements"] = txtEditEmployeeInstallenmentsNumber.Text;
            paymentDictionary["purchaseAmount"] = txtEditEmployeePurchaseAmount.Text;
            paymentDictionary["installementAmount"] = txtEditEmployeeInstallenmentAmount.Text;
            paymentDictionary["totalDebt"] = txtEditEmployeedTotalDebt.Text;
            paymentDictionary["paid"] = txtEditEmployeePaid.Text;
            paymentDictionary["debtLeft"] = txtEditEmployeeDebtLeft.Text;
            paymentDictionary["employeeId"] = txtEditEmployeeId.Text;

            var isSuccess = PaymentHelper.InsertPayment(paymentDictionary);

            if (isSuccess)
            {
                MessageBox.Show($"Nova uplata za radnika {txtEditEmployeeFirstName.Text} {txtEditEmployeeLastName.Text} je uspesno kreirana.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Doslo je do greske. Pokusajte ponovo kasnije.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdatePaymentsGridView();
        }

        private void UpdatePaymentsGridView()
        {
            var employeePayments = PaymentHelper.GetPaymentsByEmployee(int.Parse(txtEditEmployeeId.Text));

            using (DataContext data = new DataContext())
            {
                paymentsBindingSource.DataSource = employeePayments.OrderBy(d => d.PurchaseDate).ToList();
            }
        }

        private void txtEditEmployeePurchaseAmount_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEditEmployeeInstallenmentsNumber.Text) && !string.IsNullOrEmpty(txtEditEmployeePurchaseAmount.Text))
            {
                txtEditEmployeeInstallenmentAmount.Text = (double.Parse(txtEditEmployeePurchaseAmount.Text) / int.Parse(txtEditEmployeeInstallenmentsNumber.Text)).ToString();
            }
        }

        private void txtEditEmployeePurchaseAmount_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEditEmployeePurchaseAmount.Text))
            {
                txtEditEmployeedTotalDebt.Text = txtEditEmployeePurchaseAmount.Text;
            }
        }

        private void txtEditEmployeePaid_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtEditEmployeePaid.Text))
            {
                txtEditEmployeeDebtLeft.Text = (double.Parse(txtEditEmployeedTotalDebt.Text) - double.Parse(txtEditEmployeePaid.Text)).ToString();
            }
        }

        private void paymentsBindingSource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var data = paymentsBindingSource.Rows[e.RowIndex];

                EmployeePaymentsDetails employeePaymentsDetails = new EmployeePaymentsDetails();
                employeePaymentsDetails.txtPaymentId.Text = data.Cells["Id"].Value.ToString();
                employeePaymentsDetails.ShowDialog();
            }
        }
    }
}
