using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class AttendanceShiftDetailsView
    {
		public int ShiftDetailsId { get; set; }
		public string ShiftName { get; set; }
		public string ShiftCode { get; set; }
		public string TimeFrom { get; set; }
		public string TimeTo { get; set; }
		public string ShiftDescription { get; set; }
		//public int EmployeeGroupId { get; set; }
		public bool OverTime { get; set; }
		public bool IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
		public TimeDefinitionView timeDefinition { get; set; }
		public List<WeekendDefinitionView> weekendDefinition { get; set; }
		public bool IsFlexyShift { get; set; }
	}
}
