using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionKRAGridView
    {
        public List<VersionKeyResults> VersionKeyResults { get; set; }
        public List<KeyResultMaster> KeyResultMaster { get; set; }
        public List<ObjectiveMaster> ObjectiveMaster { get; set; }
        public List<VersionMaster> VersionMasters { get; set; }
    }
    public class VersionKRABenchmarkGridDetails
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public string VersionCode { get; set; }
        public string VersionDesciption { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public int? KeyResultId { get; set; }
        public string KeyResultName { get; set; }
        public bool IsBenchmarkCompleted { get; set; }
    }
    public class VersionRoleGridDetails
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public string VersionCode { get; set; }
        public string VersionDesciption { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
