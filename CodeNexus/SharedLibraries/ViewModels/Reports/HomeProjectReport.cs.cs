using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
   public class HomeProjectReport
    {
        public List<ReportData> ReportData { get; set; }
        public List<ProjectReport> ReportDetail { get; set; }
        public int TotalProject { get; set; }
        public int TotalCustomerProject { get; set; }
        public int TotalInternalProject { get; set; }
    }
}
