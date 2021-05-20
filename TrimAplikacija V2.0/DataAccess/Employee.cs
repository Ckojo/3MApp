using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimAplikacija_V2._0.DataAccess
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string IdNumber { get; set; }

        [MaxLength(255)]
        public string UniqueNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int Installements { get; set; }
        public double PurchaseAmount { get; set; }
        public double InstallementAmount { get; set; }
        public double TotalDebt { get; set; }
        public double Paid { get; set; }
        public double DebtLeft { get; set; }
        public Company CompanyId { get; set; }
    }
}
