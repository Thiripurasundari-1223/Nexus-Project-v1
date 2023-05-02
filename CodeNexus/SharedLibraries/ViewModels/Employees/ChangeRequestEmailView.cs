using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class ChangeRequestEmailView
    {
        public List<EmployeeRequestView> employeeRequests { get; set; }
        public List<EmployeeRequest> ApprovedData { get; set; }
        public EmployeeMasterEmailTemplate employeeMasterEmail { get; set; }
       public EmployeeName EmployeeDetail { get; set; }
        public List<EmployeeName> NotifiedEmployee { get; set; }
        
    }
}
