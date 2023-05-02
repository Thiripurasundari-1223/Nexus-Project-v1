using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Leaves
{
    public class SpecificEmployeeDetailLeave
    {
        [Key]
        public int SpecificEmployeeDetailLeaveId { get; set; }
        public int LeaveTypeId { get; set; }
        public int? EmployeeDetailLeaveId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
