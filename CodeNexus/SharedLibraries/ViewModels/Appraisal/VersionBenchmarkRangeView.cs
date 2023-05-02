using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionBenchmarkRangeView
    {
        public int VersionId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public int ObjectiveId { get; set; }
        public int KeyResultId { get; set; }
        public List<KRABenchmark> BenchmarkRange { get; set; }
    }
}
