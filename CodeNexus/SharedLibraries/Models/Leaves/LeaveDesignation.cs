using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveDesignation
    {
		[Key]
		public int LeaveDesignationId { get; set; }
		public int? LeaveTypeId { get; set; }
		public int? LeaveApplicableDesignationId { get; set; }
		public int? LeaveExceptionDesignationId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
