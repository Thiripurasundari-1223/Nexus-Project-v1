using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeNationalityRepository:IBaseRepository<EmployeeNationality>
    {
        List<EmployeeNationality> GetAllNationality();
        public string GetNationalityNameById(int nationalityId);
    }
    public class EmployeeNationalityRepository:BaseRepository<EmployeeNationality>, IEmployeeNationalityRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeNationalityRepository(IAMDBContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public List<EmployeeNationality> GetAllNationality()
        {
            return dbContext.EmployeeNationality.ToList();
        }
        public string GetNationalityNameById(int nationalityId)
        {
            return dbContext.EmployeeNationality.Where(x => x.NationalityId == nationalityId).Select(x => x.NationalityName).FirstOrDefault();
        }
    }
}
