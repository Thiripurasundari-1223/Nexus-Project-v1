using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAM.DAL.Repository
{
    public interface IEmployeeTypeRepository : IBaseRepository<EmployeesTypes>
    {
        List<EmployeesTypes> GetEmployeeTypeList();
        List<EmployeeTypeNames> GetEmployeeTypeNameById(List<int> employeesTypeId);
        List<EmployeeDetailsForLeaveView> GetAllEmployeeforLeave();
        EmployeeDetailsForLeaveView GetNewEmployeeDetailsbyID(int EmployeeID);
    }
    public class EmployeeTypeRepository : BaseRepository<EmployeesTypes>, IEmployeeTypeRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeTypeRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<EmployeesTypes> GetEmployeeTypeList()
        {
            return dbContext.EmployeesType.ToList();
        }
        public List<EmployeeTypeNames> GetEmployeeTypeNameById(List<int> employeesTypeId)
        {
            return dbContext.EmployeesType.Where(x => employeesTypeId.Contains(x.EmployeesTypeId)).Select(x => new EmployeeTypeNames { EmployeesTypeId = x.EmployeesTypeId, EmployeesType = x.EmployeesType }).ToList();
        }

        public List<EmployeeDetailsForLeaveView> GetAllEmployeeforLeave()
        {
            
            return dbContext.Employees.Where(x=>x.IsActive==true).Select(x => new EmployeeDetailsForLeaveView { EmployeeID=x.EmployeeID,  EmployeeTypeID=x.EmployeeTypeId, 
                                                                                     DepartmentID=x.DepartmentId, RoleID=x.SystemRoleId, IsActive=x.IsActive, 
                                                                                     Gender=x.Gender, LocationID=x.LocationId,MaritalStatus=x.Maritalstatus,
                                                                                     DesignationID=x.DesignationId,DateOfJoining=x.DateOfJoining,
                                                                                     DateOfContract=x.DateOfContract,ProbationStatusID=x.ProbationStatusId }).ToList();
        }

        public EmployeeDetailsForLeaveView GetNewEmployeeDetailsbyID(int EmployeeID)
        {

            return dbContext.Employees.Where(x => x.EmployeeID == EmployeeID).Select(x => new EmployeeDetailsForLeaveView
            {
                EmployeeID = x.EmployeeID,
                EmployeeTypeID = x.EmployeeTypeId,
                DepartmentID = x.DepartmentId,
                RoleID = x.RoleId,
                IsActive = x.IsActive,
                Gender = x.Gender,
                LocationID = x.LocationId,
                MaritalStatus = x.Maritalstatus,
                DesignationID = x.DesignationId,
                DateOfJoining = x.DateOfJoining,
                DateOfContract = x.DateOfContract,
                ProbationStatusID = x.ProbationStatusId
            }).FirstOrDefault();
        }
    }
}
