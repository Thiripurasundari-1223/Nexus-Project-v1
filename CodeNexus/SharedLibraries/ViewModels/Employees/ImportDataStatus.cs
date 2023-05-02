using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class ImportDataStatus
    {
        public string FormattedEmployeeId{get;set;}
        public string EmployeeName { get; set; }
        public string EmployeeEmailId { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

    }
}
