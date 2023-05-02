using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class AuditView
    {
        public int? AuditId { get; set; }
        public int? EmployeeId { get; set; }
        public Guid CRID { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ActionType { get; set; }
        public string Field { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
