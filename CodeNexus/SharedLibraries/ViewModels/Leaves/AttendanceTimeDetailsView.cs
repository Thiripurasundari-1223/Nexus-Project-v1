using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class AttendanceTimeDetailsView
    {
        public DateTime? AttendanceDate { get; set; }
        public TimeSpan? timeSpans { get; set; }
        public TimeSpan? checkinTime { get; set; }
    }
}
