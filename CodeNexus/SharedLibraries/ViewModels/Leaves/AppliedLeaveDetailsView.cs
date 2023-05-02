using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class AppliedLeaveDetailsView
    {
		public int AppliedLeaveDetailsID { get; set; }
		public DateTime Date { get; set; }
		public bool IsFullDay { get; set; }
		public bool IsFirstHalf { get; set; }
		public bool IsSecondHalf { get; set; }
		public int? LeaveId { get; set; }
		public int CompensatoryOffId { get; set; }
		public int? CreatedBy { get; set; }
		public bool? AppliedLeaveStatus { get; set; }
		public bool? IsLeave { get; set; }
	}
}
