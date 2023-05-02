using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionKeyResultsView
    {
        public int VersionId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public int ObjectiveId { get; set; }
        public int CreatedBy { get; set; }
        public decimal ObjectiveWeightage { get; set; }
        public List<KeyResultIds> KeyResultIds { get; set; }
    }
    public class KeyResultIds
    {
        public int KeyResultId { get; set; }
    }
}
