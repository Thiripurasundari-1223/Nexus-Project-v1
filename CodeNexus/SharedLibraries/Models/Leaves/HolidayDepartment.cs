using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class HolidayDepartment
    {
		[Key]
		public int HolidayDepartmentId { get; set; }
		public int? HolidayId { get; set; }
		public int? DepartmentId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
