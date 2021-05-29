using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimAplikacija_V2._0.DataAccess
{
    public class Payment
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{MM/DD/YY}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }

        public int Installements { get; set; }

        public double PurchaseAmount { get; set; }

        public double InstallementAmount { get; set; }

        public double TotalDebt { get; set; }

        public double Paid { get; set; }

        public double DebtLeft { get; set; }

        public Employee Employee { get; set; }
    }
}
