using IAM.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;

namespace IAM.DAL.Repository
{
    public interface IEmployeesPersonalInfoRepository : IBaseRepository<EmployeesPersonalInfo>
    {
        EmployeesPersonalInfo GetEmployeePersonalIdByEmployeeID(int? employeeId);
        EmployeesPersonalInfo GetEmployeePersonalInfoById(int? personalInfoId);
    }

    public class EmployeesPersonalInfoRepository : BaseRepository<EmployeesPersonalInfo>, IEmployeesPersonalInfoRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeesPersonalInfoRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeesPersonalInfo GetEmployeePersonalIdByEmployeeID(int? employeeId)
        {
            return dbContext.EmployeesPersonalInfo.Where(x => x.EmployeeId == employeeId).FirstOrDefault();
        }
        public EmployeesPersonalInfo GetEmployeePersonalInfoById(int? personalInfoId)
        {
            return dbContext.EmployeesPersonalInfo.Where(x => x.PersonalInfoId == personalInfoId).FirstOrDefault();
        }
    }
}
