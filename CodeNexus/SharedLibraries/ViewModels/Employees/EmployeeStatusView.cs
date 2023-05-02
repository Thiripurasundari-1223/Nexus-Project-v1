using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeStatusView
    {
        public int EmployeeId { get; set; }
        public bool? IsEnabled { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
