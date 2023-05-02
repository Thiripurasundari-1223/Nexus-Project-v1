using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAM.DAL.DBContext;
using IAM.DAL.Models;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;

namespace IAM.DAL.Repository
{
    public interface IWorkHistoryRepository : IBaseRepository<WorkHistory>
    {
        List<WorkHistory> GetWorkHistoryByEmployeeId(int employeeId);
        WorkHistory GetWorkHistoryWorkHistoryId(int workHistoryId);
        List<WorkHistoryView> GetWorkHistoryViewByEmployeeId(int employeeId);
        WorkHistory GetWorkHistoryWorkHistoryByOrganizationName(int employeeId, string organizationName);
    }
    public class WorkHistoryRepository : BaseRepository<WorkHistory>, IWorkHistoryRepository
    {
        private readonly IAMDBContext dbContext;
        public WorkHistoryRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }

        public List<WorkHistory> GetWorkHistoryByEmployeeId(int employeeId)
        {
            return dbContext.WorkHistory.Where(x => x.EmployeeId == employeeId).ToList();
        }
        public WorkHistory GetWorkHistoryWorkHistoryId(int workHistoryId)
        {
            return dbContext.WorkHistory.Where(x => x.WorkHistoryId == workHistoryId).FirstOrDefault();
        }
        public List<WorkHistoryView> GetWorkHistoryViewByEmployeeId(int employeeId)
        {
            return dbContext.WorkHistory.Where(x => x.EmployeeId == employeeId).Select(x => new
            WorkHistoryView
            {
                WorkHistoryId = x.WorkHistoryId,
                EmployeeId = x.EmployeeId,
                Designation = x.Designation,
                OrganizationName = x.OrganizationName,
                EmployeeTypeId = x.EmployeeTypeId,
                EmployeeType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(x => x.EmployeesType).FirstOrDefault(),
                LeavingReason = x.LeavingReason,
                StartDate = x.StartDate,
                ReleivingDate = x.EndDate,
                LastCTC = x.LastCTC,
                ModifiedByName = dbContext.Employees.Where(y => y.EmployeeID == x.ModifiedBy).Select(y => y.EmployeeName + " - " + y.FormattedEmployeeId).FirstOrDefault(),
                ModifiedOn = x.ModifiedOn,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                serviceLetter = dbContext.EmployeeDocument.Where(y => y.SourceId == (int)x.WorkHistoryId && y.DocumentType == "serviceLetter").Select(x => new DocumentsToUpload
                {
                    Path = x.DocumentPath,
                    DocumentId = (int)x.EmployeeDocumentId,
                    DocumentName = x.DocumentName,
                    DocumentCategory = x.DocumentType,
                    CreatedOn = x.CreatedOn,
                    EmployeeId = (int)x.EmployeeID
                }).FirstOrDefault(),
                paySlip = dbContext.EmployeeDocument.Where(y => y.SourceId == (int)x.WorkHistoryId && y.DocumentType == "paySlip").Select(x => new DocumentsToUpload
                {
                    Path = x.DocumentPath,
                    DocumentId = (int)x.EmployeeDocumentId,
                    DocumentName = x.DocumentName,
                    DocumentCategory = x.DocumentType,
                    CreatedOn = x.CreatedOn,
                    EmployeeId = (int)x.EmployeeID
                }).FirstOrDefault(),
                OfferLetter = dbContext.EmployeeDocument.Where(y => y.SourceId == (int)x.WorkHistoryId && y.DocumentType == "offerLetter").Select(x => new DocumentsToUpload
                {
                    Path = x.DocumentPath,
                    DocumentId = (int)x.EmployeeDocumentId,
                    DocumentName = x.DocumentName,
                    DocumentCategory = x.DocumentType,
                    CreatedOn = x.CreatedOn,
                    EmployeeId = (int)x.EmployeeID
                }).FirstOrDefault(),
            }
            ).OrderByDescending(x => x.StartDate).ToList();
        }

        public WorkHistory GetWorkHistoryWorkHistoryByOrganizationName(int employeeId, string organizationName)
        {
            return dbContext.WorkHistory.Where(x => x.EmployeeId == employeeId && x.OrganizationName == organizationName).FirstOrDefault();
        }
    }
}
