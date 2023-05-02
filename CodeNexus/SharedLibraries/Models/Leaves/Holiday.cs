using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Leaves
{
    public class Holiday
    {
        [Key]
		public int HolidayID { get; set; }
		public int? Year { get; set; }
		public string HolidayName { get; set; }
		public string HolidayCode { get; set; }
		public string HolidayDescription { get; set; }
		public bool IsActive { get; set; }
		public bool? IsRestrictHoliday { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime? HolidayDate { get; set; }
	}
	public class HolidayInput
    {
        public int DepartmentId { get; set; }
        public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public int LocationId { get; set; }
		public int ShiftDetailsId { get; set; }
	}
}