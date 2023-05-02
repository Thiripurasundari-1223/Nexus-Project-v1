using ProjectManagement.DAL.DBContext;
using SharedLibraries.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.DAL.Repository
{
    public interface ICustomerSPOCDetailsRepository : IBaseRepository<CustomerSPOCDetails>
    {
        CustomerSPOCDetails GetPCustomerSPOCDetailsByID(int projectId);
    }

    public class CustomerSPOCDetailsRepository : BaseRepository<CustomerSPOCDetails>, ICustomerSPOCDetailsRepository
    {
        private readonly PMDBContext _dbContext;
        public CustomerSPOCDetailsRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public CustomerSPOCDetails GetPCustomerSPOCDetailsByID(int projectId)
        {
            if (projectId > 0)
            {
                return _dbContext.CustomerSPOCDetails.Where(r => r.ProjectID == projectId).FirstOrDefault();
            }
            return null;
        }
    }
}
