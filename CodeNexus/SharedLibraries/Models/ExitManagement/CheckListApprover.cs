using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.ExitManagement
{
    public class CheckListApprover
    {
    public int CheckListApproverId{ get; set; } 
    public int? LevelId{ get; set; } 
	public string ApprovedRole{ get; set; } 
	public int? CreatedBy{ get; set; } 
	public DateTime? CreatedOn{ get; set; } 
    }
}
