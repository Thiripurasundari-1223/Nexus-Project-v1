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
        CustomerSPOCDetails GetPCustomerSPOCDetailsByID(int pCustomerSPOCDetailsID);
    }

    public class CustomerSPOCDetailsRepository : BaseRepository<CustomerSPOCDetails>, ICustomerSPOCDetailsRepository
    {
        private readonly PMDBContext _dbContext;
        public CustomerSPOCDetailsRepository(PMDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public CustomerSPOCDetails GetPCustomerSPOCDetailsByID(int pCustomerSPOCDetailsID)
        {
            if (pCustomerSPOCDetailsID > 0)
            {
                return _dbContext.CustomerSPOCDetails.Where(r => r.CustomerSPOCDetailsID == pCustomerSPOCDetailsID).FirstOrDefault();
            }
            return null;
        }
    }
}
