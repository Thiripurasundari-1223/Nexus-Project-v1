using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionBenchmarkKeyResultGroup
    {
        public int VersionId { get; set; }
        public int DeptId { get; set; }
        public int RoleId { get; set; }
        public int ObjectiveId { get; set; }
        public int KeyResultGroupId { get; set; }
        public string KeyResultGroupName { get; set; }
        public decimal? KeyResultGroupWeightage { get; set; }
        public int? MandatoryKeyResultOption { get; set; }
        public int? MandatoryKeyResultOperator { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public List<KeyResultDetailView> KeyResultDetail { get; set; }
    }
}
