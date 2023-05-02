using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class KeyResultDetailView
    {
        public int VersionId { get; set; }
        public int Dept_Id { get; set; }
        public int Role_Id { get; set; }
        public int Objective_Id { get; set; }
        public int KeyResult_Id { get; set; }
        public string KeyResult_Name { get; set; }
        public decimal? Key_Result_Weightage { get; set; }
        public int? Benchmark_Type { get; set; }
        public int? Benchmark_UIType { get; set; }
        public int? Benchmark_Duration { get; set; }
        public int? Benchmark_Operator { get; set; }
        public decimal? Benchmark_Value { get; set; }
        public decimal? Benchmark_From_Value { get; set; }
        public decimal? Benchmark_To_Value { get; set; }
        public int? Updated_By { get; set; }
        public bool? Is_Document_Mandatory { get; set; }
        public List<KRABenchmark> KRABenchmark { get; set; }
    }
    public class KRABenchmark
    {
        public int BenchmarkId { get; set; }
        public decimal? BenchmarkValue { get; set; }
        public decimal? RangeFrom { get; set; }
        public decimal? RangeTo { get; set; }
        public decimal BenchmarkWeightage { get; set; }
        public int? CreatedBy { get; set; }
    }
}
