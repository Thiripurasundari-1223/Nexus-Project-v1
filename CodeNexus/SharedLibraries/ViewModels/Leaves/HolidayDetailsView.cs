using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class HolidayDetailsView
    {
        public List<Holiday> HolidayList { get; set; }
        public List<HolidayLocation> HolidayLocation { get; set; }
        public List<HolidayShift> HolidayShift { get; set; }
        public List<HolidayDepartment> HolidayDepartment { get; set; }
    }
}
