using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeCompensationCompareView
    {
        public int EmployeeId { get; set; }
        public bool IsCompareMode { get; set; }
        public int Year { get; set; }
    }
}
