using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Attendance
{
    public class AttendanceMasterDataView
    {
        public List<Department> DepartmentList { get; set; }
        public List<RoleName> RoleNamesList { get; set; }
        public List<EmployeesTypes> EmployeeTypeList { get; set; }
        public List<EmployeeList> EmployeeList { get; set; }
        public List<EmployeeLocation> LocationList { get; set; }
        public List<Designation> DesignationList { get; set; }
        public List<ProbationStatusView> ProbationStatusList { get; set; }
    }
}
