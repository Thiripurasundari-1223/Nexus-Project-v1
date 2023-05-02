using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class WeekMonthAttendanceDetailedView
    {
        public List<WeeklyMonthlyAttendance> Attendances { get; set; }
        public AbsentRestrictionView Restriction { get; set; }
        public bool IsApplicable { get; set; }
        public EmployeeShiftDetailView ShiftDetail { get; set; }
        public List<ShiftViewDetails> WeekendShiftDetailList { get; set; }
    }
}
