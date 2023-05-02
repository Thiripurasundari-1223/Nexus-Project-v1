using IAM.DAL.DBContext;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAM.DAL.Repository
{
    public interface IRoleRepository : IBaseRepository<Roles>
    {
        //Roles GetByName(string pRoleName, int pRoleID = 0);
        Roles GetRoleByID(int pRoleID);
        //List<Roles> GetByName(string[] pRoleNames);
        int GetFinanceManagerId();
        List<RoleName> GetRoleNameById(List<int> lstRoleId);
        bool RoleNameDuplication(string pRoleName);
        List<RoleName> GetSystemRoleNameList();
        int GetRoleByName(string pRoleName);
        List<Designation> GetDesignationList();
        List<BUAccountableForProject> GetBUAccountableForProjects(int? departmentHeadId);
        FinanceManagerDetails GetfinanceManagerDetails(AccountDetails accountDetails);
    }
    public class RoleRepository : BaseRepository<Roles>, IRoleRepository
    {
        private readonly IAMDBContext dbContext;
        public RoleRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        //public Roles GetByName(string pRoleName, int pRoleID = 0)
        //{
        //    if (pRoleID > 0)
        //    {
        //        return dbContext.Role.Where(x => x.RoleName == pRoleName && x.RoleId == pRoleID).FirstOrDefault();
        //    }
        //    return dbContext.Role.Where(x => x.RoleId == pRoleID).FirstOrDefault();
        //}
        //public List<Roles> GetByName(string[] pRoleNames)
        //{
        //    return dbContext.Role.Where(x => pRoleNames.Contains(x.RoleName)).ToList();
        //}
        public Roles GetRoleByID(int pRoleID)
        {
            return dbContext.Role.Where(x => x.RoleId == pRoleID).FirstOrDefault();
        }
        public int GetFinanceManagerId()
        {
            //return (from emp in dbContext.Employees
            //        join role in dbContext.Role on emp.RoleId equals role.RoleId
            //        where role.RoleName == "Finance Manager"
            //        select emp.EmployeeID).FirstOrDefault();
            int? financiManager=(from emp in dbContext.Employees
             join dept in dbContext.Department on emp.DepartmentId equals dept.DepartmentId
             join empcat in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals empcat.EmployeeCategoryId
             where empcat.EmployeeCategoryName.ToLower() == "bu head" && dept.DepartmentName.ToLower()== "finance"             
             select emp.EmployeeID).FirstOrDefault();

            return financiManager == null ? 0 : (int)financiManager;
        }
        public List<RoleName> GetRoleNameById(List<int> lstRoleId)
        {
            return dbContext.Role.Where(x => lstRoleId.Contains(x.RoleId)).Select(y => new RoleName { RoleId = y.RoleId, RoleFullName = y.RoleName }).ToList();
        }
        public bool RoleNameDuplication(string pRoleName)
        {
            Roles roles = dbContext.Roles.Where(x => x.RoleName == pRoleName).FirstOrDefault();
            if (roles?.RoleId > 0)
                return true;
            return false;
        }
        public List<RoleName> GetSystemRoleNameList()
        {
            return dbContext.SystemRoles.Select(x => new RoleName { RoleId = x.RoleId, RoleFullName = x.RoleName }).ToList();
        }
        public int GetRoleByName(string pRoleName)
        {
            return dbContext.Role.Where(x => x.RoleName == pRoleName).Select(x => x.RoleId).FirstOrDefault();
        }
        public List<Designation> GetDesignationList()
        {
            return dbContext.Designation.ToList();
        }
        public List<BUAccountableForProject> GetBUAccountableForProjects(int? departmentHeadId)
        {
            if (departmentHeadId > 0)
            {
                return (from emp in dbContext.Employees
                        join dept in dbContext.Department on emp.DepartmentId equals dept.DepartmentId
                        join empcat in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals empcat.EmployeeCategoryId
                        where empcat.EmployeeCategoryName.ToLower() == "bu head" && empcat.DepartmentId == dept.DepartmentId
                        && emp.EmployeeID == departmentHeadId && dept.IsEnableBUAccountable == true
                        select new BUAccountableForProject
                        {
                            DepartmentHeadId = emp.EmployeeID,
                            DepartmentHead = dept.DepartmentName + " (" + emp.FirstName + " " + emp.LastName + ")"
                        }).ToList();
            }
            return (from emp in dbContext.Employees
                    join dept in dbContext.Department on emp.DepartmentId equals dept.DepartmentId
                    join empcat in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals empcat.EmployeeCategoryId
                    where empcat.EmployeeCategoryName.ToLower() == "bu head" && empcat.DepartmentId == dept.DepartmentId
                    && dept.IsEnableBUAccountable == true
                    select new BUAccountableForProject
                    {
                        DepartmentHeadId = emp.EmployeeID,
                        DepartmentHead = dept.DepartmentName + " (" + emp.FirstName + " " + emp.LastName + ")"
                    }).OrderBy(x => x.DepartmentHead).ToList();
        }

        public FinanceManagerDetails GetfinanceManagerDetails(AccountDetails accountDetails)
        {
            //return (from emp in dbContext.Employees
            //        join role in dbContext.Role on emp.RoleId equals role.RoleId
            //        where role.RoleName == "Finance Manager"
            //        select emp.EmployeeID).FirstOrDefault();
            FinanceManagerDetails financiManager = (from emp in dbContext.Employees
                                   join dept in dbContext.Department on emp.DepartmentId equals dept.DepartmentId
                                   join empcat in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals empcat.EmployeeCategoryId
                                   where empcat.EmployeeCategoryName.ToLower() == "bu head" && dept.DepartmentName.ToLower() == "finance"
                                   select new FinanceManagerDetails 
                                   {
                                       FinanceId=emp.EmployeeID,
                                       FinanceName= emp.EmployeeName,
                                       FinanceEmail =emp.EmailAddress
                                   }).FirstOrDefault();
            if (financiManager != null)
            {
                if (accountDetails.AccountManagerId == accountDetails.CreatedBy)
                {
                    financiManager.AccountManagerEmailId = dbContext.Employees.Where(x => x.EmployeeID == accountDetails.AccountManagerId).Select(x => x.EmailAddress).FirstOrDefault();
                    financiManager.CreatedByEmailId = financiManager.AccountManagerEmailId;
                }
                else
                {
                    financiManager.AccountManagerEmailId = dbContext.Employees.Where(x => x.EmployeeID == accountDetails.AccountManagerId)?.Select(x => x.EmailAddress)?.FirstOrDefault();
                    financiManager.CreatedByEmailId = dbContext.Employees.Where(x => x.EmployeeID == accountDetails.CreatedBy)?.Select(x => x.EmailAddress)?.FirstOrDefault();
                }
            }
            return financiManager == null ? new FinanceManagerDetails() : financiManager;
        }
    }
}