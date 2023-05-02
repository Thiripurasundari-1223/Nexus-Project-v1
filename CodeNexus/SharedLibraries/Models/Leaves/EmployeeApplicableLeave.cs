using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class EmployeeApplicableLeave
    {
        public int EmployeeApplicableLeaveId { get; set; }

        public int? LeaveTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public int? LeaveExceptionEmployeeId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
