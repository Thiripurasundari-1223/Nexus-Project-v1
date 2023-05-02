namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeDetails
    { 
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? DepartmentId { get; set; }
        public string Department { get; set; }
        public int? RoleId { get; set; }
        public string Role { get; set; }
        public int? SkillsetId { get; set; }
        public string Skillset { get; set; }
        public string FormattedEmployeeId { get; set; }
        public string EmployeeEmailId { get; set; }
    }
}