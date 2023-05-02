using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentProbationStatusRepository : IBaseRepository<AbsentProbationStatus>
    {
        List<AbsentProbationStatus> GetAbsentProbationStatusByAbsentSettingId(int absentSettingId);
        List<int> GetAbsentApplicableProbationStatus(int absentSettingId);
        List<int> GetAbsentExceptionProbationStatus(int absentSettingId);
        List<AbsentProbationStatus> GetApplicableAbsentProbationStatus(int absentSettingId);
        List<AbsentProbationStatus> GetExceptionAbsentProbationStatus(int absentSettingId);
    }
    public class AbsentProbationStatusRepository : BaseRepository<AbsentProbationStatus>, IAbsentProbationStatusRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentProbationStatusRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AbsentProbationStatus> GetAbsentProbationStatusByAbsentSettingId(int absentSettingId)
        {
            return dbContext.AbsentProbationStatus.Where(x => x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<int> GetAbsentApplicableProbationStatus(int absentSettingId)
        {
            return dbContext.AbsentProbationStatus.Where(x => x.AbsentApplicableProbationStatusId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentApplicableProbationStatusId == null ? 0 : (int)x.AbsentApplicableProbationStatusId).ToList();
        }
        public List<int> GetAbsentExceptionProbationStatus(int absentSettingId)
        {
            return dbContext.AbsentProbationStatus.Where(x => x.AbsentExceptionProbationStatusId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentExceptionProbationStatusId == null ? 0 : (int)x.AbsentExceptionProbationStatusId).ToList();
        }
        public List<AbsentProbationStatus> GetApplicableAbsentProbationStatus(int absentSettingId)
        {
            return dbContext.AbsentProbationStatus.Where(x => x.AbsentApplicableProbationStatusId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<AbsentProbationStatus> GetExceptionAbsentProbationStatus(int absentSettingId)
        {
            return dbContext.AbsentProbationStatus.Where(x => x.AbsentExceptionProbationStatusId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
    }
}
