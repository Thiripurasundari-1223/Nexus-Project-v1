using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    public class EmployeeAudit
    {
        [key]
        public int EmployeeAuditId { get; set; }
        public int? EmployeeId { get; set; }
        public Guid ChangeRequestID { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ActionType { get; set; }
        public string Field { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
