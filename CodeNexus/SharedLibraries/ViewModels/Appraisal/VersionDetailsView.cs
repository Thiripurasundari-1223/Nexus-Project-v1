using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionDetailsView
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public string VersionCode { get; set; }
        public string VersionDescription { get; set; }
        public List<VersionRoleView> VersionRoleMapping { get; set; }        
    }
    public class VersionRoleView
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<RolesList> Roles { get; set; }
    }
    public class CopyVersion
    {
        public int versionId { get; set; }
        public int createdBy { get; set; }
    }
    

}
