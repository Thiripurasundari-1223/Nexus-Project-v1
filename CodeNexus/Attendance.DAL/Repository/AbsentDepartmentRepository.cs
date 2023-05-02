using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentDepartmentRepository : IBaseRepository<AbsentDepartment>
    {
        List<AbsentDepartment> GetAbsentDepartmentByAbsentSettingId(int absentSettingId);
        List<int> GetAbsentApplicableDepartment(int absettingId);
        List<int> GetAbsentExceptionDepartment(int absettingId);
        List<AbsentDepartment> GetApplicableAbsentDepartment(int absettingId);
        List<AbsentDepartment> GetExceptionAbsentDepartment(int absettingId);
    }
    public class AbsentDepartmentRepository : BaseRepository<AbsentDepartment>, IAbsentDepartmentRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentDepartmentRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AbsentDepartment> GetAbsentDepartmentByAbsentSettingId(int absentSettingId)
        {
            return dbContext.AbsentDepartment.Where(x => x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<int> GetAbsentApplicableDepartment(int absettingId)
        {
            return dbContext.AbsentDepartment.Where(x => x.AbsentApplicableDepartmentId > 0 && x.AbsentSettingId== absettingId).Select(x => x.AbsentApplicableDepartmentId == null ? 0 : (int)x.AbsentApplicableDepartmentId).ToList();
        }
        public List<int> GetAbsentExceptionDepartment(int absettingId)
        {
            return dbContext.AbsentDepartment.Where(x => x.AbsentExceptionDepartmentId > 0 && x.AbsentSettingId == absettingId).Select(x => x.AbsentExceptionDepartmentId == null ? 0 : (int)x.AbsentExceptionDepartmentId).ToList();
        }
        public List<AbsentDepartment> GetApplicableAbsentDepartment(int absettingId)
        {
            return dbContext.AbsentDepartment.Where(x => x.AbsentApplicableDepartmentId > 0 && x.AbsentSettingId == absettingId).ToList();
        }
        public List<AbsentDepartment> GetExceptionAbsentDepartment(int absettingId)
        {
            return dbContext.AbsentDepartment.Where(x => x.AbsentExceptionDepartmentId > 0 && x.AbsentSettingId == absettingId).ToList();
        }
    }
}
