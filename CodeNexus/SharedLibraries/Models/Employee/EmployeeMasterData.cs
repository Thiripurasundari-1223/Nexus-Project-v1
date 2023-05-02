using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using System.Collections.Generic;

namespace SharedLibraries.Models.Employee
{
    public class EmployeeMasterData
    {
        public List<EmployeesTypes> EmployeeTypeList { get; set; }
        public List<Department> EmployeeDepartmentList { get; set; }
        public List<Skillsets> SkillsetList { get; set; }
        public List<Roles> RolesList { get; set; }
        public List<EmployeeDataForDropDown> ReportingManagerList { get; set; }
        public List<KeyValue> EmployeeWorkPlaceList { get; set; }
        public List<EmployeeLocation> EmployeeLocationList { get; set; }
        public List<Designation> DesignationList { get; set; }
        public List<EmployeeCategory> EmployeeCategoryList { get; set; }
        public List<EmployeeCountry> CountryList { get; set; }
        public List<EmployeeRelationship> EmployeeRelationshipList { get; set; }
        public List<EmployeeShiftMasterData> EmployeeShiftMasterDataList { get; set; }
        public List<ProbationStatus> EmployeeProbationStatus { get; set; }
        public string nextFormattedEmployeeId { get; set; }
        public List<SystemRoles> SystemRolesList { get; set; }
        public List<KeyValue> BloodGroupList { get; set; }
        public List<KeyValue> ExitTypeList { get; set; }
        public List<KeyValue> MaritalStatusList { get; set; }
        public List<KeyValue> QualificationLIst { get; set; }
        public List<KeyValue> SourceOfHireList { get; set; }
        public List<KeyValue> SpecialAbilityList { get; set; }
        public List<KeyValue> Entity { get; set; }
        public List<EmployeeState> StateList { get; set; }
        public List<KeyValue> Board { get; set; }
        public List<KeyValue> ResignationStatus { get; set; }
        public List<EmployeeNationality> NationalityList { get; set; }
        public List<NoticePeriodCategory> NoticePeriodCategyList { get; set; }
    }
    public class EmployeeMasterDataForOrgChart
    {
        public List<EmployeeLocation> EmployeeLocationList { get; set; }
    }
}
