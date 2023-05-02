using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    [Table("EmployeeCategory")]
    public class EmployeeCategorys
    {
        [Key]
        public int EmployeeCategoryId { get; set; }
        public string EmployeeCategoryName { get; set; }
    }
}
