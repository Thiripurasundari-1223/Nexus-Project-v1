namespace SharedLibraries.ViewModels.Employees
{
    public class RolePermissionView
    {
        public int RolePermissionId { get; set; }
        public int? ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int? FeatureId { get; set; }
        public string FeatureName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public bool? IsEnabled { get; set; }
    }
}