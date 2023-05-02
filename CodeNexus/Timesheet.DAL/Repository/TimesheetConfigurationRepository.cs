using SharedLibraries.Models.Timesheet;
using SharedLibraries.ViewModels.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Timesheet.DAL.DBContext;

namespace Timesheet.DAL.Repository
{
    public interface ITimesheetConfigurationRepository : IBaseRepository<TimesheetConfigurationDetails>
    {
        TimesheetConfigurationDetails GetByConfigurationID(int TimesheetConfigurationId);
        TimesheetConfigurationView GetConfigurationDetails();
    }
    public class TimesheetConfigurationRepository : BaseRepository<TimesheetConfigurationDetails>, ITimesheetConfigurationRepository
    {
        private readonly TSDBContext _dbContext;
        public TimesheetConfigurationRepository(TSDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public TimesheetConfigurationDetails GetByConfigurationID(int TimesheetConfigurationId)
        {
            return _dbContext.TimesheetConfigurationDetails.Where(x => x.TimesheetConfigurationId == TimesheetConfigurationId).FirstOrDefault();
        }
        public TimesheetConfigurationView GetConfigurationDetails()
        {
            return (from TimesheetConfigurationDetails in _dbContext.TimesheetConfigurationDetails
                    select new TimesheetConfigurationView
                    {
                        TimesheetConfigurationId = TimesheetConfigurationDetails.TimesheetConfigurationId,
                        TimesheetSubmissionDayId = TimesheetConfigurationDetails.TimesheetSubmissionDayId,
                        TimesheetSubmissionTime = ConvertingAsString(TimesheetConfigurationDetails.TimesheetSubmissionTime),
                        TimesheetAlertSubmissionFromDayId = TimesheetConfigurationDetails.TimesheetAlertSubmissionFromDayId,
                        TimesheetAlertSubmissionToDayId = TimesheetConfigurationDetails.TimesheetAlertSubmissionToDayId,
                        TimesheetApprovalFromDayId = TimesheetConfigurationDetails.TimesheetApprovalFromDayId,
                        TimesheetApprovalToDayId = TimesheetConfigurationDetails.TimesheetApprovalToDayId,
                        TimesheetAlertApprovalFromDayId = TimesheetConfigurationDetails.TimesheetAlertApprovalFromDayId,
                        TimesheetAlertApprovalToDayId = TimesheetConfigurationDetails.TimesheetAlertApprovalToDayId,
                    }).FirstOrDefault();
        }
        private static string ConvertingAsString(TimeSpan? timeSpan)
        {
            string returnValue = "00:00";
            if (timeSpan != null)
                returnValue = timeSpan.Value.Hours.ToString().PadLeft(2, '0') + ":" + timeSpan.Value.Minutes.ToString().PadLeft(2, '0') + ":" + timeSpan.Value.Seconds.ToString().PadLeft(2, '0');
            return returnValue;
        }
    }
}