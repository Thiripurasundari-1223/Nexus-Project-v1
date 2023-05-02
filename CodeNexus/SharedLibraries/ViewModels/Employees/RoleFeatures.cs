using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.ViewModels.Employees
{
    public class RoleFeatures
    {
        [Key]
        public int RoleFeatureId { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }

    public class RoleFeatureList
    {
        [Key]
        public int RoleFeatureId { get; set; }
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool? IsMenu { get; set; }
        public string NavigationURL { get; set; }
        public string FeatureNavigationURL { get; set; }
        public string ModuleIcon { get; set; }
    }
}
