using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class ResignedEmployeeView
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string DesignationName { get; set; }
        public int DesignationId { get; set; }
        public string EmployeeEmailId { get; set; }

    }
}
