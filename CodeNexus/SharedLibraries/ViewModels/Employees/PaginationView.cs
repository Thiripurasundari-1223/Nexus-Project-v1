using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class PaginationView
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int NoOfRecord { get; set; }
        public EmployeeFilterView  EmployeeFilter { get; set; }
        public Boolean IsFilterApplied { get; set; }
    }

    public class PaginationViewModel
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int NoOfRecord { get; set; } = 20;
        public Boolean IsFilterApplied { get; set; }
    }
}
