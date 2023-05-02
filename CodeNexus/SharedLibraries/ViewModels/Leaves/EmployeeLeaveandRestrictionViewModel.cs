using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class EmployeeLeaveandRestrictionViewModel : PaginationViewModel
    {
        public int EmployeeId { get; set; }
        public int LeaveId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DepartmentId { get; set; }
        public int ShiftId { get; set; }
        public int LocationId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public PaginationView Pagination { get; set; }
        public FilterData FilterData { get; set; }
        public bool? isFiltered {get;set;}
        public int? managerId { get;set; }
        public bool? isManager { get; set;}
        public bool isExport { get; set; }
    }

    public class FilterData
    {
        public List<string> statusList { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string DateCondition { get;set; }
        public int? NoOfDays { get; set; }
        public string NoOfDaysCondition { get;set; }
        public List<int> LeaveTypeIdList { get; set; }

    }
}
