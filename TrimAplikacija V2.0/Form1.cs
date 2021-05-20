using System.Linq;
using System.Windows.Forms;
using TrimAplikacija_V2._0.DataAccess;

namespace TrimAplikacija_V2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            using (DataContext dataContext = new DataContext()) 
            {
                employeesDataGridView.DataSource = dataContext.Employees.ToList();
            }
        }
    }
}