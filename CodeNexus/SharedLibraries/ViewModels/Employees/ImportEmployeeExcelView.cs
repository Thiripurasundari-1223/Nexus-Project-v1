using SharedLibraries.Common;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class ImportEmployeeExcelView
    {
        public string Base64Format { get; set; }
        public EmployeeMasterData EmployeeMaster { get; set; }
        public List<Models.Employee.Employees> Employees { get; set; }
        public List<ImportDataStatus> ImportDataStatuses { get; set; }
        public List<EmployeeStatusDetail> EmployeeDetailList { get; set; }
        public List<CompensationDetail> CompensationDetails { get; set; }
        public List<WorkHistory> WorkHistories { get; set; }
        public List<EmployeesPersonalInfo> EmployeePersonalInfo { get; set; }
        public List<EducationDetail> EducationDetails { get; set; }
        public List<EmployeesSkillset> SkillsetsEmployee { get; set; }
        public List<EmployeeSpecialAbilityView> SpecialAbility { get; set; }
        public List<EmployeeDependent> EmployeeDependency { get; set; }
        public int UploadedBy { get; set; }
    }

    public class EmployeeStatusDetail
    {
        public int EmployeeId { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmailId { get; set; }
        public string Remarks { get; set; }
        public int id { get; set; }
        public string oldData { get; set; }
    }
}
