using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class EmployeeLeaveAdjustment
    {
        public DateTime? AdjustmentEffectiveFromDate { get; set; }
        public int? LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public decimal? LeaveBalance { get; set; }
        public int? EmployeeId { get; set; }
        public decimal? AdjustmentLeaveBalance { get; set; }
        public decimal? ActualBalanceLeave { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public int? LeaveAccruedType { get; set; }
        public string LeaveAccruedTypeName { get; set; }
        public string LeaveAccruedDay { get; set; }
        public decimal? LeaveAccruedNoOfDays { get; set; }
        public int? CarryForwardId { get; set; }
        public string CarryForwardIdName { get; set; }
        public decimal? MaximumCarryForwardDays { get; set; }
        public int? ReimbursementId { get; set; }
        public string ReimbursementIdName { get; set; }
        public decimal? MaximumReimbursementDays { get; set; }
        public int? ResetYear { get; set; }
        public string ResetYearName { get; set; }
        public int? ResetMonth { get; set; }
        public string ResetDay { get; set; }
        public List<AppliedLeaveTypeDetails> AppliedLeaveDates { get; set; }
        public List<LeaveAdjustmentDetails> LeaveAdjustmentDetails { get; set; }
        public List<LeaveCarryForward> LeaveCarryForward { get; set; }
        public decimal? AdjustmentDays { get; set; }
        public DateTime? LeaveResetOn { get; set; }
        public List<LeaveGrantRequestDetails> LeaveGrantRequestDetails { get; set; }
        public BalanceBasedOnDetails BalanceBasedOn { get; set; }
    }
}
