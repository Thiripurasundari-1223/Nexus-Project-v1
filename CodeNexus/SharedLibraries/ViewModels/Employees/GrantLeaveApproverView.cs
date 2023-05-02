using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class GrantLeaveApproverView
    {
        public int? ReportingManagerEmployeeId { get; set; }
        public int? DepartmentHeadEmployeeId { get; set; }
        public int? HRHeadEmployeeId { get; set; }
        public DateTime? RelivingDate { get; set; }
    }
}
