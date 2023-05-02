using System;
using System.Collections.Generic;
namespace SharedLibraries.ViewModels.Attendance
{
    public class ShiftView
    {
		public int ShiftDetailsId { get; set; }
		public string ShiftName { get; set; }
		public string ShiftCode { get; set; }
		public string TimeFrom { get; set; }
		public string TimeTo { get; set; }
        public string Status { get; set; }
        //public int EmployeeGroupId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<int> WeekendId { get; set; }
        public string TotalHours { get; set; }
        public int WeekEndDays { get; set; }
    }
}