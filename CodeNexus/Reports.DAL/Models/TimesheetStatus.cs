using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.DAL.Models
{
    class TimesheetStatus
    {
        
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int Submitted { get; set; }
        public int NotSubmitted { get; set; }
    }
}
