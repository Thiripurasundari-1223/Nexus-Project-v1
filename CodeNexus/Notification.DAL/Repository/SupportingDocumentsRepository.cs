using Notification.DAL.DBContext;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notifications.DAL.Repository
{
    public interface ISupportingDocumentsRepository : IBaseRepository<SupportingDocuments>
    {
        SupportingDocuments GetByID(int pDocumentId);
        List<SupportingDocuments> GetDocumentBySourceIdAndType(SourceDocuments sourceDocuments);
    }
    public class SupportingDocumentsRepository : BaseRepository<SupportingDocuments>, ISupportingDocumentsRepository
    {
        private readonly NotificationsDBContext dbContext;
        public SupportingDocumentsRepository(NotificationsDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        
        public SupportingDocuments GetByID(int pDocumentId)
        {
            return dbContext.SupportingDocuments.Where(x => x.DocumentId == pDocumentId).FirstOrDefault();
        }
        public List<SupportingDocuments> GetDocumentBySourceIdAndType(SourceDocuments sourceDocuments)
        {
            return dbContext.SupportingDocuments.Where(x => sourceDocuments.SourceId.Contains(x.SourceId==null?0: (int)x.SourceId) && sourceDocuments.SourceType.Contains(x.SourceType)).ToList();
        }
    }
}
