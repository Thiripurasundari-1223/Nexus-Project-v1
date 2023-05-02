using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Attendance
{
    public class AbsentRestrictions
    {
		[Key]
		public int AbsentRestrictionId { get; set; }
		public int? AbsentSettingId { get; set; }
		public bool? WeekendsBetweenAttendacePeriod { get; set; }
		public bool? HolidaysBetweenAttendancePeriod { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
