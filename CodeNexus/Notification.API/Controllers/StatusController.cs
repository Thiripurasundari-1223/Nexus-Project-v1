using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notifications.DAL.Services;
using SharedLibraries.Common;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;

namespace Notification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly NotificationsServices _notificationServices;
        public StatusController(NotificationsServices notificationServices)
        {
            this._notificationServices = notificationServices;
        }

        #region Get All Status List
        [HttpGet]
        [Route("GetAllStatusList")]
        public IActionResult GetAllStatusList()
        {
            //List<StatusViewModel> statusViews = new List<StatusViewModel>();
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetAllStatusList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetAllStatusList");
                return Ok(new 
                { 
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<StatusViewModel>()
                });
            }
        }
        #endregion

        #region Get Status By Id
        [HttpGet]
        [Route("GetStatusByID")]
        public IActionResult GetStatusByID(int pStatusId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetStatusByID(pStatusId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetStatusByID", Convert.ToString(pStatusId));
                return Ok(new 
                { 
                    StatusCode = "FAILURE", 
                    StatusText = ex.ToString(), 
                    Data = new StatusViewModel() 
                });
            }
        }
        #endregion

        #region Get Status By Code
        [HttpGet]
        [Route("GetStatusByCode")]
        public IActionResult GetStatusByCode(string pStatusCode)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetStatusByCode(pStatusCode)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetStatusByCode", JsonConvert.SerializeObject(pStatusCode));
                return Ok(new 
                { 
                    StatusCode = "FAILURE", 
                    StatusText = ex.ToString(), 
                    Data = new StatusViewModel() 
                });
            }
        }
        #endregion        

        #region Get Status By Name
        [HttpGet]
        [Route("GetStatusByName")]
        public IActionResult GetStatusByName(string pStatusCode)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetStatusByName(pStatusCode)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetStatusByName", JsonConvert.SerializeObject(pStatusCode));
                return Ok(new 
                { 
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new StatusViewModel() 
                });
            }
        }
        #endregion

        #region Get Status By Code
        [HttpPost]
        [Route("GetStatusByCode")]
        public IActionResult GetStatusByCode(List<string> pStatusCodeList)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.GetStatusByCode(pStatusCodeList)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetStatusByCode", JsonConvert.SerializeObject(pStatusCodeList));
                return Ok(new 
                { 
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<StatusViewModel>() 
                });
            }
        }
        #endregion
    }
}