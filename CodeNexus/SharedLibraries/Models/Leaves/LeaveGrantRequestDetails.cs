using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveGrantRequestDetails
    {
        [Key]
        public int LeaveGrantDetailId { get; set; }
        public int LeaveTypeId { get; set; }
        public int EmployeeID { get; set; }
        public decimal? NumberOfDay { get; set; }
        public decimal? BalanceDay { get; set; }
        public string Reason { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsLeaveAdjustment { get; set; }
    }
}
