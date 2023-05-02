using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveAdjustmentDetails
    {
        [Key]
        public int LeaveAdjustmentDetailsId { get; set; }
        public int? EmployeeId { get; set; }
        public int? LeavetypeId { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public decimal? PreviousBalance { get; set; }
        public decimal? AdjustmentBalance { get; set; }
        public decimal? NoOfDays { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
