using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class DesignationDetail
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string DesignationShortName { get; set; }
        public string DesignationDescription { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int DesignationCount { get; set; }
    }
}
