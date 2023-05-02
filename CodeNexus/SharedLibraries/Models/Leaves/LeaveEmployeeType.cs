using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveEmployeeType
    {
        [Key]
		public int LeaveEmployeeTypeId { get; set; }
		public int? LeaveTypeId { get; set; }
		public int? LeaveApplicableEmployeeTypeId { get; set; }
		public int? LeaveExceptionEmployeeTypeId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
