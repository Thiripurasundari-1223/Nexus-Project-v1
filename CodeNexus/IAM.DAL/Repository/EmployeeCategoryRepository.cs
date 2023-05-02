using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System.Collections.Generic;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IEmployeeCategoryRepository : IBaseRepository<EmployeeCategory>
    {
        string GetEmployeeCategoryNameById(int employeeCategoryId);
        int GetEmployeeCategoryByNameAndDepartmentId(int pDepartmentId, string pEmployeeCategoryName);
        List<EmployeeCategory> GetEmployeeCategoryList();
    }
    public class EmployeeCategoryRepository : BaseRepository<EmployeeCategory>, IEmployeeCategoryRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeCategoryRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public string GetEmployeeCategoryNameById(int employeeCategoryId)
        {
            return dbContext.EmployeeCategory.Where(x => x.EmployeeCategoryId == employeeCategoryId).Select(x=>x.EmployeeCategoryName).FirstOrDefault();
        }
        public int GetEmployeeCategoryByNameAndDepartmentId(int pDepartmentId, string pEmployeeCategoryName)
        {
            return dbContext.EmployeeCategory.Where(x => x.DepartmentId == pDepartmentId &&
                        x.EmployeeCategoryName == pEmployeeCategoryName).
                        Select(x => x.EmployeeCategoryId).FirstOrDefault();
        }
        public List<EmployeeCategory> GetEmployeeCategoryList()
        {
            return dbContext.EmployeeCategory.ToList();
        }
    }
}