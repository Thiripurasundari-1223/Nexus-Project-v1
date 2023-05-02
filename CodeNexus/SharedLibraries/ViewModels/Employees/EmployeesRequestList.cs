using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraries.Models.Employee;
namespace SharedLibraries.ViewModels.Employee
{
    public class EmployeesRequestList
    {
        public string EmployeeName { get; set; }
        public string EmployeeType { get; set; }
        public int EmployeeId { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string ReportingTo { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public bool? IsActive { get; set; }
        public string ProfilePicture { get; set; }        
        public List<EmployeeRequestListView> EmployeeRequestList { get; set; }
    }
}
