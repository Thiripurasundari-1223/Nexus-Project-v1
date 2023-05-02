using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SharedLibraries.Models.Leaves
{
    public class LeaveCarryForward
    {
        [Key]

        public int LeaveCarryForwardID { get; set; }
        public int EmployeeID { get; set; }
        public int LeaveTypeID { get; set; }
        public decimal BalanceLeave { get; set; }
        public decimal? AdjustmentBalanceLeave { get; set; }
        public DateTime? AdjustmentEffectiveFromDate { get; set; }
        public decimal? CarryForwardLeaves { get; set; }
        public decimal? ReimbursementLeaves { get; set; }
        public DateTime? ResetDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public decimal? AdjustmentDays { get; set; }


    }
}
