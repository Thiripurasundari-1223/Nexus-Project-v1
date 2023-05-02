using SharedLibraries.Models.Employee;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.PolicyManagement
{
    public class PolicyMgmtMasterData
    {
        public List<Department> Departments { get; set; }
        public List<EmployeeLocation> Locations { get; set; }
        public List<FolderView> FolderView { get; set; }
        public List<Roles> Roles { get; set; }
    }
}