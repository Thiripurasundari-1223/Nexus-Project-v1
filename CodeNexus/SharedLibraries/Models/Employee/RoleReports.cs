using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    [Table("RoleReports")]
    public class RoleReports
    {
        [Key]
        public int RoleReportID { get; set; }
        public int RoleID { get; set; }
        public int ReportID { get; set; }
        
    }
}
