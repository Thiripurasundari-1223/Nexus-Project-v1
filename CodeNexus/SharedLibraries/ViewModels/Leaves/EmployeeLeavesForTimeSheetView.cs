using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
   public class EmployeeLeavesForTimeSheetView
    {
		public int? LeaveID { get; set; }
		public int AppliedLeaveDetailsID { get; set; }
		public DateTime Date { get; set; }
		public bool IsFullDay { get; set; }
		public bool IsFirstHalf { get; set; }
		public bool IsSecondHalf { get; set; }		
		public bool? AppliedLeaveStatus { get; set; }
		public bool? IsAllowTimesheet { get; set; }

	}
}
