using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class ResignationEmployeeFilterView : PaginationViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeNameFilter { get; set; }
        public string ResignationStatus { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsManager { get; set; }
        public bool IsAllReportees { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCheckList { get; set; }
        public List<int> EmployeeIdList { get; set; }
        public List<int> ReporteesList { get; set; }
        public AllResignationInputView ResignationInputFilter { get; set; }
    }
}
