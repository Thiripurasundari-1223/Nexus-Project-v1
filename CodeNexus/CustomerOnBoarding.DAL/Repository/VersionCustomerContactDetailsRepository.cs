using CustomerOnBoarding.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnBoarding.DAL.Repository
{
    
    public interface IVersionCustomerContactDetailsRepository : IBaseRepository<VersionCustomerContactDetails>
    {
    }
    public class VersionCustomerContactDetailsRepository : BaseRepository<VersionCustomerContactDetails>, IVersionCustomerContactDetailsRepository
    {
        private readonly COBDBContext dbContext;
        public VersionCustomerContactDetailsRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        
    }
}
