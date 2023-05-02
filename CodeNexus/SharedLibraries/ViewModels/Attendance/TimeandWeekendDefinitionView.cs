using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class TimeandWeekendDefinitionView
    {
        public List<WeekendShiftName> WeekEndNameList { get; set; }
        public ShiftTimeDefinition ShiftTime { get; set; }
    }

    public class ShiftTimeDefinition
    {
        public string TotalHours { get; set; }
        public string PresentHour { get; set; }
        public bool IsGenralShift { get; set; } 
    }

    public class WeekendShiftName
    {
        public int WeekEndID { get; set; }
        public string WeekEndName { get; set; }
        public int? shiftId { get; set; }
    }
}
