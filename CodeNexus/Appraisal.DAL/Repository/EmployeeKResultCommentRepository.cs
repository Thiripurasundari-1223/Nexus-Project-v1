using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharedLibraries.ViewModels.Appraisal;

namespace Appraisal.DAL.Repository
{
    public interface IEmployeeKResultCommentRepository : IBaseRepository<EmployeeKeyResultConversation>
    {
        public EmployeeKeyResultConversation GetKeyResultComment(int commentId);
        public List<KraComments> GetKRACommentsById(int appcycleId, int employeeId, int ObjId, int KraId);
        public List<EmployeeKeyResultConversation> GetKRACommentsByKRAId(int APP_CYCLE_ID, int EMPLOYEE_ID, int OBJECTIVE_ID, int KEY_RESULT_ID);
    }
    public class EmployeeKResultCommentRepository : BaseRepository<EmployeeKeyResultConversation>, IEmployeeKResultCommentRepository
    {
        private readonly AppraisalDBContext dbContext;
        public EmployeeKResultCommentRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeeKeyResultConversation GetKeyResultComment(int commentId)
        {
            return dbContext.EmployeeKeyResultConversation.Where(x => x.COMMENT_ID == commentId).FirstOrDefault();
        }
        public List<KraComments> GetKRACommentsById(int appcycleId, int employeeId , int ObjId, int KraId)
        {
            List<KraComments> KRAsViews = new List<KraComments>();
            {
                 KRAsViews = (from comment in dbContext.EmployeeKeyResultConversation
                                                        where comment.APP_CYCLE_ID == appcycleId && comment.EMPLOYEE_ID == employeeId && comment.OBJECTIVE_ID == ObjId
                                                        && comment.OBJECTIVE_ID== ObjId && comment.KEY_RESULT_ID==KraId && comment.COMMENT != null
                                                        select new KraComments
                                                        {
                                                            App_Cycle_Id = comment.APP_CYCLE_ID,
                                                            Objective_Id = comment.OBJECTIVE_ID,
                                                            Key_Result_Id = comment.KEY_RESULT_ID,
                                                            Comment_Id = comment.COMMENT_ID,
                                                            Comment = comment.COMMENT,
                                                            CreatedBy=comment.CREATED_BY,
                                                            CommentDateTime = (DateTime)comment.CREATED_DATE
                                                        }).ToList();
            }
            return KRAsViews;
        }
        public List<EmployeeKeyResultConversation> GetKRACommentsByKRAId(int APP_CYCLE_ID, int EMPLOYEE_ID, int OBJECTIVE_ID, int KEY_RESULT_ID)
        {
            return dbContext.EmployeeKeyResultConversation.Where(x => x.APP_CYCLE_ID == APP_CYCLE_ID && x.EMPLOYEE_ID == EMPLOYEE_ID && x.OBJECTIVE_ID == OBJECTIVE_ID && x.KEY_RESULT_ID == KEY_RESULT_ID).ToList();
        }
    }
}
