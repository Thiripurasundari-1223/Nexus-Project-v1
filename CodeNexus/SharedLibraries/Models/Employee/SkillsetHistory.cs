using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
    public class SkillsetHistory
    {
        [key]
        public int SkillsetHistoryId { get; set; }
        public int? SkillsetId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Category { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
