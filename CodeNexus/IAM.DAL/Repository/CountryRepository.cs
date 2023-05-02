using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface ICountryRepository : IBaseRepository<EmployeeCountry>
    {
        List<EmployeeCountry> GetAllCountry();
        public string GetCountryNameById(int countryId);

    }
    public class CountryRepository : BaseRepository<EmployeeCountry>,ICountryRepository
    {
        private readonly IAMDBContext dbContext;
        public CountryRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<EmployeeCountry> GetAllCountry()
        {
            return dbContext.EmployeeCountry.ToList();
        }
        public string GetCountryNameById(int countryId)
        {
            return dbContext.EmployeeCountry.Where(x=>x.CountryId == countryId).Select(x=>x.CountryName).FirstOrDefault();
        }
    }
}
