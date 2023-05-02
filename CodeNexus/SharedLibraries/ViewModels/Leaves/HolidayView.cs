using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class HolidayView
    {
        public int HolidayID { get; set; }
        public int? Year { get; set; }
        public string HolidayName { get; set; }
        public List<int> EmployeeShiftId { get; set; }
        public string EmployeeShiftName { get; set; }
        public List<int> EmployeeDepartmentId { get; set; }
        public string EmployeeDepartmentName { get; set; }
        public List<int> EmployeeLocationId { get; set; }
        public string EmployeeLocationName { get; set; }
        public DateTime? HolidayDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
