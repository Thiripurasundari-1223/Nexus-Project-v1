using CustomerOnBoarding.DAL.DBContext;
using CustomerOnBoarding.DAL.Repository;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnBoarding.DAL.Repository
{

    public interface IAppConstantsRepository : IBaseRepository<AppConstants>
    {
        AppConstants GetAppconstantByID(int? appConstantID);
        List<KeyWithValue> GetAppConstantByType(string appConstantType);
        int GetAppconstantIdByValue(string appConstantType, string appConstantValue);
        List<StringKeyWithValue> GetAccountStatusList();
    }


    public class AppConstantsRepository : BaseRepository<AppConstants>, IAppConstantsRepository
    {
        private readonly COBDBContext dbContext;
        public AppConstantsRepository(COBDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public AppConstants GetAppconstantByID(int? appConstantID)
        {
            return dbContext.AppConstants.Where(x => x.AppConstantId == appConstantID).FirstOrDefault();

        }
        public List<KeyWithValue> GetAppConstantByType(string appConstantType)
        {
            return dbContext.AppConstants.Where(x => x.AppConstantType.ToLower() == appConstantType.ToLower()).Select(x => new KeyWithValue { Key=x.AppConstantId, Value=x.DisplayName }).ToList();

        }
        public List<StringKeyWithValue> GetAccountStatusList()
        {
            return dbContext.AppConstants.Where(x => x.AppConstantType == "CustomerStatus").Select(x => new StringKeyWithValue { Key = x.AppConstantValue, Value = x.DisplayName }).ToList();

        }
        public int GetAppconstantIdByValue(string appConstantType, string appConstantValue)
        {
            return dbContext.AppConstants.Where(x => x.AppConstantType.ToLower() == appConstantType.ToLower() && x.AppConstantValue.ToLower() == appConstantValue.ToLower()).Select(x => x.AppConstantId).FirstOrDefault();

        }
        
    }
}
