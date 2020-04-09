using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrimAplikacija_V2._0
{
    public partial class Form2 : Form
    {
        private DataGridView gridView;

        public DataGridView GridView
        {
            get { return gridView; }
            set { gridView = value; }
        }

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(DataGridView gridView)
        {
            InitializeComponent();
            this.GridView = gridView;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            Employee employee = new Employee();
            employee.SearchEmployee(GridView, txtEmployeeName, txtEmployeeLastName);

            this.Close();
            txtEmployeeName.Text = "";
            txtEmployeeLastName.Text = "";
        }
    }
}
