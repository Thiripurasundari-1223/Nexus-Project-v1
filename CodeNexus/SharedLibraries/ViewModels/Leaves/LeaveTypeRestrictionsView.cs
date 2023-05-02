using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.Leaves
{
    public class LeaveTypeRestrictionsView
    {
        public int LeaveRestrictionsId { get; set; }
        public bool? MaximumLeave { get; set; }
        public decimal? MaximumLeavePerApplication { get; set; }
        public bool? MinimumGap { get; set; }
        public decimal? MinimumGapTwoApplication { get; set; }
        public decimal? MinimumNoOfApplicationsPeriod { get; set; }
        public int? AllowRequestPeriodId { get; set; }
        public List<AppConstantsView> AppConstantsView { get; set; }
        public List<ActiveLeaveList> activeLeaveType { get; set; }
        public List<ActiveHolidayList> activeHoliday { get; set; }
        public int? MaxLeaveAvailedYearId { get; set; }
        public decimal? MaxLeaveAvailedDays { get; set; }
        public List<AppConstantsView> EntitlementAppConstantsView { get; set; }
        public List<AppConstantsView> GrantMaximumPeriodAppConstantsView { get; set; }
        public int? GrantMaximumNoOfDay { get; set; }
        public decimal? GrantMinimumGapTwoApplicationDay { get; set; }
        public decimal? MaximumConsecutiveDays { get; set; }
        public bool? Weekendsbetweenleaveperiod { get; set; }
        public int? GrantRequestFutureDay { get; set; }
        public bool? Holidaybetweenleaveperiod { get; set; }
        public decimal? GrantResetLeaveAfterDays { get; set; }
    }
}
