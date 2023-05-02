using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class HolidayViewDetails
    {
        public int HolidayID { get; set; }
        public int? Year { get; set; }
        public string HolidayName { get; set; }
        public string HolidayCode { get; set; }
        public string HolidayDescription { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? HolidayDate { get; set; }
        public bool? IsRestrictHoliday { get; set; }
    }
}
