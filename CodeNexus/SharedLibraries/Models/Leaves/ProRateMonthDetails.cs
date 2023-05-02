using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Leaves
{
    public class ProRateMonthDetails
    {
        [Key]
        public int ProRateMonthDetailId { get; set; }
        public int LeaveTypeId { get; set; }
        public string Fromday { get; set; }
        public string Today { get; set; }
        public decimal? Count { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
