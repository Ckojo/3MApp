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
    public partial class EmployeePaymentsDetails : Form
    {
        public EmployeePaymentsDetails()
        {
            InitializeComponent();
        }

        private void btnUpdatePayment_Click(object sender, EventArgs e)
        {
            PaymentHelper.InsertInstallenmentDetails(int.Parse(txtPaymentId.Text));
        }
    }
}
