using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class GrantLeaveApprovalView
    {
		public int? LevelId { get; set; }
		public int? LevelApprovalId { get; set; }
		public int? LevelApprovalEmployeeId { get; set; }
		public string LevelApproverName { get; set; }
		public string Comments { get; set; }
		public string Status { get; set; }

	}
}
