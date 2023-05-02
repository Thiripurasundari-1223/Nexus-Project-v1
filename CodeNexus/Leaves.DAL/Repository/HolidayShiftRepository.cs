using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface IHolidayShiftRepository : IBaseRepository<HolidayShift>
    {
        public List<HolidayShift> GetHolidayShiftDetailsByHolidayId(int holidayId);
        public List<int> GetHolidayShiftByHolidayId(int holidayId);
        HolidayShift GetHolidayShiftByID(int holidayShiftId);
    }
    public class HolidayShiftRepository : BaseRepository<HolidayShift>, IHolidayShiftRepository
    {
        private readonly LeaveDBContext dbContext;
        public HolidayShiftRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public HolidayShift GetHolidayShiftByID(int holidayShiftId)
        {
            return dbContext.HolidayShift.Where(x => x.HolidayShiftId == holidayShiftId).FirstOrDefault();
        }
        public List<HolidayShift> GetHolidayShiftDetailsByHolidayId(int holidayId)
        {
            return dbContext.HolidayShift.Where(x => x.HolidayId == holidayId).ToList();
        }
        public List<int> GetHolidayShiftByHolidayId(int holidayId)
        {
            return dbContext.HolidayShift.Where(x => x.HolidayId == holidayId).Select(x => x.ShiftDetailsId == null ? 0 : (int)x.ShiftDetailsId).ToList();
        }
    }
}
