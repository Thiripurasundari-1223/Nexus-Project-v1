using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class EmployeeDependent
    {
        [Key]
        public int EmployeeDependentId { get; set; }
        public string EmployeeRelationName { get; set; }
        public int? EmployeeRelationshipId { get; set; }
        public DateTime? EmployeeRelationDateOfBirth { get; set; }
        public int? EmployeeID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
