using ExitManagement.DAL.DBContext;
using SharedLibraries.Common;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{

    public interface IAppConstantsRepository : IBaseRepository<AppConstants>
    {
        AppConstants GetAppconstantByID(int? appConstantID);
        List<KeyWithValue> GetAppConstantByType(string appConstantType);
        int GetAppconstantIdByValue(string appConstantType, string appConstantValue);
    }


    public class AppConstantsRepository : BaseRepository<AppConstants>, IAppConstantsRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public AppConstantsRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public AppConstants GetAppconstantByID(int? appConstantID)
        {
            return dbContext.AppConstants.Where(x => x.AppConstantId == appConstantID).FirstOrDefault();

        }
        public List<KeyWithValue> GetAppConstantByType(string appConstantType)
        {
            return dbContext.AppConstants.Where(x => x.AppConstantType.ToLower() == appConstantType.ToLower()).Select(x => new KeyWithValue { Key=x.AppConstantId, Value=x.DisplayName }).ToList();

        }
        public int GetAppconstantIdByValue(string appConstantType, string appConstantValue)
        {
            return dbContext.AppConstants.Where(x => x.AppConstantType.ToLower() == appConstantType.ToLower() && x.AppConstantValue.ToLower() == appConstantValue.ToLower()).Select(x => x.AppConstantId).FirstOrDefault();

        }
    }
}
