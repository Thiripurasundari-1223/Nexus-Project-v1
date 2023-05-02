using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class TimeLogApproveOrRejectView
    {
        public int EmployeeId { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string TotalHours { get; set; }
        public int AttendanceDetailId { get; set; }
        public string IsApproveOrCancel { get; set; }
        public string RejectReason { get; set; }
        public int ModifiedBy { get; set; }
        public string LeaveDate { get; set; }
        public int ApproverManagerId { get; set; }
    }
}
