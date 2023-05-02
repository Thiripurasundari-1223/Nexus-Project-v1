using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentRestrictionRepository : IBaseRepository<AbsentRestrictions>
    {
        AbsentRestrictions GetAbsentRestrictionsById(int absentSettingId);
        AbsentRestrictionView GetAbsentRestriction();
    }
    public class AbsentRestrictionRepository : BaseRepository<AbsentRestrictions>, IAbsentRestrictionRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentRestrictionRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public AbsentRestrictions GetAbsentRestrictionsById(int absentSettingId)
        {
            return dbContext.AbsentRestrictions.Where(x => x.AbsentSettingId == absentSettingId).FirstOrDefault();
        }
        public AbsentRestrictionView GetAbsentRestriction()
        {
            return (from restrictions in dbContext.AbsentRestrictions
                    select new AbsentRestrictionView
                    {
                        AbsentSettingId = restrictions.AbsentSettingId,
                        WeekendsBetweenAttendacePeriod = restrictions.WeekendsBetweenAttendacePeriod,
                        HolidaysBetweenAttendancePeriod = restrictions.HolidaysBetweenAttendancePeriod,
                    }).FirstOrDefault();
        }
    }
}
