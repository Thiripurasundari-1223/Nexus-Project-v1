using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentLocationRepository : IBaseRepository<AbsentLocation>
    {
        List<AbsentLocation> GetAbsentLocationByAbsentSettingId(int absentSettingId);
        List<int> GetAbsentApplicableLocation(int absentSettingId);
        List<int> GetAbsentExceptionLocation(int absentSettingId);
        List<AbsentLocation> GetApplicableAbsentLocation(int absentSettingId);
        List<AbsentLocation> GetExceptionAbsentLocation(int absentSettingId);
    }
    public class AbsentLocationRepository : BaseRepository<AbsentLocation>, IAbsentLocationRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentLocationRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AbsentLocation> GetAbsentLocationByAbsentSettingId(int absentSettingId)
        {
            return dbContext.AbsentLocation.Where(x => x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<int> GetAbsentApplicableLocation(int absentSettingId)
        {
            return dbContext.AbsentLocation.Where(x => x.AbsentApplicableLocationId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentApplicableLocationId == null ? 0 : (int)x.AbsentApplicableLocationId).ToList();
        }
        public List<int> GetAbsentExceptionLocation(int absentSettingId)
        {
            return dbContext.AbsentLocation.Where(x => x.AbsentExceptionLocationId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentExceptionLocationId == null ? 0 : (int)x.AbsentExceptionLocationId).ToList();
        }
        public List<AbsentLocation> GetApplicableAbsentLocation(int absentSettingId)
        {
            return dbContext.AbsentLocation.Where(x => x.AbsentApplicableLocationId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<AbsentLocation> GetExceptionAbsentLocation(int absentSettingId)
        {
            return dbContext.AbsentLocation.Where(x => x.AbsentExceptionLocationId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
    }
}
