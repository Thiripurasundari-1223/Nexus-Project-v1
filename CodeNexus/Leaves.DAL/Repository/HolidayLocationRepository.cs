using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaves.DAL.Repository
{
    public interface IHolidayLocationRepository : IBaseRepository<HolidayLocation>
    {
        public List<HolidayLocation> GetHolidayLocationDetailsByHolidayId(int holidayId);
        HolidayLocation GetHolidayLocationByID(int holidayLocationId);
        List<int> GetHolidayLocationByHolidayId(int holidayId);
    }
    public class HolidayLocationRepository : BaseRepository<HolidayLocation>, IHolidayLocationRepository
    {
        private readonly LeaveDBContext dbContext;
        public HolidayLocationRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public HolidayLocation GetHolidayLocationByID(int holidayLocationId)
        {
            return dbContext.HolidayLocation.Where(x => x.HolidayLocationId == holidayLocationId).FirstOrDefault();
        }
        public List<HolidayLocation> GetHolidayLocationDetailsByHolidayId(int holidayId)
        {
            return dbContext.HolidayLocation.Where(x => x.HolidayId == holidayId).ToList();
        }
        public List<int> GetHolidayLocationByHolidayId(int holidayId)
        {
            return dbContext.HolidayLocation.Where(x => x.HolidayId == holidayId).Select(x => x.LocationId == null ? 0 : (int)x.LocationId).ToList();
        }

    }
}
