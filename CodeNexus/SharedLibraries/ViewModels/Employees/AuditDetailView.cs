using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class AuditDetailView
    {
        public Guid ChangeRequestId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public List<EmployeeAudit> EmployeeAuditList { get; set; }
    }
}
