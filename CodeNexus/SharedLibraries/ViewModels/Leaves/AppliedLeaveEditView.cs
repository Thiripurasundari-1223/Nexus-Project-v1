using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class AppliedLeaveEditView
    {
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public List<AppliedLeaveDetailsView> AppliedLeaveDetails { get; set; }
        public List<DocumentDetails> ListOfDocuments { get; set; }
        public int? CreatedBy { get; set; }
    }
}
