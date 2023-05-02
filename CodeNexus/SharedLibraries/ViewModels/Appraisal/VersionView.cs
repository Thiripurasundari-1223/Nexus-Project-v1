using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class VersionView
    {
        public int VersionId { get; set; }
        public string VersionName { get; set; }
        public string VersionCode { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
    }
}
