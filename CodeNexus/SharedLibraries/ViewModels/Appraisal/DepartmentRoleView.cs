using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class DepartmentRoleView
    {
        public int VersionId { get; set; }
        public int DepartmentId { get; set; }
        public int CreatedBy { get; set; }
        public List<RoleIds> RoleIds { get; set; }
    }
    public class RoleIds
    {
        public int RoleId { get; set; }
    }
}
