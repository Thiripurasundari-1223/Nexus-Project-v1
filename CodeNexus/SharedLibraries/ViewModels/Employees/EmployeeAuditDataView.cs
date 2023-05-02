using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeAuditDataView
    {
        public Models.Employee.Employees Employee { get; set; }
        public EmployeesPersonalInfo EmployeesPersonalInfoDetail { get; set; }
        public List<WorkHistory> WorkHistory { get; set; }
        public List<EducationDetail> EducationDetail { get; set; }
        public List<CompensationDetail> CompensationDetail { get; set; }
        public List<EmployeesSkillset> EmployeesSkillsets { get; set; }
        public List<EmployeeDependent> EmployeeDependents { get; set; }
        public List<EmployeeShiftDetails> EmployeeShiftDetails { get; set; }
        public List<EmployeeSpecialAbility> EmployeeSpecialAbility { get; set; }
        public int CreatedBy { get; set; }
        public string ActionType { get; set; }

    }
}
