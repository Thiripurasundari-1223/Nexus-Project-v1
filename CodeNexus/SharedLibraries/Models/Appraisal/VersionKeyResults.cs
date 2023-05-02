using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Appraisal
{
    public class VersionKeyResults
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
        public int KEY_RESULT_ID { get; set; }
        public decimal? KEY_RESULT_WEIGHTAGE { get; set; }
        public int? BENCHMARK_TYPE { get; set; }
        public int? BENCHMARK_UITYPE { get; set; }
        public int? BENCHMARK_DURATION { get; set; }
        public int? BENCHMARK_OPERATOR { get; set; }
        public decimal? BENCHMARK_VALUE { get; set; }
        public decimal? BENCHMARK_FROM_VALUE { get; set; }
        public decimal? BENCHMARK_TO_VALUE { get; set; }
        public int? CREATED_BY { get; set; }
        public DateTime CREATED_DATE { get; set; }
        public int? UPDATED_BY { get; set; }
        public DateTime UPDATED_DATE { get; set; }
        public bool? IS_DOCUMENT_MANDATORY { get; set; }
    }
}
