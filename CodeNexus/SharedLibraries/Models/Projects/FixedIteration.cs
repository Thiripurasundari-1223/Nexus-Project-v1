using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class FixedIteration
    {
        [Key]
        public int IterationID { get; set; }
        public int? ProjectID { get; set; }
        public string IterationScope { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set;}

        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
