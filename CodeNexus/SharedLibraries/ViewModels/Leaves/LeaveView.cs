using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveView
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string LeaveCode { get; set; }
        public int? EmployeesTypeId { get; set; }
        public string EmployeesType { get; set; }
        public bool? ProRate { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}
