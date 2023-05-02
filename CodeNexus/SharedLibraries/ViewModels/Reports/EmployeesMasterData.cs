using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Notifications;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Reports
{
    public class EmployeesMasterData
    {
        public List<Models.Employee.Employees> Employees { get; set; }
        public List<EmployeesSkillset> EmployeesSkillset { get; set; }
        public List<Skillsets> Skillsets { get; set; }
        public List<EmployeeList> AllLevelEmployee { get; set; }
        public List<RoleName> RoleNameList { get; set; }
        public List<Designation> ListOfDesignation { get; set; }
        public List<BUAccountableForProject> BUAccountableForProjects { get; set; }
    }
}