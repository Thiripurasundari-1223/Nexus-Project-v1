using Attendance.DAL.DBContext;
using SharedLibraries.Models.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.DAL.Repository
{
    public interface IAbsentDesignationRepository : IBaseRepository<AbsentDesignation>
    {
        List<AbsentDesignation> GetAbsentDesignationByAbsentSettingId(int absentSettingId);
        List<int> GetAbsentApplicableDesignation(int absentSettingId);
        List<int> GetAbsentExceptionDesignation(int absentSettingId);
        List<AbsentDesignation> GetApplicableAbsentDesignation(int absentSettingId);
        List<AbsentDesignation> GetExceptionAbsentDesignation(int absentSettingId);
    }
    public class AbsentDesignationRepository : BaseRepository<AbsentDesignation>, IAbsentDesignationRepository
    {
        private readonly AttendanceDBContext dbContext;
        public AbsentDesignationRepository(AttendanceDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<AbsentDesignation> GetAbsentDesignationByAbsentSettingId(int absentSettingId)
        {
            return dbContext.AbsentDesignation.Where(x => x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<int> GetAbsentApplicableDesignation(int absentSettingId)
        {
            return dbContext.AbsentDesignation.Where(x => x.AbsentApplicableDesignationId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentApplicableDesignationId == null ? 0 : (int)x.AbsentApplicableDesignationId).ToList();
        }
        public List<int> GetAbsentExceptionDesignation(int absentSettingId)
        {
            return dbContext.AbsentDesignation.Where(x => x.AbsentExceptionDesignationId > 0 && x.AbsentSettingId == absentSettingId).Select(x => x.AbsentExceptionDesignationId == null ? 0 : (int)x.AbsentExceptionDesignationId).ToList();
        }
        public List<AbsentDesignation> GetApplicableAbsentDesignation(int absentSettingId)
        {
            return dbContext.AbsentDesignation.Where(x => x.AbsentApplicableDesignationId > 0 && x.AbsentSettingId == absentSettingId).ToList();
        }
        public List<AbsentDesignation> GetExceptionAbsentDesignation(int absentSettingId)
        {
            return dbContext.AbsentDesignation.Where(x => x.AbsentExceptionDesignationId > 0 && x.AbsentSettingId== absentSettingId).ToList();
        }
    }
}
