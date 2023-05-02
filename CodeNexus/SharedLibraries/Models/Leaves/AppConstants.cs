using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Leaves
{
    public class AppConstants
    {
        [Key]
        public int AppConstantId { get; set; }
        public string AppConstantType { get; set; }
        public string DisplayName { get; set; }
        public string AppConstantValue { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
