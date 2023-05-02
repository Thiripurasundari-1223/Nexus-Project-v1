using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;

namespace IAM.DAL.Repository
{
    public interface IEmployeesSkillsetRepository : IBaseRepository<EmployeesSkillset>
    {
        EmployeesSkillset GetEmployeesSkillsetBySkillsetId(int skillsetId,int employeeId);
        List<EmployeesSkillset> GetEmployeesSkillsetByEmployeeId(int employeeId);
        Task<EmployeesSkillset> GetEmployeesSkillsetById(int employeeSkillsetId);
        List<EmployeesSkillset> GetAllEmployeesSkillset();
        Task<EmployeesSkillset> GetAllEmployeesSkillsetByIds(int skillsetId, int employeeId);
        List<int> GetEmployeeSkillsIdByEmployeeId(int employeeId);
    }
    public class EmployeesSkillsetRepository: BaseRepository<EmployeesSkillset>, IEmployeesSkillsetRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeesSkillsetRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public async Task<EmployeesSkillset>  GetEmployeesSkillsetById(int employeeSkillsetId)
        {
            return dbContext.EmployeesSkillset.Where(x => x.EmployeesSkillsetId == employeeSkillsetId).FirstOrDefault();
        }
        public EmployeesSkillset GetEmployeesSkillsetBySkillsetId(int skillsetId, int employeeId)
        {
            return dbContext.EmployeesSkillset.Where(x => x.SkillsetId == skillsetId && x.EmployeeId==employeeId).FirstOrDefault();
        }
        public List<EmployeesSkillset> GetEmployeesSkillsetByEmployeeId(int employeeId)
        {
            return dbContext.EmployeesSkillset.Where(x => x.EmployeeId == employeeId).ToList();
        }
        public List<EmployeesSkillset> GetAllEmployeesSkillset()
        {
            return dbContext.EmployeesSkillset.ToList();
        }
        public async Task<EmployeesSkillset> GetAllEmployeesSkillsetByIds(int skillsetId , int employeeId)
        {
           if(skillsetId != 0)
           {
                return dbContext.EmployeesSkillset.Where(x => x.EmployeeId == employeeId && x.SkillsetId == skillsetId).FirstOrDefault();
           }
           else
           {
                return null;
           }
        }
        public List<int> GetEmployeeSkillsIdByEmployeeId(int employeeId)
        {
            return dbContext.EmployeesSkillset.Where(x => x.EmployeeId == employeeId).OrderBy(x=>x.SkillsetId).Select(x => x.SkillsetId).ToList();
        }
    }
}
