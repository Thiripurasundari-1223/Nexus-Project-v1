using PolicyManagement.DAL.DBContext;
using PolicyManagement.DAL.Models;

namespace PolicyManagement.DAL.Repository
{
    public interface IRequestedDocumentRepository : IBaseRepository<RequestedDocuments>
    { }

    public class RequestedDocumentRepository : BaseRepository<RequestedDocuments>, IRequestedDocumentRepository
    {
        public RequestedDocumentRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
        }
    }
    public interface IDocumentTypesRepository : IBaseRepository<DocumentTypes>
    { }

    public class DocumentTypesRepository : BaseRepository<DocumentTypes>, IDocumentTypesRepository
    {
        public DocumentTypesRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
        }
    }
    public interface IDocumentTagRepository : IBaseRepository<DocumentTag>
    { }

    public class DocumentTagRepository : BaseRepository<DocumentTag>, IDocumentTagRepository
    {
        public DocumentTagRepository(PolicyMgmtDBContext dbContext) : base(dbContext)
        {
        }
    }
}