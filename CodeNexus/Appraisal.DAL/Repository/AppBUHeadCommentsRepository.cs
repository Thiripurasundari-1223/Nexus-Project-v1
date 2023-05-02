using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{
    public interface IAppBUHeadCommentsRepository : IBaseRepository<AppraisalBUHeadComments>
    {
        AppraisalBUHeadComments GetByID(int AppraisalBUHeadCommentsId);
        List<AppraisalBUHeadCommentsView> GetAllComments(int departmentId);
    }
    public class AppBUHeadCommentsRepository : BaseRepository<AppraisalBUHeadComments>, IAppBUHeadCommentsRepository
    {
        private readonly AppraisalDBContext dbContext;
        public AppBUHeadCommentsRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public AppraisalBUHeadComments GetByID(int AppraisalBUHeadCommentsId)
        {
            return dbContext.AppraisalBUHeadComments.Where(x => x.AppraisalBUHeadCommentsId == AppraisalBUHeadCommentsId).FirstOrDefault();
        }
        public List<AppraisalBUHeadCommentsView> GetAllComments(int departmentId)
        {
            List<AppraisalBUHeadCommentsView> appraisalBUHeadComments = new List<AppraisalBUHeadCommentsView>();
            appraisalBUHeadComments = (from master in dbContext.AppraisalMaster
                           join comm in dbContext.AppraisalBUHeadComments on master.APP_CYCLE_ID equals comm.AppCycle_Id
                           where master.APPRAISAL_STATUS == 1 && comm.Department_Id == departmentId
                                       select new AppraisalBUHeadCommentsView
                           {
                               AppraisalBUHeadCommentsId = comm.AppraisalBUHeadCommentsId,
                               AppCycle_Id = comm.AppCycle_Id,
                               Department_Id = comm.Department_Id,
                               Employee_Id = comm.Employee_Id,
                               Comment = comm.Comment,
                               Created_By = comm.Created_By,
                               Created_On = comm.Created_On,
                               Employee_Name=""
                               
                           }).ToList();
            return appraisalBUHeadComments;
        }
    }
}
