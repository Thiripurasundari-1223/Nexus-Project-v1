using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEducationDetailrepository : IBaseRepository<EducationDetail>
    {
        List<EducationDetail> GetEducationDetailByEmployeeId(int employeeId);
        EducationDetail GetEducationDetailEducationDetailId(int workHistoryId);
        List<EducationDetailView> GetEducationDetailViewByEmployeeId(int employeeId);
        EducationDetail GetEducationDetailByEducationType(int employeeId, int educationTypeId);
    }
    public class EducationDetailrepository : BaseRepository<EducationDetail>, IEducationDetailrepository
    {
        private readonly IAMDBContext dbContext;
        public EducationDetailrepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<EducationDetail> GetEducationDetailByEmployeeId(int employeeId)
        {
            return dbContext.EducationDetail.Where(x => x.EmployeeId == employeeId).ToList();
        }
        public EducationDetail GetEducationDetailEducationDetailId(int EducationDetailId)
        {
            return dbContext.EducationDetail.Where(x => x.EducationDetailId == EducationDetailId).FirstOrDefault();
        }
        public List<EducationDetailView> GetEducationDetailViewByEmployeeId(int employeeId)
        {
            return dbContext.EducationDetail.Where(x => x.EmployeeId == employeeId).Select(x => new
            EducationDetailView
            {
                EducationDetailId = x.EducationDetailId,
                EmployeeId = x.EmployeeId,
                EducationTypeId = x.EducationTypeId,
                EducationType = dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == x.EducationTypeId).Select(x => x.AppConstantValue).FirstOrDefault(),
                InstitutionName = x.InstitutionName,
                UniversityName = x.UniversityName,
                BoardId = x.BoardId,
                BoardName = dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == x.BoardId).Select(x => x.AppConstantValue).FirstOrDefault(),
                YearOfCompletion = x.YearOfCompletion,
                ExpiryYear = x.ExpiryYear,
                CertificateName = x.CertificateName,
                Specialization = x.Specialization,
                MarkPercentage = x.MarkPercentage,
                Degree = x.Degree,
                Marksheet = dbContext.EmployeeDocument.Where(y => y.SourceId == (int)x.EducationDetailId && y.DocumentType == "marksheet").Select(x => new DocumentsToUpload
                {
                    Path = x.DocumentPath,
                    DocumentId = (int)x.EmployeeDocumentId,
                    DocumentName = x.DocumentName,
                    DocumentCategory = x.DocumentType,
                    CreatedOn = x.CreatedOn,
                    EmployeeId = (int)x.EmployeeID
                }).FirstOrDefault(),
                Certificate = dbContext.EmployeeDocument.Where(y => y.SourceId == (int)x.EducationDetailId && y.DocumentType == "certificate").Select(x => new DocumentsToUpload
                {
                    Path = x.DocumentPath,
                    DocumentId = (int)x.EmployeeDocumentId,
                    DocumentName = x.DocumentName,
                    DocumentCategory = x.DocumentType,
                    CreatedOn = x.CreatedOn,
                    EmployeeId = (int)x.EmployeeID
                }).FirstOrDefault(),
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn
            }
        ).OrderBy(x => x.EducationDetailId).ToList();
        }

        public EducationDetail GetEducationDetailByEducationType(int employeeId, int educationTypeId)
        {
            return dbContext.EducationDetail.Where(x => x.EmployeeId == employeeId && x.EducationTypeId == educationTypeId).FirstOrDefault();
        }
    }
}
