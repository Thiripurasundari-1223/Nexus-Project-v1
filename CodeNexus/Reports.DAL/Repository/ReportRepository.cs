using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Reports.DAL.DBContext;
using Reports.DAL.Models;
using SharedLibraries.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Data;

namespace Reports.DAL.Repository
{
    public interface IReportRepository : IBaseRepository<ReportsModel>
    {
        NexusInfo GetNexusInfo();
        ReportsModel GetAccountReport(int resourceId);
        ReportsModel GetProjectReport(int resourceId);
        List<ReportsModel> GetResourceReport(int resourceId);
        List<ReportsModel> GetTimesheetReport(int resourceId);
    }
    public class ReportRepository : BaseRepository<ReportsModel>, IReportRepository
    {
        private readonly RDBContext _dbContext;
        public IConfiguration Configuration { get; }
        public ReportRepository(RDBContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _dbContext = dbContext;
            Configuration = configuration;
        }
        public NexusInfo GetNexusInfo()
        {
            DataSet reports = ExecuteStoredProcedure("usp_NexusInfo");
            if (reports.Tables.Count > 0)
            {
                DataTable dt = reports.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    NexusInfo data = new NexusInfo()
                    {
                        Id = 1,
                        NoOfAccounts = Convert.ToInt32(row["NoOfAccounts"]),
                        NoOfProjects = Convert.ToInt32(row["NoOfProjects"]),
                        NoOfUsers = Convert.ToInt32(row["NoOfUsers"])
                    };
                    return data;
                }
            }
            return null;
        }
        public ReportsModel GetAccountReport(int resourceId)
        {
            ReportsModel dataModel = new ReportsModel();
            List<ReportData> reportData = new List<ReportData>();
            List<AccountReport> accountData = new List<AccountReport>();
            SqlParameter[] param = new[]
            {
               new SqlParameter("@ResourseId", resourceId)
            };
            DataSet reports = ExecuteStoredProcedure("GetAccountReport", param);
            if (reports.Tables.Count > 0)
            {
                DataTable dt = reports.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    ReportData data = new ReportData()
                    {
                        Status = row["Status"].ToString(),
                        Count = Convert.ToInt32(row["Count"])
                    };
                    reportData.Add(data);
                }
            }
            if (reports.Tables.Count > 1)
            {
                DataTable dt = reports.Tables[1];
                foreach (DataRow row in dt.Rows)
                {
                    AccountReport data = new AccountReport()
                    {
                        AccountId = Convert.ToInt32(row["AccountId"]),
                        FormattedAccountId = "ACC1" + row["AccountId"].ToString().PadLeft(4, '0'),
                        AccountName = row["AccountName"] == DBNull.Value ? "" : row["AccountName"].ToString(),
                        ProjectCount = Convert.ToInt32(row["ProjectCount"]),
                        OwnerName = row["OwnerName"] == DBNull.Value ? "" : row["OwnerName"].ToString(),
                        ContactPersonName = row["ContactPersonName"] == DBNull.Value ? "" : row["ContactPersonName"].ToString(),
                        AccountStatus = row["AccountStatus"].ToString()
                    };
                    accountData.Add(data);
                }
            }
            dataModel.ReportData = JsonConvert.SerializeObject(reportData);
            dataModel.ReportDetail = JsonConvert.SerializeObject(accountData);
            return dataModel;
        }
        public ReportsModel GetProjectReport(int resourceId)
        {
            ReportsModel dataModel = new ReportsModel();
            List<ReportData> reportData = new List<ReportData>();
            List<ProjectReport> accountData = new List<ProjectReport>();
            SqlParameter[] param = new[]
            {
               new SqlParameter("@ResourseId", resourceId)
            };
            DataSet reports = ExecuteStoredProcedure("GetProjectReport", param);
            if (reports.Tables.Count > 0)
            {
                DataTable dt = reports.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    ReportData data = new ReportData()
                    {
                        Status = row["Status"].ToString(),
                        Count = Convert.ToInt32(row["Count"])
                    };
                    reportData.Add(data);
                }
            }
            if (reports.Tables.Count > 1)
            {
                DataTable dt = reports.Tables[1];
                foreach (DataRow row in dt.Rows)
                {
                    ProjectReport data = new ProjectReport()
                    {
                        ProjectId = Convert.ToInt32(row["ProjectId"]),
                        FormattedProjectId = "ACC1" + row["ProjectId"].ToString().PadLeft(4, '0'),
                        AccountName = row["AccountName"] == DBNull.Value ? "" : row["AccountName"].ToString(),
                        ProjectName = row["ProjectName"] == DBNull.Value ? "" : row["ProjectName"].ToString(),
                        ProjectType = row["ProjectType"] == DBNull.Value ? "" : row["ProjectType"].ToString(),
                        OwnerName = row["OwnerName"] == DBNull.Value ? "" : row["OwnerName"].ToString(),
                        ProjectSPOC = row["ProjectSPOC"] == DBNull.Value ? "" : row["ProjectSPOC"].ToString(),
                        ProjectStartDate = row["ProjectStartdate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ProjectStartdate"].ToString()),
                        ProjectEndDate = row["ProjectEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ProjectEndDate"].ToString()),
                        ProjectDuration = row["ProjectDuration"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ProjectDuration"].ToString()),
                        ProjectStatus = row["ProjectStatus"].ToString()
                    };
                    accountData.Add(data);
                }
            }
            dataModel.ReportData = JsonConvert.SerializeObject(reportData);
            dataModel.ReportDetail = JsonConvert.SerializeObject(accountData);
            return dataModel;
        }

        public List<ReportsModel> GetResourceReport(int resourceId)
        {
            List<ReportsModel> lstDataModel = new List<ReportsModel>();
            List<ResourceUtilization> resourceUtilization = new List<ResourceUtilization>();
            List<ResourceReport> accountData = new List<ResourceReport>();
            SqlParameter[] param = new[]
            {
               new SqlParameter("@ResourseId", resourceId)
            };
            DataSet reports = ExecuteStoredProcedure("GetResourceReport", param);
            if (reports.Tables.Count > 0)
            {
                for (int i = 0; i < reports.Tables.Count; i++)
                {
                    List<ReportData> reportData = new List<ReportData>();
                    ReportsModel dataModel = new ReportsModel();
                    if (i == 0)
                    {
                        foreach (DataRow row in reports.Tables[i].Rows)
                        {
                            ResourceReport data = new ResourceReport()
                            {
                                EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                                FormattedEmployeeId = row["FormattedEmployeeId"] == null ? "" : (row["FormattedEmployeeId"].ToString()),
                                ResourceName = row["ResourceName"] == null ? "" : row["ResourceName"].ToString(),
                                Role = row["Role"] == null ? "" : row["Role"].ToString(),
                                ProjectId = Convert.ToInt32(row["ProjectId"]),
                                FormattedProjectId = "ACC1" + row["ProjectId"].ToString().PadLeft(4, '0'),
                                ProjectName = row["ProjectName"] == null ? "" : row["ProjectName"].ToString(),
                                Utilization = row["Utilization"] == null ? 0 : Convert.ToDecimal(row["Utilization"].ToString())
                            };
                            accountData.Add(data);
                        }
                        dataModel.ReportDetail = JsonConvert.SerializeObject(accountData);
                    }
                    else if (i == 1)
                    {
                        foreach (DataRow row in reports.Tables[i].Rows)
                        {
                            ResourceUtilization data = new ResourceUtilization()
                            {
                                ProjectName = row["ProjectName"].ToString(),
                                Billable = Convert.ToInt32(row["Billable"]),
                                NonBillable = Convert.ToInt32(row["NonBillable"])
                            };
                            resourceUtilization.Add(data);
                        }
                        dataModel.ReportData = JsonConvert.SerializeObject(resourceUtilization);
                    }
                    else
                    {
                        foreach (DataRow row in reports.Tables[i].Rows)
                        {
                            ReportData data = new ReportData()
                            {
                                Status = row["Status"].ToString(),
                                Count = Convert.ToInt32(row["Count"]),
                                ProjectId = row.Table.Columns.Contains("ProjectId") == true ? row["ProjectId"] != null ? Convert.ToInt32(row["ProjectId"]) : 0 : 0
                            };
                            reportData.Add(data);
                        }
                        dataModel.ReportData = JsonConvert.SerializeObject(reportData);
                    }

                    lstDataModel.Add(dataModel);
                }
            }
            return lstDataModel;
        }

        public List<ReportsModel> GetTimesheetReport(int resourceId)
        {
            List<ReportsModel> DataModel = new List<ReportsModel>();
            List<ResourceTimesheet> resourceTimesheets = new List<ResourceTimesheet>();
            List<TimesheetReport> accountData = new List<TimesheetReport>();
            SqlParameter[] param = new[]
            {
               new SqlParameter("@ResourseId", resourceId)
            };
            DataSet reports = ExecuteStoredProcedure("GetTimesheetReport", param);
            if (reports.Tables.Count > 0)
            {
                for (int i = 0; i < reports.Tables.Count; i++)
                {
                    List<TimesheetStatus> timesheets = new List<TimesheetStatus>();
                    ReportsModel dataModel = new ReportsModel();
                    if (i == 0)
                    {
                        foreach (DataRow row in reports.Tables[i].Rows)
                        {
                            TimesheetReport data = new TimesheetReport()
                            {
                                ProjectId = Convert.ToInt32(row["ProjectId"]),
                                ProjectName = row["ProjectName"] == null ? "" : row["ProjectName"].ToString(),
                                ResourceId = Convert.ToInt32(row["ResourceId"]),
                                ResourceName = row["ResourceName"] == null ? "" : row["ResourceName"].ToString(),
                                FormattedProjectId = "PRJ1" + row["ProjectId"].ToString().PadLeft(4, '0'),
                                PlannedHours = row["PlannedHours"] == null ? "00:00" : row["PlannedHours"].ToString(),
                                ClockedHours = row["ClockedHours"] == null ? "00:00" : row["ClockedHours"].ToString()
                                // TimesheetStatus
                            };
                            accountData.Add(data);
                        }
                        dataModel.ReportDetail = JsonConvert.SerializeObject(accountData);
                    }
                    else if (i == 1)
                    {
                        foreach (DataRow row in reports.Tables[i].Rows)
                        {
                            ResourceTimesheet data = new ResourceTimesheet()
                            {
                                ResourceName = row["ResourceName"].ToString(),
                                PlannedHours = Convert.ToDecimal(row["PlannedHours"].ToString()),
                                SubmittedHours = Convert.ToDecimal(row["PlannedHours"].ToString()),
                                NotSubmittedHours = Convert.ToDecimal(row["NotSubmittedHours"].ToString())
                            };
                            resourceTimesheets.Add(data);
                        }
                        dataModel.ReportData = JsonConvert.SerializeObject(resourceTimesheets);
                    }
                    else
                    {
                        foreach (DataRow row in reports.Tables[i].Rows)
                        {
                            TimesheetStatus data = new TimesheetStatus()
                            {
                                ProjectName = row["ProjectName"].ToString(),
                                Submitted = Convert.ToInt32(row["Submitted"]),
                                NotSubmitted = Convert.ToInt32(row["NotSubmitted"])
                            };
                            timesheets.Add(data);
                        }
                        dataModel.ReportData = JsonConvert.SerializeObject(timesheets);
                    }

                    DataModel.Add(dataModel);
                }
            }
            return DataModel;
        }
        public DataSet ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] paramters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(Configuration.GetConnectionString("PMSNexus")))
            {
                SqlCommand sqlComm = new SqlCommand(storedProcedureName, conn);
                if (paramters != null)
                {
                    foreach (var item in paramters)
                    {
                        sqlComm.Parameters.AddWithValue(item.ParameterName, item.Value);
                    }
                }
                sqlComm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;
                da.Fill(ds);
            }
            return ds;
        }
    }
}
