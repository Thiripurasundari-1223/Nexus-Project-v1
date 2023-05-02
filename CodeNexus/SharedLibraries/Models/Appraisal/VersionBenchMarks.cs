using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class VersionBenchMarks
    {
        [Key]
        public int BENCHMARK_ID { get; set; }
        public int VERSION_ID { get; set; }
        public int DEPT_ID { get; set; }
        public int ROLE_ID { get; set; }
        public int OBJECTIVE_ID { get; set; }
        public int KEY_RESULT_ID { get; set; }
        public decimal? RANGE_FROM { get; set; }
        public decimal? RANGE_TO { get; set; }
        public decimal? BENCHMARK_VALUE { get; set; }
        public decimal BENCHMARK_WEIGHTAGE { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string BENCHMARK_SUBJECTIVE_NAME { get; set; }
    }
}
