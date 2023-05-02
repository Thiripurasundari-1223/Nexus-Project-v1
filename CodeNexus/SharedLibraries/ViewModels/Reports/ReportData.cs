using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ReportData
    {
        [Key]
        public int Id { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
        public int ProjectId { get; set; }
    }
}
