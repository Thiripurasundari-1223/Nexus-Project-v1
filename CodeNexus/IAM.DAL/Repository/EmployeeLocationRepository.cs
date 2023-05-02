using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeLocationRepository : IBaseRepository<EmployeeLocation>
    {
        EmployeeLocation GetLocationById(int LocationId);
        bool LocationNameDuplicate(string locationName);
    }
    public class EmployeeLocationRepository : BaseRepository<EmployeeLocation>, IEmployeeLocationRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeLocationRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public EmployeeLocation GetLocationById(int LocationId)
        {
            return dbContext.EmployeeLocation.Where(x => x.LocationId == LocationId).FirstOrDefault();
        }

        public bool LocationNameDuplicate(string locationName)
        {
            EmployeeLocation location = dbContext.EmployeeLocation.Where(x => x.Location == locationName).FirstOrDefault();
            if (location != null)
                return true;
            return false;
        }
    }
}
