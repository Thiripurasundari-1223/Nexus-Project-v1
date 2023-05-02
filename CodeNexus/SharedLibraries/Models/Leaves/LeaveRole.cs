using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveRole
    {
		[Key]
		public int LeaveRoleId { get; set; }
		public int? LeaveTypeId { get; set; }
		public int? LeaveApplicableRoleId { get; set; }
		public int? LeaveExceptionRoleId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
