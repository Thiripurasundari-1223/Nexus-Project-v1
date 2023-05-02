using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class UpcomingHoliday
    {
		public int HolidayID { get; set; }
		public int? Year { get; set; }
		public string HolidayName { get; set; }
		public string HolidayDescription { get; set; }
		public DateTime? HolidayDate { get; set; }
	}
}
