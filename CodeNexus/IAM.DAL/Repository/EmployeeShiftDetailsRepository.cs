using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeShiftDetailsRepository : IBaseRepository<EmployeeShiftDetails>
    {
        List<EmployeeShiftDetails> GetEmployeeShiftByEmployeeId(int EmployeeId);
        EmployeeShiftDetails GetEmployeeShiftDetailsByEmployeeId(int employeeId);
        EmployeeShiftDetails GetEmployeePreviousShiftDetailsByEmployeeId(int employeeId);
        Task<EmployeeShiftDetails> GetShiftDetailsById(int employeeShiftDetailsId);
    }
    public class EmployeeShiftDetailsRepository : BaseRepository<EmployeeShiftDetails>, IEmployeeShiftDetailsRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeShiftDetailsRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<EmployeeShiftDetails> GetEmployeeShiftByEmployeeId(int EmployeeId)
        {
            return dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == EmployeeId).ToList();
        }
        public EmployeeShiftDetails GetEmployeeShiftDetailsByEmployeeId(int employeeId)
        {
            EmployeeShiftDetails employeeShift = new EmployeeShiftDetails();
            employeeShift = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
            if (employeeShift == null)
            {
                EmployeeShiftDetails employeeShiftDetails = new EmployeeShiftDetails()
                {
                    EmployeeID = 0
                };
                return employeeShiftDetails;
            }
            return employeeShift;
        }
        public EmployeeShiftDetails GetEmployeePreviousShiftDetailsByEmployeeId(int employeeId)
        {
            return dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == employeeId).OrderBy(x=>x.EmployeeShiftDetailsId).LastOrDefault(); ;
        }
        public async Task<EmployeeShiftDetails> GetShiftDetailsById(int employeeShiftDetailsId)
        {
            return dbContext.EmployeeShiftDetails.Where(x => x.EmployeeShiftDetailsId == employeeShiftDetailsId).FirstOrDefault();
        }
    }
}

