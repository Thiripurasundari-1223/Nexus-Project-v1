using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Leaves.DAL.Repository
{
    public interface IAppConstantsRepository : IBaseRepository<AppConstants>
    {
        AppConstants GetAppconstantByID(int? appConstantID);
        int GetAppconstantIdByValue(string appConstantType, string appConstantValue);
        List<AppConstants> GetAppconstantsList();
    }


    public class AppConstantsRepository:BaseRepository<AppConstants>, IAppConstantsRepository
    {
        private readonly LeaveDBContext dbContext;
        public AppConstantsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public AppConstants GetAppconstantByID(int? appConstantID)
        {
            return  dbContext.AppConstants.Where(x => x.AppConstantId == appConstantID).FirstOrDefault();
             
        }
        public int GetAppconstantIdByValue(string appConstantType,string appConstantValue)
        {
            return dbContext.AppConstants.Where(x => x.AppConstantType.ToLower() == appConstantType.ToLower() && x.AppConstantValue.ToLower()==appConstantValue.ToLower()).Select(x=>x.AppConstantId).FirstOrDefault();

        }
        public List<AppConstants> GetAppconstantsList()
        {
            return dbContext.AppConstants.Select(x => x).ToList();
        }

    }
}
