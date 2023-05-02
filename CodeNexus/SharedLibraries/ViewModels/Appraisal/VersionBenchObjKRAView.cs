using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionBenchObjKRAView
    {
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public decimal ObjectiveWeightage { get; set; }
        public string ObjectiveDescription { get; set; }
        public List<KeyResultDetailView> KRAsViews { get; set; }
        public List<VersionBenchmarkKeyResultGroup> VersionBenchmarkKeyResultGroup { get; set; }
    }    
}
