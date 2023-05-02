using Appraisal.DAL.DBContext; 
using SharedLibraries.Models.Appraisal;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Appraisal.DAL.Repository
{

    public interface IEmployeeKeyResultAttachmentRepository : IBaseRepository<EmployeeKeyResultAttachments>
    {
        EmployeeKeyResultAttachments GetByID(int DocID);

        public List<EmployeeKeyResultAttachments> GetDocumentByObjAndKraId(AppraisalSourceDocuments sourceDocuments);
        public List<EmployeeKeyResultAttachments> GetDocumentByKraId(int APP_CYCLE_ID, int EMPLOYEE_ID, int OBJECTIVE_ID, int KEY_RESULT_ID);

    }
    public class EmployeeKeyResultAttachmentRepository : BaseRepository<EmployeeKeyResultAttachments>, IEmployeeKeyResultAttachmentRepository
    {
        private readonly AppraisalDBContext  dbContext;
        public  EmployeeKeyResultAttachmentRepository(AppraisalDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeeKeyResultAttachments GetByID(int DocID)
        {
            return dbContext.EmployeeKeyResultAttachments.Where(x => x.DOC_ID == DocID).FirstOrDefault();
        }
        public List<EmployeeKeyResultAttachments> GetDocumentByObjAndKraId(AppraisalSourceDocuments sourceDocuments)
        {
            return dbContext.EmployeeKeyResultAttachments.Where(x => x.OBJECTIVE_ID == sourceDocuments.ObjectiveId && x.KEY_RESULT_ID == sourceDocuments.KraId && x.EMPLOYEE_ID == sourceDocuments.employeeId && x.APP_CYCLE_ID == sourceDocuments.appcycleId).ToList();
        }
        public List<EmployeeKeyResultAttachments> GetDocumentByKraId (int APP_CYCLE_ID, int EMPLOYEE_ID, int OBJECTIVE_ID, int KEY_RESULT_ID)
        {
            return dbContext.EmployeeKeyResultAttachments.Where(x => x.OBJECTIVE_ID == OBJECTIVE_ID && x.EMPLOYEE_ID == EMPLOYEE_ID && x.APP_CYCLE_ID == APP_CYCLE_ID && x.KEY_RESULT_ID == KEY_RESULT_ID).ToList();
        }
    }

    
}
