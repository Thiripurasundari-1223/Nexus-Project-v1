using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Projects
{
    public class ResourceAvailability
    {
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public int? AllocationPercent { get; set; }
        public int? Availability { get; set; }
        public int? SkillsetId { get; set; }

    }
    public class AvailabilityPercent
    {
        public double? Available { get; set; }
        public double? UnAvailable { get; set; }
        public double? InnovationHub { get; set; }
    }
    public class EmployeeAvailabilityGridView
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string SkillSet { get; set; }
        public double? UtilizedPercent { get; set; }
        public double? AvailablePercent { get; set; }
    }
    public class EmployeeDepartmentAvailability
    {
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public double? Available { get; set; }
        public double? UnAvailable { get; set; }
        public double? InnovationHub { get; set; }
    }
    public class EmployeeAvailability
    {
        public string Type { get; set; }
        public double? Count { get; set; }
    }
    public class ResourceAvailabilityReport
    {
        public List<EmployeeAvailability> EmployeeAvailability { get; set; }
        public List<EmployeeDepartmentAvailability> EmployeeDepartmentAvailability { get; set; }
        public List<EmployeeAvailabilityGridView> EmployeeAvailabilityGridView { get; set; }
    }
}
