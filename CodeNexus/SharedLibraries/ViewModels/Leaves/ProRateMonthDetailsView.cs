using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class ProRateMonthDetailsView
    {
        public int ProRateMonthDetailId { get; set; }
        public int LeaveTypeId { get; set; }
        public string Fromday { get; set; }
        public string Today { get; set; }
        public decimal? Count { get; set; }

    }
}
