using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class EmployeesSkillset
    {
		[Key]
		public int EmployeesSkillsetId { get; set; }
		public int EmployeeId { get; set; }
		public int SkillsetId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
