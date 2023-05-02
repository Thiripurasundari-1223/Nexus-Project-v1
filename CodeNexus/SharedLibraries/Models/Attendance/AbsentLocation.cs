using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Attendance
{
    public class AbsentLocation
    {
		public int AbsentLocationId { get; set; }
		public int? AbsentSettingId { get; set; }
		public int? AbsentApplicableLocationId { get; set; }
		public int? AbsentExceptionLocationId { get; set; }
		public string Type { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
