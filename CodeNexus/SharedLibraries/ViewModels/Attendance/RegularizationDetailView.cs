using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class RegularizationDetailView
    {
        public List<WeeklyMonthlyAttendanceDetail> RegularizationDetailList { get; set; }
        public int? TotalCount { get; set; }

    }
}
