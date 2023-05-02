using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Projects
{
    public class FixedIterationList
    {
        [Key]
        public int IterationID { get; set; }
        public int? ProjectID { get; set; }
        public string IterationScope { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<ResourceAllocationList> ResourceAllocation { get; set; }
    }
}
