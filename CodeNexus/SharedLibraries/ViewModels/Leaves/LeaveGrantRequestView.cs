using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{

    public class LeaveGrantRequestView
    {
        public LeaveGrantRequestDetailsView leaveGrantRequestDetails { get; set; }
        public List<LeaveGrantDocumentUpload> LeaveGrantDocumentList { get; set; }
    }
    public class LeaveGrantRequestAndDocumentView
    {
        public LeaveGrantRequestDetailsView leaveGrantRequestDetail { get; set; }
        public List<LeaveGrantDocument> leaveGrantDocument { get; set; }

        public GrantLeaveApproverView GrantLeaveApprover { get; set; }
    }
   
    public class LeaveGrantRequestDetailsView
    {
        public int LeaveGrantDetailId { get; set; }
        public int LeaveTypeId { get; set; }
        public int EmployeeID { get; set; }
        public decimal? NumberOfDay { get; set; }
        public string Reason { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public string LeaveName { get; set; }
        public string Status { get; set; }
    }
    public class LeaveGrantDocument
    {
        public int LeaveGrantDocumentDetailId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
        public bool? IsActive { get; set; }
    }
}
