using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        public Company Company { get; set; }
    }
}
