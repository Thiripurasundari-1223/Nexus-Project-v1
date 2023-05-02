using ExitManagement.DAL.DBContext;
using SharedLibraries.Models.ExitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitManagement.DAL.Repository
{
    public interface IResignationFeedbackToManagementRepository : IBaseRepository<ResignationFeedbackToManagement>
    {
        ResignationFeedbackToManagement GetByResignationInterviewFeedbackByID(int EmployeeResignationInterviewId);
    }
    public class ResignationFeedbackToManagementRepository : BaseRepository<ResignationFeedbackToManagement>, IResignationFeedbackToManagementRepository
    {
        private readonly ExitManagementDBContext dbContext;
        public ResignationFeedbackToManagementRepository(ExitManagementDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public ResignationFeedbackToManagement GetByResignationInterviewFeedbackByID(int EmployeeResignationInterviewId)
        {
            return dbContext.ResignationFeedbackToManagement.Where(x => x.ResignationInterviewId == EmployeeResignationInterviewId).FirstOrDefault();
        }
    }
}
