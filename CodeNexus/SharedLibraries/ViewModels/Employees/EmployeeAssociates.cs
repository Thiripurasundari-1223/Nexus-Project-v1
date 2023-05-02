using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeAssociates
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public int? LocationId { get; set; }
        public string Location { get; set; }
        public Boolean? IsSpecialAbility { get; set; }
        public string FormattedEmployeeId { get; set; }
    }
    public class TotalEmployees
    {
        public int TotalEmployee { get; set; }
        public int? TotalMale { get; set; }
        public int? TotalFemale { get; set; }
        public string Department { get; set; }
        public int? DeptMaleCount { get; set; }
        public int? DeptFemaleCount { get; set; }
    }
    public class LocationwiseEmployee
    {
        public string Location { get; set; }
        public int? Count { get; set; }
        public double? Percentage { get; set; }
    }
    public class SpecialAbility
    {
        public string Type { get; set; }
        public int? Count { get; set; }
        public double? Percentage { get; set; }
    }
    public class InnovationHub
    {
        public string Type { get; set; }
        public int? Count { get; set; }
        public double? Percentage { get; set; }
    }
    public class EmployeeAssociateReport
    {
       public List<TotalEmployees> TotalEmployees { get; set; }
       public List<LocationwiseEmployee> LocationwiseEmployee { get; set; }
       public List<SpecialAbility> SpecialAbility { get; set; }
       public List<InnovationHub> EmployeeInnovationHub { get; set; }
    }
}
