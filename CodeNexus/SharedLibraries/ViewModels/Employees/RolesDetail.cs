using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Employees
{
    public class RolesDetail
    {
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RolePermission> RolePermission { get; set; }
    }
    public class RolePermission
    {
        public int? ModuleId { get; set; }
        public int? FeatureId { get; set; }
        public string ModuleName { get; set; }
        public string FeatureName { get; set; }
    }
}
