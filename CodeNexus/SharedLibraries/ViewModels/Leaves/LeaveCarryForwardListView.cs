using System;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveCarryForwardListView
    {
        public int LeaveTypeID { get; set; }
        public int? CarryForwardID { get; set; }
        public string CarryForwardName { get; set; }
        public int? ReimbursementID { get; set; }
        public string ReimbursementName { get; set; }
        public int? ResetYear { get; set; }
        public string Period { get; set; }
        public int? ResetMonth { get; set; }
        public string ResetDay { get; set; }
        public decimal? MaximumCarryForwardDays { get; set; }
        public decimal? MaximumReimbursementDays { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
    }
}