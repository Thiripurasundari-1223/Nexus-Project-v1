using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeDocumentRepository : IBaseRepository<EmployeeDocument>
    {
        Task<List<EmployeeDocument>> GetEmployeeDocumentDetailBySourceId(int sourceId);
        EmployeeDocument GetEmployeeDocumentDetailById(int documentId);
        List<DocumentsToUpload> GetDocumentsDetail(int sourceId);
        EmployeeDocument GetDocumentDetailBySourceIdAndType(int sourceId, string DocumentType);
        Task<List<EmployeeDocument>> GetEmployeeDocumentDetailBySourceIdAndDocType(int sourceId, string documentType);
        DocumentsToUpload GetAddressProof(int sourceId, string pdocumentType);
        Task<List<EmployeeDocument>> GetEmployeeDocumentDetailBySourceIdSourceType(int sourceId, string sourceType);
    }
    public class EmployeeDocumentRepository : BaseRepository<EmployeeDocument>, IEmployeeDocumentRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeDocumentRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeeDocument GetEmployeeDocumentDetailById(int documentId)
        {
            return dbContext.EmployeeDocument.Where(x => x.EmployeeDocumentId == documentId).FirstOrDefault();
        }
        public async Task<List<EmployeeDocument>> GetEmployeeDocumentDetailBySourceId(int sourceId)
        {
            return dbContext.EmployeeDocument.Where(x => x.SourceId == sourceId).ToList();
        }
        public List<DocumentsToUpload> GetDocumentsDetail(int sourceId)
        {
            return dbContext.EmployeeDocument.Where(x => x.SourceId == sourceId).Select(x =>
            new DocumentsToUpload
            {
                EmployeeId = (int)x.EmployeeID,
                CreatedOn = x.CreatedOn,
                Path = x.DocumentPath != null && File.Exists(x.DocumentPath) ? x.DocumentPath : "",
                DocumentId = (int)x.EmployeeDocumentId,
                DocumentName = x.DocumentName,
                DocumentCategory = x.DocumentType,
                DocumentAsBase64 = x.DocumentPath != null && File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)) : "",
                DocumentSize = x.DocumentPath != null && File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)).Length : 0,
            }).ToList();
        }
        public EmployeeDocument GetDocumentDetailBySourceIdAndType(int sourceId, string DocumentType)
        {
            return dbContext.EmployeeDocument.Where(x => x.SourceId == sourceId && x.DocumentType == DocumentType).FirstOrDefault();

        }
        public DocumentsToUpload GetAddressProof(int employeeId, string pdocumentType)
        {
            return dbContext.EmployeeDocument.Where(x => x.EmployeeID == employeeId && x.DocumentType == pdocumentType).Select(x =>
                 new DocumentsToUpload
                 {
                     EmployeeId = (int)x.EmployeeID,
                     CreatedOn = x.CreatedOn,
                     Path = x.DocumentPath != null && File.Exists(x.DocumentPath) ? x.DocumentPath : "",
                     DocumentId = (int)x.EmployeeDocumentId,
                     DocumentName = x.DocumentName,
                     DocumentCategory = x.DocumentType,
                     DocumentAsBase64 = x.DocumentPath != null && File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)) : "",
                     DocumentSize = x.DocumentPath != null && File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)).Length : 0,
                 }).FirstOrDefault();
        }

        public async Task<List<EmployeeDocument>> GetEmployeeDocumentDetailBySourceIdAndDocType(int sourceId, string documentType)
        {
            return dbContext.EmployeeDocument.Where(x => x.SourceId == sourceId && x.DocumentType == documentType).ToList();
        }
        public async Task<List<EmployeeDocument>> GetEmployeeDocumentDetailBySourceIdSourceType(int sourceId, string sourceType)
        {
            return dbContext.EmployeeDocument.Where(x => x.SourceId == sourceId && x.SourceType == sourceType).ToList();
        }
    }
}