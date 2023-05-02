using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibraries.ViewModels.Home
{
   public class HomeTeamAppraisalRatingView
    {
        public decimal? TeamAvgRating { get; set; }
        public int TeamBelowRatingCount { get; set; }
    }
}
