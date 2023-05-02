using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface ISkillsetRepository : IBaseRepository<Skillsets>
    {
        Skillsets GetSkillsetById(int skillsetId);
        List<Skillsets> GetAllSkillset();
        Task<Skillsets> GetSkillsetByName(string skillset);
        List<string> GetSkillsetsByEmployeeId(int employeeId);
        List<SkillsetEmployeeDetails> GetEmployeeListBySkillsetId(EmployeeSkillsetCategoryInput skillsetInput);
        List<SkillsetCategory> GetSkillsetCategoryNames();
        public List<SkillsetCategoryName> GetSkillsetNames();
        string GetSkillsetCategoryNameById(int? categoryId);
        List<EmployeeDetails> GetEmployeeListBySkillsetIdForDownload(EmployeeSkillsetCategoryInput skillsetInput);

    }
    public class SkillsetRepository : BaseRepository<Skillsets>, ISkillsetRepository
    {
        private readonly IAMDBContext dbContext;
        public SkillsetRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public Skillsets GetSkillsetById(int skillsetId)
        {
            return dbContext.Skillset.Where(x => x.SkillsetId == skillsetId).FirstOrDefault();
        }
        public async Task<Skillsets> GetSkillsetByName(string skillset)
        {
            return dbContext.Skillset.Where(x => x.Skillset.ToLower() == skillset.ToLower()).FirstOrDefault();
        }
        public List<Skillsets> GetAllSkillset()
        {
            return dbContext.Skillset.ToList();
        }
        public List<string> GetSkillsetsByEmployeeId(int employeeId)
        {
            return (from a in dbContext.EmployeesSkillset
                                          join b in dbContext.Skillset on a.SkillsetId equals b.SkillsetId
                                          where a.EmployeeId == employeeId
                                          select b.Skillset).ToList();
        }
        public List<SkillsetEmployeeDetails> GetEmployeeListBySkillsetId(EmployeeSkillsetCategoryInput skillsetInput)
        {
            List<string> condition = new List<string>();
            condition = skillsetInput?.SkillsetId?.Select(x => "skillSet.SkillsetId ==" + x).ToList();
           
            string whereCondition =  string.Join(" || ", condition);            
            List<SkillsetEmployeeDetails> employee = new();
            if(skillsetInput?.Condition == "and")
            {
                employee = dbContext.EmployeesSkillset
                .Join(dbContext.Employees, empSkill => empSkill.EmployeeId, emp => emp.EmployeeID, (empSkill, emp) => new { empSkill, emp })
                .Join(dbContext.Skillset, empSkillSet => empSkillSet.empSkill.SkillsetId, skillSet => skillSet.SkillsetId, (empSkillSet, skillSet) =>
                      new { empSkillSet, skillSet }).Where(whereCondition != "" && whereCondition != null ? whereCondition : "skillSet.SkillsetId==skillSet.SkillsetId").ToList()
                .GroupBy(x => new
                {
                    x.empSkillSet.emp.EmployeeID,
                    x.empSkillSet.emp.EmployeeName,
                    x.empSkillSet.emp.FormattedEmployeeId,
                    x.empSkillSet.emp.EmployeeTypeId,
                    x.empSkillSet.emp.ProfilePicture
                }).Where(x=>x.Count()== skillsetInput?.SkillsetId?.Count)
                .Select(employeeSkillset =>
                new SkillsetEmployeeDetails
                {
                    EmployeeId = employeeSkillset.Key.EmployeeID,
                    EmployeeName = employeeSkillset.Key.EmployeeName,
                    FormattedEmployeeId = employeeSkillset.Key.FormattedEmployeeId,
                    EmployeeTypeName = dbContext.EmployeesType.Where(x => x.EmployeesTypeId == employeeSkillset.Key.EmployeeTypeId).Select(x => x.EmployeesType).FirstOrDefault(),
                    Skillset = employeeSkillset.Select(x => x.skillSet.Skillset).ToList(),
                    ProfilePic=employeeSkillset.Key.ProfilePicture
                }).ToList();
            }
            else
            {
                employee = dbContext.EmployeesSkillset
                .Join(dbContext.Employees, empSkill => empSkill.EmployeeId, emp => emp.EmployeeID, (empSkill, emp) => new { empSkill, emp })
                .Join(dbContext.Skillset, empSkillSet => empSkillSet.empSkill.SkillsetId, skillSet => skillSet.SkillsetId, (empSkillSet, skillSet) =>
                      new { empSkillSet, skillSet }).Where(whereCondition != "" && whereCondition != null ? whereCondition : "skillSet.SkillsetId==skillSet.SkillsetId").ToList()
                .GroupBy(x => new
                {
                    x.empSkillSet.emp.EmployeeID,
                    x.empSkillSet.emp.EmployeeName,
                    x.empSkillSet.emp.FormattedEmployeeId,
                    x.empSkillSet.emp.EmployeeTypeId,
                    x.empSkillSet.emp.ProfilePicture
                }).Select(employeeSkillset =>
                new SkillsetEmployeeDetails
                {
                    EmployeeId = employeeSkillset.Key.EmployeeID,
                    EmployeeName = employeeSkillset.Key.EmployeeName,
                    FormattedEmployeeId = employeeSkillset.Key.FormattedEmployeeId,
                    EmployeeTypeName = dbContext.EmployeesType.Where(x => x.EmployeesTypeId == employeeSkillset.Key.EmployeeTypeId).Select(x => x.EmployeesType).FirstOrDefault(),
                    Skillset = employeeSkillset.Select(x => x.skillSet.Skillset).ToList(),
                    ProfilePic = employeeSkillset.Key.ProfilePicture
                }).ToList();
            }
            
            return employee;
        }

        public List<SkillsetCategory> GetSkillsetCategoryNames()
        {
            return dbContext.SkillsetCategory.ToList();
        }

        public List<SkillsetCategoryName> GetSkillsetNames()
        {

            List<SkillsetCategoryName> skillset = new();
            skillset= dbContext.SkillsetCategory
                .Join(dbContext.Skillset, skillCat=>skillCat.SkillsetCategoryId, skill=>skill.SkillsetCategoryId, (skillCat, skill)=>new { skillCat, skill } ).ToList()
                .GroupBy(x=>x.skillCat.SkillsetCategoryId).Select(a=>
                new SkillsetCategoryName
                {
                    SkillsetCategoryId = a.Key,
                    SkillsetCategory = a.Select(x => x.skill).ToList()
                }
                ).OrderBy(x => x.SkillsetCategoryId).ToList();
            return skillset;
        }
        public string GetSkillsetCategoryNameById(int? categoryId)
        {
            return dbContext.SkillsetCategory.Where(x=>x.SkillsetCategoryId==categoryId).Select(x=>x.SkillsetCategoryName).FirstOrDefault();
        }
        public List<EmployeeDetails> GetEmployeeListBySkillsetIdForDownload(EmployeeSkillsetCategoryInput skillsetInput)
        {
            List<string> condition = new List<string>();
            condition = skillsetInput?.SkillsetId?.Select(x => "skillSet.SkillsetId ==" + x).ToList();
            string whereCondition = string.Join(" || ", condition);
            List<EmployeeDetails> employee = new();
            if (skillsetInput?.Condition == "and")
            {
                List<SkillsetEmployeeDetails> data = dbContext.EmployeesSkillset
                .Join(dbContext.Employees, empSkill => empSkill.EmployeeId, emp => emp.EmployeeID, (empSkill, emp) => new { empSkill, emp })
                .Join(dbContext.Skillset, empSkillSet => empSkillSet.empSkill.SkillsetId, skillSet => skillSet.SkillsetId, (empSkillSet, skillSet) =>
                      new { empSkillSet, skillSet }).Where(whereCondition != "" && whereCondition != null ? whereCondition : "skillSet.SkillsetId==skillSet.SkillsetId").ToList()
                .GroupBy(x => new
                {
                    x.empSkillSet.emp.EmployeeID,
                    x.empSkillSet.emp.EmployeeName,
                    x.empSkillSet.emp.FormattedEmployeeId,
                    x.empSkillSet.emp.EmployeeTypeId
                }).Where(x => x.Count() == skillsetInput?.SkillsetId?.Count)
                .Select(employeeSkillset =>
                new SkillsetEmployeeDetails
                {
                    EmployeeId = employeeSkillset.Key.EmployeeID,
                    EmployeeName = employeeSkillset.Key.EmployeeName,
                    FormattedEmployeeId = employeeSkillset.Key.FormattedEmployeeId,
                    EmailAddress= employeeSkillset.Select(x=>x.empSkillSet.emp.EmailAddress).FirstOrDefault(),
                    EmployeeTypeName = dbContext.EmployeesType.Where(x => x.EmployeesTypeId == employeeSkillset.Key.EmployeeTypeId).Select(x => x.EmployeesType).FirstOrDefault(),
                    Skillset = employeeSkillset.Select(x => x.skillSet.Skillset).ToList()
                }).ToList();
                if(data?.Count>0)
                {
                    foreach(SkillsetEmployeeDetails item in data)
                    {
                        foreach(string skill in item.Skillset)
                        {
                            EmployeeDetails emp = new EmployeeDetails
                            {
                                EmployeeId = item.EmployeeId,
                                EmployeeName = item.EmployeeName,
                                FormattedEmployeeId = item.FormattedEmployeeId,
                                EmployeeEmailId = item.EmailAddress,
                                Skillset = skill
                            };
                            employee.Add(emp);
                        }
                    }
                }
            }
            else
            {
                employee = dbContext.EmployeesSkillset
                .Join(dbContext.Employees, empSkill => empSkill.EmployeeId, emp => emp.EmployeeID, (empSkill, emp) => new { empSkill, emp })
                .Join(dbContext.Skillset, empSkillSet => empSkillSet.empSkill.SkillsetId, skillSet => skillSet.SkillsetId, (empSkillSet, skillSet) =>
                      new { empSkillSet, skillSet }).Where(whereCondition != "" && whereCondition != null ? whereCondition : "skillSet.SkillsetId==skillSet.SkillsetId").ToList()
                .Select(employeeSkillset =>
                new EmployeeDetails
                {
                    EmployeeId = employeeSkillset.empSkillSet.emp.EmployeeID,
                    EmployeeName = employeeSkillset.empSkillSet.emp.EmployeeName,
                    FormattedEmployeeId = employeeSkillset.empSkillSet.emp.FormattedEmployeeId,
                    EmployeeEmailId = employeeSkillset.empSkillSet.emp.EmailAddress,
                    Skillset = employeeSkillset.skillSet.Skillset
                }).ToList();
            }

            return employee;
        }

    }
}
