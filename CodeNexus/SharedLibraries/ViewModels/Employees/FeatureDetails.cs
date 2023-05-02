using System.Collections.Generic;

namespace SharedLibraries.ViewModels.Employees
{
    public class ModuleWiseFeatureDetails
    {
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<FeatureDetails> FeatureDetails { get; set; }
    }
    public class FeatureDetails
    {
        public int? FeatureId { get; set; }
        public string FeatureName { get; set; }
    }
}