using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.ViewModels.Reports
{
    public class ResourceUtilization
    {
        [Key]
        public int Id { get; set; }
        public int Billable { get; set; }
        public int NonBillable { get; set; }
        public string ProjectName { get; set; }
    }
}
