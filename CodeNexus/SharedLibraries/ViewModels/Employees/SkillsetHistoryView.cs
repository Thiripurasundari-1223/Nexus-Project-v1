using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Employees
{
    public class SkillsetHistoryListView
    {        
        public DateTime? historyDate { get; set; }
        public List<SkillsetHistoryDetails> skillsetHistory { get; set; }
    }
    public class SkillsetHistoryView
    {
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string SkillName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public List<SkillsetHistoryListView> skillsetHistoryList { get; set; }
    }
    public class SkillsetHistoryDetails
    {
       
        public int SkillsetHistoryId { get; set; }
        public int? SkillsetId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}
