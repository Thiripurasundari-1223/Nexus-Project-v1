using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibraries.Models.Leaves
{
	[Table("LeaveMaxLimitAction")]
	public class LeaveMaxLimitAction
	{
		[Key]
		public int LeaveMaxLimitActionId { get; set; }
		public string LeaveMaxLimitActionName { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
