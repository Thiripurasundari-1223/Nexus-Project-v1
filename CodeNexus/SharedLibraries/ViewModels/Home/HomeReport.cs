using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Home
{
   public class HomeReport
    {
        public string ReportIconPath { get; set; }
        public string NavigateTo { get; set; }
        public string ReportTitle { get; set; }
        public string ReportSubTitle { get; set; }
        public List<HomeReportData> HomeReportData { get; set; }
    }
    
}
