using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentEmployeeTypeRepository : IBaseRepository<AbsentEmployeeType>
    {
        List<AbsentEmployeeType> GetAbsentEmployeeTypeByAbsentSettingId(int absentSettingId);
        List<int> GetAbsentApplicableEmployeeType(int absentSettingId);
        List<int> GetAbsentExceptionEmployeeType(int absentSettingId);
        List<AbsentEmployeeType> GetApplicableAbsentEmployeeType(int absentSettingId);
        List<AbsentEmployeeType> GetExceptionAbsentEmployeeType(int absentSettingId);
    }
    public class AbsentEmployeeTypeRepository : BaseRepository<AbsentEmployeeType>, IAbsentEmployeeTypeRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentEmployeeTypeRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AbsentEmployeeType> GetAbsentEmployeeTypeByAbsentSettingId(int absentSettingId)
        {
            return dbContext.AbsentEmployeeType.Where(x => x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<int> GetAbsentApplicableEmployeeType(int absentSettingId)
        {
            return dbContext.AbsentEmployeeType.Where(x => x.AbsentApplicableEmployeeTypeId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentApplicableEmployeeTypeId == null ? 0 : (int)x.AbsentApplicableEmployeeTypeId).ToList();
        }
        public List<int> GetAbsentExceptionEmployeeType(int absentSettingId)
        {
            return dbContext.AbsentEmployeeType.Where(x => x.AbsentExceptionEmployeeTypeId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentExceptionEmployeeTypeId == null ? 0 : (int)x.AbsentExceptionEmployeeTypeId).ToList();
        }
        public List<AbsentEmployeeType> GetApplicableAbsentEmployeeType(int absentSettingId)
        {
            return dbContext.AbsentEmployeeType.Where(x => x.AbsentApplicableEmployeeTypeId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<AbsentEmployeeType> GetExceptionAbsentEmployeeType(int absentSettingId)
        {
            return dbContext.AbsentEmployeeType.Where(x => x.AbsentExceptionEmployeeTypeId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
    }
}
