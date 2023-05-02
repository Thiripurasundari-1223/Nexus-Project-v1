using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class SpecificEmployeeDetailLeaveView
    {
        public int LeaveTypeId { get; set; }
        public int? EmployeeDetailLeaveId { get; set; }
        public string EmployeeDetailLeaveText { get; set; }
    }
}
