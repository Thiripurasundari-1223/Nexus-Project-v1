using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class WorkdayFilterView
    {
        public int EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set;}
        public int PageNumber { get; set; }
        public int NOOfRecords { get; set; }
        public int TotalRecords { get; set; }
        public bool isFiltered { get; set; }
        public string dateCondition { get; set; }
        public List<string> statusList { get; set; }
        public bool isExport { get; set; }

    }
}
