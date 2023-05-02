using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Attendance
{
    public class WeekendDefinitionView
    {
        public int ShiftWeekendDefinitionId { get; set; }
        public int WeekendDayId { get; set; }
        public int ShiftDetailsId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class ShiftViewDetails
    {
        public int ShiftDetailsId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftCode { get; set; }
        public string TotalHours { get; set; }
        public bool? IsConsiderAbsent { get; set; }
        public string AbsentFromHour { get; set; }
        public string AbsentFromOperator { get; set; }
        public string AbsentToHour { get; set; }
        public string AbsentToOperator { get; set; }
        public bool? IsConsiderPresent { get; set; }
        public string PresentHour { get; set; }
        public string PresentHourOperator { get; set; }
        public bool? IsConsiderHalfaDay { get; set; }
        public string HalfaDayFromHour { get; set; }
        public string HalfaDayFromOperator { get; set; }
        public string HalfaDayToHour { get; set; }
        public string HalfaDayToOperator { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public List<WeekendViewDefinition> WeekendList { get; set; }
        public string ShiftTimeFrom { get; set; }
        public string ShiftTimeTo { get; set; }
        public bool? IsGenralShift { get; set; }

    }
    public class WeekendViewDefinition
    {
        public int WeekendDayId { get; set; }
        public string WeekendDayName { get; set; }
        public int? ShiftDetailsId { get; set; }
    }
}
