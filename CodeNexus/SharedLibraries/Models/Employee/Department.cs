using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
   public class Department
    {
		[Key]
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public string DepartmentShortName { get; set; }
		public string DepartmentDescription { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public bool? IsEnableBUAccountable { get; set; }

		public int? ParentDepartmentId { get; set; }

		public int? DepartmentHeadEmployeeId { get; set; }
    }
}
