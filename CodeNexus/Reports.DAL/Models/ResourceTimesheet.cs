using System;
using System.Collections.Generic;
using System.Text;

namespace Reports.DAL.Models
{
    class ResourceTimesheet
    {
        public int Id { get; set; }
        public string ResourceName { get; set; }
        public decimal SubmittedHours { get; set; }
        public decimal NotSubmittedHours { get; set; }
        public decimal PlannedHours { get; set; }
       
    }
}
