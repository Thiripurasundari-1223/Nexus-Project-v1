using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class ShiftTimeandWeekendView
    {
        public List<WeekendShiftName> WeekEndNameList { get; set; }
        public string TotalHours { get; set; }
        public string PresentHour { get; set; }
        public bool IsGenralShift { get; set; }
        public int ShiftDetailsId { get; set; }
        public string ShiftName { get; set; }
    }
}
