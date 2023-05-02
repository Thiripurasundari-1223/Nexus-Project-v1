using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class BenchmarkKRARangeView
    {
        public int Benchmark_Id { get; set; }
        public int Version_Id { get; set; }
        public int Department_Id { get; set; }
        public int Role_Id { get; set; }
        public int Objective_Id { get; set; }
        public int Key_Result_Id { get; set; }
        public decimal? Range_From { get; set; }
        public decimal? Range_To { get; set; }
        public decimal? Benchmark_Value { get; set; }
        public decimal Benchmark_Weightage { get; set; }
        public int? Created_By { get; set; }
        public string Benchmark_Subjective_Name { get; set; }
    }
}
