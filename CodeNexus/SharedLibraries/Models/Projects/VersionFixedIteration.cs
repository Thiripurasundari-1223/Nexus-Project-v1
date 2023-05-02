﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Projects
{
    public class VersionFixedIteration
    {
        [Key]
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public int? IterationID { get; set; }
       
        //public int? ProjectID { get; set; }
        public string IterationScope { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
