using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class EmployeeCategory
    {
        [Key]
        public int EmployeeCategoryId { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeCategoryName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}