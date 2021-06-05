using Newtonsoft.Json;
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
    public partial class EmployeePaymentsDetails : Form
    {
        public EmployeePaymentsDetails()
        {
            InitializeComponent();
        }

        private void btnUpdatePayment_Click(object sender, EventArgs e)
        {
            string textBoxValue = "";
            DateTime? dateTimeValue = null;
            var paymentDetails = new List<(DateTime InstallenmentDate, double InstallenmentAmount)>();

            foreach (var control in groupBoxPaymentDetails.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
            {
                if (control is TextBox textBox)
                {
                    textBoxValue = textBox.Text;
                }
                if (control is DateTimePicker dateTime)
                {
                    dateTimeValue = dateTime.Value;
                }

                if (!string.IsNullOrEmpty(textBoxValue) && dateTimeValue != null)
                {
                    var paymentDateDetail = (DateTime.Parse(dateTimeValue.ToString()), double.Parse(textBoxValue));
                    paymentDetails.Add(paymentDateDetail);
                    
                    textBoxValue = "";
                    dateTimeValue = null;
                }
            }

            PaymentHelper.InsertInstallenmentDetails(int.Parse(txtPaymentId.Text), paymentDetails);
        }
    }
}
