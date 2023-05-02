using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class EmployeeLeaveAdjustmentFilterView : PaginationViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? EmployeeId { get; set; }
        public List<int> ResourceId { get; set; }
        public List<int> DepartmentId { get; set; }
        public List<int> DesignationId { get; set; }
        //public decimal? LeaveBalance { get; set; } // As disussed and confirmed
    }
}
