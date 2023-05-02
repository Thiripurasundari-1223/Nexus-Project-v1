using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionKRAMasterdata
    {
        public List<DepartmentList> VersionDepartmentRoleMapping { get; set; }
        public List<KeyResultMaster> KeyResultMasters { get; set; }
        public List<ObjectiveMaster> ObjectiveMaster { get; set; }
        public List<VersionKeyResults> VersionKeyResults { get; set; }
    }
    public class VersionKRAData
    {
        public List<DepartmentList> Departments { get; set; }     
        public List<ObjectiveData> Objectives { get; set; }
        public List<KRADetails> KRAs { get; set; }
    }
    public class VersionRoleData
    {
        public List<DepartmentList> Departments { get; set; }
        public List<RolesList> Roles { get; set; }
    }
    public class DepartmentList
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<RolesList> Roles { get; set; }
    }
    public class RolesList
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class KRADetails
    {
        public int KRAId { get; set; }
        public string KRAName { get; set; }
    }
    public class ObjectiveKRA
    {
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
        public decimal ObjectiveWeightage { get; set; }
        public List<KRADetails> kRADetails { get; set; }
    }
    public class ObjectiveData
    {
        public int ObjectiveId { get; set; }
        public string ObjectiveName { get; set; }
    }
    
    
}
