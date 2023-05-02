using Reports.DAL.Models;
using Reports.DAL.Repository;
using SharedLibraries.ViewModels.Reports;
using System;
using System.Collections.Generic;

namespace Reports.DAL.Services
{
    public class ReportServices
    {
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IReportRepository _reportRepository;
        public ReportServices(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        #region Get Nexus Info
        /// <summary>
        /// Get Nexus Info
        /// </summary>
        public NexusInfo GetNexusInfo()
        {
            try
            {
                return _reportRepository.GetNexusInfo();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new NexusInfo();
            }
        }
        #endregion

        #region Get Account report
        /// <summary>
        /// Get Account report
        /// </summary>
        /// <param name="resourceId"></param>
        public ReportsModel GetAccountReport(int resourceId)
        {
            try
            {
                return _reportRepository.GetAccountReport(resourceId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new ReportsModel();
            }
        }
        #endregion

        #region Get Project report
        /// <summary>
        /// Get Project report
        /// </summary>
        /// <param name="resourceId"></param>
        public ReportsModel GetProjectReport(int resourceId)
        {
            try
            {
                return _reportRepository.GetProjectReport(resourceId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new ReportsModel();
            }
        }
        #endregion

        #region Get Resource report
        /// <summary>
        /// Get Resource report
        /// </summary>
        /// <param name="resourceId"></param>
        public List<ReportsModel> GetResourceReport(int resourceId)
        {
            try
            {
                return _reportRepository.GetResourceReport(resourceId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new List<ReportsModel>();
            }
        }
        #endregion

        #region Get Timesheet report
        /// <summary>
        /// Get Timesheeet report
        /// </summary>
        /// <param name="resourceId"></param>
        public List<ReportsModel> GetTimesheetReport(int resourceId)
        {
            try
            {
                return _reportRepository.GetTimesheetReport(resourceId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message.ToString());
                return new List<ReportsModel>();
            }
        }
        #endregion
    }
}
