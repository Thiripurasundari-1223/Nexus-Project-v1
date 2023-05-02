using APIGateWay.API.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Projects;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Projects;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.Common;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.Models.Leaves;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HTTPClient _client;
        private readonly IConfiguration _configuration;
        private readonly string _timesheetBaseURL = string.Empty;
        private readonly string _projectBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _accountsBaseURL = string.Empty;
        private readonly string _attendanceBaseURL = string.Empty;
        private readonly string _leavesBaseURL = string.Empty;
        private readonly string _appraisalBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";

        #region Constructor
        public HomeController(IConfiguration configuration)
        {
            _client = new HTTPClient();
            _configuration = configuration;
            _attendanceBaseURL = _configuration.GetValue<string>("ApplicationURL:Attendance:BaseURL");
            _timesheetBaseURL = _configuration.GetValue<string>("ApplicationURL:Timesheet:BaseURL");
            _projectBaseURL = _configuration.GetValue<string>("ApplicationURL:Projects:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _accountsBaseURL = _configuration.GetValue<string>("ApplicationURL:Accounts:BaseURL");
            _leavesBaseURL = _configuration.GetValue<string>("ApplicationURL:Leaves:BaseURL");
            _appraisalBaseURL = _configuration.GetValue<string>("ApplicationURL:Appraisal:BaseURL");
        }
        #endregion

        #region Get Employee Home Details
        [HttpPost]
        [Route("GetEmployeeHomeDetails")]
        public async Task<IActionResult> GetEmployeeHomeDetails(WeekMonthAttendanceView employeeDetails)
        {
            WeeklyMonthlyAttendance AttendanceWithBreakDetail = new();
            List<Holiday> UpcomingHoliday = new();
            HolidayDetailsView holidayList = new();
            string statusText = "", statusCode = "SUCCESS";
            try
            {
                var result = await _client.PostAsJsonAsync(employeeDetails,_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetDailyAttendanceDetails") );
                AttendanceWithBreakDetail = JsonConvert.DeserializeObject<WeeklyMonthlyAttendance>(JsonConvert.SerializeObject(result?.Data));
                var shiftDetais = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetShiftWeekendDetails") + employeeDetails.ShiftDetailsId);
                ShiftDetailedView ShiftDetails = JsonConvert.DeserializeObject<ShiftDetailedView>(JsonConvert.SerializeObject(shiftDetais.Data));
                //ShiftViewDetails ShiftDetails = await GetShiftList(employeeId);
                AttendanceWithBreakDetail.ShiftHour = ShiftDetails?.shiftViewDetails?.TotalHours;
                var resultHoliday = await _client.PostAsJsonAsync(employeeDetails,_leavesBaseURL, _configuration.GetValue<string>("ApplicationURL:Leaves:GetUpcomingHolidays"));
                holidayList = JsonConvert.DeserializeObject<HolidayDetailsView>(JsonConvert.SerializeObject(resultHoliday?.Data));
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeShiftDetailsById") + employeeDetails.EmployeeId);
                List<EmployeeShiftDetailsView> employeeShiftDetails = JsonConvert.DeserializeObject<List<EmployeeShiftDetailsView>>(JsonConvert.SerializeObject(employeeResult?.Data));
                List<Holiday> financialHolidayList = new();
                int? shiftId = 0;
                if (holidayList?.HolidayList?.Count > 0)
                {
                    foreach (var item in holidayList?.HolidayList)
                    {
                        if (employeeShiftDetails?.Count > 0)
                        {
                            foreach (EmployeeShiftDetailsView shift in employeeShiftDetails)
                            {
                                if (item.HolidayDate >= shift?.ShiftFromDate && (shift?.ShiftToDate == null || item.HolidayDate <= shift.ShiftToDate))
                                {
                                    shiftId = ShiftDetails?.DefaultShiftView.Where(x => x.ShiftDetailsId == shift?.ShiftDetailsId).Select(x => x.ShiftDetailsId).FirstOrDefault();
                                }
                            }
                        }
                        if (shiftId == 0)
                        {
                            shiftId = ShiftDetails?.DefaultShiftView.Where(x => x.IsGenralShift == true).Select(x => x.ShiftDetailsId).FirstOrDefault();
                            if (shiftId == 0)
                            {
                                shiftId = ShiftDetails?.DefaultShiftView.Select(x => x.ShiftDetailsId).FirstOrDefault();
                            }
                        }
                        Holiday holiday = GetApplicableHolidayList(item, employeeDetails.DepartmentId, employeeDetails.LocationId, (int)shiftId, holidayList);
                        financialHolidayList.Add(holiday);
                    }
                    UpcomingHoliday = financialHolidayList.Where(x => x.HolidayName != null).Select(x => x).ToList();
                }
            }
            catch (Exception ex)
            {
                statusCode = "FAILURE";
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Home/GetEmployeeHomeDetails", employeeDetails.EmployeeId.ToString());
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText,
                AttendanceWithBreakDetail,
                UpcomingHoliday
            });
        }
        #endregion

        #region Get Home employee Report Details
        [HttpPost]
        [Route("GetHomeEmployeeReportDetails")]
        public async Task<IActionResult> GetHomeEmployeeReportDetails(WeekMonthAttendanceView employeeDetails)
        {
            List<HomeReport> homeReportList = new();
            string statusText = "", statusCode = "SUCCESS";
            try
            {                
                var listOfEmployeesReportsResponse = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetReportDetailsByEmployeeId") + employeeDetails.EmployeeId + "&employeeCategoryId=" + employeeDetails.EmployeeCategoryId);
                List<Reports> listOfEmployeesReports = JsonConvert.DeserializeObject<List<Reports>>(JsonConvert.SerializeObject(listOfEmployeesReportsResponse?.Data));
                
                if (listOfEmployeesReports?.Count > 0)
                {
                    foreach (Reports reports in listOfEmployeesReports)
                    {
                        if (reports?.ReportName?.ToLower() == "customeronboard"
                            || reports?.ReportName?.ToLower() == "associates"
                            || reports?.ReportName?.ToLower() == "timesheets"
                            || reports?.ReportName?.ToLower() == "projects"
                            || reports?.ReportName?.ToLower() == "attendance"
                            )
                        {
                            HomeReport homeReport = new()
                            {
                                HomeReportData = new List<HomeReportData>()
                            };
                            switch (reports?.ReportName?.ToLower())
                            {
                                case "customeronboard":
                                    var customerResult = await _client.GetAsync(_accountsBaseURL, _configuration.GetValue<string>("ApplicationURL:Accounts:GetCustomerOnBoardHomeReport") + employeeDetails.EmployeeId);
                                    List<HomeReportData> customerReportData = JsonConvert.DeserializeObject<List<HomeReportData>>(JsonConvert.SerializeObject(customerResult?.Data));
                                    homeReport.HomeReportData = customerReportData ?? new List<HomeReportData>();
                                    break;
                                case "associates":
                                    var associateResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAssociateHomeReport"));
                                    List<HomeReportData> associateReportData = JsonConvert.DeserializeObject<List<HomeReportData>>(JsonConvert.SerializeObject(associateResult?.Data));
                                    homeReport.HomeReportData = associateReportData ?? new List<HomeReportData>();
                                    break;
                                case "timesheets":
                                    var projectResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetProjectTimesheet") + employeeDetails.EmployeeId);
                                    DateTime previouWeekStartDate = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek).AddDays(-7);
                                    ProjectTimesheet projectTimesheet = JsonConvert.DeserializeObject<ProjectTimesheet>(JsonConvert.SerializeObject(projectResult?.Data));
                                    projectTimesheet.ResourceId = employeeDetails==null?0: employeeDetails.EmployeeId;
                                    projectTimesheet.WeekStartDate = previouWeekStartDate;
                                    var timesheetResult = await _client.PostAsJsonAsync(projectTimesheet, _timesheetBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetTimesheetHomeReport"));
                                    HomeReportData timesheetHomeReportData = JsonConvert.DeserializeObject<HomeReportData>(JsonConvert.SerializeObject(timesheetResult?.Data));
                                    homeReport.HomeReportData.Add(timesheetHomeReportData);
                                    break;
                                //case "contribution":
                                //    var contributionResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Timesheet:GetContributionHomeReport") + employeeId);
                                //    int? contributionReportData = JsonConvert.DeserializeObject<int?>(JsonConvert.SerializeObject(contributionResult.Data));
                                //    if (contributionReportData == null) contributionReportData = 0;
                                //    homeReport.ReportTitle = "Contribution";
                                //    HomeReportData contributionHomeReportData = new HomeReportData();
                                //    contributionHomeReportData.ReportTitle = "Contribution Progress";
                                //    contributionHomeReportData.ReportData = contributionReportData.ToString() + "%";
                                //    homeReport.HomeReportData.Add(contributionHomeReportData);
                                //    homeReportList.Add(homeReport);
                                //    break;
                                case "projects":
                                    var activeProjectDetailResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetActiveProjectDetailsByResourceId") + employeeDetails.EmployeeId);
                                    List<ProjectDetails> activeProjectlist = JsonConvert.DeserializeObject<List<ProjectDetails>>(JsonConvert.SerializeObject(activeProjectDetailResult?.Data));
                                    HomeReportData customerCount = new();
                                    customerCount.ReportTitle = "Customers";
                                    customerCount.ReportSubTitle = "  ";
                                    customerCount.ReportData = activeProjectlist.GroupBy(x => x.AccountId).Count().ToString();
                                    homeReport.HomeReportData.Add(customerCount);
                                    HomeReportData projectCount = new();
                                    projectCount.ReportTitle = "Projects";
                                    projectCount.ReportSubTitle = "  ";
                                    projectCount.ReportData = activeProjectlist.Count().ToString();
                                    homeReport.HomeReportData.Add(projectCount);
                                    break;

                                case "attendance":
                                    var attendanceResult = await _client.GetAsync(_attendanceBaseURL, _configuration.GetValue<string>("ApplicationURL:Attendance:GetAttendanceHomeReport"));
                                    HomeReportData attendanceReportData = JsonConvert.DeserializeObject<HomeReportData>(JsonConvert.SerializeObject(attendanceResult?.Data));
                                    (homeReport.HomeReportData ?? new List<HomeReportData>()).Add(attendanceReportData);
                                    var allEmployeesResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeesDetails"));
                                    List<Employees> allEmployeeList = JsonConvert.DeserializeObject<List<Employees>>(JsonConvert.SerializeObject(allEmployeesResult?.Data));
                                    int totalEmployees = allEmployeeList.Where(x => x.IsActive == true).Select(s => s.EmployeeID).ToList().Count;
                                    HomeReportData leaveReport = new HomeReportData()
                                    {
                                        ReportTitle = "Leaves",
                                        ReportData = (totalEmployees - Convert.ToInt32(attendanceReportData.ReportData)).ToString()
                                    };
                                    homeReport.HomeReportData.Add(leaveReport);
                                    break;
                            }
                            homeReport.ReportTitle = reports?.ReportTitle;
                            homeReport.ReportIconPath = reports?.ReportIconPath;
                            homeReport.NavigateTo = reports?.ReportNavigationUrl;
                            homeReportList.Add(homeReport);
                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {
                statusCode = "FAILURE";
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Home/GetHomeReportDetails", employeeDetails.EmployeeId.ToString());
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText,
                Report = homeReportList
            });
        }
        #endregion

        #region Get Home appraisal resource Report Details
        [HttpPost]
        [Route("GetHomeAppraisalResourceReportDetails")]
        public async Task<IActionResult> GetHomeAppraisalResourceReportDetails(WeekMonthAttendanceView employeeDetails)
        {
            List<HomeReport> homeReportList = new();
            string statusText = "", statusCode = "SUCCESS";
            try
            {
                var listOfEmployeesReportsResponse = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetReportDetailsByEmployeeId") + employeeDetails.EmployeeId + "&employeeCategoryId=" + employeeDetails.EmployeeCategoryId);
                List<Reports> listOfEmployeesReports = JsonConvert.DeserializeObject<List<Reports>>(JsonConvert.SerializeObject(listOfEmployeesReportsResponse?.Data));

                if (listOfEmployeesReports?.Count > 0)
                {
                    foreach (Reports reports in listOfEmployeesReports)
                    {
                        if(reports?.ReportName?.ToLower() == "resourcebillability"
                            || reports?.ReportName?.ToLower() == "resourceavailability"
                            || reports?.ReportName?.ToLower() == "appraisalreview"
                            || reports?.ReportName?.ToLower() == "appraisalstatus"
                            || reports?.ReportName?.ToLower() == "appraisal"
                            )
                        {
                            HomeReport homeReport = new()
                            {
                                HomeReportData = new List<HomeReportData>()
                            };
                            switch (reports?.ReportName?.ToLower())
                            {
                                
                                case "resourcebillability":
                                    int nonBillable = 0;
                                    List<HomeReportData> resourceBillability = new();
                                    var allEmployeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeesDetails"));
                                    List<Employees> allEmployeesDetails = JsonConvert.DeserializeObject<List<Employees>>(JsonConvert.SerializeObject(allEmployeeResult?.Data));
                                    var resourceBillabilityResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetResourceBillabilityHomeReport"));
                                    HomeReportData resourceBillabilityReportData = JsonConvert.DeserializeObject<HomeReportData>(JsonConvert.SerializeObject(resourceBillabilityResult?.Data));
                                    resourceBillability.Add(resourceBillabilityReportData);
                                    HomeReportData nonBillableResource = new();
                                    nonBillableResource.ReportTitle = "Non Billable";
                                    if (!string.IsNullOrEmpty(resourceBillabilityReportData?.ReportData) && allEmployeesDetails?.Count > 0)
                                    {
                                        int? employeeCount = allEmployeesDetails?.Where(x => x.IsActive == true).Select(x => x.EmployeeID).ToList().Count();
                                        nonBillable = employeeCount == null ? 0 : (int)employeeCount - Convert.ToInt32(resourceBillabilityReportData?.ReportData);
                                    }
                                    nonBillableResource.ReportData = nonBillable.ToString();
                                    resourceBillability.Add(nonBillableResource);
                                    homeReport.HomeReportData = resourceBillability ?? new List<HomeReportData>();
                                    break;
                                case "resourceavailability":
                                    int innovationHub = 0;
                                    List<HomeReportData> resourceAvailability = new();
                                    var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeesDetails"));
                                    List<Employees> lstEmployeesDetails = JsonConvert.DeserializeObject<List<Employees>>(JsonConvert.SerializeObject(employeeResult.Data));
                                    var resourceOnBoardingResult = await _client.GetAsync(_projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetResourceAvailabilityHomeReport"));
                                    HomeReportData resourceOnBoardingReportData = JsonConvert.DeserializeObject<HomeReportData>(JsonConvert.SerializeObject(resourceOnBoardingResult?.Data));
                                    HomeReportData billableResource = new();
                                    billableResource.ReportTitle = "Innovation Hub";
                                    if (!string.IsNullOrEmpty(resourceOnBoardingReportData?.ReportData) && lstEmployeesDetails?.Count > 0)
                                    {
                                        int? employeeCount = lstEmployeesDetails?.Where(x => x.IsActive == true).Select(x => x.EmployeeID).ToList().Count();
                                        innovationHub = employeeCount == null ? 0 : (int)employeeCount - Convert.ToInt32(resourceOnBoardingReportData?.ReportData);
                                    }
                                    billableResource.ReportData = innovationHub.ToString();
                                    resourceAvailability.Add(billableResource);
                                    resourceAvailability.Add(resourceOnBoardingReportData);
                                    homeReport.HomeReportData = resourceAvailability ?? new List<HomeReportData>();
                                    break;
                                case "appraisalreview":
                                    List<EmployeeAppraisalMasterDetailView> EmployeAppraisalList = new();
                                    List<EmployeeViewDetails> EmployeeList = new();
                                    var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeListForManagerReport") + employeeDetails.EmployeeId + "&isAll=false");
                                    EmployeeList = JsonConvert.DeserializeObject<List<EmployeeViewDetails>>(JsonConvert.SerializeObject(employeeListresult.Data));
                                    List<int> employeeids;
                                    if (EmployeeList != null)
                                    {
                                        employeeids = EmployeeList.Select(ea => ea.EmployeeId).ToList();
                                    }
                                    else
                                    {
                                        employeeids = new List<int>(0);
                                    }
                                    var employeeappraisalresult = await _client.PostAsJsonAsync(employeeids, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEmployeeAppraisalListByManager"));
                                    EmployeAppraisalList = JsonConvert.DeserializeObject<List<EmployeeAppraisalMasterDetailView>>(JsonConvert.SerializeObject(employeeappraisalresult.Data));
                                    HomeReportData appraisalapprovalCount = new();
                                    appraisalapprovalCount.ReportTitle = "Approved";
                                    appraisalapprovalCount.ReportSubTitle = "  ";
                                    appraisalapprovalCount.ReportData = EmployeAppraisalList.Count(rs => rs.APPRAISAL_STATUS == 12).ToString();
                                    homeReport.HomeReportData.Add(appraisalapprovalCount);
                                    HomeReportData appraisalpendingCount = new();
                                    appraisalpendingCount.ReportTitle = "Pending";
                                    appraisalpendingCount.ReportSubTitle = "  ";
                                    appraisalpendingCount.ReportData = EmployeAppraisalList.Count(rs => rs.APPRAISAL_STATUS == 10 || rs.APPRAISAL_STATUS == 40).ToString();
                                    homeReport.HomeReportData.Add(appraisalpendingCount);
                                    break;
                                case "appraisalstatus":
                                    AppraisalStatusReportCount appraisalStatusReportCount = new();
                                    var appraisalresult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAppraisalStatusReportCount"));
                                    appraisalStatusReportCount = JsonConvert.DeserializeObject<AppraisalStatusReportCount>(JsonConvert.SerializeObject(appraisalresult?.Data));
                                    HomeReportData appraisalStatusCount = new();
                                    appraisalStatusCount.ReportTitle = "Approved";
                                    appraisalStatusCount.ReportSubTitle = "  ";
                                    appraisalStatusCount.ReportData = appraisalStatusReportCount.Approved.ToString();
                                    homeReport.HomeReportData.Add(appraisalStatusCount);
                                    HomeReportData appraisalpendingStatusCount = new();
                                    appraisalpendingStatusCount.ReportTitle = "Pending";
                                    appraisalpendingStatusCount.ReportSubTitle = "  ";
                                    appraisalpendingStatusCount.ReportData = appraisalStatusReportCount.Pending.ToString();
                                    homeReport.HomeReportData.Add(appraisalpendingStatusCount);
                                    break;
                                case "appraisal":
                                    IndividualEmployeeAppraisalRating individualAppraisal = new();
                                    var individualappraisalresult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEmployeeAppraisalRating") + employeeDetails.EmployeeId);
                                    individualAppraisal = JsonConvert.DeserializeObject<IndividualEmployeeAppraisalRating>(JsonConvert.SerializeObject(individualappraisalresult?.Data));
                                    HomeReportData individualappraisalStatus = new();
                                    individualappraisalStatus.ReportTitle = individualAppraisal.PreviousAppcycle_monthyear;
                                    individualappraisalStatus.ReportSubTitle = "  ";
                                    individualappraisalStatus.ReportData = individualAppraisal.Previous_Rating?.ToString();
                                    homeReport.HomeReportData.Add(individualappraisalStatus);
                                    if (!string.IsNullOrEmpty(individualAppraisal.CurrentAppcycle_monthyear) && individualAppraisal.Current_Rating != null)
                                    {
                                        HomeReportData crrentappraisalStatus = new();
                                        crrentappraisalStatus.ReportTitle = individualAppraisal.CurrentAppcycle_monthyear;
                                        crrentappraisalStatus.ReportSubTitle = "  ";
                                        crrentappraisalStatus.ReportData = individualAppraisal.Current_Rating?.ToString();
                                        homeReport.HomeReportData.Add(crrentappraisalStatus);
                                    }
                                    break;

                            }
                            homeReport.ReportTitle = reports?.ReportTitle;
                            homeReport.ReportIconPath = reports?.ReportIconPath;
                            homeReport.NavigateTo = reports?.ReportNavigationUrl;
                            homeReportList.Add(homeReport);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                statusCode = "FAILURE";
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Home/GetHomeReportDetails", employeeDetails.EmployeeId.ToString());
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = statusCode,
                StatusText = statusText,
                Report = homeReportList
            });
        }
        #endregion

        #region Get Social Media Feeds
        [HttpGet]
        [Route("GetMyFacebookPageFeeds")]
        public ActionResult GetMyFacebookPageFeeds()
        {
            Root posts;
            string FeedRequestUrl = "https://graph.facebook.com/v10.0/me?fields=posts%7Bcreated_time%2Cmessage%7D&access_token=EAAQgYov8TlsBAMZBFeBMx2HYYN2DjZAxVj7U1ouXxmzZCfZCDE0zN6ZBc7ZCQ2KYH8FPrZCYZCEd3lEG6eparq4imR0hkZAKIAk71QjT72wwNm8p9piZAntJ97O2s3s6PrZBNcZC8JRfVCBU6F5F4Jh9E257YqbGmL8BqUZAr4vil4UNprf3syURkNTUyVxL0UMgBeVcwnepRtTLZBVUhnbpoK1643";
            HttpWebRequest feedRequest = (HttpWebRequest)WebRequest.Create(FeedRequestUrl);
            feedRequest.Method = "GET";
            feedRequest.Accept = "application/json";
            feedRequest.ContentType = "application/json; charset=utf-8";
            WebResponse feedResponse = (HttpWebResponse)feedRequest.GetResponse();
            using (feedResponse)
            {
                using var reader = new StreamReader(feedResponse.GetResponseStream());
                var read = reader.ReadToEnd();
                posts = JsonConvert.DeserializeObject<Root>(read);
            }
            return Ok(new
            {
                StatusCode = "Success",
                Result = posts
            });
        }
        #endregion

        #region Get LinkedIn Feeds
        [HttpGet]
        [Route("GetMyLinkedInPageFeeds")]
        public ActionResult GetMyLinkedInPageFeedsAsync()
        {
            string FeedRequestUrl = "https://api.linkedin.com/v1/people/~:(first-name,last-name,headline,picture-url,industry,summary,positions:(title,summary,start-date,end-date,company:(name,industry)))?fields=posts%7Bmessage%7D&access_token=AQW7j7TrgJU1GO5RSIQ7FhI_5cPoZVot0x-kMt_EdN4hPop8jFTBuhZCR2JdBqLJPDNnDMf5hT5N-Pxn-GeTVzu-i74-M5ISj6q_tsIkwW5ALC5WJhjkGp92QhRkAyk9npUlyWjnlQvEnLQbt3ut_n6TrTiSDxdqczlbfpbe54CIPxsjnL6YjCgpgOZ-KdJ5KgGp4YHLbc8Bp3L8F_uHiDgh1BZrkk88pkJs6xCiEz1gd-ZHxguaPIMhOY3eOJ4lVah2pqkV_0gQyyu-6PPHYl6tHmzIAMPc72N1B4l_2uXJgpVCQoEmPurK7YQKcAVG0zfCdELi8_yaTpCs-TWmE-YQiFoe4g";
            HttpWebRequest feedRequest = (HttpWebRequest)WebRequest.Create(FeedRequestUrl);
            feedRequest.Method = "GET";
            feedRequest.Accept = "application/json";
            feedRequest.ContentType = "application/json; charset=utf-8";
            WebResponse feedResponse = (HttpWebResponse)feedRequest.GetResponse();
            using (feedResponse)
            {
                using var reader = new StreamReader(feedResponse.GetResponseStream());
                var read = reader.ReadToEnd();
            }
            return Ok(new
            {
                StatusCode = "Success",
                Result = 0
            });
        }
        #endregion

        #region GetApplicableHolidayList
        [NonAction]
        private Holiday GetApplicableHolidayList(Holiday holidayList, int departmentId, int locationId, int shiftId, HolidayDetailsView holidayDetails)
        {
            Holiday empHolidayList = new Holiday();
            if (holidayList != null)
            {
                bool applicable = false;
                //Check department
                var depList = holidayDetails?.HolidayDepartment.Where(x => x.HolidayId == holidayList.HolidayID).Select(x => x.DepartmentId).ToList();
                if (depList?.Count > 0)
                {
                    if (depList.Contains(departmentId))
                    {
                        applicable = true;
                    }
                    else
                    {
                        applicable = false;
                    }
                }
                else
                {
                    applicable = true;
                }
                //Check shift
                if (applicable)
                {
                    var shiftList = holidayDetails?.HolidayShift.Where(x => x.HolidayId == holidayList.HolidayID).Select(x => x.ShiftDetailsId).ToList();
                    if (shiftList?.Count > 0)
                    {
                        if (shiftList.Contains(shiftId))
                        {
                            applicable = true;
                        }
                        else
                        {
                            applicable = false;
                        }
                    }
                    else
                    {
                        applicable = true;
                    }
                    //check location
                    if (applicable)
                    {
                        var locationList = holidayDetails?.HolidayLocation.Where(x => x.HolidayId == holidayList.HolidayID).Select(x => x.LocationId).ToList();
                        if (locationList?.Count > 0)
                        {
                            if (locationList.Contains(locationId))
                            {
                                applicable = true;
                            }
                            else
                            {
                                applicable = false;
                            }
                        }
                        else
                        {
                            applicable = true;
                        }
                    }
                }
                if (applicable)
                {
                    return holidayList;
                }
            }
            return new Holiday();
        }
        #endregion
    }
}