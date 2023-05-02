using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IDesignationRepository : IBaseRepository<Designation>
    {

        Designation GetDesignationDetailByDesignationId(int designationId);
        List<DesignationEmployeeDetails> GetEmployeeListByDesignationId(int designationID);
        List<DesignationDetail> GetDesignationListAndCount();
    }
    public class DesignationRepository : BaseRepository<Designation>, IDesignationRepository
    {
        private readonly IAMDBContext dbContext;
        public DesignationRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public Designation GetDesignationDetailByDesignationId(int designationId)
        {
            return dbContext.Designation.Where(x => x.DesignationId == designationId).FirstOrDefault();
        }
        public List<DesignationEmployeeDetails> GetEmployeeListByDesignationId(int designationID)
        {
            List<DesignationEmployeeDetails> employee = new();
            employee = (from employees in dbContext.Employees
                        join employeetype in dbContext.EmployeesType on employees.EmployeeTypeId equals employeetype.EmployeesTypeId
                        join designation in dbContext.Designation on employees.DesignationId equals designation.DesignationId
                        where employees.DesignationId == designationID
                        select new DesignationEmployeeDetails
                        {
                            EmployeeId = employees.EmployeeID,
                            EmployeeName = employees.EmployeeName,
                            FormattedEmployeeId = employees.FormattedEmployeeId,
                            EmployeeTypeName = employeetype.EmployeesType,
                            DesignationName = designation.DesignationName,
                            ProfilePic = employees.ProfilePicture,
                        }).OrderByDescending(x => x.EmployeeId).ToList();
            return employee;
        }
        public List<DesignationDetail> GetDesignationListAndCount()
        {

            List<DesignationDetail> designation = new();
            designation = (
                           from designations in dbContext.Designation
                           select new DesignationDetail
                           {
                               DesignationId = designations.DesignationId,
                               DesignationName = designations.DesignationName,
                               DesignationShortName = designations.DesignationShortName,
                               DesignationDescription = designations.DesignationDescription,
                               CreatedBy = designations.CreatedBy,
                               CreatedOn = designations.CreatedOn,
                               DesignationCount = dbContext.Employees.Count(x => x.DesignationId == designations.DesignationId),
                           }).OrderByDescending(x => x.DesignationId).ToList();
            return designation;
        }
    }
}
