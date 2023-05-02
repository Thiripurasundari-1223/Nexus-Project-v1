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

    public interface IEmployeeAppConstantRepository : IBaseRepository<EmployeeAppConstants>
    {
        EmployeeAppConstants GetAppconstantByID(int? appConstantID);
        List<KeyValue> GetAppConstantByType(string appConstantType);
        int GetAppconstantIdByValue(string appConstantType, string appConstantValue);
        string GetAppconstantValueById(int? appConstantID);
         Task<EmployeeMasterEmailTemplate> GetEmployeeEmailByName(string templateName);
    }


    public class EmployeeAppConstantRepository : BaseRepository<EmployeeAppConstants>, IEmployeeAppConstantRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeAppConstantRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeeAppConstants GetAppconstantByID(int? appConstantID)
        {
            return dbContext.EmployeeAppConstants.Where(x => x.AppConstantId == appConstantID).FirstOrDefault();

        }
        public List<KeyValue> GetAppConstantByType(string appConstantType)
        {
            return dbContext.EmployeeAppConstants.Where(x => x.AppConstantType.ToLower() == appConstantType.ToLower()).Select(x => new KeyValue { Key = x.AppConstantId, Value = x.AppConstantValue,DisplayName=x.DisplayName }).ToList();

        }
        public int GetAppconstantIdByValue(string appConstantType, string appConstantValue)
        {
            return dbContext.EmployeeAppConstants.Where(x => x.AppConstantType.ToLower() == appConstantType.ToLower() && x.AppConstantValue.ToLower() == appConstantValue.ToLower()).Select(x => x.AppConstantId).FirstOrDefault();

        }
        public string GetAppconstantValueById(int? appConstantID)
        {
            return dbContext.EmployeeAppConstants.Where(x => x.AppConstantId ==
            appConstantID).Select(x => x.AppConstantValue).FirstOrDefault();

        }

        public async Task<EmployeeMasterEmailTemplate> GetEmployeeEmailByName(string templateName)
        {
            EmployeeMasterEmailTemplate res = (from templatedata in dbContext.EmployeeMasterEmailTemplate
                                               where templatedata.TemplateName == templateName
                                               select new EmployeeMasterEmailTemplate()
                                               {
                                                   TemplateId = templatedata.TemplateId,
                                                   TemplateName = templatedata.TemplateName,
                                                   Body = templatedata.Body,
                                                   Subject = templatedata.Subject,

                                               }).FirstOrDefault();
            return res;
        }

    }

}
