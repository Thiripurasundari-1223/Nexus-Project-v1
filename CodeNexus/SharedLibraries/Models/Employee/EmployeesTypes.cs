using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    [Table("EmployeesType")]
    public class EmployeesTypes
    {
        [Key]
        public int EmployeesTypeId { get; set; }
        public string EmployeesType { get; set; }
    }
}
