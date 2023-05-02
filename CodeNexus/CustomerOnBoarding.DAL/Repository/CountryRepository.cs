using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOnBoarding.DAL.Repository
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Country GetByName(string pCountryName, int pCountryId = 0);
        Country GetByID(int pCountryId);
        List<Country> GetAllCountry();
    }
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        private readonly COBDBContext dbContext;
        public CountryRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public Country GetByName(string pCountryName, int pCountryId = 0)
        {
            if (pCountryId > 0)
            {
                return dbContext.Country.Where(x => x.CountryName == pCountryName && x.CountryId == pCountryId).FirstOrDefault();
            }
            return dbContext.Country.Where(x => x.CountryId == pCountryId).FirstOrDefault();
        }
        public Country GetByID(int pCountryId)
        {
            return dbContext.Country.Where(x => x.CountryId == pCountryId).FirstOrDefault();
        }
        public List<Country> GetAllCountry()
        {
            return dbContext.Country.ToList();
        }
    }
}