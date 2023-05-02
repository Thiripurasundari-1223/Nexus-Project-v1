using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class ShiftDetailedView
    {
        public ShiftViewDetails shiftViewDetails { get; set; }
        public List<ShiftViewDetails> DefaultShiftView { get; set; }

    }
}
