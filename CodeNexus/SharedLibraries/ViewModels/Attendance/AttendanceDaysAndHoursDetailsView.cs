using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
     public class AttendanceDaysAndHoursDetailsView
    {
        public int? PayableDays { get; set; }
        public int? Present { get; set; }
        public int? OnDuty { get; set; }
        public int? PaidLeave { get; set; }
        public int? Holidays { get; set; }
        public int? Weekend { get; set; }
        public int? Absent { get; set; }
        public int? UnpaidLeave { get; set; }
        public string TotalHours { get; set; }
        public string PayableHours { get; set; }
        public string PresentHours { get; set; }
        public string OnDutyHours { get; set; }
        public string PaidLeaveHours { get; set; }
        public string HolidaysHours { get; set; }
        public string WeekendHours { get; set; }
    }
}
