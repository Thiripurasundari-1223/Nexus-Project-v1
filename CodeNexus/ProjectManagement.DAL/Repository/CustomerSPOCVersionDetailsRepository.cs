using ProjectManagement.DAL.DBContext;
using ProjectManagement.DAL.Repository;
using SharedLibraries.Models.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{
    public interface ICustomerSPOCVersionDetailsRepository : IBaseRepository<VersionCustomerSPOCDetails>
    {


    }


    public class CustomerSPOCVersionDetailsRepository : BaseRepository<VersionCustomerSPOCDetails>, ICustomerSPOCVersionDetailsRepository
    {
        private readonly PMDBContext _dbContext;
        public CustomerSPOCVersionDetailsRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
