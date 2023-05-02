using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentEmployeeRepository : IBaseRepository<AbsentEmployee>
    {
        List<AbsentEmployee> GetAbsentEmployeeByAbsentSettingId(int absentSettingId);
        List<int> GetAbsentApplicableEmployee(int absentSettingId);
        List<int> GetAbsentExceptionEmployee(int absentSettingId);
        List<AbsentEmployee> GetApplicableAbsentEmployee(int absentSettingId);
        List<AbsentEmployee> GetExceptionAbsentEmployee(int absentSettingId);
    }
    public class AbsentEmployeeRepository : BaseRepository<AbsentEmployee>, IAbsentEmployeeRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentEmployeeRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AbsentEmployee> GetAbsentEmployeeByAbsentSettingId(int absentSettingId)
        {
            return dbContext.AbsentEmployee.Where(x => x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<int> GetAbsentApplicableEmployee(int absentSettingId)
        {
            return dbContext.AbsentEmployee.Where(x => x.AbsentApplicableEmployeeId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentApplicableEmployeeId == null ? 0 : (int)x.AbsentApplicableEmployeeId).ToList();
        }
        public List<int> GetAbsentExceptionEmployee(int absentSettingId)
        {
            return dbContext.AbsentEmployee.Where(x => x.AbsentExceptionEmployeeId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentExceptionEmployeeId == null ? 0 : (int)x.AbsentExceptionEmployeeId).ToList();
        }
        public List<AbsentEmployee> GetApplicableAbsentEmployee(int absentSettingId)
        {
            return dbContext.AbsentEmployee.Where(x => x.AbsentApplicableEmployeeId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<AbsentEmployee> GetExceptionAbsentEmployee(int absentSettingId)
        {
            return dbContext.AbsentEmployee.Where(x => x.AbsentExceptionEmployeeId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
    }
}
