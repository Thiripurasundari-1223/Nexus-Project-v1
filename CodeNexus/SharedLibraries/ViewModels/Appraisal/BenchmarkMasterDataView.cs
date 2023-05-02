using SharedLibraries.Common;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class BenchmarkMasterDataView
    {
        public List<KeyWithValue> Types { get; set; }
        public List<KeyWithValue> UITypes { get; set; }
        public List<KeyWithValue> Durations { get; set; }
        public List<KeyWithValue> Operators { get; set; }
        public List<KeyWithValue> Smiley { get; set; }
        public List<DepartmentList> VersionDepartmentRoleMapping { get; set; }
    }
    
}
