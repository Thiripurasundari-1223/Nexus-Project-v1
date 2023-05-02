using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Attendance
{
    public class AbsentEmployee
    {
		public int AbsentEmployeeId { get; set; }
		public int? AbsentSettingId { get; set; }
		public int? AbsentApplicableEmployeeId { get; set; }
		public int? AbsentExceptionEmployeeId { get; set; }
		public string Type { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
