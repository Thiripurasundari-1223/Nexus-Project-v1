using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface IHolidayDepartmentRepository : IBaseRepository<HolidayDepartment>
    {
        List<HolidayDepartment> GetHolidayDepartmentDetailsByHolidayId(int holidayId);
        HolidayDepartment GetHolidayDepartmentByID(int holidayDepartmentId);
        List<int> GetHolidayDepartmentByHolidayId(int holidayId);
    }
    public class HolidayDepartmentRepository : BaseRepository<HolidayDepartment>, IHolidayDepartmentRepository
    {
        private readonly LeaveDBContext dbContext;
        public HolidayDepartmentRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public HolidayDepartment GetHolidayDepartmentByID(int holidayDepartmentId)
        {
            return dbContext.HolidayDepartment.Where(x => x.HolidayDepartmentId == holidayDepartmentId).FirstOrDefault();
        }
        public List<HolidayDepartment> GetHolidayDepartmentDetailsByHolidayId(int holidayId)
        {
            return dbContext.HolidayDepartment.Where(x => x.HolidayId == holidayId).ToList();
        }
        public List<int> GetHolidayDepartmentByHolidayId(int holidayId)
        {
            return dbContext.HolidayDepartment.Where(x => x.HolidayId == holidayId).Select(x=>x.DepartmentId==null?0:(int)x.DepartmentId).ToList();
        }

    }
}
