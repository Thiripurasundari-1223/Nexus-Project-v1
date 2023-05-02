using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class WorkDayStatusView
    {
        public int WorkDayDetailId { get; set; }
        //public int EmployeeId { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
        public bool? IsCanceled { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; }
        public string ApproverRemark { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
