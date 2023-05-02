using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Appraisal
{
    public class EmployeeAppraisalRatingView
    {
        public int AppCycleId { get; set; }
        public decimal? FinalRating { get; set; }
        public DateTime? AppraisalStartDate { get; set; }
        public DateTime? AppraisalEndDate { get; set; }

    }
}
