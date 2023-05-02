using IAM.DAL.DBContext;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeRequestRepository : IBaseRepository<EmployeeRequest>
    {
        Task<EmployeeRequest> GetEmployeeRequestById(int employeeRequestId);
        List<EmployeeRequestListView> GetEmployeeRequest(int employeeID);
        EmployeesRequestList GetEmployeeApproval(int employeeID);
        List<EmployeeRequestDetailsView> GetPendingRequestByEmployeeId(int employeeId);
        List<EmployeeRequestDetail> GetPendingRequestByEmployeeIdForFieldCheck(int employeeId);
        List<EmployeeDetailListView> GetMyApprovalEmployeeList(PaginationView pagination);
        int GetMyApprovalEmployeeCount(PaginationView pagination);
        List<EmployeeRequestListView> GetPendingRequestByEmployeeIdAndRequestCat(int employeeId, string requestCategory);
    }
    public class EmployeeRequestRepository : BaseRepository<EmployeeRequest>, IEmployeeRequestRepository
    {
        private readonly IAMDBContext dbContext;
        public EmployeeRequestRepository(IAMDBContext dbContext) : base(dbContext) { this.dbContext = dbContext; }
        public async Task<EmployeeRequest> GetEmployeeRequestById(int employeeRequestId)
        {
            return dbContext.EmployeeRequest.Where(x => x.EmployeeRequestId == employeeRequestId).FirstOrDefault();
        }
        public List<EmployeeRequestListView> GetEmployeeRequest(int employeeID)
        {
            List<EmployeeRequestListView> data =  dbContext.EmployeeRequest
                .Join(dbContext.EmployeeRequestDetails, r => r.ChangeRequestId, rd => rd.ChangeRequestId, (r, rd) => new { r, rd }).Where(r => r.r.EmployeeId == employeeID).ToList()
                .GroupBy(r => r.r.RequestCategory)?.Select(x => new EmployeeRequestListView
                {
                    RequestCategory = x.Key,
                    EmployeeRequestlst = x?.GroupBy(x => x.r.ChangeRequestId).Select(y => new EmployeeRequestView
                    {
                        EmployeeRequestId = y.Select(z => z.r.EmployeeRequestId).FirstOrDefault(),
                        EmployeeId = employeeID,
                        //ChangeRequestId = y.Select(z => z.r.ChangeRequestId).FirstOrDefault(),
                        Status = y.Select(z => z.r.Status).FirstOrDefault(),
                        CreatedOn = y.Select(z => z.r.CreatedOn).FirstOrDefault(),
                        //ModifiedOn = y.Select(z => z.r.ModifiedOn).FirstOrDefault(),
                        //CreatedBy = y.Select(z => z.r.CreatedBy).FirstOrDefault(),
                        //ModifiedBy = y.Select(z => z.r.ModifiedBy).FirstOrDefault(),
                        ApprovedByName = dbContext.Employees.Where(a=>a.EmployeeID==  y.Select(z => z.r.ApprovedBy).FirstOrDefault()).Select(x=>x.EmployeeName).FirstOrDefault(),
                        Remarks = y.Select(z => z.r.Remark).FirstOrDefault(),
                        ApprovedOn = y.Select(z => z.r.ApprovedOn).FirstOrDefault(),
                        DocumentId = dbContext.EmployeeRequestDocument.Where(b=>b.ChangeRequestId== y.Select(z => z.r.ChangeRequestId).FirstOrDefault()).Select(c=>c.EmployeeRequestDocumentId).FirstOrDefault(),
                        EmployeeRequestDetailslst = y.Select(z => new EmployeeRequestDetailsView
                        {
                            Field = z.rd.Field,
                            OldValue = z.rd.OldValue,
                            NewValue = z.rd.NewValue == null ? z.rd.OldValue: z.rd.NewValue
                        }).ToList(),
                        RequestProof = dbContext.EmployeeRequestDocument.Where(b => b.ChangeRequestId == y.Select(z=>z.r.ChangeRequestId).FirstOrDefault()).Select(x => new DocumentsToUpload
                        {
                            DocumentId = (int)x.EmployeeRequestDocumentId,
                            DocumentName = x.DocumentName,
                            DocumentCategory = x.DocumentType,
                        }).FirstOrDefault(),
                    }).ToList()
                }).ToList();
            return data;
        }
        public EmployeesRequestList GetEmployeeApproval(int employeeID)
        {
            
            EmployeesRequestList data = dbContext.Employees.Where(x => x.EmployeeID == employeeID).Select(x =>
                 new EmployeesRequestList
                 {
                     EmployeeId = x.EmployeeID,
                     EmployeeName = x.EmployeeName,
                     FormattedEmployeeId = x.FormattedEmployeeId,
                     DateOfJoining = x.DateOfJoining,
                     EmployeeType = dbContext.EmployeesType.Where(a => a.EmployeesTypeId == x.EmployeeTypeId).Select(b => b.EmployeesType).FirstOrDefault(),
                     ReportingTo = dbContext.Employees.Where(a => a.EmployeeID == x.ReportingManagerId).Select(b => b.EmployeeName).FirstOrDefault(),
                     Department = dbContext.Department.Where(a => a.DepartmentId == x.DepartmentId).Select(b => b.DepartmentName).FirstOrDefault(),
                     Designation = dbContext.Designation.Where(a => a.DesignationId == x.DesignationId).Select(b => b.DesignationName).FirstOrDefault(),
                     IsActive=x.IsActive,
                     ProfilePicture=x.ProfilePicture
                 }).FirstOrDefault();
            if (data != null)
            {
                data.EmployeeRequestList = dbContext.EmployeeRequest
                .Join(dbContext.EmployeeRequestDetails, r => r.ChangeRequestId, rd => rd.ChangeRequestId, (r, rd) => new { r, rd }).Where(r => r.r.EmployeeId == employeeID).ToList()
                .GroupBy(r => r.r.RequestCategory)?.Select(x => new EmployeeRequestListView
                {
                    RequestCategory = x.Key,
                    EmployeeRequestlst = x?.GroupBy(x => x.r.ChangeRequestId).Select(y => new EmployeeRequestView
                    {
                        EmployeeRequestId = y.Select(z => z.r.EmployeeRequestId).FirstOrDefault(),
                        EmployeeId = employeeID,
                        //ChangeRequestId = y.Select(z => z.r.ChangeRequestId).FirstOrDefault(),
                        Status = y.Select(z => z.r.Status).FirstOrDefault(),
                        CreatedOn = y.Select(z => z.r.CreatedOn).FirstOrDefault(),
                        //ModifiedOn = y.Select(z => z.r.ModifiedOn).FirstOrDefault(),
                        //CreatedBy = y.Select(z => z.r.CreatedBy).FirstOrDefault(),
                        //ModifiedBy = y.Select(z => z.r.ModifiedBy).FirstOrDefault(),
                        //ApprovedBy = y.Select(z => z.r.ApprovedBy).FirstOrDefault(),
                        Remarks = y.Select(z => z.r.Remark).FirstOrDefault(),
                        DocumentId = dbContext.EmployeeRequestDocument.Where(b => b.ChangeRequestId == y.Select(z => z.r.ChangeRequestId).FirstOrDefault()).Select(c => c.EmployeeRequestDocumentId).FirstOrDefault(),
                        EmployeeRequestDetailslst = y.Select(z => new EmployeeRequestDetailsView
                        {
                            Field = z.rd.Field,
                            OldValue = z.rd.OldValue,
                            NewValue = z.rd.NewValue == null ? z.rd.OldValue : z.rd.NewValue
                        }).ToList(),
                        RequestProof = dbContext.EmployeeRequestDocument.Where(b => b.ChangeRequestId == y.Select(z => z.r.ChangeRequestId).FirstOrDefault()).Select(x => new DocumentsToUpload
                        {
                            Path = x.DocumentPath,
                            DocumentId = (int)x.EmployeeRequestDocumentId,
                            DocumentName = x.DocumentName,
                            DocumentCategory = x.DocumentType,
                        }).FirstOrDefault(),
                    }).ToList()
                }).ToList();
            }            
            return data;
        }

        public List<EmployeeDetailListView> GetMyApprovalEmployeeList(PaginationView pagination)
        {
            var query = "(e.FirstName.StartsWith( \"@fullName\") || e.LastName.StartsWith( \"@fullName\") || e.EmployeeName.StartsWith(\"@fullName\") || \"@fullName\" == \"\") && (r.Status== \"@status\" || \"@status\" == \"\" )";
            query = query.Replace("@fullName", pagination?.EmployeeFilter?.FullName == null ? "" : pagination?.EmployeeFilter?.FullName.Trim());
            query = query.Replace("@status", pagination?.EmployeeFilter?.Status == "All" ? "" : pagination?.EmployeeFilter?.Status);
            
            return dbContext.Employees
                .Join(dbContext.EmployeeRequest, e => e.EmployeeID, r => r.EmployeeId, (e, r) => new { e, r })
                .Where(query).ToList().GroupBy(a => new
                {
                    a.e.EmployeeID,
                    a.e.EmployeeName,
                    a.e.FormattedEmployeeId,
                    a.e.EmployeeTypeId,
                    a.e.ProfilePicture
                }).Select(x => new EmployeeDetailListView
                {
                    EmployeeId = x.Key.EmployeeID,
                    EmployeeFullName = x.Key.EmployeeName,
                    FormattedEmployeeId = x.Key.FormattedEmployeeId,
                    EmployeementType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.Key.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault(),
                    ProfilePic = x.Key.ProfilePicture,
                    CreatedOn=x.Select(b=>b.r.CreatedOn).FirstOrDefault(),
                    HavingPendingRequest= x.Where(b => b.r.Status == "Pending").Select(a => a).Any()
                }).OrderByDescending(x => x.CreatedOn).Skip(pagination.NoOfRecord * (pagination.PageNumber)).Take(pagination.NoOfRecord).ToList();
        }
        public int GetMyApprovalEmployeeCount(PaginationView pagination)
        {
            var query = "(e.FirstName.StartsWith( \"@fullName\") || e.LastName.StartsWith( \"@fullName\") || e.EmployeeName.StartsWith(\"@fullName\") || \"@fullName\" == \"\") && (r.Status== \"@status\" || \"@status\" == \"\" )";
            query = query.Replace("@fullName", pagination?.EmployeeFilter?.FullName == null ? "" : pagination?.EmployeeFilter?.FullName.Trim());
            query = query.Replace("@status", pagination?.EmployeeFilter?.Status=="All"?"": pagination?.EmployeeFilter?.Status);
            int? data= dbContext.Employees
                .Join(dbContext.EmployeeRequest, e => e.EmployeeID, r => r.EmployeeId, (e, r) => new { e, r })
                .Where(query).ToList().GroupBy(a => new
                {
                    a.e.EmployeeID,
                    a.e.EmployeeName,
                    a.e.FormattedEmployeeId,
                    a.e.EmployeeTypeId,
                    a.e.ProfilePicture
                })
                .Select(x => x).Count();
            return data == null ? 0 : (int)data;
        }
        public List<EmployeeRequestDetailsView> GetPendingRequestByEmployeeId(int employeeId)
        {
            return dbContext.EmployeeRequest.Join(dbContext.EmployeeRequestDetails, r => r.ChangeRequestId, rd => rd.ChangeRequestId, (r, rd) => new { r, rd }).Where(x => x.r.EmployeeId == employeeId && x.r.Status == "Pending").Select(x=>
            new EmployeeRequestDetailsView
            {
                Field=x.rd.Field== "CommunicationState"? "communicationStateName":
                x.rd.Field=="CommunicationCountry"? "communicationCountryName":
                x.rd.Field == "CommunicationAddressZip" ? "communicationZip" :
                x.rd.Field == "PermanentState" ? "permanentStateName" :
                x.rd.Field == "PermanentCountry" ? "permanentCountryName" :
                x.rd.Field == "PermanentAddressZip" ? "permanentZip" : x.rd.Field,
                OldValue=x.rd.OldValue,
                NewValue=x.rd.NewValue == null ? x.rd.OldValue : x.rd.NewValue,
                sourceId = x.r.SourceId,
                ChangeRequestId = x.r.ChangeRequestId
              }).ToList();
        }

        public List<EmployeeRequestDetail> GetPendingRequestByEmployeeIdForFieldCheck(int employeeId)
        {
            return dbContext.EmployeeRequest.Join(dbContext.EmployeeRequestDetails, r => r.ChangeRequestId, rd => rd.ChangeRequestId, (r, rd) => new { r, rd }).Where(x => x.r.EmployeeId == employeeId && x.r.Status == "Pending").Select(x => x.rd).ToList();
        }
        public List<EmployeeRequestListView> GetPendingRequestByEmployeeIdAndRequestCat(int employeeId , string requestCategory)
        {
            return dbContext.EmployeeRequest
                .Join(dbContext.EmployeeRequestDetails, r => r.ChangeRequestId, rd => rd.ChangeRequestId, (r, rd) => new { r, rd }).Where(r => r.r.EmployeeId == employeeId && r.r.RequestCategory == requestCategory && r.r.Status == "Pending").ToList()
                .GroupBy(r => r.r.RequestCategory)?.Select(x => new EmployeeRequestListView
                {
                    RequestCategory = x.Key,
                    EmployeeRequestlst = x?.GroupBy(x => x.r.ChangeRequestId).Select(y => new EmployeeRequestView
                    {
                        EmployeeRequestId = y.Select(z => z.r.EmployeeRequestId).FirstOrDefault(),
                        EmployeeId = employeeId,
                        EmployeeRequestDetailslst = y.Select(z => new EmployeeRequestDetailsView
                        {
                            Field = z.rd.Field,
                            OldValue = z.rd.OldValue,
                            NewValue = z.rd.NewValue,
                            sourceId =z.r.SourceId
                        }).ToList(),
                        RequestProof = dbContext.EmployeeRequestDocument.Where(b => b.ChangeRequestId == y.Select(z => z.r.ChangeRequestId).FirstOrDefault()).Select(x => new DocumentsToUpload
                        {
                            Path = x.DocumentPath,
                            DocumentId = (int)x.EmployeeRequestDocumentId,
                            DocumentName = x.DocumentName,
                            DocumentCategory = x.DocumentType,
                        }).FirstOrDefault(),
                    }).ToList()
                }).ToList();
        }
    }
}
