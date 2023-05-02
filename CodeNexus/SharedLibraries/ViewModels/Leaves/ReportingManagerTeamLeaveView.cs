using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class ReportingManagerTeamLeaveView : PaginationViewModel
    {
        public List<int> ResourceId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? ManagerEmployeeId { get; set; }
        public int? ManagerId { get; set; }
        public int? LeaveTypeId { get; set; }
        public decimal? NoOfDays { get; set; }
        public string LeaveStatus { get; set; }
        public bool isRegularization { get; set; }
    }
}
