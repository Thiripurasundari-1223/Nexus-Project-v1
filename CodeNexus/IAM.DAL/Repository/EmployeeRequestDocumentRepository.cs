using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeRequestDocumentRepository : IBaseRepository<EmployeeRequestDocument>
    {
        EmployeeRequestDocument GetEmployeeProofDocumentByCRId(Guid CRId);
        DocumentsToUpload GetDocumentByDocumentId(int documentId);
    }
    public class EmployeeRequestDocumentRepository: BaseRepository<EmployeeRequestDocument>, IEmployeeRequestDocumentRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeRequestDocumentRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public EmployeeRequestDocument GetEmployeeProofDocumentByCRId(Guid CRId)
        {
            return dbContext.EmployeeRequestDocument.Where(x => x.ChangeRequestId == CRId).FirstOrDefault();
        }

        public DocumentsToUpload GetDocumentByDocumentId(int documentId)
        {
            return dbContext.EmployeeRequestDocument.Where(x => x.EmployeeRequestDocumentId == documentId).Select(x => new DocumentsToUpload
            {
                Path = x.DocumentPath,
                DocumentId = (int)x.EmployeeRequestDocumentId,
                DocumentName = x.DocumentName,
                DocumentCategory = x.DocumentType,
                DocumentAsBase64 = Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)),
                DocumentSize = File.ReadAllBytes(x.DocumentPath).Length
            }).FirstOrDefault();
        }
    }
}
