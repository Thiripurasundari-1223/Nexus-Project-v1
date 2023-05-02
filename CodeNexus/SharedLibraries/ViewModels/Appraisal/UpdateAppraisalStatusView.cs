using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class UpdateAppraisalStatusView
    {
        public int AppCycleId { get; set; }
        public int AppraisalStatus { get; set; }
        public List<Models.Employee.Employees> EmployeeDetails { get; set; }
        public int UpdatedBy { get; set; }
    }
}
