using Appraisal.DAL.DBContext;
using SharedLibraries.Models.Appraisal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SharedLibraries.ViewModels.Appraisal;

namespace Appraisal.DAL.Repository
{
    public interface IEmployeeAppraisalCommentRepository : IBaseRepository<EmployeeAppraisalConversation>
    {
        public EmployeeAppraisalConversation GetAppraisalCommentById(int CommentId);
        public List<IndividualAppraisalCommentsView> GetAppraisalCommentsById(int appcycleId, int employeeId);

    }
    public class EmployeeAppraisalCommentRepository : BaseRepository<EmployeeAppraisalConversation>, IEmployeeAppraisalCommentRepository
    {
        private readonly AppraisalDBContext dbContext;
        public EmployeeAppraisalCommentRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public EmployeeAppraisalConversation GetAppraisalCommentById(int CommentId)
        {
            return dbContext.EmployeeAppraisalConversation.Where(x => x.COMMENT_ID == CommentId).FirstOrDefault();
        }
        public List<IndividualAppraisalCommentsView> GetAppraisalCommentsById(int appcycleId, int employeeId)
        {
            List<IndividualAppraisalCommentsView> commentViews =new List<IndividualAppraisalCommentsView>();
            {
                commentViews = (from comment in dbContext.EmployeeAppraisalConversation
                                                        where comment.APP_CYCLE_ID == appcycleId && comment.EMPLOYEE_ID == employeeId
                                                        select new IndividualAppraisalCommentsView
                                                        {
                                                            App_Cycle_Id = appcycleId,
                                                            Employee_Id = employeeId,
                                                            CommentId = comment.COMMENT_ID,
                                                            Comment = comment.COMMENT,
                                                            CreatedBy = comment.CREATED_BY,
                                                            Comment_DateTime = (DateTime)comment.CREATED_DATE,
                                                        }).ToList();
            }
            return commentViews;
        }
    }
}
