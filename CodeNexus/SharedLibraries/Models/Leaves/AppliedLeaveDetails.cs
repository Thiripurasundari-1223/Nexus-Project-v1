using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class AppliedLeaveDetails
    {
		[Key]
		public int AppliedLeaveDetailsID { get; set; }
		public DateTime Date { get; set; }
		public bool IsFullDay { get; set; }
		public bool IsFirstHalf { get; set; }
		public bool IsSecondHalf { get; set; }
		public int? LeaveId { get; set; }
		public int CompensatoryOffId { get; set; }
		public bool? AppliedLeaveStatus { get; set; }
		public DateTime CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
    }
}
