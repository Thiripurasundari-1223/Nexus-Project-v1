using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class AllResignationInputView
    {
        public List<int> ReporteesList { get; set; }
        public int EmployeeId { get; set; }
        public bool IsMyData { get; set; }
        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAllData { get; set; }
        public List<string> EmployeeRole { get; set; }
        public string PMORole { get; set; }
        public string ITRole { get; set; }
        public string AdminRole { get; set; }
        public string FinanceRole { get; set; }
        public string HRRole { get; set; }
    }
}
