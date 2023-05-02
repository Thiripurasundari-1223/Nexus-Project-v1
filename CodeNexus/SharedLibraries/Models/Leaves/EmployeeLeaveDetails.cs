using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Leaves
{
   public class EmployeeLeaveDetails
    {
        [Key]
        public int EmployeeLeaveDetailsID { get; set; }
        public int? EmployeeID { get; set; }
        public int? LeaveTypeID { get; set; }
        public decimal? BalanceLeave { get; set; }
        public decimal? AdjustmentBalanceLeave { get; set; }   
        public DateTime? AdjustmentEffectiveFromDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public decimal? AdjustmentDays { get; set; }
    }
}
