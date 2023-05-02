using SharedLibraries.Common;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.Models.Employee;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class AppraisalMasterData
    {
        public List<Department> Department { get; set; }
        public List<Roles> Roles { get; set; }
        public List<EmployeesTypes> EmployeesTypes { get; set; }
        public List<EntityMaster> EntityMaster { get; set; }
        public List<VersionMaster> VersionMaster { get; set; }
        public List<EmployeeList> ReportingManagerList { get; set; }
        public List<ReportingManagerEmployeeList> ReportingManagerEmployeeList { get; set; }
        public List<KeyWithValue> Durations { get; set; }

    }
}