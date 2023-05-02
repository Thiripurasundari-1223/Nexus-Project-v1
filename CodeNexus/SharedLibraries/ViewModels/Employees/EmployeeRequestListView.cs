using System.Collections.Generic;
namespace SharedLibraries.ViewModels.Employee
{
    public class EmployeeRequestListView
    {
        public string RequestCategory { get; set; }
        public List<EmployeeRequestView> EmployeeRequestlst { get; set; }
    }
}
