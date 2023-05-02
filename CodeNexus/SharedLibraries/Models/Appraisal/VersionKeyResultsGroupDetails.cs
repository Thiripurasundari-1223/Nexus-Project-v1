using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class VersionKeyResultsGroupDetails
    {
        [Key]
        public int VERSION_ID { get; set; }
        [Key]
        public int DEPT_ID { get; set; }
        [Key]
        public int ROLE_ID { get; set; }
        [Key]
        public int OBJECTIVE_ID { get; set; }
        [Key]
        public int KEY_RESULTS_GROUP_ID { get; set; }
        [Key]
        public int KEY_RESULT_ID { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
    }
}
