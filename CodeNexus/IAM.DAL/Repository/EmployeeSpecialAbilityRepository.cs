using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeSpecialAbilityRepository : IBaseRepository<EmployeeSpecialAbility>
    {
        List<EmployeeSpecialAbility> GetEmployeeSpecialAbilityByEmployeeId(int employeeId);
        Task<EmployeeSpecialAbility> GetEmployeeSpecialAbilityById(int employeeSpecialAbilityId);
        Task<EmployeeSpecialAbility> GetEmployeeSpecialAbilityByIdAndEmployeeId(int specialAbilityId, int employeeId);
    }
    public class EmployeeSpecialAbilityRepository : BaseRepository<EmployeeSpecialAbility>, IEmployeeSpecialAbilityRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeSpecialAbilityRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<EmployeeSpecialAbility> GetEmployeeSpecialAbilityByEmployeeId(int employeeId)
        {
            return dbContext.EmployeeSpecialAbility.Where(x => x.EmployeeId == employeeId).ToList();
        }
        public async Task<EmployeeSpecialAbility>  GetEmployeeSpecialAbilityById(int employeeSpecialAbilityId)
        {
            return dbContext.EmployeeSpecialAbility.Where(x => x.EmployeeSpecialAbilityId == employeeSpecialAbilityId).FirstOrDefault();
        }
        public async Task<EmployeeSpecialAbility> GetEmployeeSpecialAbilityByIdAndEmployeeId(int specialAbilityId,int employeeId)
        {
            return  dbContext.EmployeeSpecialAbility.Where(x => x.SpecialAbilityId == specialAbilityId && x.EmployeeId==employeeId).FirstOrDefault();
        }
    }



}
