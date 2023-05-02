using SharedLibraries.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class AppliedLeaveTypeDetails
    {
        public string LeaveType { get; set; }
        public bool IsFullDay { get; set; }
        public bool IsFirstHalf { get; set; }
        public bool IsSecondHalf { get; set; }
        public int? LeaveId { get; set; }
        public int? LeaveTypeId { get; set; }
        public int CompensatoryOffId { get; set; }
        public bool? AppliedLeaveStatus { get; set; }
        public DateTime Date { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LeaveTypeName { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string reason { get; set; }
        public decimal NoOfDays { get; set; }
        public int appliedLeaveDetailId { get; set; }
        public List<AppliedLeaveDetailsView> AppliedLeaveDetails { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Feedback { get; set; }
        public List<SupportingDocuments> ListOfDocuments { get; set; }
        public int? LeaveGrantDetailId { get; set; }
        public bool? IsGrantLeaveRequest { get; set; }
        public int? LevelId { get; set; }
        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string EmployeeFormattedId { get; set; }
        public string ManagerFormattedId { get; set; }

    }
}
