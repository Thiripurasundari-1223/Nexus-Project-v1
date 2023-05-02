using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    [Table("Leaves")]
    public class ApplyLeaves
    {
        [Key]
        public int LeaveId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal NoOfDays { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Feedback { get; set; }
        public int? LeaveRejectionReasonId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public int? ManagerId { get; set; }
        public DateTime? ApproveRejectOn { get; set; }
        public int? ApproveRejectBy { get; set; }
        public string ApproveRejectName { get; set; }
    }
}
