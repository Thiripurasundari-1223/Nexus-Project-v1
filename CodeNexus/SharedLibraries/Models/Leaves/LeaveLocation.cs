using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveLocation
    {
        [Key]
        public int LeaveLocationId { get; set; }
        public int? LeaveTypeId { get; set; }
        public int? LeaveApplicableLocationId { get; set; }
        public int? LeaveExceptionLocationId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
