﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class VersionKeyResultsGroup
    {
        [Key]
        public int KEY_RESULTS_GROUP_ID { get; set; }
        public int VERSION_ID { get; set; }
        public int DEPT_ID { get; set; }
        public int ROLE_ID { get; set; }
        public int OBJECTIVE_ID { get; set; }
        public string KEY_RESULTS_GROUP_NAME { get; set; }
        public int? MANDATORY_KEY_RESULT_OPTIONS { get; set; }
        public decimal? KEY_RESULT_GROUP_WEIGHTAGE { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}
