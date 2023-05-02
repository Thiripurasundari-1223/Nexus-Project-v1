using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Leaves
{
    public class LeaveRejectionReason
    {
        [Key]
        public int LeaveRejectionReasonId { get; set; }
        public string LeaveRejectionReasons { get; set; }
    }
}
