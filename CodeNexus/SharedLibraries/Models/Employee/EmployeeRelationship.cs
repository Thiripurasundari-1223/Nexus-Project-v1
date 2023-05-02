using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class EmployeeRelationship
    {
        [Key]
        public int EmployeeRelationshipId { get; set; }
        public string EmployeeRelationshipName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
