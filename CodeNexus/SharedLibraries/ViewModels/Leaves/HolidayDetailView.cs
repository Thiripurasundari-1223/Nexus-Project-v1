using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Leaves
{
    public class HolidayDetailView
	{ 
		public int HolidayID { get; set; }
		public int? Year { get; set; }
		public string HolidayName { get; set; }
		public string HolidayCode { get; set; }
		public string HolidayDescription { get; set; }
		public List<int> ShiftDetailId { get; set; }
		public List<int> DepartmentId { get; set; }
		public List<int> LocationId { get; set; }
		public bool IsActive { get; set; }
		public DateTime? HolidayDate { get; set; }
		public bool? IsRestrictHoliday { get; set; }

		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
