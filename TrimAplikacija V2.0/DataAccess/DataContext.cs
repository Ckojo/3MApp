using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrimAplikacija_V2._0.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext() : base("name=con") { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentDate> PaymentDates { get; set; }
        public DbSet<InstallenmentAmount> InstallenmentAmounts { get; set; }
        public DbSet<InstallenmentDate> InstallenmentDates { get; set; }
    }
}
