using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notifications.DAL.Services;
using SharedLibraries.Common;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;

namespace Notifications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetConfigurationWeekDayController : ControllerBase
    {
        private readonly NotificationsServices _notificationServices;
        public TimesheetConfigurationWeekDayController(NotificationsServices notificationServices)
        {
            this._notificationServices = notificationServices;
        }
        #region Get All WeekDay List
        [HttpGet]
        [Route("GetAllWeekDayList")]
        public IActionResult GetAllWeekDayList()
        {
            List<TimesheetConfigurationWeekdayView> timesheetConfigurationWeekdayViews = new List<TimesheetConfigurationWeekdayView>();
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetAllWeekDayList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetAllWeekDayList");
                return Ok(new 
                {
                    StatusCode = "FAILURE", 
                    StatusText = ex.ToString(),
                    Data = timesheetConfigurationWeekdayViews
                });
            }
        }
        #endregion
        #region Get WeekDay By Id
        [HttpGet]
        [Route("GetWeekDayByID")]
        public IActionResult GetWeekDayByID(int pTimesheetConfigurationWeekdayId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetWeekDayByID(pTimesheetConfigurationWeekdayId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetWeekDayByID", Convert.ToString(pTimesheetConfigurationWeekdayId));
                return Ok(new 
                {
                    StatusCode = "FAILURE", 
                    StatusText = ex.ToString(), 
                    Data = new TimesheetConfigurationWeekdayView() 
                });
            }
        }
        #endregion
        #region Get WeekDay By Name
        [HttpGet]
        [Route("GetWeekDayByName")]
        public IActionResult GetWeekDayByName(string pDayName)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetWeekDayByName(pDayName)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetWeekDayByName", JsonConvert.SerializeObject(pDayName));
                return Ok(new 
                { 
                    StatusCode = "FAILURE", 
                    StatusText = ex.ToString(), 
                    Data = new TimesheetConfigurationWeekdayView() 
                });
            }
        }
        #endregion

    }
}
