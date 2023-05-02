using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAM.DAL.Repository
{
    public interface IEmployeeRelationshipRepository : IBaseRepository<EmployeeRelationship>
    {
        EmployeeRelationship GetEmployeeRelationshipById(int? employeeRelationshipId);
    }
    public class EmployeeRelationshipRepository : BaseRepository<EmployeeRelationship>, IEmployeeRelationshipRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeRelationshipRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public EmployeeRelationship GetEmployeeRelationshipById(int? employeeRelationshipId)
        {
            return dbContext.EmployeeRelationship.Where(x => x.EmployeeRelationshipId == employeeRelationshipId).FirstOrDefault();
        }
    }
}
