using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimAplikacija_V2._0.DataAccess
{
    public class PaymentDateDetail
    {
        public int Id { get; set; }
        public double InstallenmentPaymentAmount { get; set; }
        public DateTime InstallenmentPurchaseDate { get; set; }
    }
}
