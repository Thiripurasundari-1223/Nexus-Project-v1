using Leaves.DAL.DBContext;
using SharedLibraries.Models.Leaves;
using SharedLibraries.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaves.DAL.Repository
{
    public interface ILeaveGrantDocumentDetailsRepository : IBaseRepository<LeaveGrantDocumentDetails>
    {
        List<LeaveGrantDocumentDetails> GetByID(int leaveGrantDetailId);
        LeaveGrantDocumentDetails GetLeaveGrantDocument(int leaveGrantDocumentDetailId);
        SupportingDocuments DownloadLeaveGrantDocumentById(int documentId);
    }
    public class LeaveGrantDocumentDetailsRepository : BaseRepository<LeaveGrantDocumentDetails>, ILeaveGrantDocumentDetailsRepository
    {
        private readonly LeaveDBContext dbContext;
        public LeaveGrantDocumentDetailsRepository(LeaveDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public List<LeaveGrantDocumentDetails> GetByID(int leaveGrantDetailId)
        {
            if (leaveGrantDetailId > 0)
            {
                return dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDetailId == leaveGrantDetailId).ToList();
            }
            return null;
        }
        public LeaveGrantDocumentDetails GetLeaveGrantDocument(int leaveGrantDocumentDetailId)
        {
            if(leaveGrantDocumentDetailId>0)
            {
                return dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDocumentDetailId == leaveGrantDocumentDetailId).FirstOrDefault();
            }
            return null;
        }
        public SupportingDocuments DownloadLeaveGrantDocumentById(int documentId)
        {
            return dbContext.LeaveGrantDocumentDetails.Where(x => x.LeaveGrantDocumentDetailId == documentId).Select(x=>
            new SupportingDocuments { DocumentId =x.LeaveGrantDocumentDetailId, DocumentPath =x.DocumentPath,
            DocumentName=x.DocumentName, DocumentType=x.DocumentType,CreatedBy=x.CreatedBy}).FirstOrDefault();
        }
    }
}
