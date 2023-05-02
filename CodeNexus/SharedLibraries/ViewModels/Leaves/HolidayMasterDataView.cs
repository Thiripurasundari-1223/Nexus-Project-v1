using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class HolidayMasterDataView
    {
        public List<HolidayView> HolidayViews { get; set; }
        public HolidayEmployeeMasterData HolidayEmployeeMasterData { get; set; }

    }
}
