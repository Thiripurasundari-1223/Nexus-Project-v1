using IAM.DAL.DBContext;
using SharedLibraries.Common;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IAM.DAL.Repository
{
    public interface IDepartmentRepository: IBaseRepository<Department>
    {
        Department GetDepartmentById(int departmentId);
        bool DepartmentNameDuplication(string pDepartmentName);
        List<KeyWithValue> GetDepartmentNameById(List<int> departmentId);
        List<KeyWithValue> GetAllDepartmentName();
        List<DepartmentDetailList> GetDepartmentListAndCount();
        List<DepartmentEmployeeList> GetEmployeeListByDepartmentId(int departmentID);
        List<DepartmentEmployeeList> GetDepartmentHeadEmployees();
    }
    public class DepartmentRepository: BaseRepository<Department>, IDepartmentRepository
    {
        private readonly IAMDBContext dbContext;
        public DepartmentRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public Department GetDepartmentById(int departmentId)
        {
            return dbContext.Department.Where(x => x.DepartmentId == departmentId).FirstOrDefault();
        }
        public bool DepartmentNameDuplication(string pDepartmentName)
        {
            Department department = dbContext.Department.Where(x => x.DepartmentName == pDepartmentName).FirstOrDefault();
            if (department?.DepartmentId > 0)
                return true;
            return false;
        }
        public List<KeyWithValue> GetDepartmentNameById(List<int> departmentId)
        {
            return dbContext.Department.Where(x => departmentId.Contains(x.DepartmentId)).Select(y => new
                            KeyWithValue
            { Key = y.DepartmentId, Value = y.DepartmentName }).ToList();
        }
        public List<KeyWithValue> GetAllDepartmentName()
        {
            return dbContext.Department.Select(y => new
                            KeyWithValue
            { Key = y.DepartmentId, Value = y.DepartmentName }).ToList();
        }
        public List<DepartmentDetailList> GetDepartmentListAndCount()
        {



            List<DepartmentDetailList> department = new();
            department = (
                           from departments in dbContext.Department

                           select new DepartmentDetailList
                           {
                               DepartmentId = departments.DepartmentId,
                               DepartmentName = departments.DepartmentName,
                               DepartmentShortName = departments.DepartmentShortName,
                               DepartmentDescription = departments.DepartmentDescription,
                               CreatedBy = departments.CreatedBy,
                               CreatedOn = departments.CreatedOn,
                               IsEnableBUAccountable= departments.IsEnableBUAccountable,
                               ParentDepartmentId = departments.ParentDepartmentId,
                               DepartmentHeadEmployeeId = departments.DepartmentHeadEmployeeId,
                               DepartmentCount = dbContext.Employees.Where(x => x.DepartmentId == departments.DepartmentId && x.IsActive == true).Count(),
                           }).OrderBy(x => x.DepartmentId).ToList();
            return department;
        }

        public List<DepartmentEmployeeList> GetEmployeeListByDepartmentId(int departmentID)
        {
            List<DepartmentEmployeeList> employee = new();
            employee = (from employees in dbContext.Employees
                       // join employeetype in dbContext.EmployeesType on employees.EmployeeTypeId equals employeetype.EmployeesTypeId
                        where employees.DepartmentId == departmentID && employees.IsActive == true
                        select new DepartmentEmployeeList
                        {
                            EmployeeId = employees.EmployeeID,
                            EmployeeName = employees.EmployeeName,
                            FormattedEmployeeId = employees.FormattedEmployeeId,
                            EmployeeTypeName = dbContext.EmployeesType.Where(x=>x.EmployeesTypeId==employees.EmployeeTypeId).Select(x=>x.EmployeesType).FirstOrDefault(),
                            //JobTitle = employees.JobTitle,
                            JobTitle=dbContext.Designation.Where(x=>x.DesignationId==employees.DesignationId).Select(x=>x.DesignationName).FirstOrDefault(),
                            ProfilePic =employees.ProfilePicture,
                        }).OrderBy(x => x.EmployeeId).ToList();
            return employee;
        }
        public List<DepartmentEmployeeList> GetDepartmentHeadEmployees()
        {
            List<DepartmentEmployeeList> department = new();
            department = (from employees in dbContext.Employees
                          where employees.IsActive==true
                          select new DepartmentEmployeeList
                          {
                              EmployeeId = employees.EmployeeID,
                              DepartmentHeadFullName = employees.EmployeeName + " - " + employees.FormattedEmployeeId,
                              EmployeeEmailId =employees.EmailAddress,
                              ProfilePic=employees.ProfilePicture,
                              EmployeeTypeName = dbContext.EmployeesType.Where(x=>x.EmployeesTypeId == employees.EmployeeTypeId).Select(x=>x.EmployeesType).FirstOrDefault(),
                              JobTitle=dbContext.Designation.Where(x=>x.DesignationId==employees.DesignationId).Select(x=>x.DesignationName).FirstOrDefault(),
                          }
                          ).ToList();
            return department;
        }

    }
}
