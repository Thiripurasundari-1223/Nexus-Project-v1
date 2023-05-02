using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeFilterView
    {
        public string FullName { get; set; }
        public string EmployeeId { get; set; }
        public List<string> Gender { get; set; }
        public List<int?> Department { get; set; }
        public List<int?> Location { get; set; }
        public List<int?> ReportingManager { get; set; }
        public List<int?> EmployeeType { get; set; }
        public List<int?> ProbationStatus { get; set; }
        public List<int?> Designation { get; set; }
        public List<int?> SystemRole { get; set; }
        public List<int?> EmployeeCategory { get; set; }
        public List<int?> Entity { get; set; }
        public List<string> ResignationStatus { get; set; }
        public List<string> ExitType { get; set; }
        public List<bool?> EmployeeStatus { get; set; }
        public List<int?> BloodGroup { get; set; }
        public NumericFilter Age { get; set; }
        public int? Grade { get; set; }
        public DateFilter ContractEndDate { get; set; }
        public DateFilter DateOfJoining { get; set; }
        public Experience TVSNextExperience { get; set; }
        public Experience TotalExperience { get; set; }
        public string Status { get; set; }
    }

    public class NumericFilter
    {
        public int? Value { get; set; }
        public string Condition { get; set; }
    }

    public class DateFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Condition { get; set; }
    }
    public class Experience
    {
        public double FromExperience { get; set; }
        public double ToExperience { get; set; }
        public string Condition { get; set; }
    }
}
