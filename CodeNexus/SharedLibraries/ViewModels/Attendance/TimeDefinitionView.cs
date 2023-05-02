using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class TimeDefinitionView
    {
		public int TimeDefinitionId { get; set; }
		public string TimeFrom { get; set; }
		public string TimeTo { get; set; }
		public string BreakTime { get; set; }
		public string TotalHours { get; set; }
		public string AbsentFromHour { get; set; }
		public string AbsentFromOperator { get; set; }
		public string AbsentToHour { get; set; }
		public string AbsentToOperator { get; set; }
		public string HalfaDayFromHour { get; set; }
		public string HalfaDayFromOperator { get; set; }
		public string HalfaDayToHour { get; set; }
		public string HalfaDayToOperator { get; set; }
		public string PresentHour { get; set; }
		public string PresentOperator { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
		public int ShiftDetailsId { get; set; }
		public bool? IsConsiderAbsent { get; set; }
		public bool? IsConsiderPresent { get; set; }
		public bool? IsConsiderHalfaDay { get; set; }
	}
}
