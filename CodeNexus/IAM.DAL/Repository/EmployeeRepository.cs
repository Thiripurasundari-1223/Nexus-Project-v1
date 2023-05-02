using IAM.DAL.DBContext;
using Microsoft.Extensions.Configuration;
using SharedLibraries.Common;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IAM.DAL.Repository
{
    public interface IEmployeeRepository : IBaseRepository<Employees>
    {
        EmployeesViewModel GetEmployeeById(int employeeId);
        Employees GetEmployeeByEmailId(string emailId);
        Employees GetEmployeeByFormattedEmployeeId(string pFormattedEmployeeId, int employeeId);
        Employees CheckEmployeeByemailId(string emailAddress, int employeeId);
        string GetRoleNameByRoleId(int? roleId);
        List<EmployeesTypes> GetEmployeeTypeList();
        List<Department> GetEmployeeDepartmentList();
        List<Skillsets> GetSkillsetList();
        List<Roles> GetRolesList();
        List<Employees> GetEmployeesList(string pRole = "");
        List<EmployeeName> GetEmployeeNameById(List<int> listEmployeeId);
        string GetEmployeeTypeById(int? employeeTypeId);
        string GetDepartmentById(int? departmentId);
        List<int> GetReportingEmployeeById(int? employeeId);
        List<EmployeeList> GetEmployeeList(string pRole = "");
        List<TeamMemberDetails> GetTeamMemberDetails(List<TeamMemberDetails> resourceId);
        List<Employees> GetAllEmployeesDetails();
        List<EmployeeList> GetEmployeeListForManager(int employeeId);
        List<SearchEmployeesMasterDataViewModel> GetEmployeesMasterDataForSearch();
        List<int> GetActiveEmployeeIdList();
        int GetAdminEmployeeId(string roleName);
        List<Reports> GetReportsList(int employeeId, int employeeCategoryId);
        List<EmployeeList> GetEmployeeLeaveAdjustment();
        int GetEmployeeDepartmentIdByEmployeeId(int employeeId);
        List<EmployeeList> GetEmployeeAttendanceDetails();
        List<EmployeeList> GetEmployeesForManagerId(int managerEmployeeID);
        List<EmployeeDetails> GetEmployeeAvailabilityDetails(int EmployeeId);
        List<EmployeeAssociates> GetEmployeeDetailByManagerId(int employeeId);
        List<EmployeeLocation> GetEmployeeLocationList();
        List<HomeReportData> GetAssociateHomeReport();
        int GetEmployeeLocationIdByName(string strLocationName);
        int GetEmployeeTypeIdByType(string strEmployeesType);
        List<Designation> GetDesignationList();
        void UpdateAllEmployeesAsInacive();
        List<EmployeeCategory> GetEmployeeCategoryList();
        List<KeyWithValue> GetLocationNameById(List<int> locationId);
        List<KeyWithValue> GetAllLocationName();
        List<EmployeeRelationship> GetEmployeeRelationshipList();
        string GetDepartmentNameByDepartmentId(int? departmentId);
        List<EmployeeViewDetails> GetAllEmployeeListForManagerReport(int employeeId, bool isAll = false);
        List<EmployeeShiftDetailsView> GetEmployeeShiftDetails(int employeeID);
        List<ReportingManagerEmployeeList> GetManagerEmployeeList(string pRole = "");
        AppraisalManagerEmployeeDetailsView AppraisalManagerEmployeeDetails(int managerId);
        List<AppraisalManagerEmployeeDetailsView> GetEmployeesByPermanentandActive();
        int GetDepartmentByName(string pDepartmentName);
        EmployeeandManagerView GetEmployeeandManagerByEmployeeID(int employeeID);
        EmployeeCategoryView GetEmployeeCategoryDetailsByEmployeeId(int employeeId, int employeeCategoryId);
        List<EmployeeandManagerView> GetEmployeesListByDepartment(EmployeeListByDepartment employeeView);
        EmployeeDepartmentAndLocationView GetEmployeeDepartmentAndLocation(int employeeId);
        List<EmployeeList> GetEmployeeListByManagerId(int? employeeId);
        string GetDesignationById(int? designationId);
        List<EmployeeAttendanceDetails> GetAllEmployeesAttendanceDetails(EmployeesAttendanceFilterView employeesAttendanceFilterView);
        List<ProbationStatusView> GetAllProbationStatusDetails();
        string GetNextFormattedEmployeeId(string pEmployeeIdFormat);
        Employees GetEmployeeByEmployeeId(int employeeID);
        GrantLeaveApproverView GetGrantLeaveApprover(int employeeId, string hrDepartmentName);
        string GetLocationNameByLocationId(int? locationId);
        int? GetShiftByemployeeId(int? employeeId);
        List<EmployeeShiftDetailsView> GetShiftListByemployeeId(int EmployeeID);
        List<Employees> GetAllEmployeesList();
        EmployeeShiftDetails GetShifByDate(int EmployeeID, DateTime date);
        List<EmployeeDetail> GetAllActiveEmployeeDetails();
        List<Employees> GetEmployees(EmployeeListView employee);
        List<EmployeeLeaveAdjustmentView> GetEmployeeLeaveAdjustmentListWithFilter(EmployeeLeaveAdjustmentFilterView employeeLeaveAdjustmentView);
        List<EmployeeList> GetEmployeeDetailsList();
        EmployeeandManagerView GetEmployeeAndApproverDetails(int employeeId, int approverId);
        EmployeeDetailsForLeaveView GetEmployeeDetailsById(int employeeId);
        EmployeeManagerAndHeadDetailsView GetEmployeeManagerAndHeadDetails(int employeeId, int approverId);
        EmployeeName GetEmployeeNameByEmployeeId(int employeeId);
        List<ResignedEmployeeView> GetEmployeeDetailsById(List<int> listEmployeeId);
        List<SystemRoles> GetSystemRolesList();
        Employees GetEmployeeDetailByEmployeeId(int employeeId);
        List<EmployeeResignationDetailsView> GetEmployeeResignationDetails(List<EmployeeResignationDetailsView> employeeList);
        ResignationApproverView GetResignationApprover(int employeeId, string hrDepartmentName);
        List<ResignationEmployeeMasterView> GetResignationEmployeeMasterData(List<int> employeeId);
        List<ResignationInterviewDetailView> GetEmployeeExitInterviewDetails(List<ResignationInterviewDetailView> employeeDetails);
        DateTime? GetEmployeeResignationDate(int employeeId);
        List<ResignationEmployeeMasterView> GetExitEmployeeMaster(List<int> employeeId);
        ProjectCustomerEmployeeList GetEmployeeListForProjectAndCustomer(int employeeId, bool isAllEmployee);
        List<int> GetEmployeesListBySystemRole(string sRole = "");
        List<string> GetExitCheckListRole(int employeeId, int loginUserId, bool isAllReportees);
        List<EmployeeViewDetails> GetCheckListEmployeeList(int employeeId, bool isAll);
        List<ChecklistEmployeeView> GetCheckListEmployeeDetails(List<ChecklistEmployeeView> employeeDetails);
        ReporteesChecklistEmployeeView GetReporteesCheckListEmployee(int employeeId, bool isAll);
        List<ResignationEmployeeMasterView> GetEmployeeListForResignation(int employeeId);
        List<EmployeeList> GetResignationEmployeeList(int employeeId);
        List<ResignationEmployeeMasterView> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter);
        List<ResignationEmployeeMasterView> GetEmployeesDetailsBySystemRole(string sRole);
        List<Employees> GetEmployeeListByRelievingDate();
        string GetSystemRoleNameById(int systemRoleId);
        int GetTotalRecordCount();
        List<EmployeeDetailListView> GetEmployeeListForGrid(PaginationView pagination);
        List<EmployeeDetailListView> GetEmployeeListForRequestedDocumentGrid(List<int> employeeIds);
        List<EmployeeDetailListViewForOrgChart> GetEmployeesListForOrgChart(PaginationView pagination);
        EmployeeBasicInfoView GetEmployeeBasicInfoById(int employeeId);
        List<EmployeeDetailListView> GetListOfEmployeeBirthDay(DateTime fromDate, DateTime toDate);
        List<EmployeeDetailListView> employeeFilterData(PaginationView paginationView);
        List<EmployeeWorkAnniversaries> GetListOfEmployeeWorkAnniversaries(DateTime fromDate, DateTime toDate);
        List<EmployeeList> GetAbsentNotificationEmployeeList();
        int GetEmployeeIdByFormattedEmployeeId(string formattedEmployeeId);
        EmployeeorganizationcharView GetorganizationchartDetails(int employeeId);
        int employeeFilterDataCount(PaginationView paginationView);
        Task<Employees> GetEmployeeByFormattedEmployeeIdForBulkUpload(string pFormattedEmployeeId);
        EmployeeDownloadData EmployeeListDownload(PaginationView paginationView);

        Task<EmployeeManagerDetails> reportingManagerData(int reportingManagerid);


        Task<EmployeeDesignationDetails> designationData(int designationid);

        Task<EmployeeBaseWorkLocationDetails> baseWorkLocationData(int locationid);

        //Task<EmployeeProbationStatusDetails> probationStatusData(int  probationstatusid);

        Task<EmployeeMasterEmailTemplate> getEmployeeEmailByName(string templateName);
        //EmployeeRequestListView GetEmployeeRequest(int employeeID);
        EmployeeRequestListView GetEmployeeRequestForAdmin();
        Task<List<EmployeeName>> ContractEndDateNotification();
        List<EmployeeDataForDropDown> GetEmployeeDropDownList(bool isAll);
        Task<Employees> GetEmployeeForBUlkUploadByEmployeeId(int employeeID);
        List<NoticePeriodCategory> GetNoticeCategory();
        ResignationEmployeeMasterView GetEmployeePersonalInfoByEmployeeID(int employeeId);
        EmployeeBasicInfoView GetEmployeeBasicInfoByEmployeeID(int employeeId);
        List<EmployeeDataForDropDown> GetEmployeeListForNewResignation();
        int GetEmployeeAttendanceDetailsCount(EmployeesAttendanceFilterView employeesAttendanceFilterView);
        EmployeeDetailsForLeave GetEmployeeByEmployeeIdForLeaves(int employeeID);
        string GetEntityName(int Entity);
        List<EmployeeList> GetEmployeeDetailForGrantLeaveById(EmployeeListByDepartment employeeList);
    }
    public class EmployeeRepository : BaseRepository<Employees>, IEmployeeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IAMDBContext dbContext;
        public EmployeeRepository(IAMDBContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            this.dbContext = dbContext;
            _configuration = configuration;
        }
        public EmployeesViewModel GetEmployeeById(int employeeId)
        {

            EmployeesViewModel employeeViewModel = new();
            Employees employee = dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x).FirstOrDefault();
            if (employee != null)
            {
                employeeViewModel.Employee = employee;

                employeeViewModel.EmployeespecialAbilities = dbContext.EmployeeSpecialAbility.Where(y => y.EmployeeId == employeeId).Select(z => new EmployeeSpecialAbility { SpecialAbilityId = z.SpecialAbilityId }).ToList();
                //if (employee?.DateOfJoining != null && employee?.DateOfJoining?.Date < DateTime.Now.Date)
                //{
                //    DateTime zeroTime = new(1, 1, 1);
                //    DateTime doj = (DateTime)employee?.DateOfJoining;
                //    DateTime curdate = DateTime.Now.ToLocalTime();
                //    TimeSpan span = curdate - doj;
                //    int years = (zeroTime + span).Year - 1;
                //    int months = (zeroTime + span).Month - 1;
                //    employeeViewModel.Employee.TVSNextExperience = (decimal?)((years * 1.0) + (months * 0.1));
                //}
                employeeViewModel.CreateBy = dbContext.Employees.Where(x => x.CreatedBy == employeeViewModel.Employee.CreatedBy).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault();
                employeeViewModel.ModifiedBy = dbContext.Employees.Where(x => x.ModifiedBy == employeeViewModel.Employee.ModifiedBy).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault();
                employeeViewModel.Skillset = (from a in dbContext.EmployeesSkillset
                                              join b in dbContext.Skillset on a.SkillsetId equals b.SkillsetId
                                              where a.EmployeeId == employeeId
                                              select new Skillsets { SkillsetId = a.SkillsetId, Skillset = b.Skillset }).ToList();
                employeeViewModel.RoleName = dbContext.Roles.Where(x => x.RoleId == employee.RoleId).Select(x => x.RoleName).FirstOrDefault();
                employeeViewModel.SystemRoleName = dbContext.SystemRoles.Where(x => x.RoleId == employee.SystemRoleId).Select(x => x.RoleName).FirstOrDefault();
                employeeViewModel.DepartmentName = dbContext.Department.Where(x => x.DepartmentId == employee.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();

                employeeViewModel.Location = dbContext.EmployeeLocation.Where(x => x.LocationId == employee.LocationId).Select(x => x.Location).FirstOrDefault();
                employeeViewModel.DesignationName = dbContext.Designation.Where(x => x.DesignationId == employee.DesignationId).Select(x => x.DesignationName).FirstOrDefault();
                employeeViewModel.EmployeeCategoryName = dbContext.EmployeeCategory.Where(x => x.EmployeeCategoryId == employee.EmployeeCategoryId).Select(x => x.EmployeeCategoryName).FirstOrDefault();
                employeeViewModel.EmployeeDependent = (from a in dbContext.EmployeeDependent
                                                       join b in dbContext.Employees on a.EmployeeID equals b.EmployeeID
                                                       where a.EmployeeID == employeeId
                                                       select new EmployeeDependentView
                                                       {
                                                           EmployeeDependentId = a.EmployeeDependentId,
                                                           EmployeeRelationName = a.EmployeeRelationName,
                                                           EmployeeRelationshipId = a.EmployeeRelationshipId,
                                                           EmployeeRelationship = dbContext.EmployeeRelationship.Where(x => x.EmployeeRelationshipId == a.EmployeeRelationshipId).Select(x => x.EmployeeRelationshipName).FirstOrDefault(),
                                                           EmployeeRelationDateOfBirth = a.EmployeeRelationDateOfBirth,
                                                           DependentDetailsProof = dbContext.EmployeeDocument.Where(x => x.SourceId == a.EmployeeDependentId && x.EmployeeID == employeeId && x.DocumentType == "dependentDetailsProof").Select(x =>
                                                            new DocumentsToUpload
                                                            {
                                                                CreatedOn = x.CreatedOn,
                                                                Path = File.Exists(x.DocumentPath) ? x.DocumentPath : "",
                                                                DocumentId = (int)x.EmployeeDocumentId,
                                                                DocumentName = x.DocumentName,
                                                                DocumentCategory = x.DocumentType,
                                                                DocumentAsBase64 = File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)) : "",
                                                                DocumentSize = File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)).Length : 0,
                                                            }).FirstOrDefault()
                                                       }).ToList();
                employeeViewModel.EmployeeShiftDetails = (from a in dbContext.EmployeeShiftDetails
                                                          join b in dbContext.Employees on a.EmployeeID equals b.EmployeeID
                                                          where a.EmployeeID == employeeId
                                                          select new EmployeeShiftDetails
                                                          {
                                                              EmployeeShiftDetailsId = a.EmployeeShiftDetailsId,
                                                              ShiftDetailsId = a.ShiftDetailsId,
                                                              ShiftFromDate = a.ShiftFromDate,
                                                              ShiftToDate = a.ShiftToDate
                                                          }).ToList();
                employeeViewModel.CurrentWorkPlace = dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == employee.CurrentWorkPlaceId).Select(x => x.DisplayName).FirstOrDefault();
                employeeViewModel.DepartmentHead = dbContext.Employees.Where(x => x.EmployeeID == dbContext.Department.Where(x => x.DepartmentId == employee.DepartmentId).Select(x => x.DepartmentHeadEmployeeId).FirstOrDefault()).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault();
            }
            return employeeViewModel;
        }
        public Employees GetEmployeeByEmailId(string emailId)
        {
            return dbContext.Employees.Where(x => x.EmailAddress == emailId).FirstOrDefault();
        }
        public string GetRoleNameByRoleId(int? roleId)
        {
            return dbContext.Role.Where(y => y.RoleId == roleId).Select(x => x.RoleName).FirstOrDefault();
        }
        public List<EmployeesTypes> GetEmployeeTypeList()
        {
            return dbContext.EmployeesType.ToList();
        }
        public string GetEmployeeTypeById(int? employeeTypeId)
        {
            return dbContext.EmployeesType.Where(x => x.EmployeesTypeId == employeeTypeId).Select(x => x.EmployeesType).FirstOrDefault();
        }
        public List<Department> GetEmployeeDepartmentList()
        {
            return dbContext.Department.ToList();
        }
        public string GetDepartmentById(int? departmentId)
        {
            return dbContext.Department.Where(x => x.DepartmentId == departmentId).Select(x => x.DepartmentName).FirstOrDefault();
        }
        public int GetDepartmentByName(string pDepartmentName)
        {
            return dbContext.Department.Where(x => x.DepartmentName == pDepartmentName).Select(x => x.DepartmentId).FirstOrDefault();
        }
        public List<Skillsets> GetSkillsetList()
        {
            return dbContext.Skillset.ToList();
        }
        public List<Roles> GetRolesList()
        {
            return dbContext.Roles.ToList();
        }
        public List<Employees> GetEmployeesList(string pRole = "")
        {
            if (pRole == null) pRole = "";
            if (pRole != "")
            {
                return (from users in dbContext.Employees
                        join roles in dbContext.Roles on users.RoleId equals roles.RoleId
                        where roles.RoleName == pRole && users.IsActive == true
                        select users).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            }
            return dbContext.Employees.Where(x => x.IsActive == true).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
        }
        public List<EmployeeDetail> GetAllActiveEmployeeDetails()
        {
            return dbContext.Employees.Where(x => x.IsActive == true).Select(x => new
            EmployeeDetail
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                DateOfJoining = x.DateOfJoining,
                DateOfContract = x.DateOfContract,
                DateOfRelieving = x.DateOfRelieving,
                IsActive = x.IsActive,
                FormattedEmployeeID = x.FormattedEmployeeId,
                DesignationId = x.DesignationId,
            }
            ).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
        }
        public List<Employees> GetAllEmployeesList()
        {
            return dbContext.Employees.OrderByDescending(x => x.DateOfJoining).ToList();
        }
        public List<EmployeeName> GetEmployeeNameById(List<int> listEmployeeId)
        {
            return dbContext.Employees.Where(x => listEmployeeId.Contains(x.EmployeeID)).Select(y => new
                            EmployeeName
            {
                EmployeeId = y.EmployeeID,
                EmployeeFullName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId,
                EmployeeEmailId = y.EmailAddress,
                profilePic = y.ProfilePicture
            }).OrderBy(x => x.EmployeeFullName).ToList();
        }
        public List<int> GetReportingEmployeeById(int? employeeId)
        {
            return dbContext.Employees.Where(x => x.ReportingManagerId == employeeId).Select(x => x.EmployeeID).ToList();
        }
        public List<EmployeeList> GetEmployeeList(string pRole = "")
        {
            if (pRole == null) pRole = "";
            if (pRole != "")
            {
                return (from users in dbContext.Employees
                        join roles in dbContext.Roles on users.RoleId equals roles.RoleId
                        join dep in dbContext.Department on users.DepartmentId equals dep.DepartmentId
                        where roles.RoleName == pRole && users.IsActive == true
                        select new EmployeeList
                        {
                            EmployeeId = users.EmployeeID,
                            FormattedEmployeeId = users.FormattedEmployeeId,
                            Employee_Id = users.FormattedEmployeeId,
                            EmployeeName = (users.FirstName + " " + users.LastName),
                            EmployeeEmailId = users.EmailAddress,
                            DepartmentId = users.DepartmentId,
                            LocationId = users.LocationId,
                            EmployeeFullName = users.FormattedEmployeeId + " " + users.FirstName + " " + users.LastName,
                            ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == users.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault(),
                        }).OrderBy(x => x.EmployeeName).ToList();
            }
            return dbContext.Employees.Where(x => x.IsActive == true).Select(y => new EmployeeList
            {
                EmployeeId = y.EmployeeID,
                FormattedEmployeeId = y.FormattedEmployeeId,
                Employee_Id = y.FormattedEmployeeId,
                EmployeeName = y.FirstName + " " + y.LastName,
                EmployeeEmailId = y.EmailAddress,
                DepartmentId = y.DepartmentId,
                LocationId = y.LocationId,
                EmployeeFullName = y.FormattedEmployeeId + " " + y.FirstName + " " + y.LastName,
                ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == y.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault(),
            }).OrderBy(x => x.EmployeeName).ToList();
        }
        public List<EmployeeList> GetEmployeeDetailsList()
        {

            return dbContext.Employees.Where(x => x.IsActive == true).Select(y => new EmployeeList
            {
                EmployeeId = y.EmployeeID,
                FormattedEmployeeId = y.FormattedEmployeeId,
                Employee_Id = y.FormattedEmployeeId,
                EmployeeName = y.FirstName + " " + y.LastName,
                EmployeeEmailId = y.EmailAddress,
                DepartmentId = y.DepartmentId,
                DepartmentName = dbContext.Department.Where(x => x.DepartmentId == y.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault(),
                LocationId = y.LocationId,
                LocationName = dbContext.EmployeeLocation.Where(x => x.LocationId == y.LocationId).Select(x => x.Location).FirstOrDefault(),
                ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == y.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault(),
                DesignationId = y.DesignationId,
                DesignationName = dbContext.Designation.Where(x => x.DesignationId == y.DesignationId).Select(x => x.DesignationName).FirstOrDefault(),
                RoleId = y.RoleId,
                RoleName = dbContext.Role.Where(x => x.RoleId == y.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                ProbationStatusId = y.ProbationStatusId,
                ProbationStatusName = dbContext.ProbationStatus.Where(x => x.ProbationStatusId == y.ProbationStatusId).Select(x => x.ProbationStatusName).FirstOrDefault(),
                EmployeeTypeId = y.EmployeeTypeId,
                EmployeeTypeName = dbContext.EmployeesType.Where(x => x.EmployeesTypeId == y.EmployeeTypeId).Select(x => x.EmployeesType).FirstOrDefault(),
                SystemRoleId = y.SystemRoleId,
                SystemRoleName = dbContext.SystemRoles.Where(x => x.RoleId == y.SystemRoleId).Select(x => x.RoleName).FirstOrDefault(),
            }).OrderBy(x => x.EmployeeName).ToList();
        }

        public List<TeamMemberDetails> GetTeamMemberDetails(List<TeamMemberDetails> resources)
        {
            resources = resources == null ? new List<TeamMemberDetails>() : resources;
            List<Employees> employees = dbContext.Employees.Where(x => resources.Select(x => x.UserId).Contains(x.EmployeeID)).ToList();
            foreach (var item in resources)
            {
                item.UserName = employees.Where(x => x.EmployeeID == item.UserId).Select(y => y.FirstName + " " + y.LastName).FirstOrDefault();
                item.FormattedEmployeeId = employees.Where(x => x.EmployeeID == item.UserId).Select(y => y.FormattedEmployeeId).FirstOrDefault();
                item.ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == item.UserId && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault();
            }
            return resources;
        }
        public List<Employees> GetAllEmployeesDetails()
        {
            return dbContext.Employees.ToList();
        }
        //private IQueryable<Employees> GetEmployeesForManagerRecursively(int managerEmployeeID)
        //{
        //    return from managerEmployee in dbContext.Employees
        //           join employee in dbContext.Employees on managerEmployee.EmployeeID equals employee.EmployeeID
        //           where managerEmployee.ReportingManagerId == managerEmployeeID
        //           select employee;
        //}
        private List<int> employeeIds;
        //private IQueryable<Employees> GetManagersForEmployeeRecursively(int employeeID)
        //{
        //    return from managerEmployee in dbContext.Employees
        //           join manager in dbContext.Employees on managerEmployee.ReportingManagerId equals manager.EmployeeID
        //           where managerEmployee.EmployeeID == employeeID
        //           select manager;
        //}
        public List<EmployeeList> GetEmployeeListForManager(int employeeId)
        {
            List<EmployeeList> El = new();
            employeeIds = new List<int> { employeeId };
            GetEmployeesForManagerRecursively(employeeIds);
            return dbContext.Employees.Where(x => employeeIds.Contains(x.EmployeeID)).Select(y => new EmployeeList
            {
                EmployeeId = y.EmployeeID,
                EmployeeName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId
            }).OrderBy(x => x.EmployeeName).ToList();
        }
        public ProjectCustomerEmployeeList GetEmployeeListForProjectAndCustomer(int employeeId, bool isAllEmployee)
        {
            ProjectCustomerEmployeeList employeeList = new ProjectCustomerEmployeeList();
            if (isAllEmployee)
            {
                //List<EmployeeList> El = new();
                employeeIds = new List<int> { employeeId };
                GetEmployeesForManagerRecursively(employeeIds);
                employeeList.EmployeeList = dbContext.Employees.Where(x => employeeIds.Contains(x.EmployeeID)).Select(y => y.EmployeeID).ToList();
            }
            else
            {
                employeeList.EmployeeList = new List<int>() { employeeId };
            }
            employeeList.RoleName = (from a in dbContext.Employees
                                     join b in dbContext.SystemRoles
       on a.SystemRoleId equals b.RoleId
                                     where a.EmployeeID == employeeId
                                     select b.RoleName).FirstOrDefault();

            return employeeList;
        }
        private List<int> GetEmployeesForManagerRecursively(List<int> employeeId)
        {
            List<int> recList = dbContext.Employees.Where(x => x.IsActive == true && x.EmployeeID != (int)x.ReportingManagerId
                                && x.ReportingManagerId.HasValue && employeeId.Contains((int)x.ReportingManagerId)).Select(x => x.EmployeeID).Distinct().ToList();
            if (recList != null && recList?.Count > 0)
            {
                employeeIds = employeeIds.Concat(recList).ToList();
                GetEmployeesForManagerRecursively(recList);
            }
            return employeeIds;
        }
        public List<SearchEmployeesMasterDataViewModel> GetEmployeesMasterDataForSearch()
        {
            List<SearchEmployeesMasterDataViewModel> employeeViewModel = new();
            employeeViewModel = (from emp in dbContext.Employees
                                     //join roles in dbContext.Roles on emp.RoleId equals roles.RoleId
                                 join designation in dbContext.Designation on emp.DesignationId equals designation.DesignationId
                                 select new SearchEmployeesMasterDataViewModel
                                 {
                                     EmployeeId = emp.EmployeeID,
                                     FirstName = emp.FirstName,
                                     LastName = emp.LastName,
                                     RoleId = emp.RoleId,
                                     RoleName = "", //roles.RoleName,
                                     FormattedEmployeeId = emp.FormattedEmployeeId,
                                     Designation = designation.DesignationName
                                 }).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
            List<EmployeesSkillsetViewModel> employeesSkillsetList = new();
            employeesSkillsetList = (from emp in dbContext.Employees
                                     join empSkills in dbContext.EmployeesSkillset on emp.EmployeeID equals empSkills.EmployeeId
                                     join skills in dbContext.Skillset on empSkills.SkillsetId equals skills.SkillsetId
                                     join designation in dbContext.Designation on emp.DesignationId equals designation.DesignationId
                                     select new EmployeesSkillsetViewModel
                                     {
                                         EmployeeId = emp.EmployeeID,
                                         SkillSetId = empSkills.SkillsetId,
                                         SkillSetName = skills.Skillset,
                                         FormattedEmployeeId = emp.FormattedEmployeeId,
                                         Designation = designation.DesignationName
                                     }).ToList();
            employeeViewModel?.ForEach(x => x.employeesSkillsetList = employeesSkillsetList?.Where(y => y.EmployeeId == x.EmployeeId).ToList());
            return employeeViewModel;
        }
        public List<int> GetActiveEmployeeIdList()
        {
            return (from employees in dbContext.Employees
                    where employees.IsActive == true
                    select employees.EmployeeID).ToList();
        }
        public int GetAdminEmployeeId(string roleName)
        {
            return (from employees in dbContext.Employees
                    join role in dbContext.Roles on employees.RoleId equals role.RoleId
                    where role.RoleName == roleName
                    select employees.EmployeeID).FirstOrDefault();
        }
        public List<Reports> GetReportsList(int employeeId, int employeeCategoryId)
        {
            return (from DepartmentReportMapping in dbContext.DepartmentReportMapping
                    join Reports in dbContext.Reports on DepartmentReportMapping.ReportId equals Reports.ReportID
                    join Employee in dbContext.Employees on DepartmentReportMapping.DepartmentId equals Employee.DepartmentId
                    where Employee.EmployeeID == employeeId && Employee.EmployeeCategoryId == DepartmentReportMapping.EmployeeCategoryId
                    select new Reports
                    {
                        ReportID = Reports.ReportID,
                        ReportName = Reports.ReportName,
                        ReportTitle = Reports.ReportTitle,
                        ReportIconPath = Reports.ReportIconPath,
                        ReportNavigationUrl = Reports.ReportNavigationUrl
                    }).ToList();
        }
        public List<EmployeeList> GetEmployeeLeaveAdjustment()
        {
            return dbContext.Employees.Where(x => x.IsActive == true).Select(y => new EmployeeList
            {
                EmployeeId = y.EmployeeID,
                EmployeeName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId
            }).OrderBy(x => x.EmployeeName).ToList();
        }
        public int GetEmployeeDepartmentIdByEmployeeId(int employeeId)
        {
            int departmentid = 0;
            departmentid = (int)dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x.DepartmentId).FirstOrDefault();
            return departmentid;
        }
        public List<EmployeeList> GetEmployeeAttendanceDetails()
        {
            return dbContext.Employees.Where(x => x.IsActive == true).Select(y => new EmployeeList
            {
                EmployeeId = y.EmployeeID,
                EmployeeName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId
            }).OrderBy(x => x.EmployeeName).ToList();
        }
        public List<EmployeeList> GetEmployeesForManagerId(int managerEmployeeID)
        {
            List<EmployeeList> employee = new();
            employee = (from managerEmployee in dbContext.Employees
                        where managerEmployee.ReportingManagerId == managerEmployeeID && managerEmployee.IsActive == true
                        select new EmployeeList
                        {
                            EmployeeId = managerEmployee.EmployeeID,
                            EmployeeName = managerEmployee.FirstName + " " + managerEmployee.LastName,
                            EmpName = managerEmployee.FirstName + " " + managerEmployee.LastName,
                            FormattedEmployeeId = managerEmployee.FormattedEmployeeId,
                            DepartmentId = managerEmployee.DepartmentId,
                            LocationId = managerEmployee.LocationId,
                            profilePic = managerEmployee.ProfilePicture,
                            ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == managerEmployee.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault(),
                        }).OrderBy(x => x.EmployeeName).ToList();
            return employee;
        }
        public List<EmployeeDetails> GetEmployeeAvailabilityDetails(int employeeId)
        {
            List<EmployeeDetails> employeeList = new();
            employeeList = (from emp in dbContext.Employees
                            join dep in dbContext.Department on emp.DepartmentId equals dep.DepartmentId
                            join role in dbContext.Roles on emp.RoleId equals role.RoleId
                            join empSkills in dbContext.EmployeesSkillset on emp.EmployeeID equals empSkills.EmployeeId
                            join skills in dbContext.Skillset on empSkills.SkillsetId equals skills.SkillsetId
                            where emp.ReportingManagerId == employeeId
                            select new EmployeeDetails
                            {
                                EmployeeId = emp.EmployeeID,
                                EmployeeName = emp.FirstName + " " + emp.LastName,
                                DepartmentId = emp.DepartmentId,
                                Department = dep.DepartmentName,
                                RoleId = emp.RoleId,
                                Role = role.RoleName,
                                SkillsetId = skills.SkillsetId,
                                Skillset = skills.Skillset,
                                FormattedEmployeeId = emp.FormattedEmployeeId
                            }).OrderBy(x => x.EmployeeName).ToList();
            return employeeList;
        }
        public List<EmployeeAssociates> GetEmployeeDetailByManagerId(int employeeId)
        {
            List<EmployeeAssociates> employees = new();
            employees = (from emp in dbContext.Employees
                         join dept in dbContext.Department on emp.DepartmentId equals dept.DepartmentId
                         join loc in dbContext.EmployeeLocation on emp.LocationId equals loc.LocationId
                         where emp.ReportingManagerId == employeeId
                         select new EmployeeAssociates
                         {
                             EmployeeId = emp.EmployeeID,
                             EmployeeName = emp.FirstName + " " + emp.LastName,
                             Gender = emp.Gender,
                             DepartmentId = emp.DepartmentId,
                             Department = dept.DepartmentName,
                             LocationId = emp.LocationId,
                             Location = loc.Location,
                             IsSpecialAbility = emp.IsSpecialAbility,
                             FormattedEmployeeId = emp.FormattedEmployeeId
                         }).OrderBy(x => x.EmployeeName).ToList();
            return employees;
        }
        public List<EmployeeLocation> GetEmployeeLocationList()
        {
            return dbContext.EmployeeLocation.ToList();
        }
        public List<HomeReportData> GetAssociateHomeReport()
        {
            List<HomeReportData> associatesList = new();
            HomeReportData totalAssociates = new();
            totalAssociates.ReportTitle = "Total";
            totalAssociates.ReportData = dbContext.Employees.Where(x => x.IsActive == true).Select(x => x.EmployeeID).ToList().Count().ToString();
            associatesList.Add(totalAssociates);
            HomeReportData currentMonthAssociate = new();
            currentMonthAssociate.ReportTitle = "This Month";
            var monthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            currentMonthAssociate.ReportData = dbContext.Employees.Where(x => x.IsActive == true && x.DateOfJoining >= monthStartDate.Date && x.DateOfJoining <= DateTime.Now.Date).Select(x => x.EmployeeID).ToList().Count().ToString();
            associatesList.Add(currentMonthAssociate);
            return associatesList;
        }
        public int GetEmployeeLocationIdByName(string strLocationName)
        {
            return dbContext.EmployeeLocation.Where(x => x.Location.Equals(strLocationName)).Select(x => x.LocationId).FirstOrDefault();
        }
        public int GetEmployeeTypeIdByType(string strEmployeesType)
        {
            return dbContext.EmployeesType.Where(x => x.EmployeesType.Equals(strEmployeesType)).Select(x => x.EmployeesTypeId).FirstOrDefault();
        }
        public List<Designation> GetDesignationList()
        {
            return dbContext.Designation.ToList();
        }
        public void UpdateAllEmployeesAsInacive()
        {
            dbContext.Employees.ToList().ForEach((x => { x.IsActive = false; }));
        }
        public List<EmployeeCategory> GetEmployeeCategoryList()
        {
            return dbContext.EmployeeCategory.ToList();
        }
        public List<KeyWithValue> GetLocationNameById(List<int> locationId)
        {
            return dbContext.EmployeeLocation.Where(x => locationId.Contains(x.LocationId)).Select(y => new KeyWithValue
            { Key = y.LocationId, Value = y.Location }).ToList();
        }
        public List<KeyWithValue> GetAllLocationName()
        {
            return dbContext.EmployeeLocation.Select(y => new KeyWithValue
            { Key = y.LocationId, Value = y.Location }).ToList();
        }
        public List<EmployeeRelationship> GetEmployeeRelationshipList()
        {
            return dbContext.EmployeeRelationship.ToList();
        }
        public string GetDepartmentNameByDepartmentId(int? departmentId)
        {
            return dbContext.Department.Where(y => y.DepartmentId == departmentId).Select(x => x.DepartmentName).FirstOrDefault();
        }
        int i = 0;
        public List<EmployeeViewDetails> GetAllEmployeeListForManagerReport(int employeeId, bool isAll)
        {
            employeeIds = new List<int> { employeeId };
            GetAllEmployeesForManagerRecursively(employeeIds, isAll);
            return dbContext.Employees
                .Join(dbContext.Department, e => e.DepartmentId, d => d.DepartmentId, (e, d) => new { e, d })
                //.Join(dbContext.Roles, ed => ed.e.RoleId, r => r.RoleId, (ed, r) => new { ed, r })
                .Join(dbContext.Designation, ede => ede.e.DesignationId, de => de.DesignationId, (ede, de) => new { ede, de })
                .Where(rs => employeeIds.Contains(rs.ede.e.EmployeeID)).Select(rs => new EmployeeViewDetails
                {
                    EmployeeId = rs.ede.e.EmployeeID,
                    FormattedEmployeeId = rs.ede.e.FormattedEmployeeId,
                    EmployeeName = rs.ede.e.FirstName + " " + rs.ede.e.LastName,
                    EmployeeEmail = rs.ede.e.EmailAddress,
                    EmployeeType = dbContext.EmployeesType.Where(es => es.EmployeesTypeId == rs.ede.e.EmployeeTypeId).Select(esr => esr.EmployeesType).FirstOrDefault(),
                    DepartmentId = rs.ede.e.DepartmentId,
                    DepartmentName = rs.ede.d.DepartmentName,
                    RoleId = rs.ede.e.RoleId,
                    RoleName = rs.de.DesignationName,
                    SystemRoleName = dbContext.SystemRoles.Where(x => x.RoleId == rs.ede.e.SystemRoleId).Select(x => x.RoleName).FirstOrDefault(),
                    ReportingTo = dbContext.Employees.Where(es => es.EmployeeID == rs.ede.e.ReportingManagerId).Select(esr => esr.FirstName + " " + esr.LastName).FirstOrDefault(),
                    ReportingEmail = dbContext.Employees.Where(es => es.EmployeeID == rs.ede.e.ReportingManagerId).Select(esr => esr.EmailAddress).FirstOrDefault()
                }).OrderBy(x => x.EmployeeName).ToList();
        }
        public ReporteesChecklistEmployeeView GetReporteesCheckListEmployee(int employeeId, bool isAll)
        {
            ReporteesChecklistEmployeeView data = new ReporteesChecklistEmployeeView();
            employeeIds = new List<int> { employeeId };
            GetAllEmployeesForManagerRecursively(employeeIds, isAll);
            data.EmployeeDetails = dbContext.Employees.Where(rs => employeeIds.Contains(rs.EmployeeID)).Select(rs => new EmployeeViewDetails
            {
                EmployeeId = rs.EmployeeID,
                FormattedEmployeeId = rs.FormattedEmployeeId,
                EmployeeName = rs.FirstName + " " + rs.LastName,
                EmployeeEmail = rs.EmailAddress
            }).OrderBy(x => x.EmployeeName).ToList();

            employeeIds.Add(employeeId);
            //data.Role= (from e in dbContext.Employees
            //            join r in dbContext.SystemRoles on e.SystemRoleId equals r.RoleId
            //            where employeeIds.Contains(e.EmployeeID)
            //            select r.RoleName==null?"":r.RoleName.ToLower()).ToList();
            List<string> roleData = new List<string>();
            if (isAll)
            {
                roleData = (from e in dbContext.Employees
                            join r in dbContext.SystemRoles on e.SystemRoleId equals r.RoleId
                            where employeeIds.Contains(e.EmployeeID)
                            select r.RoleName == null ? "" : r.RoleName.ToLower()).Distinct().ToList();
            }
            else
            {
                string result = (from e in dbContext.Employees
                                 join r in dbContext.SystemRoles on e.SystemRoleId equals r.RoleId
                                 where e.EmployeeID == employeeId
                                 select r.RoleName == null ? "" : r.RoleName.ToLower()).FirstOrDefault();
                roleData.Add(result);
            }
            if (employeeIds.Where(x => x == employeeId).Any())
            {
                if (roleData == null)
                {
                    roleData = new List<string>();
                }
                roleData.Add("manager");
            }
            data.Role = roleData;
            return data;
        }
        private List<int> GetAllEmployeesForManagerRecursively(List<int> employeeId, bool isAll)
        {
            List<int> recList = dbContext.Employees.Where(x => x.IsActive == true && x.EmployeeID != (int)x.ReportingManagerId
                                && x.ReportingManagerId.HasValue && employeeId.Contains((int)x.ReportingManagerId)).Select(x => x.EmployeeID).Distinct().ToList();
            if (recList != null && recList?.Count > 0)
            {
                if (i == 0)
                {
                    employeeIds = new List<int>();
                    employeeIds = employeeIds.Concat(recList).ToList();
                }
                else
                {
                    employeeIds = employeeIds.Concat(recList).ToList();
                }
                i++;
                if (isAll)
                {
                    GetAllEmployeesForManagerRecursively(recList, isAll);
                }
            }
            if (i == 0)
            {
                employeeIds = new List<int> { 0 };
            }
            return employeeIds;
        }
        public List<EmployeeShiftDetailsView> GetEmployeeShiftDetails(int employeeID)
        {
            List<EmployeeShiftDetailsView> employeeShiftDetailsView = (from shiftDetails in dbContext.EmployeeShiftDetails
                                                                       where shiftDetails.EmployeeID == employeeID
                                                                       select new EmployeeShiftDetailsView
                                                                       {
                                                                           EmployeeID = (int)shiftDetails.EmployeeID,
                                                                           ShiftDetailsId = shiftDetails.ShiftDetailsId,
                                                                           ShiftFromDate = shiftDetails.ShiftFromDate,
                                                                           ShiftToDate = shiftDetails.ShiftToDate
                                                                       }).ToList();
            return employeeShiftDetailsView;
        }
        public List<ReportingManagerEmployeeList> GetManagerEmployeeList(string pRole = "")
        {
            if (pRole == null) pRole = "";
            if (pRole != "")
            {
                return (from users in dbContext.Employees
                        join roles in dbContext.Roles on users.RoleId equals roles.RoleId
                        where roles.RoleName == pRole && users.IsActive == true
                        select new ReportingManagerEmployeeList
                        {
                            EmployeeId = users.EmployeeID,
                            FormattedEmployeeId = users.FormattedEmployeeId,
                            EmployeeName = (users.FirstName + " " + users.LastName),
                            EmployeeEmailId = users.EmailAddress,
                            ReportingManagerId = users.ReportingManagerId,
                            ReportingTo = dbContext.Employees.Where(es => es.EmployeeID == users.ReportingManagerId).Select(esr => esr.FirstName + " " + esr.LastName).FirstOrDefault(),
                            DesignationName = dbContext.Designation.Where(es => es.DesignationId == users.DesignationId).Select(esr => esr.DesignationName).FirstOrDefault(),
                        }).OrderBy(x => x.EmployeeName).ToList();
            }
            return dbContext.Employees.Where(x => x.IsActive == true).Select(y => new ReportingManagerEmployeeList
            {
                EmployeeId = y.EmployeeID,
                FormattedEmployeeId = y.FormattedEmployeeId,
                EmployeeName = y.FirstName + " " + y.LastName,
                EmployeeEmailId = y.EmailAddress,
                ReportingManagerId = y.ReportingManagerId,
                ReportingTo = dbContext.Employees.Where(es => es.EmployeeID == y.ReportingManagerId).Select(esr => esr.FirstName + " " + esr.LastName).FirstOrDefault(),
                ReportingEmailId = dbContext.Employees.Where(es => es.EmployeeID == y.ReportingManagerId).Select(esr => esr.EmailAddress).FirstOrDefault(),
                DesignationName = dbContext.Designation.Where(es => es.DesignationId == y.DesignationId).Select(esr => esr.DesignationName).FirstOrDefault(),

            }).OrderBy(x => x.EmployeeName).ToList();
        }
        public AppraisalManagerEmployeeDetailsView AppraisalManagerEmployeeDetails(int managerId)
        {
            return dbContext.Employees.Where(x => x.EmployeeID == managerId).Select(y => new AppraisalManagerEmployeeDetailsView
            {
                ReportingManagerId = y.EmployeeID,
                ReportingManagerDeptID = y.DepartmentId,
                ReportingManagerRoleID = y.RoleId,
                LastName = y.LastName,
                FirstName = y.FirstName,
                ReportingManagerName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId
            }).FirstOrDefault();
        }
        public List<AppraisalManagerEmployeeDetailsView> GetEmployeesByPermanentandActive()
        {
            return dbContext.Employees.Where(x => x.IsActive == true && x.EmployeeTypeId == 1).Select(y => new AppraisalManagerEmployeeDetailsView
            {
                EmployeeID = y.EmployeeID,
                LastName = y.LastName,
                FirstName = y.FirstName,
                ReportingManagerId = y.ReportingManagerId,
                ReportingManagerName = dbContext.Employees.Where(e => e.EmployeeID == y.ReportingManagerId).Select(e => e.FirstName + " " + e.LastName).FirstOrDefault(),
                ReportingManagerRoleID = dbContext.Employees.Where(e => e.EmployeeID == y.ReportingManagerId).Select(e => e.RoleId).FirstOrDefault(),
                ReportingManagerDeptID = dbContext.Employees.Where(e => e.EmployeeID == y.ReportingManagerId).Select(e => e.DepartmentId).FirstOrDefault(),
                FormattedEmployeeId = y.FormattedEmployeeId
            }).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
        }
        public EmployeeandManagerView GetEmployeeandManagerByEmployeeID(int employeeID)
        {
            return (from emp in dbContext.Employees
                    join empmang in dbContext.Employees
                    on emp.ReportingManagerId equals empmang.EmployeeID
                    where emp.EmployeeID == employeeID
                    select new EmployeeandManagerView
                    {
                        EmployeeID = emp.EmployeeID,
                        ReportingManagerID = emp.ReportingManagerId,
                        EmployeeName = emp.FirstName + " " + emp.LastName,
                        ManagerName = empmang.FirstName + " " + empmang.LastName,
                        EmployeeEmailID = emp.EmailAddress,
                        ManagerEmailID = empmang.EmailAddress,
                        FormattedEmployeeId = emp.FormattedEmployeeId,
                        LocationId = emp.LocationId,
                        DepartmentId = emp.DepartmentId,
                        RelivingDate = emp.DateOfRelieving
                    }
                    ).FirstOrDefault();
        }
        public EmployeeCategoryView GetEmployeeCategoryDetailsByEmployeeId(int employeeId, int employeeCategoryId)
        {
            return (from empcategory in dbContext.EmployeeCategory
                    join Employee in dbContext.Employees on empcategory.DepartmentId equals Employee.DepartmentId
                    where Employee.EmployeeID == employeeId && Employee.EmployeeCategoryId == empcategory.EmployeeCategoryId
                    select new EmployeeCategoryView
                    {
                        EmployeeCategoryId = empcategory.EmployeeCategoryId,
                        EmployeeCategoryName = empcategory.EmployeeCategoryName,
                        DepartmentId = empcategory.DepartmentId,
                        Description = empcategory.Description
                    }).FirstOrDefault();
        }
        public List<EmployeeandManagerView> GetEmployeesListByDepartment(EmployeeListByDepartment employeeView)
        {
            return (from emp in dbContext.Employees
                    join empmang in dbContext.Employees
                    on emp.ReportingManagerId equals empmang.EmployeeID
                    where employeeView.EmployeeId.Contains(emp.EmployeeID) && emp.DepartmentId == employeeView.DepartmentId
                    select new EmployeeandManagerView
                    {
                        EmployeeID = emp.EmployeeID,
                        ReportingManagerID = emp.ReportingManagerId,
                        EmployeeName = emp.FirstName + " " + emp.LastName,
                        ManagerName = empmang.FirstName + " " + empmang.LastName,
                        EmployeeEmailID = emp.EmailAddress,
                        ManagerEmailID = empmang.EmailAddress,
                        FormattedEmployeeId = emp.FormattedEmployeeId
                    }).OrderBy(x => x.EmployeeName).ToList();
        }
        public EmployeeDepartmentAndLocationView GetEmployeeDepartmentAndLocation(int employeeId)
        {

            return (from emp in dbContext.Employees
                    where emp.EmployeeID == employeeId
                    select new EmployeeDepartmentAndLocationView
                    {
                        employeeId = emp.EmployeeID,
                        DepartmentId = emp.DepartmentId == null ? 0 : (int)emp.DepartmentId,
                        LocationId = emp.LocationId == null ? 0 : (int)emp.LocationId,
                        ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == emp.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId == null ? 0 : (int)x.ShiftDetailsId).FirstOrDefault(),
                        //ShiftId = empshift.ShiftDetailsId==null?0:(int)empshift.ShiftDetailsId,
                        FormattedEmployeeId = emp.FormattedEmployeeId,
                        DOJ = emp.DateOfJoining == null ? DateTime.MinValue : emp.DateOfJoining.Value.Date
                    }).FirstOrDefault();
        }
        public List<EmployeeList> GetEmployeeListByManagerId(int? employeeId)
        {
            if (employeeId == 0)
            {
                return (from emp in dbContext.Employees
                        where emp.IsActive == true
                        select new EmployeeList
                        {
                            //EmployeeId = emp.EmployeeID,
                            //FormattedEmployeeId = emp.FormattedEmployeeId,
                            //ManagerId = emp.ReportingManagerId
                            EmployeeId = emp.EmployeeID,
                            EmpName = emp.FirstName + " " + emp.LastName,
                            FormattedEmployeeId = emp.FormattedEmployeeId,
                            Employee_Id = emp.FormattedEmployeeId,
                            EmployeeName = emp.FormattedEmployeeId + " " + (emp.FirstName + " " + emp.LastName),
                            EmployeeEmailId = emp.EmailAddress,
                            DepartmentId = emp.DepartmentId,
                            LocationId = emp.LocationId,
                            ManagerId = emp.ReportingManagerId,
                            DOJ = emp.DateOfJoining,
                            ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == emp.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault(),
                            employeeShiftDetails = dbContext.EmployeeShiftDetails.Where(y => y.EmployeeID == emp.EmployeeID).Select(y => new EmployeeShiftDetailsView
                            {
                                EmployeeID = y.EmployeeID == null ? 0 : (int)y.EmployeeID,
                                ShiftDetailsId = y.ShiftDetailsId,
                                ShiftFromDate = y.ShiftFromDate,
                                ShiftToDate = y.ShiftToDate
                            }).ToList(),
                        }).ToList();
            }
            else
            {
                return (from emp in dbContext.Employees
                        where (emp.ReportingManagerId == employeeId || emp.EmployeeID == employeeId) && emp.IsActive == true
                        select new EmployeeList
                        {
                            //EmployeeId = emp.EmployeeID,
                            //FormattedEmployeeId = emp.FormattedEmployeeId,
                            //ManagerId = emp.ReportingManagerId
                            EmployeeId = emp.EmployeeID,
                            FormattedEmployeeId = emp.FormattedEmployeeId,
                            Employee_Id = emp.FormattedEmployeeId,
                            EmpName = emp.FirstName + " " + emp.LastName,
                            EmployeeName = emp.FormattedEmployeeId + " " + (emp.FirstName + " " + emp.LastName),
                            EmployeeEmailId = emp.EmailAddress,
                            DepartmentId = emp.DepartmentId,
                            LocationId = emp.LocationId,
                            ManagerId = emp.ReportingManagerId,
                            ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == emp.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault(),
                            employeeShiftDetails = dbContext.EmployeeShiftDetails.Where(y => y.EmployeeID == emp.EmployeeID).Select(y => new EmployeeShiftDetailsView
                            {
                                EmployeeID = y.EmployeeID == null ? 0 : (int)y.EmployeeID,
                                ShiftDetailsId = y.ShiftDetailsId,
                                ShiftFromDate = y.ShiftFromDate,
                                ShiftToDate = y.ShiftToDate,
                            }).ToList(),
                        }).ToList();
            }
        }
        public string GetDesignationById(int? designationId)
        {
            return dbContext.Designation.Where(x => x.DesignationId == designationId).Select(x => x.DesignationName).FirstOrDefault();
        }


        public List<EmployeeAttendanceDetails> GetAllEmployeesAttendanceDetails(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            int resouceCount = employeesAttendanceFilterView?.ResourceId?.Count == 0 ? 0 : employeesAttendanceFilterView.ResourceId.Count;
            int departmentCount = employeesAttendanceFilterView?.DepartmentId?.Count == 0 ? 0 : employeesAttendanceFilterView.DepartmentId.Count;
            int designationCount = employeesAttendanceFilterView?.DesignationId?.Count == 0 ? 0 : employeesAttendanceFilterView.DesignationId.Count;
            int locationCount = employeesAttendanceFilterView?.LocationId?.Count == 0 ? 0 : employeesAttendanceFilterView.LocationId.Count;
            return (from users in dbContext.Employees
                    join dept in dbContext.Department on users.DepartmentId equals dept.DepartmentId into jnDeptDetails
                    from dept in jnDeptDetails.DefaultIfEmpty()
                    join loc in dbContext.EmployeeLocation on users.LocationId equals loc.LocationId into jnlocDetails
                    from loc in jnlocDetails.DefaultIfEmpty()
                    join desg in dbContext.Designation on users.DesignationId equals desg.DesignationId into jndesgDetails
                    from desg in jndesgDetails.DefaultIfEmpty()
                    where users.IsActive == true && users.ReportingManagerId == (employeesAttendanceFilterView.EmployeeId == 0 ? users.ReportingManagerId : employeesAttendanceFilterView.EmployeeId)
                    && users.DateOfJoining.Value.Date <= employeesAttendanceFilterView.ToDate.Date
                    && (resouceCount == 0 || employeesAttendanceFilterView.ResourceId.Contains(users.EmployeeID))
                    && (departmentCount == 0 || employeesAttendanceFilterView.DepartmentId.Contains(users.DepartmentId == null ? 0 : (int)users.DepartmentId))
                    && (designationCount == 0 || employeesAttendanceFilterView.DesignationId.Contains(users.DesignationId == null ? 0 : (int)users.DesignationId))
                    && (locationCount == 0 || employeesAttendanceFilterView.LocationId.Contains(users.LocationId == null ? 0 : (int)users.LocationId))
                    select new EmployeeAttendanceDetails()
                    {
                        EmployeeId = users.EmployeeID,
                        EmployeeName = users.FirstName + " " + users.LastName,
                        Department = dept != null ? dept.DepartmentName : string.Empty,
                        Location = loc != null ? loc.Location : string.Empty,
                        Designation = desg != null ? desg.DesignationName : string.Empty,
                        ShiftDetailId = (int)dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == users.EmployeeID
                                        && employeesAttendanceFilterView.FromDate.Date >= x.ShiftFromDate
                                        && (x.ShiftToDate == null || employeesAttendanceFilterView.FromDate.Date <= x.ShiftToDate))
                                        .Select(x => x.ShiftDetailsId).FirstOrDefault(),
                        EmployeeEmailId = users.EmailAddress,
                        FormattedEmployeeId = users.FormattedEmployeeId,
                    }).OrderBy(x => x.EmployeeName).Skip(employeesAttendanceFilterView.NoOfRecord * (employeesAttendanceFilterView.PageNumber))
                        .Take(employeesAttendanceFilterView.NoOfRecord).Distinct().ToList();
        }
        public List<ProbationStatusView> GetAllProbationStatusDetails()
        {
            return dbContext.ProbationStatus.Select(x => new ProbationStatusView { ProbationStatusId = x.ProbationStatusId, ProbationStatusName = x.ProbationStatusName }).ToList();
        }
        public string GetNextFormattedEmployeeId(string pEmployeeIdFormat)
        {
            string FormattedEmployeeId = ""; int employeeID = 0;
            Employees employee = dbContext.Employees.OrderBy(x => x.EmployeeID).LastOrDefault();
            if (employee != null) FormattedEmployeeId = employee?.FormattedEmployeeId;
            if (!string.IsNullOrEmpty(FormattedEmployeeId))
            {
                string[] FormattedEmployeeIds = FormattedEmployeeId?.Split(" ");
                if (FormattedEmployeeIds?.Length > 1 && FormattedEmployeeIds[1] != "")
                {
                    string empId = FormattedEmployeeIds[1];
                    employeeID = Convert.ToInt32(empId);
                }
            }
            else
            {
                return "";
            }
            pEmployeeIdFormat += (employeeID + 1).ToString();
            return pEmployeeIdFormat;
        }
        public Employees GetEmployeeByEmployeeId(int employeeID)
        {
            return dbContext.Employees.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
        }
        public Employees GetEmployeeByFormattedEmployeeId(string pFormattedEmployeeId, int employeeId)
        {
            if (employeeId == 0)
                return dbContext.Employees.Where(x => x.FormattedEmployeeId == pFormattedEmployeeId).FirstOrDefault();
            else
                return dbContext.Employees.Where(x => x.FormattedEmployeeId == pFormattedEmployeeId && x.EmployeeID != employeeId).FirstOrDefault();
        }
        public Employees CheckEmployeeByemailId(string emailAddress, int employeeId)
        {
            if (employeeId == 0)
                return dbContext.Employees.Where(x => x.EmailAddress == emailAddress).FirstOrDefault();
            else
                return dbContext.Employees.Where(x => x.EmailAddress == emailAddress && x.EmployeeID != employeeId).FirstOrDefault();
        }
        public GrantLeaveApproverView GetGrantLeaveApprover(int employeeId, string hrDepartmentName)
        {
            GrantLeaveApproverView grantLeaveApproverView = new();
            Employees employee = dbContext.Employees.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
            if (employee != null)
            {
                grantLeaveApproverView.ReportingManagerEmployeeId = employee?.ReportingManagerId;
                grantLeaveApproverView.DepartmentHeadEmployeeId = (from emp in dbContext.Employees
                                                                   join emc in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals emc.EmployeeCategoryId
                                                                   where emc.EmployeeCategoryName == "BU Head" && emc.DepartmentId == employee.DepartmentId
                                                                   select emp.EmployeeID
                                                                 ).FirstOrDefault();
            }
            grantLeaveApproverView.HRHeadEmployeeId = (from emp in dbContext.Employees
                                                       join dep in dbContext.Department on emp.DepartmentId equals dep.DepartmentId
                                                       join emc in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals emc.EmployeeCategoryId
                                                       where emc.EmployeeCategoryName == "BU Head" && dep.DepartmentName == hrDepartmentName
                                                       select emp.EmployeeID
                                                             ).FirstOrDefault();
            grantLeaveApproverView.RelivingDate = employee.DateOfRelieving;
            return grantLeaveApproverView;
        }
        public string GetLocationNameByLocationId(int? locationId)
        {
            return dbContext.EmployeeLocation.Where(y => y.LocationId == locationId).Select(x => x.Location).FirstOrDefault();
        }
        public int? GetShiftByemployeeId(int? employeeId)
        {
            //return dbContext.EmployeeShiftDetails.Where(y => y.EmployeeID == employeeId && DateTime.Now.Date>=y.ShiftFromDate && (y.ShiftToDate == null || DateTime.Now.Date<= y.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault();
            List<EmployeeShiftDetails> shiftList = dbContext.EmployeeShiftDetails.Where(y => y.EmployeeID == employeeId).ToList();
            if (shiftList?.Count > 0)
            {
                foreach (EmployeeShiftDetails item in shiftList)
                {
                    if (item?.ShiftFromDate != null && item?.ShiftToDate != null)
                    {
                        if (DateTime.Now.Date >= item?.ShiftFromDate && DateTime.Now.Date <= item?.ShiftToDate)
                        {
                            return item?.ShiftDetailsId;
                        }
                    }
                    else if (item?.ShiftFromDate != null && item?.ShiftToDate == null)
                    {
                        if (DateTime.Now.Date >= item?.ShiftFromDate)
                        {
                            return item?.ShiftDetailsId;
                        }
                    }
                }
            }
            else
            {
                return 0;
            }
            return 0;
        }
        public List<EmployeeShiftDetailsView> GetShiftListByemployeeId(int EmployeeID)
        {
            List<EmployeeShiftDetailsView> shiftList = new();
            return shiftList = dbContext.EmployeeShiftDetails.Where(y => y.EmployeeID == EmployeeID).Select(y => new EmployeeShiftDetailsView
            {
                EmployeeID = y.EmployeeID == null ? 0 : (int)y.EmployeeID,
                ShiftDetailsId = y.ShiftDetailsId,
                ShiftFromDate = y.ShiftFromDate,
                ShiftToDate = y.ShiftToDate,
            }).ToList();
        }
        public EmployeeShiftDetails GetShifByDate(int EmployeeID, DateTime date)
        {
            return dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == EmployeeID && date >= x.ShiftFromDate && (x.ShiftToDate == null || date <= x.ShiftToDate)).FirstOrDefault();
        }
        public List<Employees> GetEmployees(EmployeeListView employee)
        {
            return dbContext.Employees.Where(x => x.IsActive == true && x.DateOfJoining <= employee.ToDate).OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
        }

        public List<EmployeeLeaveAdjustmentView> GetEmployeeLeaveAdjustmentListWithFilter(EmployeeLeaveAdjustmentFilterView employeeLeaveAdjustmentView)
        {
            int resouceCount = employeeLeaveAdjustmentView?.ResourceId?.Count == 0 ? 0 : employeeLeaveAdjustmentView.ResourceId.Count;
            int departmentCount = employeeLeaveAdjustmentView?.DepartmentId?.Count == 0 ? 0 : employeeLeaveAdjustmentView.DepartmentId.Count;
            int designationCount = employeeLeaveAdjustmentView?.DesignationId?.Count == 0 ? 0 : employeeLeaveAdjustmentView.DesignationId.Count;
            return (from users in dbContext.Employees
                    join dept in dbContext.Department on users.DepartmentId equals dept.DepartmentId into jnDeptDetails
                    from dept in jnDeptDetails.DefaultIfEmpty()
                    join desg in dbContext.Designation on users.DesignationId equals desg.DesignationId into jndesgDetails
                    from desg in jndesgDetails.DefaultIfEmpty()
                    where users.IsActive == true && users.DateOfJoining.Value.Date <= employeeLeaveAdjustmentView.ToDate.Date
                    && (resouceCount == 0 || employeeLeaveAdjustmentView.ResourceId.Contains(users.EmployeeID))
                    && users.ReportingManagerId == (employeeLeaveAdjustmentView.EmployeeId == 0 ? users.ReportingManagerId : employeeLeaveAdjustmentView.EmployeeId)
                    && (departmentCount == 0 || employeeLeaveAdjustmentView.DepartmentId.Contains(users.DepartmentId == null ? 0 : (int)users.DepartmentId))
                    && (designationCount == 0 || employeeLeaveAdjustmentView.DesignationId.Contains(users.DesignationId == null ? 0 : (int)users.DesignationId))
                    select new EmployeeLeaveAdjustmentView()
                    {
                        EmployeeID = users.EmployeeID,
                        FormattedEmployeeId = users.FormattedEmployeeId,
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        EmployeeName = users.FirstName + " " + users.LastName,
                        DepartmentId = users.DepartmentId,
                        Department = dept != null ? dept.DepartmentName : string.Empty,
                        Designation = desg != null ? desg.DesignationName : string.Empty,
                        DesignationId = users.DesignationId,
                        DOJ = users.DateOfJoining,
                        LeaveBalance = 0,
                    }).OrderBy(x => x.EmployeeName).Skip(employeeLeaveAdjustmentView.NoOfRecord * (employeeLeaveAdjustmentView.PageNumber))
                        .Take(employeeLeaveAdjustmentView.NoOfRecord).Distinct().ToList();
        }
        public EmployeeandManagerView GetEmployeeAndApproverDetails(int employeeId, int approverId)
        {
            EmployeeandManagerView managerView = new();
            managerView.EmployeeID = dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x.EmployeeID).FirstOrDefault();
            managerView.EmployeeName = dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
            managerView.EmployeeEmailID = dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x.EmailAddress).FirstOrDefault();

            managerView.ReportingManagerID = dbContext.Employees.Where(x => x.EmployeeID == approverId).Select(x => x.EmployeeID).FirstOrDefault();
            managerView.ManagerName = dbContext.Employees.Where(x => x.EmployeeID == approverId).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
            managerView.ManagerEmailID = dbContext.Employees.Where(x => x.EmployeeID == approverId).Select(x => x.EmailAddress).FirstOrDefault();

            return managerView;
        }
        public EmployeeDetailsForLeaveView GetEmployeeDetailsById(int employeeId)
        {

            return dbContext.Employees.Where(x => x.IsActive == true && x.EmployeeID == employeeId).Select(x => new EmployeeDetailsForLeaveView
            {
                EmployeeID = x.EmployeeID,
                EmployeeTypeID = x.EmployeeTypeId,
                DepartmentID = x.DepartmentId,
                RoleID = x.RoleId,
                IsActive = x.IsActive,
                Gender = x.Gender,
                LocationID = x.LocationId,
                MaritalStatus = x.Maritalstatus,
                DesignationID = x.DesignationId,
                DateOfJoining = x.DateOfJoining,
                DateOfContract = x.DateOfContract,
                ProbationStatusID = x.ProbationStatusId,
                SystemRoleID = x.SystemRoleId
            }).FirstOrDefault();
        }
        public EmployeeName GetEmployeeNameByEmployeeId(int employeeId)
        {
            return dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(y => new
                              EmployeeName
            {
                EmployeeId = y.EmployeeID,
                EmployeeFullName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId,
                EmployeeEmailId = y.EmailAddress
            }).FirstOrDefault();
        }
        public EmployeeManagerAndHeadDetailsView GetEmployeeManagerAndHeadDetails(int employeeId, int approverId)
        {
            EmployeeManagerAndHeadDetailsView employeeManagerAndHead = new EmployeeManagerAndHeadDetailsView();
            Employees employee = dbContext.Employees.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
            Employees manager = dbContext.Employees.Where(x => x.EmployeeID == employee.ReportingManagerId).FirstOrDefault();
            Employees firstLevelApprover = dbContext.Employees.Where(x => x.EmployeeID == approverId).FirstOrDefault();
            //EmployeeName employeeName = new EmployeeName();
            //employeeManagerAndHead.employee = GetEmployeeNameByEmployeeId(employeeId);
            employeeManagerAndHead.EmployeeId = employee.EmployeeID;
            employeeManagerAndHead.EmployeeEmailAddress = employee.EmailAddress;
            employeeManagerAndHead.ManagerId = employee.ReportingManagerId == null ? 0 : (int)employee.ReportingManagerId;
            employeeManagerAndHead.EmployeeFullName = employee.FirstName + " " + employee.LastName;
            employeeManagerAndHead.FormattedEmployeeId = employee.FormattedEmployeeId;
            //employeeManagerAndHead.ManagerId = (int)dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x.ReportingManagerId).FirstOrDefault();
            //employeeName = GetEmployeeNameByEmployeeId(employeeManagerAndHead.ManagerId);
            employeeManagerAndHead.MangerName = manager.FirstName + " " + manager.LastName;
            employeeManagerAndHead.ManagerFormattedId = manager.FormattedEmployeeId;
            employeeManagerAndHead.DepartmentId = employee.DepartmentId == null ? 0 : (int)employee.DepartmentId;
            employeeManagerAndHead.DepartmentName = dbContext.Department.Where(x => x.DepartmentId == employee.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
            employeeManagerAndHead.ManagerEmail = manager.EmailAddress;

            employeeManagerAndHead.BUHeadId = (from emp in dbContext.Employees
                                               join emc in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals emc.EmployeeCategoryId
                                               where emc.EmployeeCategoryName == "BU Head" && emc.DepartmentId == employee.DepartmentId
                                               select emp.EmployeeID
                                                                             ).FirstOrDefault();
            employeeManagerAndHead.BUHeadEmail = dbContext.Employees.Where(x => x.EmployeeID == employeeManagerAndHead.BUHeadId && x.IsActive == true).Select(x => x.EmailAddress).FirstOrDefault();
            employeeManagerAndHead.ApproverEmail = firstLevelApprover.EmailAddress;
            employeeManagerAndHead.ApproverName = firstLevelApprover.FirstName + " " + firstLevelApprover.LastName;
            return employeeManagerAndHead;
        }
        public List<ResignedEmployeeView> GetEmployeeDetailsById(List<int> listEmployeeId)
        {
            return dbContext.Employees.Where(x => listEmployeeId.Contains(x.EmployeeID)).Select(y => new
                            ResignedEmployeeView
            {
                EmployeeId = y.EmployeeID,
                EmployeeName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId,
                DesignationId = (int)y.DesignationId,
                DesignationName = dbContext.Designation.Where(x => x.DesignationId == y.DesignationId).Select(x => x.DesignationName).FirstOrDefault(),
                EmployeeEmailId = y.EmailAddress
            }).ToList();
        }
        public List<SystemRoles> GetSystemRolesList()
        {
            return dbContext.SystemRoles.ToList();
        }
        public Employees GetEmployeeDetailByEmployeeId(int employeeId)
        {
            return dbContext.Employees.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
        }
        public List<EmployeeResignationDetailsView> GetEmployeeResignationDetails(List<EmployeeResignationDetailsView> employeeDetails)
        {
            foreach (EmployeeResignationDetailsView item in employeeDetails)
            {
                Employees employee = dbContext.Employees.Where(x => x.EmployeeID == item.EmployeeId).Select(x => x).FirstOrDefault();
                item.EmployeeName = employee?.EmployeeName;
                item.FormattedEmployeeId = employee?.FormattedEmployeeId;
                //item.DepartmentName = dbContext.Department.Where(x => x.DepartmentId == item.DepartmentID).Select(x => x.DepartmentName).FirstOrDefault();
                //item.Location = dbContext.EmployeeLocation.Where(x => x.LocationId == item.LocationId).Select(x => x.Location).FirstOrDefault();
                item.ReportingManager = dbContext.Employees.Where(x => x.EmployeeID == item.ReportingManagerID).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault();
                //item.ModifiedByName = dbContext.Employees.Where(x => x.EmployeeID == item.ModifiedBy).Select(x => x.EmployeeName).FirstOrDefault();
                item.DateOfJoining = employee?.DateOfJoining;
                item.IsActiveEmployee = employee?.IsActive == null ? false : (bool)employee.IsActive;
                item.EmployeeType = item.IsActiveEmployee == true ? "Active Employee" : "Ex Employee";
                foreach (ResignationApprovalStatusView approver in item.ResignationApproverList)
                {
                    approver.ApproverEmployeeName = dbContext.Employees.Where(x => x.EmployeeID == approver.ApproverEmployeeId).Select(x => x.EmployeeName).FirstOrDefault();
                    approver.ApprovedByEmployeeName = dbContext.Employees.Where(x => x.EmployeeID == approver.ApproveById).Select(x => x.EmployeeName).FirstOrDefault();
                }
            }
            return employeeDetails;
        }
        public ResignationApproverView GetResignationApprover(int employeeId, string hrDepartmentName)
        {
            ResignationApproverView resignationApproverView = new();
            Employees employee = dbContext.Employees.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
            Employees manager = dbContext.Employees.Where(x => x.EmployeeID == employee.ReportingManagerId).FirstOrDefault();
            if (employee != null)
            {
                resignationApproverView.ReportingManagerEmployeeId = employee?.ReportingManagerId;
                resignationApproverView.DepartmentHeadEmployeeId = (from emp in dbContext.Employees
                                                                    join emc in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals emc.EmployeeCategoryId
                                                                    where emc.EmployeeCategoryName == "BU Head" && emc.DepartmentId == employee.DepartmentId
                                                                    select emp.EmployeeID
                                                                 ).FirstOrDefault();
            }
            resignationApproverView.HRHeadEmployeeId = (from emp in dbContext.Employees
                                                        join dep in dbContext.Department on emp.DepartmentId equals dep.DepartmentId
                                                        join emc in dbContext.EmployeeCategory on emp.EmployeeCategoryId equals emc.EmployeeCategoryId
                                                        where emc.EmployeeCategoryName == "BU Head" && dep.DepartmentName == hrDepartmentName
                                                        select emp.EmployeeID
                                                             ).FirstOrDefault();
            resignationApproverView.NoticePeriod = CalculateNoticePeriod(employee.DateOfJoining, employee.ProbationStatusId, employee.NoticeCategory);
            return resignationApproverView;
        }
        #region Get ResignationEmployee MasterData
        public List<ResignationEmployeeMasterView> GetResignationEmployeeMasterData(List<int> employeeId)
        {
            if (employeeId?.Count > 0)
            {
                return dbContext.Employees.Where(x => employeeId.Contains(x.EmployeeID)).Select(
                   x => new ResignationEmployeeMasterView
                   {
                       EmployeeID = x.EmployeeID,
                       EmployeeName = x.EmployeeName + '-' + x.FormattedEmployeeId,
                       FormattedEmployeeID = x.FormattedEmployeeId,
                       Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                       DepartmentId = x.DepartmentId,
                       LocationId = x.LocationId,
                       DesignationId = x.DesignationId,
                       RelievingDate = x.DateOfRelieving,
                       ResignationDate = x.ResignationDate,
                       ReportingManagerId = x.ReportingManagerId == null ? 0 : (int)x.ReportingManagerId,
                       ReportingManagerName = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => x.EmployeeName).FirstOrDefault(),
                       ReportingManagerEmail = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => x.EmailAddress).FirstOrDefault(),
                       DateOfJoining = x.DateOfJoining,
                       DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                       EmployeeEmail = x.EmailAddress,
                       FirstName = x.FirstName,
                       LastName = x.LastName
                   }).OrderBy(x => x.EmployeeName).ToList();
            }
            else
            {
                return dbContext.Employees.Select(
                    x => new ResignationEmployeeMasterView
                    {
                        EmployeeID = x.EmployeeID,
                        EmployeeName = x.EmployeeName + '-' + x.FormattedEmployeeId,
                        FormattedEmployeeID = x.FormattedEmployeeId,
                        Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                        DepartmentId = x.DepartmentId,
                        LocationId = x.LocationId,
                        DesignationId = x.DesignationId,
                        RelievingDate = x.DateOfRelieving,
                        DateOfJoining = x.DateOfJoining,
                        ResignationDate = x.ResignationDate,
                        ReportingManagerId = x.ReportingManagerId == null ? 0 : (int)x.ReportingManagerId,
                        ReportingManagerName = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => x.EmployeeName).FirstOrDefault(),
                        DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                        EmployeeEmail = x.EmailAddress,
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    }).OrderBy(x => x.EmployeeName).ToList();
            }
        }
        #endregion
        public List<ResignationInterviewDetailView> GetEmployeeExitInterviewDetails(List<ResignationInterviewDetailView> employeeDetails)
        {
            foreach (ResignationInterviewDetailView item in employeeDetails)
            {
                Employees employee = dbContext.Employees.Where(x => x.EmployeeID == item.EmployeeID).Select(x => x).FirstOrDefault();
                item.EmployeeName = employee?.EmployeeName;
                item.FormattedEmployeeId = employee?.FormattedEmployeeId;
                item.DOJ = employee.DateOfJoining;
                item.Department = dbContext.Department.Where(x => x.DepartmentId == employee.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                item.Designation = dbContext.Designation.Where(x => x.DesignationId == employee.DesignationId).Select(x => x.DesignationName).FirstOrDefault();
                item.EmployeeType = employee?.IsActive == true ? "Active Employee" : "Ex Employee";
                item.ResignationDate = employee?.ResignationDate;
            }
            return employeeDetails;
        }

        public DateTime? GetEmployeeResignationDate(int employeeId)
        {
            return dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x.ResignationDate).FirstOrDefault();
        }

        #region Get Exit interview Employee MasterData
        public List<ResignationEmployeeMasterView> GetExitEmployeeMaster(List<int> employeeId)
        {
            if (employeeId?.Count > 0)
            {
                return dbContext.Employees.Where(x => employeeId.Contains(x.EmployeeID) && x.ResignationDate != null).Select(
                   x => new ResignationEmployeeMasterView
                   {
                       EmployeeID = x.EmployeeID,
                       EmployeeName = x.EmployeeName + '-' + x.FormattedEmployeeId,
                       FormattedEmployeeID = x.FormattedEmployeeId,
                       Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                       DepartmentId = x.DepartmentId,
                       LocationId = x.LocationId,
                       DesignationId = x.DesignationId,
                       RelievingDate = x.DateOfRelieving,
                       ReportingManagerId = x.ReportingManagerId == null ? 0 : (int)x.ReportingManagerId,
                       ReportingManagerName = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => x.EmployeeName).FirstOrDefault(),
                       DateOfJoining = x.DateOfJoining,
                       DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                       EmployeeEmail = x.EmailAddress,
                       ResignationDate = x.ResignationDate,
                       EmployeeType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(z => z.EmployeesType).FirstOrDefault()

                   }).OrderBy(x => x.EmployeeName).ToList();
            }
            else
            {
                return dbContext.Employees.Where(x => x.ResignationDate != null).Select(
                    x => new ResignationEmployeeMasterView
                    {
                        EmployeeID = x.EmployeeID,
                        EmployeeName = x.EmployeeName + '-' + x.FormattedEmployeeId,
                        FormattedEmployeeID = x.FormattedEmployeeId,
                        Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                        DepartmentId = x.DepartmentId,
                        LocationId = x.LocationId,
                        DesignationId = x.DesignationId,
                        RelievingDate = x.DateOfRelieving,
                        DateOfJoining = x.DateOfJoining,
                        ReportingManagerId = x.ReportingManagerId == null ? 0 : (int)x.ReportingManagerId,
                        ReportingManagerName = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => x.EmployeeName).FirstOrDefault(),
                        DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                        EmployeeEmail = x.EmailAddress,
                        ResignationDate = x.ResignationDate,
                        EmployeeType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(z => z.EmployeesType).FirstOrDefault()
                    }).OrderBy(x => x.EmployeeName).ToList();
            }
        }
        #endregion
        #region Get employee by systemRole

        public List<int> GetEmployeesListBySystemRole(string sRole = "")
        {

            if (sRole == null) sRole = "";
            if (sRole != "")
            {
                return (from users in dbContext.Employees
                        join roles in dbContext.SystemRoles on users.SystemRoleId equals roles.RoleId
                        where roles.RoleName == sRole && users.IsActive == true
                        select users.EmployeeID).ToList();
            }
            return new List<int>();
        }
        #endregion
        #region Get checklist employee role
        public List<string> GetExitCheckListRole(int employeeId, int loginUserId, bool isAllReportees)
        {
            employeeIds = new List<int> { loginUserId };
            GetAllEmployeesForManagerRecursively(employeeIds, isAllReportees);
            employeeIds.Add(loginUserId);
            List<string> data = new List<string>();
            if (isAllReportees)
            {
                data = (from e in dbContext.Employees
                        join r in dbContext.SystemRoles on e.SystemRoleId equals r.RoleId
                        where employeeIds.Contains(e.EmployeeID)
                        select r.RoleName == null ? "" : r.RoleName.ToLower()).Distinct().ToList();
            }
            else
            {
                string result = (from e in dbContext.Employees
                                 join r in dbContext.SystemRoles on e.SystemRoleId equals r.RoleId
                                 where e.EmployeeID == loginUserId
                                 select r.RoleName == null ? "" : r.RoleName.ToLower()).FirstOrDefault();
                data.Add(result);
            }
            if (employeeIds.Where(x => x == employeeId).Any())
            {
                if (data == null)
                {
                    data = new List<string>();
                }
                data.Add("manager");
            }
            return data;
        }
        #endregion
        #region Get checklist employee list
        public List<EmployeeViewDetails> GetCheckListEmployeeList(int employeeId, bool isAll)
        {
            employeeIds = new List<int> { employeeId };
            GetAllEmployeesForManagerRecursively(employeeIds, isAll);
            return dbContext.Employees
                .Join(dbContext.Department, e => e.DepartmentId, d => d.DepartmentId, (e, d) => new { e, d })
                .Join(dbContext.Roles, ed => ed.e.RoleId, r => r.RoleId, (ed, r) => new { ed, r })
                .Join(dbContext.Designation, ede => ede.ed.e.DesignationId, de => de.DesignationId, (ede, de) => new { ede, de })
                .Where(rs => employeeIds.Contains(rs.ede.ed.e.EmployeeID) && rs.ede.ed.e.ResignationDate != null).Select(rs => new EmployeeViewDetails
                {
                    EmployeeId = rs.ede.ed.e.EmployeeID,
                    FormattedEmployeeId = rs.ede.ed.e.FormattedEmployeeId,
                    EmployeeName = rs.ede.ed.e.FirstName + " " + rs.ede.ed.e.LastName,
                    EmployeeEmail = rs.ede.ed.e.EmailAddress,
                    EmployeeType = dbContext.EmployeesType.Where(es => es.EmployeesTypeId == rs.ede.ed.e.EmployeeTypeId).Select(esr => esr.EmployeesType).FirstOrDefault(),
                    DepartmentId = rs.ede.ed.e.DepartmentId,
                    DepartmentName = rs.ede.ed.d.DepartmentName,
                    RoleId = rs.ede.ed.e.RoleId,
                    RoleName = rs.de.DesignationName,
                    ReportingTo = dbContext.Employees.Where(es => es.EmployeeID == rs.ede.ed.e.ReportingManagerId).Select(esr => esr.FirstName + " " + esr.LastName).FirstOrDefault(),
                    ReportingEmail = dbContext.Employees.Where(es => es.EmployeeID == rs.ede.ed.e.ReportingManagerId).Select(esr => esr.EmailAddress).FirstOrDefault()
                }).OrderBy(x => x.EmployeeName).ToList();
        }
        #endregion
        #region Get checklist employee details
        public List<ChecklistEmployeeView> GetCheckListEmployeeDetails(List<ChecklistEmployeeView> employeeDetails)
        {
            foreach (ChecklistEmployeeView item in employeeDetails)
            {
                Employees employee = dbContext.Employees.Where(x => x.EmployeeID == item.EmployeeID).Select(x => x).FirstOrDefault();
                item.EmployeeName = employee?.EmployeeName;
                item.FormattedEmployeeID = employee?.FormattedEmployeeId;
                item.DepartmentName = dbContext.Department.Where(x => x.DepartmentId == employee.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                item.Designation = dbContext.Designation.Where(x => x.DesignationId == employee.DesignationId).Select(x => x.DesignationName).FirstOrDefault();
                item.DateOfJoining = employee?.DateOfJoining;
                item.IsActive = employee.IsActive == true ? "Active Employee" : "Ex Employee";
            }
            return employeeDetails;
        }
        #endregion
        #region Get employee list for checklist
        public List<ResignationEmployeeMasterView> GetEmployeeListForResignation(int employeeId)
        {
            if (employeeId == 0)
            {
                return dbContext.Employees.Where(x => x.IsResign == true).Select(
                    x => new ResignationEmployeeMasterView
                    {
                        EmployeeID = x.EmployeeID,
                        EmployeeName = x.EmployeeName + '-' + x.FormattedEmployeeId,
                        FormattedEmployeeID = x.FormattedEmployeeId,
                        Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                        DepartmentId = x.DepartmentId,
                        LocationId = x.LocationId,
                        DesignationId = x.DesignationId,
                        RelievingDate = x.DateOfRelieving,
                        DateOfJoining = x.DateOfJoining,
                        DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                        EmployeeEmail = x.EmailAddress,
                        ResignationDate = x.ResignationDate,
                        IsActive = x.IsActive,
                        EmployeeType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(z => z.EmployeesType).FirstOrDefault()
                    }).ToList();
            }
            else
            {
                return dbContext.Employees.Where(x => x.IsResign == true && x.EmployeeID == employeeId).Select(
                    x => new ResignationEmployeeMasterView
                    {
                        EmployeeID = x.EmployeeID,
                        EmployeeName = x.EmployeeName + '-' + x.FormattedEmployeeId,
                        FormattedEmployeeID = x.FormattedEmployeeId,
                        Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                        DepartmentId = x.DepartmentId,
                        LocationId = x.LocationId,
                        DesignationId = x.DesignationId,
                        RelievingDate = x.DateOfRelieving,
                        DateOfJoining = x.DateOfJoining,
                        DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                        EmployeeEmail = x.EmailAddress,
                        ResignationDate = x.ResignationDate,
                        IsActive = x.IsActive,
                        EmployeeType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(z => z.EmployeesType).FirstOrDefault()
                    }).ToList();
            }
        }
        #endregion
        public List<EmployeeList> GetResignationEmployeeList(int employeeId)
        {
            if (employeeId == 0)
            {
                return (from emp in dbContext.Employees
                        select new EmployeeList
                        {
                            EmployeeId = emp.EmployeeID,
                            EmployeeName = emp.FormattedEmployeeId + " " + (emp.FirstName + " " + emp.LastName)
                        }).ToList();
            }
            else
            {
                return (from emp in dbContext.Employees
                        where (emp.EmployeeID == employeeId)
                        select new EmployeeList
                        {
                            EmployeeId = emp.EmployeeID,
                            EmployeeName = emp.FormattedEmployeeId + " " + (emp.FirstName + " " + emp.LastName)
                        }).ToList();
            }
        }

        #region Get Resignation employee filter list 
        public List<ResignationEmployeeMasterView> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            //if (resignationEmployeeFilter.IsFilterApplied || resignationEmployeeFilter.EmployeeId == 0)
            return (from users in dbContext.Employees
                    where users.IsResign == true && users.ResignationStatus == (string.IsNullOrEmpty(resignationEmployeeFilter.ResignationStatus) ? "Pending" : resignationEmployeeFilter.ResignationStatus)
                    && users.ReportingManagerId == (resignationEmployeeFilter.EmployeeId == 0 ? (int)users.ReportingManagerId : resignationEmployeeFilter.EmployeeId)
                    && users.IsActive == (resignationEmployeeFilter.IsActive == null ? users.IsActive : resignationEmployeeFilter.IsActive)
                    && (string.IsNullOrEmpty(resignationEmployeeFilter.EmployeeNameFilter) || users.EmployeeName.Contains(resignationEmployeeFilter.EmployeeNameFilter) || users.FormattedEmployeeId.Contains(resignationEmployeeFilter.EmployeeNameFilter))
                    select new ResignationEmployeeMasterView
                    {
                        EmployeeID = users.EmployeeID,
                        EmployeeName = users.EmployeeName + '-' + users.FormattedEmployeeId,
                        FormattedEmployeeID = users.FormattedEmployeeId,
                        Designation = dbContext.Designation.Where(y => y.DesignationId == users.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                        DepartmentId = users.DepartmentId,
                        LocationId = users.LocationId,
                        DesignationId = users.DesignationId,
                        RelievingDate = users.DateOfRelieving,
                        ResignationDate = users.ResignationDate,
                        ReportingManagerId = users.ReportingManagerId == null ? 0 : (int)users.ReportingManagerId,
                        ReportingManagerName = dbContext.Employees.Where(y => y.EmployeeID == users.ReportingManagerId).Select(x => x.EmployeeName).FirstOrDefault(),
                        ReportingManagerEmail = dbContext.Employees.Where(y => y.EmployeeID == users.ReportingManagerId).Select(x => x.EmailAddress).FirstOrDefault(),
                        DateOfJoining = users.DateOfJoining,
                        DepartmentName = dbContext.Department.Where(y => y.DepartmentId == users.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                        EmployeeEmail = users.EmailAddress,
                        FirstName = users.FirstName,
                        LastName = users.LastName
                    }).OrderByDescending(x => x.ResignationDate).Skip(resignationEmployeeFilter.NoOfRecord * (resignationEmployeeFilter.PageNumber))
                        .Take(resignationEmployeeFilter.NoOfRecord).Distinct().ToList();
        }
        #endregion

        #region Get employee by systemRole

        public List<ResignationEmployeeMasterView> GetEmployeesDetailsBySystemRole(string sRole)
        {
            if (!string.IsNullOrEmpty(sRole))
            {
                return (from users in dbContext.Employees
                        join roles in dbContext.SystemRoles on users.SystemRoleId equals roles.RoleId
                        where (roles.RoleName == null ? "" : roles.RoleName.ToLower()) == sRole.ToLower() && users.IsActive == true
                        select new ResignationEmployeeMasterView
                        {
                            EmployeeID = users.EmployeeID,
                            Designation = dbContext.Designation.Where(y => y.DesignationId == users.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                            RelievingDate = users.DateOfRelieving,
                            ReportingManagerId = users.ReportingManagerId == null ? 0 : (int)users.ReportingManagerId,
                            ReportingManagerName = dbContext.Employees.Where(y => y.EmployeeID == users.ReportingManagerId).Select(x => x.EmployeeName).FirstOrDefault(),
                            ReportingManagerEmail = dbContext.Employees.Where(y => y.EmployeeID == users.ReportingManagerId).Select(x => x.EmailAddress).FirstOrDefault(),
                            DateOfJoining = users.DateOfJoining,
                            DepartmentName = dbContext.Department.Where(y => y.DepartmentId == users.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                            EmployeeEmail = users.EmailAddress
                        }).ToList();
            }
            return new List<ResignationEmployeeMasterView>();
        }
        #endregion


        public List<Employees> GetEmployeeListByRelievingDate()
        {
            DateTime resignadate = DateTime.Now.AddDays(-1);
            return dbContext.Employees.Where(x => x.DateOfRelieving != null && x.DateOfRelieving.Value.Date == resignadate.Date && x.IsActive == true).ToList();
        }

        public string GetSystemRoleNameById(int systemRoleId)
        {
            return dbContext.SystemRoles.Where(x => x.RoleId == systemRoleId).Select(x => x.RoleName).FirstOrDefault();
        }

        public int GetTotalRecordCount()
        {
            int? data = dbContext.Employees.Select(x => x.EmployeeID).Count();
            return data == null ? 0 : (int)data;
        }

        public List<EmployeeDetailListView> GetEmployeeListForGrid(PaginationView pagination)
        {
            return dbContext.Employees.Select(x => new EmployeeDetailListView
            {
                EmployeeId = x.EmployeeID,
                EmployeeFullName = x.EmployeeName,
                FormattedEmployeeId = x.FormattedEmployeeId,
                EmployeementType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault(),
                ProfilePic = x.ProfilePicture
            }).OrderByDescending(x => x.EmployeeId).Skip(pagination.NoOfRecord * (pagination.PageNumber)).Take(pagination.NoOfRecord).ToList();
        }

        public List<EmployeeDetailListView> GetEmployeeListForRequestedDocumentGrid(List<int> employeeIds)
        {
            return dbContext.Employees.Select(x => new EmployeeDetailListView
            {
                EmployeeId = x.EmployeeID,
                EmployeeFullName = x.EmployeeName,
                FormattedEmployeeId = x.FormattedEmployeeId,
                EmployeementType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault(),
                ProfilePic = x.ProfilePicture
            }).OrderByDescending(x => x.EmployeeId).Where(x => employeeIds.Contains(x.EmployeeId)).ToList();
        }
        public List<EmployeeDetailListViewForOrgChart> GetEmployeesListForOrgChart(PaginationView pagination)
        {
            string locationQuery = pagination?.EmployeeFilter?.Location?.Count > 0 ? "@0.Contains(LocationId)" : " \"\" == \"\"";
            var query = "(FirstName.StartsWith( \"@fullName\") || LastName.StartsWith( \"@fullName\") || EmployeeName.StartsWith(\"@fullName\") || \"@fullName\" == \"\") &&(" + locationQuery + ")";
            query = query.Replace("@fullName", pagination?.EmployeeFilter?.FullName == null ? "" : pagination?.EmployeeFilter?.FullName.Trim());
            return dbContext.Employees.Where(query, pagination?.EmployeeFilter?.Location).Select(x => new EmployeeDetailListViewForOrgChart
            {
                EmployeeId = x.EmployeeID,
                EmployeeFullName = x.EmployeeName,
                FormattedEmployeeId = x.FormattedEmployeeId,
                EmployeementType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault(),
                ProfilePic = x.ProfilePicture,
                ReporteeCount = dbContext.Employees.Where(m => m.IsActive == true && m.ReportingManagerId == x.EmployeeID).Select(m => m.EmployeeID).Count()
            }).OrderByDescending(x => x.EmployeeId).Skip(pagination.NoOfRecord * (pagination.PageNumber)).Take(pagination.NoOfRecord).ToList();
        }

        public EmployeeBasicInfoView GetEmployeeBasicInfoById(int employeeId)
        {
            EmployeeBasicInfoView employeeViewModel = new EmployeeBasicInfoView();
            employeeViewModel.Employee = dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x =>
            new EmployeeView
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                EmployeeTypeId = x.EmployeeTypeId,
                EmployeeType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(x => x.EmployeesType).FirstOrDefault(),
                DepartmentId = x.DepartmentId,
                DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault(),
                DepartmentHead = dbContext.Employees.Where(y => y.EmployeeID == dbContext.Department.Where(z => z.DepartmentId == x.DepartmentId).Select(x => x.DepartmentHeadEmployeeId).FirstOrDefault()).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault(),
                RoleId = x.RoleId,
                RoleName = dbContext.Roles.Where(y => y.RoleId == x.RoleId).Select(x => x.RoleName).FirstOrDefault(),
                DateOfJoining = x.DateOfJoining,
                DateOfContract = x.DateOfContract,
                DateOfRelieving = x.DateOfRelieving,
                DesignationEffectiveFrom = x.DesignationEffectiveFrom,
                ContractEndDate = x.ContractEndDate,
                ActualConfirmationDate = x.ActualConfirmationDate,
                MergerDate = x.MergerDate,
                ReportingManagerId = x.ReportingManagerId,
                ReportingManager = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault(),
                IsActive = x.IsActive,
                CreatedOn = x.CreatedOn,
                ModifiedOn = x.ModifiedOn,
                CreatedBy = x.CreatedBy,
                ModifiedBy = x.ModifiedBy,
                CreatedByName = dbContext.Employees.Where(y => y.EmployeeID == x.CreatedBy).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault(),
                ModifiedByName = dbContext.Employees.Where(y => y.EmployeeID == x.ModifiedBy).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault(),
                Gender = x.Gender,
                LocationId = x.LocationId,
                Location = dbContext.EmployeeLocation.Where(y => y.LocationId == x.LocationId).Select(x => x.Location).FirstOrDefault(),
                CurrentWorkLocationId = x.CurrentWorkLocationId,
                CurrentWorkLocation = dbContext.EmployeeLocation.Where(y => y.LocationId == x.CurrentWorkLocationId).Select(x => x.Location).FirstOrDefault(),
                CurrentWorkPlaceId = x.CurrentWorkPlaceId,
                //CurrentWorkPlace = dbContext.EmployeeLocation.Where(y => y.LocationId == x.CurrentWorkPlaceId).Select(x => x.Location).FirstOrDefault(),
                CurrentWorkPlace = dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == x.CurrentWorkPlaceId).Select(x => x.DisplayName).FirstOrDefault(),
                IsSpecialAbility = x.IsSpecialAbility,
                SpecialAbilityRemark = x.SpecialAbilityRemark,
                //SpecialAbilityId = dbContext.EmployeeSpecialAbility.Where(y=>y.EmployeeId==x.EmployeeID).Select(z=>z.SpecialAbilityId).ToList(),
                SpecialAbility = string.Join(',', (from a in dbContext.EmployeeSpecialAbility
                                                   join b in dbContext.EmployeeAppConstants on a.SpecialAbilityId equals b.AppConstantId
                                                   where a.EmployeeId == employeeId
                                                   select b.AppConstantId == 48 && x.SpecialAbilityRemark != null ? ("Others-" + x.SpecialAbilityRemark) : b.DisplayName).ToList()),
                // string.Join(",", dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == x.SpecialAbilityId).Select(x => x.AppConstantValue).ToList()),

                Mobile = x.Mobile,
                JobTitle = x.JobTitle,
                DesignationId = x.DesignationId,
                Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(x => x.DesignationName).FirstOrDefault(),
                EmployeeCategoryId = x.EmployeeCategoryId,
                EmployeeCategory = dbContext.EmployeeCategory.Where(y => y.EmployeeCategoryId == x.EmployeeCategoryId).Select(x => x.EmployeeCategoryName).FirstOrDefault(),
                EmployeeGrade = x.EmployeeGrade,
                ProbationExtension = x.ProbationExtension,
                //TVSNextExperience = x.TVSNextExperience,
                //TotalExperience = x.TotalExperience, 
                PreviousExperience = x.PreviousExperience,
                OfficialMobileNumber = x.OfficialMobileNumber,
                BirthDate = x.BirthDate,
                WeddingAnniversary = x.WeddingAnniversary,
                Maritalstatus = x.Maritalstatus,
                ProbationStatusId = x.ProbationStatusId,
                ProbationStatus = dbContext.ProbationStatus.Where(y => y.ProbationStatusId == x.ProbationStatusId).Select(x => x.ProbationStatusName).FirstOrDefault(),
                FormattedEmployeeId = x.FormattedEmployeeId,
                SystemRoleId = x.SystemRoleId,
                SystemRole = dbContext.SystemRoles.Where(y => y.RoleId == x.SystemRoleId).Select(x => x.RoleName).FirstOrDefault(),
                EmployeeName = x.EmployeeName,
                ExitType = x.ExitType,
                ResignationDate = x.ResignationDate,
                RetirementDate = x.RetirementDate,
                ResignationReason = x.ResignationReason,
                ResignationStatus = x.ResignationStatus,
                IsResign = x.IsResign,
                Entity = x.Entity,
                EntityName = dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == x.Entity).Select(x => x.AppConstantValue).FirstOrDefault(),
                SourceOfHireId = x.SourceOfHireId,
                NoticeCategory = x.NoticeCategory,
                SourceOfHire = dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == x.SourceOfHireId).Select(x => x.AppConstantValue).FirstOrDefault(),
                ProfilePicture = x.ProfilePicture,
                SkillSet = string.Join(',', (from a in dbContext.EmployeesSkillset
                                             join b in dbContext.Skillset on a.SkillsetId equals b.SkillsetId
                                             where a.EmployeeId == employeeId
                                             select b.Skillset).ToList())
            }).FirstOrDefault();
            if (employeeViewModel?.Employee != null)
            {
                var NoticePeriod = CalculateNoticePeriod(employeeViewModel?.Employee?.DateOfJoining, employeeViewModel?.Employee?.ProbationStatusId, employeeViewModel?.Employee?.NoticeCategory);
                employeeViewModel.Employee.NoticePeriod = NoticePeriod;
            }
            if (employeeViewModel != null)
            {
                employeeViewModel.EmployeeDependent = (from a in dbContext.EmployeeDependent
                                                       join b in dbContext.Employees on a.EmployeeID equals b.EmployeeID
                                                       where a.EmployeeID == employeeId
                                                       select new EmployeeDependentView
                                                       {
                                                           EmployeeDependentId = a.EmployeeDependentId,
                                                           EmployeeRelationName = a.EmployeeRelationName,
                                                           EmployeeRelationshipId = a.EmployeeRelationshipId,
                                                           EmployeeRelationship = dbContext.EmployeeRelationship.Where(x => x.EmployeeRelationshipId == a.EmployeeRelationshipId).Select(x => x.EmployeeRelationshipName).FirstOrDefault(),
                                                           EmployeeRelationDateOfBirth = a.EmployeeRelationDateOfBirth,
                                                           DependentDetailsProof = dbContext.EmployeeDocument.Where(x => x.SourceId == a.EmployeeDependentId && x.EmployeeID == employeeId && x.DocumentType == "dependentDetailsProof").Select(x =>
                                                            new DocumentsToUpload
                                                            {
                                                                Path = File.Exists(x.DocumentPath) ? x.DocumentPath : "",
                                                                DocumentId = (int)x.EmployeeDocumentId,
                                                                DocumentName = x.DocumentName,
                                                                DocumentCategory = x.DocumentType,
                                                                DocumentAsBase64 = File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)) : "",
                                                                DocumentSize = File.Exists(x.DocumentPath) ? Convert.ToBase64String(File.ReadAllBytes(x.DocumentPath)).Length : 0,
                                                            }
                                                            ).FirstOrDefault()
                                                       }).ToList();
                employeeViewModel.EmployeeShiftDetails = (from a in dbContext.EmployeeShiftDetails
                                                          join b in dbContext.Employees on a.EmployeeID equals b.EmployeeID
                                                          where a.EmployeeID == employeeId
                                                          select new EmployeeShiftDetails
                                                          {
                                                              EmployeeShiftDetailsId = a.EmployeeShiftDetailsId,
                                                              ShiftDetailsId = a.ShiftDetailsId,
                                                              ShiftFromDate = a.ShiftFromDate,
                                                              ShiftToDate = a.ShiftToDate
                                                          }).ToList();
            }
            return employeeViewModel;
        }

        public List<EmployeeDetailListView> GetListOfEmployeeBirthDay(DateTime fromDate, DateTime toDate)
        {

            var test = fromDate.Date;
            return dbContext.Employees.Where(x => x.BirthDate.Value.Month >= fromDate.Month && x.BirthDate.Value.Month <= toDate.Month && x.BirthDate.Value.Day >= fromDate.Day && x.BirthDate.Value.Day <= toDate.Day).Select(x =>
             new EmployeeDetailListView
             {
                 EmployeeId = x.EmployeeID,
                 EmployeeFullName = x.EmployeeName,
                 FormattedEmployeeId = x.FormattedEmployeeId,
                 EmployeementType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault(),
                 DateOfBirth = x.BirthDate,
                 EmailId = x.EmailAddress,
                 ProfilePic = x.ProfilePicture
             }
            ).ToList();
        }

        public List<EmployeeDetailListView> employeeFilterData(PaginationView paginationView)
        {
            List<EmployeeDetailListView> EmployeeList = new List<EmployeeDetailListView>();
            EmployeeFilterView filterView = paginationView.EmployeeFilter;
            List<int> ContractEmployeeList = new List<int>();
            List<int> DateOfJoiningEmployeeList = new List<int>();
            List<int> BirthDayEmployeeList = new List<int>();
            DateTime zeroTime = new(1, 1, 1);
            string DepartmentQuery = filterView.Department?.Count > 0 ? "@0.Contains(DepartmentId)" : " \"\" == \"\"";
            string locationQuery = filterView.Location?.Count > 0 ? "@1.Contains(LocationId)" : " \"\" == \"\"";
            string reportingManagerQuery = filterView.ReportingManager?.Count > 0 ? "@2.Contains(ReportingManagerId)" : " \"\" == \"\"";
            string employeeTypeQuery = filterView.EmployeeType?.Count > 0 ? "@3.Contains(EmployeeTypeId)" : " \"\" == \"\"";
            string probationStatus = filterView.ProbationStatus?.Count > 0 ? "@4.Contains(ProbationStatusId)" : " \"\" == \"\"";
            string designationQuery = filterView.Designation?.Count > 0 ? "@5.Contains(DesignationId)" : " \"\" == \"\"";
            string systemRoleQuery = filterView.SystemRole?.Count > 0 ? "@6.Contains(SystemRoleId)" : " \"\" == \"\"";
            string employeeCategoryQuery = filterView.EmployeeCategory?.Count > 0 ? "@7.Contains(EmployeeCategoryId)" : " \"\" == \"\"";
            string entityQuery = filterView.Entity?.Count > 0 ? "@8.Contains(Entity)" : " \"\" == \"\"";
            string exitTypeQuery = filterView.ExitType?.Count > 0 ? "@9.Contains(ExitType)" : " \"\" == \"\"";
            string genderQuery = filterView.Gender?.Count > 0 ? "@10.Contains(Gender)" : " \"\" == \"\"";
            string resignationStatusQuery = filterView.ResignationStatus?.Count > 0 ? "@11.Contains(ResignationStatus)" : " \"\" == \"\"";
            string employeeStatusQuery = filterView.EmployeeStatus?.Count > 0 ? "@12.Contains(isActive)" : " \"\" == \"\"";
            //string bloodGroupQuery = filterView.BloodGroup?.Count > 0 ? "@0.Contains(BloodGroup)" : " \"\" == \"\"";
            DateTime? todayDate = DateTime.Now;
            string gradeQuery = filterView?.Grade == null ? "( \"@grade\" == \"\" )" : "(EmployeeGrade ==  @grade) ";
            List<int> employeeIdList = filterView.BloodGroup?.Count > 0 ? dbContext.EmployeesPersonalInfo.Where("@0.Contains(BloodGroup)", filterView.BloodGroup).Select(x => (int)x.EmployeeId).ToList() : new List<int>();
            string employeeListQuery = employeeIdList?.Count > 0 ? "@13.Contains(EmployeeID)" : filterView.BloodGroup?.Count > 0 ? "false" : " \"\" == \"\"";
            var query = "(FirstName.StartsWith( \"@fullName\") || LastName.StartsWith( \"@fullName\") || EmployeeName.StartsWith(\"@fullName\") || \"@fullName\" == \"\")  && (FormattedEmployeeId.Contains( \"@employeeId\") || \"@employeeId\" == \"\" ) &&(" + gradeQuery + ")&&(" + DepartmentQuery + ")&&(" + locationQuery + ")&&(" + reportingManagerQuery + ")&&(" + employeeTypeQuery + ") && (" + probationStatus + ") && (" + designationQuery + ") && (" + systemRoleQuery + ") && (" + employeeCategoryQuery + ") && (" + entityQuery + ")&&(" + exitTypeQuery + ") && (" + resignationStatusQuery + ") && (" + genderQuery + ") && (" + employeeStatusQuery + ") && (" + employeeListQuery + ")";
            query = query.Replace("@fullName", filterView?.FullName?.Trim() == null ? "" : filterView.FullName.Trim());
            query = query.Replace("@employeeId", filterView?.EmployeeId == null ? "" : filterView.EmployeeId);
            query = query.Replace("@grade", filterView?.Grade == null ? "" : filterView?.Grade.ToString());
            var employees = dbContext.Employees.Where(query, filterView.Department, filterView.Location, filterView.ReportingManager, filterView.EmployeeType, filterView.ProbationStatus, filterView.Designation, filterView.SystemRole, filterView.EmployeeCategory, filterView.Entity, filterView.ExitType, filterView.Gender, filterView.ResignationStatus, filterView.EmployeeStatus, employeeIdList
            ).OrderByDescending(x => x.EmployeeID).ToList();

            if (filterView?.Age?.Condition != null && filterView.Age.Value != null && employees?.Count > 0)
            {
                if (filterView.Age.Condition == ">")
                {
                    employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) > filterView.Age.Value).ToList();
                }
                else if (filterView.Age.Condition == "<")
                {
                    employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) < filterView.Age.Value).ToList();
                }
                else
                {
                    employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) == filterView.Age.Value).ToList();
                }
                //  ageQuery = BirthDayEmployeeList?.Count > 0 ? "@14.Contains(EmployeeID)" : "false";
            }
            if (filterView.ContractEndDate?.FromDate != null && filterView.ContractEndDate.Condition != null && employees?.Count > 0)
            {
                if (filterView.ContractEndDate.Condition == ">")
                {
                    employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) > filterView.ContractEndDate.FromDate).ToList();
                }
                else if (filterView.ContractEndDate.Condition == "<")
                {
                    employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) < filterView.ContractEndDate.FromDate).ToList();
                }
                else
                {
                    employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) > filterView.ContractEndDate.FromDate && (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) < filterView.ContractEndDate.ToDate).ToList();
                }
                // ContractEndDateQuery = ContractEmployeeList?.Count > 0 ? "@15.Contains(EmployeeID)" : "false";
            }

            if (filterView.DateOfJoining?.FromDate != null && filterView.DateOfJoining.Condition != null && employees?.Count > 0)
            {
                if (filterView.DateOfJoining.Condition == ">")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > filterView.DateOfJoining.FromDate).ToList();
                }
                else if (filterView.DateOfJoining.Condition == "<")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < filterView.DateOfJoining.FromDate).ToList();
                }
                else
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > filterView.DateOfJoining.FromDate && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < filterView.DateOfJoining.ToDate).ToList();
                }
                //  DateOfJoiningQuery = DateOfJoiningEmployeeList?.Count > 0 ? "@16.Contains(EmployeeID)" : "false";
            }
            if (filterView?.TVSNextExperience?.Condition != null && employees?.Count > 0)
            {
                var fromDate = filterView.TVSNextExperience?.FromExperience.ToString().Split('.');
                var fromDateToCheck = DateTime.Now;
                if (fromDate.Length > 1)
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0])).AddMonths(-Convert.ToInt32(fromDate[1]));
                }
                else
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0]));
                }

                if (filterView.TVSNextExperience.Condition == ">")
                {
                    employees = employees.Where(x => x.DateOfJoining < fromDateToCheck).ToList();
                }
                else if (filterView.TVSNextExperience.Condition == "<")
                {
                    employees = employees.Where(x => x.DateOfJoining > fromDateToCheck).ToList();
                }
                else if (filterView.TVSNextExperience?.ToExperience != null)
                {
                    var toDate = filterView.TVSNextExperience?.ToExperience.ToString().Split('.');
                    var ToDateToCheck = DateTime.Now;
                    if (toDate.Length > 1)
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0])).AddMonths(-Convert.ToInt32(toDate[1]));
                    }
                    else
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0]));
                    }
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < fromDateToCheck && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > ToDateToCheck).ToList();
                }
                //  DateOfJoiningQuery = DateOfJoiningEmployeeList?.Count > 0 ? "@16.Contains(EmployeeID)" : "false";
            }
            //if (filterView?.TVSNextExperience?.Condition != null && employees?.Count > 0)
            //{
            //    if (filterView.TVSNextExperience.Condition == ">")
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) > filterView.TVSNextExperience.FromExperience).ToList();
            //    }
            //    else if (filterView.TVSNextExperience.Condition == "<")
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) < filterView.TVSNextExperience.FromExperience).ToList();
            //    }
            //    else
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) > filterView.TVSNextExperience.FromExperience && ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) < filterView.TVSNextExperience.ToExperience).ToList();
            //    }
            //}
            if (filterView?.TotalExperience?.Condition != null && employees?.Count > 0)
            {
                var fromDate = filterView.TotalExperience?.FromExperience.ToString().Split('.');
                var fromDateToCheck = DateTime.Now;
                if (fromDate.Length > 1)
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0])).AddMonths(-Convert.ToInt32(fromDate[1]));
                }
                else
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0]));
                }
                if (filterView.TotalExperience.Condition == ">")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) < fromDateToCheck).ToList();
                }
                else if (filterView.TotalExperience.Condition == "<")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) > fromDateToCheck).ToList();
                    //employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) < filterView.TotalExperience.FromExperience).ToList();
                }
                else if (filterView.TotalExperience.ToExperience != null)
                {
                    var toDate = filterView.TotalExperience?.ToExperience.ToString().Split('.');
                    var ToDateToCheck = DateTime.Now;
                    if (toDate.Length > 1)
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0])).AddMonths(-Convert.ToInt32(toDate[1]));
                    }
                    else
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0]));
                    }
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) < fromDateToCheck && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) > ToDateToCheck).ToList();
                    //employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) > filterView.TotalExperience.FromExperience && ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) < filterView.TotalExperience.ToExperience).ToList();
                }
            }
            //if (filterView?.TotalExperience?.Condition != null && employees?.Count > 0)
            //{
            //    if (filterView.TotalExperience.Condition == ">")
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) +(((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1)*0.01)) > filterView.TotalExperience.FromExperience).ToList();
            //    }
            //    else if (filterView.TotalExperience.Condition == "<")
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) +(((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1)*0.01)) < filterView.TotalExperience.FromExperience).ToList();
            //    }
            //    else
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) +(((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1)*0.01)) > filterView.TotalExperience.FromExperience && ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) +(((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1)*0.01)) < filterView.TotalExperience.ToExperience).ToList();
            //    }
            //}
            //return employees.Join(dbContext.EmployeesType , emp => emp.EmployeeTypeId , empType=> empType.EmployeesTypeId, (emp , empType) => new {emp, empType }).ToList().Select(x => new EmployeeDetailListView
            //{
            //    EmployeeId = x.emp.EmployeeID,
            //    EmployeeFullName = x.emp.EmployeeName,
            //    FormattedEmployeeId = x.emp.FormattedEmployeeId,
            //    EmployeementType = x.empType.EmployeesType,
            //    ProfilePic = x.emp.ProfilePicture,
            //    //TotalExp = ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.emp.DateOfJoining == null || x.emp.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.emp.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.emp.DateOfJoining == null || x.emp.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.emp.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.emp.DateOfJoining == null || x.emp.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.emp.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.emp.DateOfJoining == null || x.emp.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.emp.DateOfJoining))).Month - 1) * 0.01))
            //}).Skip(paginationView.NoOfRecord * (paginationView.PageNumber)).Take(paginationView.NoOfRecord).ToList();
            return employees.Select(x => new EmployeeDetailListView
            {
                EmployeeId = x.EmployeeID,
                EmployeeFullName = x.EmployeeName,
                FormattedEmployeeId = x.FormattedEmployeeId,
                EmployeementType = dbContext.EmployeesType.Where(z => z.EmployeesTypeId == x.EmployeeTypeId).Select(a => a.EmployeesType).FirstOrDefault(),
                ProfilePic = x.ProfilePicture,
            }).Skip(paginationView.NoOfRecord * (paginationView.PageNumber)).Take(paginationView.NoOfRecord).ToList();

            //var query = "(EmployeeName == \"@fullName\" || \"@fullName\" == \"\")  && (FormattedEmployeeId == \"@employeeId\" || \"@employeeId\" == \"\" ) &&(" + ageQuery + ")&&(" + gradeQuery + ")&&(" + DepartmentQuery + ")&&(" + locationQuery + ")&&(" + reportingManagerQuery + ")&&(" + employeeTypeQuery + ") && (" + probationStatus + ") && (" + designationQuery + ") && (" + systemRoleQuery + ") && (" + employeeCategoryQuery + ") && (" + entityQuery + ")&&(" + exitTypeQuery + ") && (" + resignationStatusQuery + ") && (" + genderQuery + ") && (" + employeeStatusQuery + ") && (" + employeeListQuery + ")&&("+ ContractEndDateQuery+")&&("+DateOfJoiningQuery+")";      
            //query = query.Replace("@fullName", filterView?.FullName == null ? "" : filterView.FullName);
            //query = query.Replace("@employeeId", filterView?.EmployeeId == null ? "" : filterView.EmployeeId);
            //query = query.Replace("@grade", filterView?.Grade == null ? "" : filterView?.Grade.ToString());
            //return dbContext.Employees.Where(query,filterView.Department, filterView.Location, filterView.ReportingManager,filterView.EmployeeType, filterView.ProbationStatus, filterView.Designation, filterView.SystemRole, filterView.EmployeeCategory, filterView.Entity, filterView.ExitType, filterView.Gender, filterView.ResignationStatus, filterView.EmployeeStatus, employeeIdList,BirthDayEmployeeList,ContractEmployeeList,DateOfJoiningEmployeeList
            //).Select(x => new EmployeeDetailListView
            //{
            //    EmployeeId = x.EmployeeID,
            //    EmployeeFullName = x.EmployeeName,
            //    FormattedEmployeeId = x.FormattedEmployeeId,
            //    EmployeementType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault(),
            //    ProfilePic = x.ProfilePicture
            //}).OrderByDescending(x => x.EmployeeId).Skip(paginationView.NoOfRecord * (paginationView.PageNumber)).Take(paginationView.NoOfRecord).ToList();
        }
        public List<EmployeeWorkAnniversaries> GetListOfEmployeeWorkAnniversaries(DateTime fromDate, DateTime toDate)
        {

            var test = fromDate.Date;
            DateTime zeroTime = new(1, 1, 1);
            return dbContext.Employees.Where(x => x.DateOfJoining.Value.Month >= fromDate.Month && x.DateOfJoining.Value.Month <= toDate.Month && x.DateOfJoining.Value.Day >= fromDate.Day && x.DateOfJoining.Value.Day <= toDate.Day).Select(x =>
             new EmployeeWorkAnniversaries
             {
                 EmployeeId = x.EmployeeID,
                 EmployeeFullName = x.EmployeeName,
                 FormattedEmployeeId = x.FormattedEmployeeId,
                 EmployeementType = dbContext.EmployeesType.Where(y => y.EmployeesTypeId == x.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault(),
                 DateofJoin = x.DateOfJoining,
                 EmailId = x.EmailAddress,
                 TotalYears = x.DateOfJoining.Value.Date > DateTime.Now.Date ? 0 : (int?)((((zeroTime + (DateTime.Now - x.DateOfJoining)).Value.Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - x.DateOfJoining)).Value.Month - 1) * 0.1)),
                 ProfilePic = x.ProfilePicture
             }
            ).ToList();
        }
        public List<EmployeeList> GetAbsentNotificationEmployeeList()
        {
            var absent = dbContext.PreventAbsentNotification.Select(x => x.EmployeeId).ToList();
            return dbContext.Employees.Where(emp => emp.IsActive == true && !absent.Contains(emp.EmployeeID)).Select(emp =>
                     new EmployeeList
                     {
                         //EmployeeId = emp.EmployeeID,
                         //FormattedEmployeeId = emp.FormattedEmployeeId,
                         //ManagerId = emp.ReportingManagerId
                         EmployeeId = emp.EmployeeID,
                         EmpName = emp.FirstName + " " + emp.LastName,
                         FormattedEmployeeId = emp.FormattedEmployeeId,
                         Employee_Id = emp.FormattedEmployeeId,
                         EmployeeName = emp.FormattedEmployeeId + " " + (emp.FirstName + " " + emp.LastName),
                         EmployeeEmailId = emp.EmailAddress,
                         DepartmentId = emp.DepartmentId,
                         LocationId = emp.LocationId,
                         ManagerId = emp.ReportingManagerId,
                         DOJ = emp.DateOfJoining,
                         ShiftId = dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == emp.EmployeeID && DateTime.Now.Date >= x.ShiftFromDate && (x.ShiftToDate == null || DateTime.Now.Date <= x.ShiftToDate)).Select(x => x.ShiftDetailsId).FirstOrDefault(),
                         employeeShiftDetails = dbContext.EmployeeShiftDetails.Where(y => y.EmployeeID == emp.EmployeeID).Select(y => new EmployeeShiftDetailsView
                         {
                             EmployeeID = y.EmployeeID == null ? 0 : (int)y.EmployeeID,
                             ShiftDetailsId = y.ShiftDetailsId,
                             ShiftFromDate = y.ShiftFromDate,
                             ShiftToDate = y.ShiftToDate
                         }).ToList()
                     }).ToList();

        }
        public int GetEmployeeIdByFormattedEmployeeId(string formattedEmployeeId)
        {
            return dbContext.Employees.Where(x => x.FormattedEmployeeId == formattedEmployeeId).Select(x => x.EmployeeID).FirstOrDefault();
        }
        public EmployeeorganizationcharView GetorganizationchartDetails(int employeeId)
        {
            EmployeeorganizationchartDetailsView EmployeeDetails = new EmployeeorganizationchartDetailsView();
            List<EmployeeorganizationchartDetailsView> ChildEmployeeDetails = new List<EmployeeorganizationchartDetailsView>();
            EmployeeorganizationchartDetailsView ParentDetails = new EmployeeorganizationchartDetailsView();
            List<EmployeeorganizationchartDetailsView> ParentsChild = new List<EmployeeorganizationchartDetailsView>();
            EmployeeorganizationcharView ResponceData = new EmployeeorganizationcharView();
            string emptyprofile = _configuration.GetSection("Orgnization:OrgnizatioEmptyimage").Value;
            ChildEmployeeDetails = dbContext.Employees.Where(emp => emp.IsActive == true && emp.ReportingManagerId == employeeId).Select(emp =>
                        new EmployeeorganizationchartDetailsView
                        {
                            name = emp.EmployeeName + " - " + emp.FormattedEmployeeId + "@"
                            + dbContext.EmployeesType.Where(y => y.EmployeesTypeId == emp.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault() + "@"
                            + emp.EmployeeID + "@"
                            + dbContext.Employees.Where(m => m.IsActive == true && m.ReportingManagerId == emp.EmployeeID).Select(m => m.EmployeeID).Count().ToString(),
                            title = dbContext.Designation.Where(x => x.DesignationId == emp.DesignationId).Select(x => x.DesignationName).FirstOrDefault(),
                            image = string.IsNullOrEmpty(emp.ProfilePicture) ? emptyprofile : emp.ProfilePicture,
                            cssClass = _configuration.GetSection("Orgnization:OrgnizationReportingEmpCss").Value
                        }).ToList();

            EmployeeDetails = dbContext.Employees.Where(emp => emp.IsActive == true && emp.EmployeeID == employeeId).Select(emp =>
                      new EmployeeorganizationchartDetailsView
                      {
                          name = emp.EmployeeName + " - " + emp.FormattedEmployeeId + "@"
                            + dbContext.EmployeesType.Where(y => y.EmployeesTypeId == emp.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault() + "@"
                             + emp.EmployeeID + "@"
                            + dbContext.Employees.Where(m => m.IsActive == true && m.ReportingManagerId == employeeId).Select(m => m.EmployeeID).Count().ToString(),
                          title = dbContext.Designation.Where(x => x.DesignationId == emp.DesignationId).Select(x => x.DesignationName).FirstOrDefault(),
                          image = string.IsNullOrEmpty(emp.ProfilePicture) ? emptyprofile : emp.ProfilePicture,
                          cssClass = _configuration.GetSection("Orgnization:OrgnanizationSelectedEmployeeCss").Value,
                          childs = ChildEmployeeDetails

                      }).FirstOrDefault();
            ResponceData = dbContext.Employees.Where(emp => emp.IsActive == true && emp.EmployeeID == employeeId).Select(emp =>
                     new EmployeeorganizationcharView
                     {
                         SelectedEmpDepartment = dbContext.Department.Where(x => x.DepartmentId == emp.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault(),
                         SelectedEmpEmailId = emp.EmailAddress,

                     }).FirstOrDefault();

            if (EmployeeDetails != null)
            {

                if (Convert.ToInt32(_configuration.GetSection("Orgnization:OrgnizationHeadEmpId").Value) == employeeId)
                {
                    ResponceData.OrgnizationData = EmployeeDetails;
                    return ResponceData;

                }
                ParentsChild.Add(EmployeeDetails);
                for (int i = 0; i < i + 1; i++)
                {
                    if (Convert.ToInt32(_configuration.GetSection("Orgnization:OrgnizationHeadEmpId").Value) != employeeId)
                    {
                        var reportingmangerid = dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x => x.ReportingManagerId).FirstOrDefault();
                        employeeId = reportingmangerid == null ? 0 : (int)reportingmangerid;

                        if (employeeId == 0)
                        {
                            ResponceData.OrgnizationData = EmployeeDetails;
                            return ResponceData;
                        }
                        List<EmployeeorganizationchartDetailsView> ExistingParentsChildDetails = new List<EmployeeorganizationchartDetailsView>(new List<EmployeeorganizationchartDetailsView>(ParentsChild));

                        ParentDetails = dbContext.Employees.Where(emp => emp.EmployeeID == reportingmangerid).Select(emp =>
                            new EmployeeorganizationchartDetailsView
                            {
                                name = emp.EmployeeName + " - " + emp.FormattedEmployeeId + "@"
                            + dbContext.EmployeesType.Where(y => y.EmployeesTypeId == emp.EmployeeTypeId).Select(y => y.EmployeesType).FirstOrDefault() + "@"
                             + emp.EmployeeID + "@"
                            + dbContext.Employees.Where(m => m.IsActive == true && m.ReportingManagerId == emp.EmployeeID).Select(m => m.EmployeeID).Count().ToString(),
                                title = dbContext.Designation.Where(x => x.DesignationId == emp.DesignationId).Select(x => x.DesignationName).FirstOrDefault(),
                                image = string.IsNullOrEmpty(emp.ProfilePicture) ? emptyprofile : emp.ProfilePicture,
                                childs = ExistingParentsChildDetails,
                                cssClass = _configuration.GetSection("Orgnization:OrgnizationCommonCss").Value

                            }).FirstOrDefault();

                        ParentsChild.Clear();
                        ParentsChild.Add(ParentDetails);
                        continue;
                    }
                    else
                    {

                        break;
                    }
                }
            }
            if (ResponceData != null)
            {
                ResponceData.OrgnizationData = ParentDetails;
            }
            return ResponceData;
        }
        public int employeeFilterDataCount(PaginationView paginationView)
        {
            List<EmployeeDetailListView> EmployeeList = new List<EmployeeDetailListView>();
            EmployeeFilterView filterView = paginationView.EmployeeFilter;
            List<int> ContractEmployeeList = new List<int>();
            List<int> DateOfJoiningEmployeeList = new List<int>();
            List<int> BirthDayEmployeeList = new List<int>();
            DateTime zeroTime = new(1, 1, 1);
            string DepartmentQuery = filterView.Department?.Count > 0 ? "@0.Contains(DepartmentId)" : " \"\" == \"\"";
            string locationQuery = filterView.Location?.Count > 0 ? "@1.Contains(LocationId)" : " \"\" == \"\"";
            string reportingManagerQuery = filterView.ReportingManager?.Count > 0 ? "@2.Contains(ReportingManagerId)" : " \"\" == \"\"";
            string employeeTypeQuery = filterView.EmployeeType?.Count > 0 ? "@3.Contains(EmployeeTypeId)" : " \"\" == \"\"";
            string probationStatus = filterView.ProbationStatus?.Count > 0 ? "@4.Contains(ProbationStatusId)" : " \"\" == \"\"";
            string designationQuery = filterView.Designation?.Count > 0 ? "@5.Contains(DesignationId)" : " \"\" == \"\"";
            string systemRoleQuery = filterView.SystemRole?.Count > 0 ? "@6.Contains(SystemRoleId)" : " \"\" == \"\"";
            string employeeCategoryQuery = filterView.EmployeeCategory?.Count > 0 ? "@7.Contains(EmployeeCategoryId)" : " \"\" == \"\"";
            string entityQuery = filterView.Entity?.Count > 0 ? "@8.Contains(Entity)" : " \"\" == \"\"";
            string exitTypeQuery = filterView.ExitType?.Count > 0 ? "@9.Contains(ExitType)" : " \"\" == \"\"";
            string genderQuery = filterView.Gender?.Count > 0 ? "@10.Contains(Gender)" : " \"\" == \"\"";
            string resignationStatusQuery = filterView.ResignationStatus?.Count > 0 ? "@11.Contains(ResignationStatus)" : " \"\" == \"\"";
            string employeeStatusQuery = filterView.EmployeeStatus?.Count > 0 ? "@12.Contains(isActive)" : " \"\" == \"\"";
            //string bloodGroupQuery = filterView.BloodGroup?.Count > 0 ? "@0.Contains(BloodGroup)" : " \"\" == \"\"";
            DateTime? todayDate = DateTime.Now;
            string gradeQuery = filterView?.Grade == null ? "( \"@grade\" == \"\" )" : "(EmployeeGrade ==  @grade) ";
            List<int> employeeIdList = filterView.BloodGroup?.Count > 0 ? dbContext.EmployeesPersonalInfo.Where("@0.Contains(BloodGroup)", filterView.BloodGroup).Select(x => (int)x.EmployeeId).ToList() : new List<int>();
            string employeeListQuery = employeeIdList?.Count > 0 ? "@13.Contains(EmployeeID)" : filterView.BloodGroup?.Count > 0 ? "false" : " \"\" == \"\"";
            var query = "(FirstName.StartsWith( \"@fullName\") || LastName.StartsWith( \"@fullName\") || EmployeeName.StartsWith(\"@fullName\") || \"@fullName\" == \"\")  && (FormattedEmployeeId.Contains( \"@employeeId\") || \"@employeeId\" == \"\" ) &&(" + gradeQuery + ")&&(" + DepartmentQuery + ")&&(" + locationQuery + ")&&(" + reportingManagerQuery + ")&&(" + employeeTypeQuery + ") && (" + probationStatus + ") && (" + designationQuery + ") && (" + systemRoleQuery + ") && (" + employeeCategoryQuery + ") && (" + entityQuery + ")&&(" + exitTypeQuery + ") && (" + resignationStatusQuery + ") && (" + genderQuery + ") && (" + employeeStatusQuery + ") && (" + employeeListQuery + ")";
            query = query.Replace("@fullName", filterView?.FullName?.Trim() == null ? "" : filterView.FullName.Trim());
            query = query.Replace("@employeeId", filterView?.EmployeeId == null ? "" : filterView.EmployeeId);
            query = query.Replace("@grade", filterView?.Grade == null ? "" : filterView?.Grade.ToString());
            var employees = dbContext.Employees.Where(query, filterView.Department, filterView.Location, filterView.ReportingManager, filterView.EmployeeType, filterView.ProbationStatus, filterView.Designation, filterView.SystemRole, filterView.EmployeeCategory, filterView.Entity, filterView.ExitType, filterView.Gender, filterView.ResignationStatus, filterView.EmployeeStatus, employeeIdList
            ).OrderByDescending(x => x.EmployeeID).ToList();

            if (filterView?.Age?.Condition != null && filterView.Age.Value != null && employees?.Count > 0)
            {
                if (filterView.Age.Condition == ">")
                {
                    employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) > filterView.Age.Value).ToList();
                }
                else if (filterView.Age.Condition == "<")
                {
                    employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) < filterView.Age.Value).ToList();
                }
                else
                {
                    employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) == filterView.Age.Value).ToList();
                }
                //  ageQuery = BirthDayEmployeeList?.Count > 0 ? "@14.Contains(EmployeeID)" : "false";
            }
            if (filterView.ContractEndDate?.FromDate != null && filterView.ContractEndDate.Condition != null && employees?.Count > 0)
            {
                if (filterView.ContractEndDate.Condition == ">")
                {
                    employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) > filterView.ContractEndDate.FromDate).ToList();
                }
                else if (filterView.ContractEndDate.Condition == "<")
                {
                    employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) < filterView.ContractEndDate.FromDate).ToList();
                }
                else
                {
                    employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) > filterView.ContractEndDate.FromDate && (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) < filterView.ContractEndDate.ToDate).ToList();
                }
                // ContractEndDateQuery = ContractEmployeeList?.Count > 0 ? "@15.Contains(EmployeeID)" : "false";
            }
            if (filterView.DateOfJoining?.FromDate != null && filterView.DateOfJoining.Condition != null && employees?.Count > 0)
            {
                if (filterView.DateOfJoining.Condition == ">")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > filterView.DateOfJoining.FromDate).ToList();
                }
                else if (filterView.DateOfJoining.Condition == "<")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < filterView.DateOfJoining.FromDate).ToList();
                }
                else
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > filterView.DateOfJoining.FromDate && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < filterView.DateOfJoining.ToDate).ToList();
                }
                //  DateOfJoiningQuery = DateOfJoiningEmployeeList?.Count > 0 ? "@16.Contains(EmployeeID)" : "false";
            }
            if (filterView?.TVSNextExperience?.Condition != null && employees?.Count > 0)
            {
                var fromDate = filterView.TVSNextExperience?.FromExperience.ToString().Split('.');
                var fromDateToCheck = DateTime.Now;
                if (fromDate.Length > 1)
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0])).AddMonths(-Convert.ToInt32(fromDate[1]));
                }
                else
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0]));
                }

                if (filterView.TVSNextExperience.Condition == ">")
                {
                    employees = employees.Where(x => x.DateOfJoining < fromDateToCheck).ToList();
                }
                else if (filterView.TVSNextExperience.Condition == "<")
                {
                    employees = employees.Where(x => x.DateOfJoining > fromDateToCheck).ToList();
                }
                else if (filterView.TVSNextExperience?.ToExperience != null)
                {
                    var toDate = filterView.TVSNextExperience?.ToExperience.ToString().Split('.');
                    var ToDateToCheck = DateTime.Now;
                    if (toDate.Length > 1)
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0])).AddMonths(-Convert.ToInt32(toDate[1]));
                    }
                    else
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0]));
                    }
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < fromDateToCheck && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > ToDateToCheck).ToList();
                }
                //  DateOfJoiningQuery = DateOfJoiningEmployeeList?.Count > 0 ? "@16.Contains(EmployeeID)" : "false";
            }
            //if (filterView?.TVSNextExperience?.Condition != null && employees?.Count > 0)
            //{
            //    if (filterView.TVSNextExperience.Condition == ">")
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null  || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) > filterView.TVSNextExperience.FromExperience).ToList();
            //    }
            //    else if (filterView.TVSNextExperience.Condition == "<")
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) < filterView.TVSNextExperience.FromExperience).ToList();
            //    }
            //    else
            //    {
            //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) > filterView.TVSNextExperience.FromExperience && ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) < filterView.TVSNextExperience.ToExperience).ToList();
            //    }
            //}
            if (filterView?.TotalExperience?.Condition != null && employees?.Count > 0)
            {
                var fromDate = filterView.TotalExperience?.FromExperience.ToString().Split('.');
                var fromDateToCheck = DateTime.Now;
                if (fromDate.Length > 1)
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0])).AddMonths(-Convert.ToInt32(fromDate[1]));
                }
                else
                {
                    fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0]));
                }
                if (filterView.TotalExperience.Condition == ">")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) < fromDateToCheck).ToList();
                }
                else if (filterView.TotalExperience.Condition == "<")
                {
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) > fromDateToCheck).ToList();
                    //employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) < filterView.TotalExperience.FromExperience).ToList();
                }
                else if (filterView.TotalExperience.ToExperience != null)
                {
                    var toDate = filterView.TotalExperience?.ToExperience.ToString().Split('.');
                    var ToDateToCheck = DateTime.Now;
                    if (toDate.Length > 1)
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0])).AddMonths(-Convert.ToInt32(toDate[1]));
                    }
                    else
                    {
                        ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0]));
                    }
                    employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) < fromDateToCheck && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) > ToDateToCheck).ToList();
                    //employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) > filterView.TotalExperience.FromExperience && ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) < filterView.TotalExperience.ToExperience).ToList();
                }
            }

            int? data = employees.Select(x => x).Count();
            return data == null ? 0 : (int)data;

        }
        public async Task<Employees> GetEmployeeByFormattedEmployeeIdForBulkUpload(string pFormattedEmployeeId)
        {
            return dbContext.Employees.Where(x => x.FormattedEmployeeId == pFormattedEmployeeId).FirstOrDefault();
        }

        public EmployeeDownloadData EmployeeListDownload(PaginationView paginationView)
        {
            EmployeeDownloadData employeeDownload = new EmployeeDownloadData();

            if (paginationView.IsFilterApplied == true)
            {
                List<EmployeeDetailListView> EmployeeList = new List<EmployeeDetailListView>();
                EmployeeFilterView filterView = paginationView.EmployeeFilter;
                List<int> ContractEmployeeList = new List<int>();
                List<int> DateOfJoiningEmployeeList = new List<int>();
                List<int> BirthDayEmployeeList = new List<int>();
                DateTime zeroTime = new(1, 1, 1);
                string DepartmentQuery = filterView.Department?.Count > 0 ? "@0.Contains(DepartmentId)" : " \"\" == \"\"";
                string locationQuery = filterView.Location?.Count > 0 ? "@1.Contains(LocationId)" : " \"\" == \"\"";
                string reportingManagerQuery = filterView.ReportingManager?.Count > 0 ? "@2.Contains(ReportingManagerId)" : " \"\" == \"\"";
                string employeeTypeQuery = filterView.EmployeeType?.Count > 0 ? "@3.Contains(EmployeeTypeId)" : " \"\" == \"\"";
                string probationStatus = filterView.ProbationStatus?.Count > 0 ? "@4.Contains(ProbationStatusId)" : " \"\" == \"\"";
                string designationQuery = filterView.Designation?.Count > 0 ? "@5.Contains(DesignationId)" : " \"\" == \"\"";
                string systemRoleQuery = filterView.SystemRole?.Count > 0 ? "@6.Contains(SystemRoleId)" : " \"\" == \"\"";
                string employeeCategoryQuery = filterView.EmployeeCategory?.Count > 0 ? "@7.Contains(EmployeeCategoryId)" : " \"\" == \"\"";
                string entityQuery = filterView.Entity?.Count > 0 ? "@8.Contains(Entity)" : " \"\" == \"\"";
                string exitTypeQuery = filterView.ExitType?.Count > 0 ? "@9.Contains(ExitType)" : " \"\" == \"\"";
                string genderQuery = filterView.Gender?.Count > 0 ? "@10.Contains(Gender)" : " \"\" == \"\"";
                string resignationStatusQuery = filterView.ResignationStatus?.Count > 0 ? "@11.Contains(ResignationStatus)" : " \"\" == \"\"";
                string employeeStatusQuery = filterView.EmployeeStatus?.Count > 0 ? "@12.Contains(isActive)" : " \"\" == \"\"";
                //string bloodGroupQuery = filterView.BloodGroup?.Count > 0 ? "@0.Contains(BloodGroup)" : " \"\" == \"\"";
                DateTime? todayDate = DateTime.Now;
                string gradeQuery = filterView?.Grade == null ? "( \"@grade\" == \"\" )" : "(EmployeeGrade ==  @grade) ";
                List<int> employeeIdList = filterView.BloodGroup?.Count > 0 ? dbContext.EmployeesPersonalInfo.Where("@0.Contains(BloodGroup)", filterView.BloodGroup).Select(x => (int)x.EmployeeId).ToList() : new List<int>();
                string employeeListQuery = employeeIdList?.Count > 0 ? "@13.Contains(EmployeeID)" : filterView.BloodGroup?.Count > 0 ? "false" : " \"\" == \"\"";
                var query = "(FirstName.StartsWith( \"@fullName\") || LastName.StartsWith( \"@fullName\") || EmployeeName.StartsWith(\"@fullName\") || \"@fullName\" == \"\")  && (FormattedEmployeeId.Contains( \"@employeeId\") || \"@employeeId\" == \"\" ) &&(" + gradeQuery + ")&&(" + DepartmentQuery + ")&&(" + locationQuery + ")&&(" + reportingManagerQuery + ")&&(" + employeeTypeQuery + ") && (" + probationStatus + ") && (" + designationQuery + ") && (" + systemRoleQuery + ") && (" + employeeCategoryQuery + ") && (" + entityQuery + ")&&(" + exitTypeQuery + ") && (" + resignationStatusQuery + ") && (" + genderQuery + ") && (" + employeeStatusQuery + ") && (" + employeeListQuery + ")";
                query = query.Replace("@fullName", filterView?.FullName == null ? "" : filterView.FullName.Trim());
                query = query.Replace("@employeeId", filterView?.EmployeeId == null ? "" : filterView.EmployeeId);
                query = query.Replace("@grade", filterView?.Grade == null ? "" : filterView?.Grade.ToString());
                var employees = dbContext.Employees.Where(query, filterView.Department, filterView.Location, filterView.ReportingManager, filterView.EmployeeType, filterView.ProbationStatus, filterView.Designation, filterView.SystemRole, filterView.EmployeeCategory, filterView.Entity, filterView.ExitType, filterView.Gender, filterView.ResignationStatus, filterView.EmployeeStatus, employeeIdList
                ).OrderByDescending(x => x.EmployeeID).ToList();

                //  var employees = dbContext.Employees.Where(query, filterView.Department, filterView.Location, filterView.ReportingManager, filterView.EmployeeType, filterView.ProbationStatus, filterView.Designation, filterView.SystemRole, filterView.EmployeeCategory, filterView.Entity, filterView.ExitType, filterView.Gender, filterView.ResignationStatus, filterView.EmployeeStatus, employeeIdList
                //).OrderByDescending(x => x.EmployeeID).ToList();
                if (filterView?.Age?.Condition != null && filterView.Age.Value != null && employees?.Count > 0)
                {
                    if (filterView.Age.Condition == ">")
                    {
                        employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) > filterView.Age.Value).ToList();
                    }
                    else if (filterView.Age.Condition == "<")
                    {
                        employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) < filterView.Age.Value).ToList();
                    }
                    else
                    {
                        employees = employees.Where(x => (DateTime.Now.Year - (x.BirthDate == null ? DateTime.Now.Year : x.BirthDate.Value.Year)) == filterView.Age.Value).ToList();
                    }
                    //  ageQuery = BirthDayEmployeeList?.Count > 0 ? "@14.Contains(EmployeeID)" : "false";
                }
                if (filterView.ContractEndDate?.FromDate != null && filterView.ContractEndDate.Condition != null && employees?.Count > 0)
                {
                    if (filterView.ContractEndDate.Condition == ">")
                    {
                        employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) > filterView.ContractEndDate.FromDate).ToList();
                    }
                    else if (filterView.ContractEndDate.Condition == "<")
                    {
                        employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) < filterView.ContractEndDate.FromDate).ToList();
                    }
                    else
                    {
                        employees = employees.Where(x => (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) > filterView.ContractEndDate.FromDate && (x.ContractEndDate == null ? DateTime.Now : x.ContractEndDate.Value) < filterView.ContractEndDate.ToDate).ToList();
                    }
                    // ContractEndDateQuery = ContractEmployeeList?.Count > 0 ? "@15.Contains(EmployeeID)" : "false";
                }
                if (filterView.DateOfJoining?.FromDate != null && filterView.DateOfJoining.Condition != null && employees?.Count > 0)
                {
                    if (filterView.DateOfJoining.Condition == ">")
                    {
                        employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > filterView.DateOfJoining.FromDate).ToList();
                    }
                    else if (filterView.DateOfJoining.Condition == "<")
                    {
                        employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < filterView.DateOfJoining.FromDate).ToList();
                    }
                    else
                    {
                        employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > filterView.DateOfJoining.FromDate && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < filterView.DateOfJoining.ToDate).ToList();
                    }
                    //  DateOfJoiningQuery = DateOfJoiningEmployeeList?.Count > 0 ? "@16.Contains(EmployeeID)" : "false";
                }
                //if (filterView?.TVSNextExperience?.Condition != null && employees?.Count > 0)
                //{
                //    if (filterView.TVSNextExperience.Condition == ">")
                //    {
                //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) > filterView.TVSNextExperience.FromExperience).ToList();
                //    }
                //    else if (filterView.TVSNextExperience.Condition == "<")
                //    {
                //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) < filterView.TVSNextExperience.FromExperience).ToList();
                //    }
                //    else
                //    {
                //        employees = employees.Where(x => ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) > filterView.TVSNextExperience.FromExperience && ((((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now.ToLocalTime() - ((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1).ToLocalTime() : (DateTime)x.DateOfJoining))).Month - 1)*0.01)) < filterView.TVSNextExperience.ToExperience).ToList();
                //    }
                //}
                if (filterView?.TVSNextExperience?.Condition != null && employees?.Count > 0)
                {
                    var fromDate = filterView.TVSNextExperience?.FromExperience.ToString().Split('.');
                    var fromDateToCheck = DateTime.Now;
                    if (fromDate.Length > 1)
                    {
                        fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0])).AddMonths(-Convert.ToInt32(fromDate[1]));
                    }
                    else
                    {
                        fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0]));
                    }

                    if (filterView.TVSNextExperience.Condition == ">")
                    {
                        employees = employees.Where(x => x.DateOfJoining < fromDateToCheck).ToList();
                    }
                    else if (filterView.TVSNextExperience.Condition == "<")
                    {
                        employees = employees.Where(x => x.DateOfJoining > fromDateToCheck).ToList();
                    }
                    else if (filterView.TVSNextExperience?.ToExperience != null)
                    {
                        var toDate = filterView.TVSNextExperience?.ToExperience.ToString().Split('.');
                        var ToDateToCheck = DateTime.Now;
                        if (toDate.Length > 1)
                        {
                            ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0])).AddMonths(-Convert.ToInt32(toDate[1]));
                        }
                        else
                        {
                            ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0]));
                        }
                        employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) < fromDateToCheck && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value) > ToDateToCheck).ToList();
                    }
                    //  DateOfJoiningQuery = DateOfJoiningEmployeeList?.Count > 0 ? "@16.Contains(EmployeeID)" : "false";
                }
                if (filterView?.TotalExperience?.Condition != null && employees?.Count > 0)
                {
                    var fromDate = filterView.TotalExperience?.FromExperience.ToString().Split('.');
                    var fromDateToCheck = DateTime.Now;
                    if (fromDate.Length > 1)
                    {
                        fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0])).AddMonths(-Convert.ToInt32(fromDate[1]));
                    }
                    else
                    {
                        fromDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(fromDate[0]));
                    }
                    if (filterView.TotalExperience.Condition == ">")
                    {
                        employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) < fromDateToCheck).ToList();
                    }
                    else if (filterView.TotalExperience.Condition == "<")
                    {
                        employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) > fromDateToCheck).ToList();
                        //employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) < filterView.TotalExperience.FromExperience).ToList();
                    }
                    else if (filterView.TotalExperience.ToExperience != null)
                    {
                        var toDate = filterView.TotalExperience?.ToExperience.ToString().Split('.');
                        var ToDateToCheck = DateTime.Now;
                        if (toDate.Length > 1)
                        {
                            ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0])).AddMonths(-Convert.ToInt32(toDate[1]));
                        }
                        else
                        {
                            ToDateToCheck = DateTime.Now.AddYears(-Convert.ToInt32(toDate[0]));
                        }
                        employees = employees.Where(x => (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) < fromDateToCheck && (x.DateOfJoining == null ? DateTime.Now : x.DateOfJoining.Value.AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))) > ToDateToCheck).ToList();
                        //employees = employees.Where(x => ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) > filterView.TotalExperience.FromExperience && ((((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Year - 1) * 1.0) + (((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) < 10 ? ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.1 : ((zeroTime + (DateTime.Now - (((x.DateOfJoining == null || x.DateOfJoining > DateTime.Now) ? DateTime.Now.AddDays(-1) : (DateTime)x.DateOfJoining).AddYears(-(int)Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)).AddMonths(-Math.Abs((int)(Math.Truncate(x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience) - (x.PreviousExperience == null ? 0 : (decimal)x.PreviousExperience)) * 10))))).Month - 1) * 0.01)) < filterView.TotalExperience.ToExperience).ToList();
                    }
                }

                List<int> employeeId = employees.Select(x => x.EmployeeID).ToList();
                employeeDownload.Employee = employees;
                employeeDownload.EmployeesPersonalInfoDetail = dbContext.EmployeesPersonalInfo.Where(x => employeeId.Contains((int)x.EmployeeId)).OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.WorkHistory = dbContext.WorkHistory.Where(x => employeeId.Contains((int)x.EmployeeId)).OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.EducationDetail = dbContext.EducationDetail.Where(x => employeeId.Contains((int)x.EmployeeId)).OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.CompensationDetail = dbContext.CompensationDetail.Where(x => employeeId.Contains((int)x.EmployeeID)).OrderBy(x => x.EmployeeID).ToList();
                employeeDownload.EmployeesSkillsets = dbContext.EmployeesSkillset.Where(x => employeeId.Contains(x.EmployeeId)).OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.EmployeeDependents = dbContext.EmployeeDependent.Where(x => employeeId.Contains((int)x.EmployeeID)).OrderBy(x => x.EmployeeID).ToList();
                employeeDownload.EmployeeSpecialAbilities = dbContext.EmployeeSpecialAbility.Where(x => employeeId.Contains((int)x.EmployeeId)).OrderBy(x => x.EmployeeId).ToList();
            }
            else
            {
                employeeDownload.Employee = dbContext.Employees.OrderBy(x => x.EmployeeID).ToList();
                employeeDownload.EmployeesPersonalInfoDetail = dbContext.EmployeesPersonalInfo.OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.WorkHistory = dbContext.WorkHistory.OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.EducationDetail = dbContext.EducationDetail.OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.CompensationDetail = dbContext.CompensationDetail.OrderBy(x => x.EmployeeID).ToList();
                employeeDownload.EmployeeDependents = dbContext.EmployeeDependent.OrderBy(x => x.EmployeeID).ToList();
                employeeDownload.EmployeeSpecialAbilities = dbContext.EmployeeSpecialAbility.OrderBy(x => x.EmployeeId).ToList();
                employeeDownload.EmployeesSkillsets = dbContext.EmployeesSkillset.OrderBy(x => x.EmployeeId).ToList();
            }
            return employeeDownload;
        }
        public async Task<EmployeeManagerDetails> reportingManagerData(int reportingManagerid)
        {

            EmployeeManagerDetails res = (from managerEmployee in dbContext.Employees
                                          where managerEmployee.EmployeeID == reportingManagerid && managerEmployee.IsActive == true
                                          select new EmployeeManagerDetails
                                          {
                                              EmailAddress = managerEmployee.EmailAddress,
                                              ManagerName = managerEmployee.EmployeeName,
                                              ManagerID = managerEmployee.EmployeeID,
                                          }).FirstOrDefault();

            return res;
        }
        public async Task<EmployeeDesignationDetails> designationData(int designationid)
        {
            EmployeeDesignationDetails res = (from designation in dbContext.Designation
                                              where designation.DesignationId == designationid
                                              select new EmployeeDesignationDetails
                                              {
                                                  DesignationId = designation.DesignationId,
                                                  DesignationName = designation.DesignationName,
                                              }).FirstOrDefault();
            return res;
        }
        public async Task<EmployeeBaseWorkLocationDetails> baseWorkLocationData(int locationid)
        {
            EmployeeBaseWorkLocationDetails res = (from location in dbContext.EmployeeLocation
                                                   where location.LocationId == locationid
                                                   select new EmployeeBaseWorkLocationDetails
                                                   {
                                                       LocationId = location.LocationId,
                                                       LocationName = location.Location,
                                                   }).FirstOrDefault();
            return res;
        }
        //public  async   Task<EmployeeProbationStatusDetails> probationStatusData(int probationstatusid)
        //   {
        //       EmployeeProbationStatusDetails res = (from probationstatus in dbContext.ProbationStatus
        //                                              where probationstatus.ProbationStatusId == probationstatusid
        //                                              select new EmployeeProbationStatusDetails
        //                                              {
        //                                                  ProbationStatusId=probationstatus.ProbationStatusId,
        //                                                  ProbationStatusName=probationstatus.ProbationStatusName,
        //                                              }).FirstOrDefault();
        //       return res;

        //   }
        public async Task<EmployeeMasterEmailTemplate> getEmployeeEmailByName(string templateName)
        {
            EmployeeMasterEmailTemplate res = (from templatedata in dbContext.EmployeeMasterEmailTemplate
                                               where templatedata.TemplateName == templateName
                                               select new EmployeeMasterEmailTemplate()
                                               {
                                                   TemplateId = templatedata.TemplateId,
                                                   TemplateName = templatedata.TemplateName,
                                                   Body = templatedata.Body,
                                                   Subject = templatedata.Subject,

                                               }).FirstOrDefault();
            return res;
        }


        //public EmployeeRequestListView GetEmployeeRequest(int employeeID)
        //{
        //    EmployeeRequestListView employeeRequestList = new EmployeeRequestListView();
        //    List<EmployeeRequestView> employeeRequestlst = new List<EmployeeRequestView>();
        //    List<EmployeeRequest> lstEmployeeRequest = dbContext.EmployeeRequest.Where(x => x.EmployeeId == employeeID).ToList();
        //    List<EmployeeRequestDetails> lstEmployeeRequestDetails = new List<EmployeeRequestDetails>();
        //    if (lstEmployeeRequest?.Count > 0)
        //    {
        //        foreach (EmployeeRequest employeeRequest in lstEmployeeRequest)
        //        {
        //            if (employeeRequest != null)
        //            {
        //                EmployeeRequestView employeeRequest1 = new EmployeeRequestView();

        //                employeeRequest1.EmployeeRequestId = employeeRequest.EmployeeRequestId;
        //                employeeRequest1.EmployeeId = employeeRequest.EmployeeId;
        //                employeeRequest1.RequestCategory = employeeRequest.RequestCategory;
        //                employeeRequest1.ChangeRequestId = employeeRequest.ChangeRequestId;
        //                employeeRequest1.Status = employeeRequest.Status;
        //                employeeRequest1.CreatedOn = employeeRequest.CreatedOn;
        //                employeeRequest1.CreatedBy = employeeRequest.CreatedBy;
        //                employeeRequest1.Remarks = employeeRequest.Remarks;
        //                employeeRequest1.ApprovedBy = employeeRequest.ApprovedBy;

        //                employeeRequestlst.Add(employeeRequest1);

        //                // if (employeeRequest1.EmployeeRequestDetailslst == null)
        //                employeeRequest1.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();

        //                List<EmployeeRequestDetails> listEmployeeRequestDetails = dbContext.EmployeeRequestDetails.Where(x => x.ChangeRequestId == employeeRequest.ChangeRequestId).ToList();

        //                foreach (EmployeeRequestDetails employeeRequestDetails in listEmployeeRequestDetails)
        //                {
        //                    lstEmployeeRequestDetails.Add(employeeRequestDetails);
        //                }

        //                foreach (EmployeeRequestDetails employeeRequestDetails1 in lstEmployeeRequestDetails)
        //                {
        //                    EmployeeRequestDetailsView emplRequestDetails = new EmployeeRequestDetailsView();
        //                    emplRequestDetails.EmployeeRequestDetailId = employeeRequestDetails1.EmployeeRequestDetailId;
        //                    emplRequestDetails.ChangeRequestId = employeeRequestDetails1.ChangeRequestId;
        //                    emplRequestDetails.Field = employeeRequestDetails1.Field;
        //                    emplRequestDetails.OldValue = employeeRequestDetails1.OldValue;
        //                    emplRequestDetails.NewValue = employeeRequestDetails1.NewValue; emplRequestDetails.CreatedOn = employeeRequestDetails1.CreatedOn;
        //                    emplRequestDetails.CreatedBy = employeeRequestDetails1.CreatedBy;
        //                    employeeRequest1.EmployeeRequestDetailslst.Add(emplRequestDetails);
        //                }
        //                lstEmployeeRequestDetails.Clear();
        //            }
        //        }
        //    }
        //    employeeRequestList.EmployeeRequestlst = employeeRequestlst;
        //    return employeeRequestList;
        //}


        public EmployeeRequestListView GetEmployeeRequestForAdmin()
        {
            EmployeeRequestListView employeeRequestList = new EmployeeRequestListView();
            List<EmployeeRequestView> employeeRequestlst = new List<EmployeeRequestView>();
            List<EmployeeRequest> lstEmployeeRequest = dbContext.EmployeeRequest.ToList();
            //List<Guid> lstChangeRequest = dbContext.EmployeeRequest.Where(x => x.EmployeeId == employeeID).ToList().Select(x => x.ChangeRequestId).Distinct().ToList();

            List<EmployeeRequestDetail> lstEmployeeRequestDetails = new List<EmployeeRequestDetail>();
            if (lstEmployeeRequest?.Count > 0)
            {
                foreach (EmployeeRequest employeeRequest in lstEmployeeRequest)
                {
                    if (employeeRequest != null)
                    {
                        EmployeeRequestView employeeRequest1 = new EmployeeRequestView();

                        employeeRequest1.EmployeeRequestId = employeeRequest.EmployeeRequestId;
                        employeeRequest1.EmployeeId = employeeRequest.EmployeeId;
                        employeeRequest1.RequestCategory = employeeRequest.RequestCategory;
                        employeeRequest1.ChangeRequestId = employeeRequest.ChangeRequestId;
                        employeeRequest1.Status = employeeRequest.Status;
                        employeeRequest1.CreatedOn = employeeRequest.CreatedOn;
                        employeeRequest1.CreatedBy = employeeRequest.CreatedBy;
                        employeeRequest1.Remarks = employeeRequest.Remark;
                        employeeRequest1.ApprovedBy = employeeRequest.ApprovedBy;

                        employeeRequestlst.Add(employeeRequest1);

                        // if (employeeRequest1.EmployeeRequestDetailslst == null)
                        employeeRequest1.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();

                        List<EmployeeRequestDetail> listEmployeeRequestDetails = dbContext.EmployeeRequestDetails.Where(x => x.ChangeRequestId == employeeRequest.ChangeRequestId).ToList();

                        foreach (EmployeeRequestDetail employeeRequestDetails in listEmployeeRequestDetails)
                        {
                            lstEmployeeRequestDetails.Add(employeeRequestDetails);
                        }

                        foreach (EmployeeRequestDetail employeeRequestDetails1 in lstEmployeeRequestDetails)
                        {
                            EmployeeRequestDetailsView emplRequestDetails = new EmployeeRequestDetailsView();
                            emplRequestDetails.EmployeeRequestDetailId = employeeRequestDetails1.EmployeeRequestDetailId;
                            emplRequestDetails.ChangeRequestId = employeeRequestDetails1.ChangeRequestId;
                            emplRequestDetails.Field = employeeRequestDetails1.Field;
                            emplRequestDetails.OldValue = employeeRequestDetails1.OldValue;
                            emplRequestDetails.NewValue = employeeRequestDetails1.NewValue; emplRequestDetails.CreatedOn = employeeRequestDetails1.CreatedOn;
                            emplRequestDetails.CreatedBy = employeeRequestDetails1.CreatedBy;
                            employeeRequest1.EmployeeRequestDetailslst.Add(emplRequestDetails);
                        }
                        lstEmployeeRequestDetails.Clear();
                    }
                }
            }
            employeeRequestList.EmployeeRequestlst = employeeRequestlst;
            return employeeRequestList;
        }

        public async Task<List<EmployeeName>> ContractEndDateNotification()
        {
            return dbContext.Employees.Where(x => x.ContractEndDate != null && (DateTime.Now.Date == x.ContractEndDate.Value.AddDays(-14).Date || DateTime.Now.Date == x.ContractEndDate.Value.AddDays(-6).Date || DateTime.Now.Date == x.ContractEndDate.Value.AddDays(-2).Date || DateTime.Now.Date == x.ContractEndDate.Value.AddDays(-29).Date)).Select(x =>
             new EmployeeName
             {
                 EmployeeId = x.EmployeeID,
                 EmployeeFullName = x.EmployeeName,
                 EmployeeEmailId = x.EmailAddress,
                 FormattedEmployeeId = x.FormattedEmployeeId,
                 ContractEndDate = x.ContractEndDate,
                 ReportingManagerData = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => new EmailClass
                 {
                     ReportingManagerName = x.EmployeeName,
                     ReportingManagerEmailId = x.EmailAddress
                 }).FirstOrDefault()
             }).ToList();
        }
        public List<EmployeeDataForDropDown> GetEmployeeDropDownList(bool isAll)
        {
            if (isAll)
            {
                return (from users in dbContext.Employees
                        select new EmployeeDataForDropDown
                        {
                            EmployeeId = users.EmployeeID,
                            FormattedEmployeeId = users.FormattedEmployeeId,
                            EmployeeName = users.EmployeeName,
                            EmployeeFullName = users.FirstName + " " + users.LastName + " - " + users.FormattedEmployeeId
                        }).OrderBy(x => x.EmployeeName).ToList();
            }
            else
            {
                return (from users in dbContext.Employees
                        where users.IsActive == true
                        select new EmployeeDataForDropDown
                        {
                            EmployeeId = users.EmployeeID,
                            FormattedEmployeeId = users.FormattedEmployeeId,
                            EmployeeName = users.EmployeeName,
                            EmployeeFullName = users.FirstName + " " + users.LastName + " - " + users.FormattedEmployeeId
                        }).OrderBy(x => x.EmployeeName).ToList();
            }

        }

        public async Task<Employees> GetEmployeeForBUlkUploadByEmployeeId(int employeeID)
        {
            return dbContext.Employees.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
        }

        private int CalculateNoticePeriod(DateTime? DateOfJoining, int? probationStatus, string noticeCategory)
        {
            int? noticePeriod = 0;
            List<NoticePeriodCategory> noticePeriodCategories = dbContext.NoticePeriodCategory.ToList();
            DateTime? doj = DateOfJoining == null ? DateTime.Now.Date : DateOfJoining;
            switch (noticeCategory)
            {
                case "general":
                    if (doj.Value.AddYears(1).Date < DateTime.Now.Date)
                    {
                        noticePeriod = noticePeriodCategories.Where(x => x.CategoryName == noticeCategory && x.SubCategoryName == "MoreThanOneYear").Select(x => x.NoticePeriodDays).FirstOrDefault();
                    }
                    else
                    {
                        noticePeriod = noticePeriodCategories.Where(x => x.CategoryName == noticeCategory && x.SubCategoryName == "LessThanOneYear").Select(x => x.NoticePeriodDays).FirstOrDefault();
                    }
                    break;
                case "sfl":
                    noticePeriod = noticePeriodCategories.Where(x => x.CategoryName == noticeCategory).Select(x => x.NoticePeriodDays).FirstOrDefault();
                    break;
                case "sales_juniors":
                    if (probationStatus == 3)
                    {
                        noticePeriod = noticePeriodCategories.Where(x => x.CategoryName == noticeCategory && x.SubCategoryName == "AfterConfirmation").Select(x => x.NoticePeriodDays).FirstOrDefault();
                    }
                    else
                    {
                        noticePeriod = noticePeriodCategories.Where(x => x.CategoryName == noticeCategory && x.SubCategoryName == "BeforeConfirmation").Select(x => x.NoticePeriodDays).FirstOrDefault();
                    }
                    break;
                case "trainee":
                    if (probationStatus == 3)
                    {
                        noticePeriod = noticePeriodCategories.Where(x => x.CategoryName == noticeCategory && x.SubCategoryName == "AfterConfirmation").Select(x => x.NoticePeriodDays).FirstOrDefault();
                    }
                    else
                    {
                        noticePeriod = noticePeriodCategories.Where(x => x.CategoryName == noticeCategory && x.SubCategoryName == "BeforeConfirmation").Select(x => x.NoticePeriodDays).FirstOrDefault();
                    }
                    break;
                default:
                    noticePeriod = 0;
                    break;
            }
            return (noticePeriod == null ? 0 : (int)noticePeriod);
        }
        public List<NoticePeriodCategory> GetNoticeCategory()
        {
            return dbContext.NoticePeriodCategory.ToList();
        }
        #region Get Employee PersonalInfo By EmployeeID
        public ResignationEmployeeMasterView GetEmployeePersonalInfoByEmployeeID(int employeeId)
        {
            return (from users in dbContext.Employees
                    join info in dbContext.EmployeesPersonalInfo on users.EmployeeID equals info.EmployeeId
                    where users.EmployeeID == employeeId
                    select new ResignationEmployeeMasterView
                    {
                        EmployeeID = users.EmployeeID,
                        EmployeeName = users.EmployeeName + '-' + users.FormattedEmployeeId,
                        FormattedEmployeeID = users.FormattedEmployeeId,
                        Designation = dbContext.Designation.Where(y => y.DesignationId == users.DesignationId).Select(z => z.DesignationName).FirstOrDefault(),
                        DepartmentId = users.DepartmentId,
                        LocationId = users.LocationId,
                        DesignationId = users.DesignationId,
                        RelievingDate = users.DateOfRelieving,
                        ResignationDate = users.ResignationDate,
                        ReportingManagerId = users.ReportingManagerId == null ? 0 : (int)users.ReportingManagerId,
                        ReportingManagerName = dbContext.Employees.Where(y => y.EmployeeID == users.ReportingManagerId).Select(x => users.EmployeeName).FirstOrDefault(),
                        ReportingManagerEmail = dbContext.Employees.Where(y => y.EmployeeID == users.ReportingManagerId).Select(x => users.EmailAddress).FirstOrDefault(),
                        DateOfJoining = users.DateOfJoining,
                        DepartmentName = dbContext.Department.Where(y => y.DepartmentId == users.DepartmentId).Select(z => z.DepartmentName).FirstOrDefault(),
                        EmployeeEmail = users.EmailAddress,
                        FirstName = users.FirstName,
                        LastName = users.LastName,
                        PersonalInfo = new EmployeesPersonalInfoView
                        {
                            EmployeeID = users.EmployeeID,
                            EmergencyMobileNumber = info.EmergencyMobileNumber,
                            PersonalMobileNumber = info.PersonalMobileNumber,
                            OtherEmail = info.OtherEmail,
                            PermanentAddressLine1 = info.PermanentAddressLine1,
                            PermanentAddressLine2 = info.PermanentAddressLine2,
                            PermanentCity = info.PermanentCity,
                            PermanentState = info.PermanentState,
                            PermanentCountry = info.PermanentCountry,
                            PermanentAddressZip = info.PermanentAddressZip,
                            CommunicationAddressLine1 = info.CommunicationAddressLine1,
                            CommunicationAddressLine2 = info.CommunicationAddressLine2,
                            CommunicationCity = info.CommunicationCity,
                            CommunicationState = info.CommunicationState,
                            CommunicationCountry = info.CommunicationCountry,
                            CommunicationAddressZip = info.CommunicationAddressZip
                        }
                    }).FirstOrDefault();
        }
        #endregion

        public EmployeeBasicInfoView GetEmployeeBasicInfoByEmployeeID(int employeeId)
        {
            EmployeeBasicInfoView employeeViewModel = new EmployeeBasicInfoView();
            employeeViewModel.Employee = dbContext.Employees.Where(x => x.EmployeeID == employeeId).Select(x =>
            new EmployeeView
            {
                EmployeeID = x.EmployeeID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                DepartmentId = x.DepartmentId,
                DepartmentName = dbContext.Department.Where(y => y.DepartmentId == x.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault(),
                DepartmentHead = dbContext.Employees.Where(y => y.EmployeeID == dbContext.Department.Where(z => z.DepartmentId == x.DepartmentId).Select(x => x.DepartmentHeadEmployeeId).FirstOrDefault()).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault(),
                DateOfJoining = x.DateOfJoining,
                DateOfContract = x.DateOfContract,
                DateOfRelieving = x.DateOfRelieving,
                DesignationEffectiveFrom = x.DesignationEffectiveFrom,
                ContractEndDate = x.ContractEndDate,
                ActualConfirmationDate = x.ActualConfirmationDate,
                MergerDate = x.MergerDate,
                JobTitle = x.JobTitle,
                IsActive = x.IsActive,
                Gender = x.Gender,
                BirthDate = x.BirthDate,
                Maritalstatus = x.Maritalstatus,
                FormattedEmployeeId = x.FormattedEmployeeId,
                Mobile = x.Mobile,
                OfficialMobileNumber = x.OfficialMobileNumber,
                EmployeeName = x.EmployeeName,
                ExitType = x.ExitType,
                ResignationDate = x.ResignationDate,
                RetirementDate = x.RetirementDate,
                ResignationReason = x.ResignationReason,
                ResignationStatus = x.ResignationStatus,
                IsResign = x.IsResign,
                ReportingManagerId = x.ReportingManagerId,
                ReportingManager = dbContext.Employees.Where(y => y.EmployeeID == x.ReportingManagerId).Select(x => x.EmployeeName + " - " + x.FormattedEmployeeId).FirstOrDefault(),
                LocationId = x.LocationId,
                Location = dbContext.EmployeeLocation.Where(y => y.LocationId == x.LocationId).Select(x => x.Location).FirstOrDefault(),
                DesignationId = x.DesignationId,
                Designation = dbContext.Designation.Where(y => y.DesignationId == x.DesignationId).Select(x => x.DesignationName).FirstOrDefault(),
                ProbationStatusId = x.ProbationStatusId,
                ProbationStatus = dbContext.ProbationStatus.Where(y => y.ProbationStatusId == x.ProbationStatusId).Select(x => x.ProbationStatusName).FirstOrDefault(),
                SystemRoleId = x.SystemRoleId,
                SystemRole = dbContext.SystemRoles.Where(y => y.RoleId == x.SystemRoleId).Select(x => x.RoleName).FirstOrDefault(),
                NoticeCategory = x.NoticeCategory,
                ProfilePicture = x.ProfilePicture,
                CurrentWorkPlaceId = x.CurrentWorkPlaceId,
                CurrentWorkLocationId = x.CurrentWorkLocationId,
                RoleId = x.RoleId
            }).FirstOrDefault();
            if (employeeViewModel?.Employee != null)
            {
                var NoticePeriod = CalculateNoticePeriod(employeeViewModel?.Employee?.DateOfJoining, employeeViewModel?.Employee?.ProbationStatusId, employeeViewModel?.Employee?.NoticeCategory);
                employeeViewModel.Employee.NoticePeriod = NoticePeriod;
            }
            return employeeViewModel;
        }

        #region Get  employee list for new resignation
        public List<EmployeeDataForDropDown> GetEmployeeListForNewResignation()
        {
            return (from users in dbContext.Employees
                    where users.IsActive == true && string.IsNullOrEmpty(users.ResignationStatus)
                    select new EmployeeDataForDropDown
                    {
                        EmployeeId = users.EmployeeID,
                        FormattedEmployeeId = users.FormattedEmployeeId,
                        EmployeeName = users.EmployeeName,
                        EmployeeFullName = users.FirstName + " " + users.LastName + " - " + users.FormattedEmployeeId
                    }).OrderBy(x => x.EmployeeName).ToList();
        }

        #endregion

        public int GetEmployeeAttendanceDetailsCount(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            int resouceCount = employeesAttendanceFilterView?.ResourceId?.Count == 0 ? 0 : employeesAttendanceFilterView.ResourceId.Count;
            int departmentCount = employeesAttendanceFilterView?.DepartmentId?.Count == 0 ? 0 : employeesAttendanceFilterView.DepartmentId.Count;
            int designationCount = employeesAttendanceFilterView?.DesignationId?.Count == 0 ? 0 : employeesAttendanceFilterView.DesignationId.Count;
            int locationCount = employeesAttendanceFilterView?.LocationId?.Count == 0 ? 0 : employeesAttendanceFilterView.LocationId.Count;
            int totalCount = (from users in dbContext.Employees
                              join dept in dbContext.Department on users.DepartmentId equals dept.DepartmentId into jnDeptDetails
                              from dept in jnDeptDetails.DefaultIfEmpty()
                              join loc in dbContext.EmployeeLocation on users.LocationId equals loc.LocationId into jnlocDetails
                              from loc in jnlocDetails.DefaultIfEmpty()
                              join desg in dbContext.Designation on users.DesignationId equals desg.DesignationId into jndesgDetails
                              from desg in jndesgDetails.DefaultIfEmpty()
                              where users.IsActive == true && users.ReportingManagerId == (employeesAttendanceFilterView.EmployeeId == 0 ? users.ReportingManagerId : employeesAttendanceFilterView.EmployeeId)
                              && users.DateOfJoining.Value.Date <= employeesAttendanceFilterView.ToDate.Date
                              && (resouceCount == 0 || employeesAttendanceFilterView.ResourceId.Contains(users.EmployeeID))
                              && (departmentCount == 0 || employeesAttendanceFilterView.DepartmentId.Contains(users.DepartmentId == null ? 0 : (int)users.DepartmentId))
                              && (designationCount == 0 || employeesAttendanceFilterView.DesignationId.Contains(users.DesignationId == null ? 0 : (int)users.DesignationId))
                              && (locationCount == 0 || employeesAttendanceFilterView.LocationId.Contains(users.LocationId == null ? 0 : (int)users.LocationId))
                              select new EmployeeAttendanceDetails()
                              {
                                  EmployeeId = users.EmployeeID,
                                  EmployeeName = users.FirstName + " " + users.LastName,
                                  Department = dept != null ? dept.DepartmentName : string.Empty,
                                  Location = loc != null ? loc.Location : string.Empty,
                                  Designation = desg != null ? desg.DesignationName : string.Empty,
                                  ShiftDetailId = (int)dbContext.EmployeeShiftDetails.Where(x => x.EmployeeID == users.EmployeeID
                                                  && employeesAttendanceFilterView.FromDate.Date >= x.ShiftFromDate
                                                  && (x.ShiftToDate == null || employeesAttendanceFilterView.FromDate.Date <= x.ShiftToDate))
                                                  .Select(x => x.ShiftDetailsId).FirstOrDefault(),
                                  EmployeeEmailId = users.EmailAddress,
                                  FormattedEmployeeId = users.FormattedEmployeeId,
                              }).ToList().Count();
            return totalCount;
        }

        public EmployeeDetailsForLeave GetEmployeeByEmployeeIdForLeaves(int employeeID)
        {
            return dbContext.Employees.Where(x => x.EmployeeID == employeeID).Select(x => new EmployeeDetailsForLeave
            {
                EmployeeId = x.EmployeeID,
                EmployeeName = x.EmployeeName,
                FormattedEmployeeId = x.FormattedEmployeeId,
                departmentId = x.DepartmentId == null ? 0 : (int)x.DepartmentId,
                locationId = x.LocationId == null ? 0 : (int)x.LocationId,
                BirthDate = x.BirthDate,
                WeddingAnniversary = x.WeddingAnniversary,
                DateOfJoining = x.DateOfJoining
            }).FirstOrDefault();
        }

        public string GetEntityName(int Entity)
        {
            return dbContext.EmployeeAppConstants.Where(y => y.AppConstantId == Entity).Select(x => x.AppConstantValue).FirstOrDefault();
        }

        public List<EmployeeList> GetEmployeeDetailForGrantLeaveById(EmployeeListByDepartment employeeList)
        {
            return dbContext.Employees.Where(x => employeeList.EmployeeId.Contains(x.EmployeeID) && x.ReportingManagerId != employeeList.managerId).Select(y => new
                            EmployeeList
            {
                EmployeeId = y.EmployeeID,
                EmployeeFullName = y.FirstName + " " + y.LastName,
                FormattedEmployeeId = y.FormattedEmployeeId,
                EmployeeEmailId = y.EmailAddress,
                profilePic = y.ProfilePicture,
                isGrantLeave = true
            }).OrderBy(x => x.EmployeeFullName).ToList();
        }
    }
}