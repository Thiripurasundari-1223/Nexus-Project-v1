using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveDepartment
    {
		[Key]
		public int LeaveDepartmentId { get; set; }
		public int? LeaveTypeId { get; set; }
		public int? LeaveApplicableDepartmentId { get; set; }
		public int? LeaveExceptionDepartmentId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
