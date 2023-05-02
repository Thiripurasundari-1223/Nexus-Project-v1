using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    [Table("Reports")]
    public class Reports
    {
        [Key]
        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public string ReportTitle { get; set; }
        public string ReportIconPath { get; set; }
        public string ReportNavigationUrl { get; set; }
    }
}
