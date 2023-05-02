using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Leaves
{
    public class EmployeeGrantLeaveApproval
    {
		[Key]
		public int EmployeeGrantLeaveApprovalId { get; set; }
		public int? LeaveGrantDetailId { get; set; }
		public int? ApproverEmployeeId { get; set; }
		public int? LevelId { get; set; }
		public string Comments { get; set; }
		public string Status { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
