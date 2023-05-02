using Notification.DAL.DBContext;
using Notifications.DAL.Repository;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace Notifications.DAL
{
    public interface ITimesheetConfigurationWeekdayRepository : IBaseRepository<TimesheetConfigurationWeekDay>
    {
        TimesheetConfigurationWeekdayView GetWeekDayByName(string pStatusName);
        TimesheetConfigurationWeekdayView GetWeekDayByID(int pStatusId);
        List<TimesheetConfigurationWeekdayView> GetAllWeekDayList();
    }
    public class TimesheetConfigurationWeekdayRepository : BaseRepository<TimesheetConfigurationWeekDay>, ITimesheetConfigurationWeekdayRepository
    {
        private readonly NotificationsDBContext dbContext;
        public TimesheetConfigurationWeekdayRepository(NotificationsDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public TimesheetConfigurationWeekdayView GetWeekDayByName(string pDayName)
        {
            return (from a in dbContext.TimesheetConfigurationWeekDay.Where(x => x.DayName == pDayName)
                    select new TimesheetConfigurationWeekdayView
                    {
                        TimesheetConfigurationWeekdayId = a.TimesheetConfigurationWeekdayId,
                        DayName = a.DayName
                    }).FirstOrDefault();
        }
        public TimesheetConfigurationWeekdayView GetWeekDayByID(int pTimesheetConfigurationWeekdayId)
        {
            return (from a in dbContext.TimesheetConfigurationWeekDay.Where(x => x.TimesheetConfigurationWeekdayId == pTimesheetConfigurationWeekdayId)
                    select new TimesheetConfigurationWeekdayView
                    {
                        DayName = a.DayName
                    }).FirstOrDefault();
        }
        public List<TimesheetConfigurationWeekdayView> GetAllWeekDayList()
        {
            return (from a in dbContext.TimesheetConfigurationWeekDay
                    select new TimesheetConfigurationWeekdayView
                    {
                        TimesheetConfigurationWeekdayId = a.TimesheetConfigurationWeekdayId,
                        DayName = a.DayName
                    }).ToList();
        }
    }
}
