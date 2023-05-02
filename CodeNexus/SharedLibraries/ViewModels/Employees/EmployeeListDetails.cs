using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class EmployeeListDetails
    {
        public List<EmployeeDetailListView> EmployeeDetailLists { get; set; }
        public int TotalRecord { get; set; }
    }
    public class EmployeeListDetailsForOrgChart 
    {
        public List<EmployeeDetailListViewForOrgChart> EmployeeDetailLists { get; set; }
        public int TotalRecord { get; set; }
       
    }
    public class EmployeeDetailListViewForOrgChart : EmployeeDetailListView
    {
        public int ReporteeCount { get; set; }
    }
}
