using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentRoleRepository : IBaseRepository<AbsentRole>
    {
        List<AbsentRole> GetAbsentRoleByAbsentSettingId(int absentSettingId);
        List<int> GetAbsentApplicableRole(int absentSettingId);
        List<int> GetAbsentExceptionRole(int absentSettingId);
        List<AbsentRole> GetApplicableAbsentRole(int absentSettingId);
        List<AbsentRole> GetExceptionAbsentRole(int absentSettingId);
    }
    public class AbsentRoleRepository : BaseRepository<AbsentRole>, IAbsentRoleRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentRoleRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AbsentRole> GetAbsentRoleByAbsentSettingId(int absentSettingId)
        {
            return dbContext.AbsentRole.Where(x => x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<int> GetAbsentApplicableRole(int absentSettingId)
        {
            return dbContext.AbsentRole.Where(x => x.AbsentApplicableRoleId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentApplicableRoleId == null ? 0 : (int)x.AbsentApplicableRoleId).ToList();
        }
        public List<int> GetAbsentExceptionRole(int absentSettingId)
        {
            return dbContext.AbsentRole.Where(x => x.AbsentExceptionRoleId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentExceptionRoleId == null ? 0 : (int)x.AbsentExceptionRoleId).ToList();
        }
        public List<AbsentRole> GetApplicableAbsentRole(int absentSettingId)
        {
            return dbContext.AbsentRole.Where(x => x.AbsentApplicableRoleId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<AbsentRole> GetExceptionAbsentRole(int absentSettingId)
        {
            return dbContext.AbsentRole.Where(x => x.AbsentExceptionRoleId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
    }
}
