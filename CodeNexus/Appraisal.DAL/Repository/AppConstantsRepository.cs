using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.Common;

namespace Appraisal.DAL.Repository
{
    public interface IAppConstantsRepository : IBaseRepository<AppConstants>
    {
        BenchmarkMasterDataView GetAppConstants();
        int GetAppraisalStatusId(string status);
        string GetAppraisalStatusName(int? appConstantId);
    }
    public class AppConstantsRepository : BaseRepository<AppConstants>, IAppConstantsRepository
    {
        private readonly AppraisalDBContext dbContext;
        public AppConstantsRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public BenchmarkMasterDataView GetAppConstants()
        {
            BenchmarkMasterDataView appConstant = new BenchmarkMasterDataView()
            {
                Durations = (from apptype in dbContext.AppConstantType
                             join appconstant in dbContext.AppConstants on apptype.APP_CONSTANT_TYPE_ID equals appconstant.APP_CONSTANT_TYPE_ID
                             where apptype.APP_CONSTANT_TYPE_DESC == "Duration"
                             select new KeyWithValue { Key = appconstant.APP_CONSTANT_ID, Value = appconstant.APP_CONSTANT_TYPE_VALUE }).ToList(),
                Types = (from apptype in dbContext.AppConstantType
                         join appconstant in dbContext.AppConstants on apptype.APP_CONSTANT_TYPE_ID equals appconstant.APP_CONSTANT_TYPE_ID
                         where apptype.APP_CONSTANT_TYPE_DESC == "Type"
                         select new KeyWithValue { Key = appconstant.APP_CONSTANT_ID, Value = appconstant.APP_CONSTANT_TYPE_VALUE }).ToList(),
                Operators = (from apptype in dbContext.AppConstantType
                             join appconstant in dbContext.AppConstants on apptype.APP_CONSTANT_TYPE_ID equals appconstant.APP_CONSTANT_TYPE_ID
                             where apptype.APP_CONSTANT_TYPE_DESC == "Operator"
                             select new KeyWithValue { Key = appconstant.APP_CONSTANT_ID, Value = appconstant.APP_CONSTANT_TYPE_VALUE }).ToList(),
                UITypes = (from apptype in dbContext.AppConstantType
                         join appconstant in dbContext.AppConstants on apptype.APP_CONSTANT_TYPE_ID equals appconstant.APP_CONSTANT_TYPE_ID
                         where apptype.APP_CONSTANT_TYPE_DESC == "UI Type"
                         select new KeyWithValue { Key = appconstant.APP_CONSTANT_ID, Value = appconstant.APP_CONSTANT_TYPE_VALUE }).ToList(),
                Smiley = (from apptype in dbContext.AppConstantType
                           join appconstant in dbContext.AppConstants on apptype.APP_CONSTANT_TYPE_ID equals appconstant.APP_CONSTANT_TYPE_ID
                           where apptype.APP_CONSTANT_TYPE_DESC == "Smiley"
                          select new KeyWithValue { Key = appconstant.APP_CONSTANT_ID, Value = appconstant.APP_CONSTANT_TYPE_VALUE }).ToList()
            };
            return appConstant==null?new BenchmarkMasterDataView(): appConstant;


        }
        public int GetAppraisalStatusId(string status)
        {
            return status==null?0: dbContext.AppConstants.Where(x => x.APP_CONSTANT_TYPE_VALUE.ToLower() == status.ToLower()).Select(x => x.APP_CONSTANT_ID).FirstOrDefault();
        }
        public string GetAppraisalStatusName(int? appConstantId)
        {
            return dbContext.AppConstants.Where(x => x.APP_CONSTANT_ID == appConstantId).Select(x => x.APP_CONSTANT_TYPE_VALUE).FirstOrDefault();
        }
    }

}
