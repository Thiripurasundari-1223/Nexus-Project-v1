using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationApproverView
    {
        public int? ReportingManagerEmployeeId { get; set; }
        public int? DepartmentHeadEmployeeId { get; set; }
        public int? HRHeadEmployeeId { get; set; }
        public int? NoticePeriod { get; set; }
        
    }
}
