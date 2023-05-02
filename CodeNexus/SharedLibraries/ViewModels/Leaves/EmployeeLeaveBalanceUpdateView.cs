using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class EmployeeLeaveBalanceUpdateView
    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public decimal? ActualLeaveBalance { get; set; }
        public decimal? AdjustmentLeaveBalance { get; set; }
        public DateTime? AdjustmentEffectiveFromDate { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
