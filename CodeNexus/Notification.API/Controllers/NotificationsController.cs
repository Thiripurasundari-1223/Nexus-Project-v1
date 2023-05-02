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
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationsServices _notificationServices;
        public NotificationsController(NotificationsServices notificationServices)
        {
            this._notificationServices = notificationServices;
        }

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Notifications API - GET Method"
            });
        }

        #region Insert Notifications
        [HttpPost]
        [Route("InsertNotifications")]
        public IActionResult InsertNotifications(List<SharedLibraries.Models.Notifications.Notifications> pNotifications)
        {
            try
            {
                if (_notificationServices.InsertNotifications(pNotifications).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "SUCCESS"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "FAILURE"
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/InsertNotifications", JsonConvert.SerializeObject(pNotifications));
                return Ok(new 
                { 
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString() 
                });
            }
        }
        #endregion

        #region Get Notifications By Resource Id
        [HttpGet]
        [Route("GetNotificationsByResourceId")]
        public IActionResult GetNotificationsByResourceId(int pResourceId)
        {
            List<NotificationView> notificationViews = new List<NotificationView>();
            NotificationViewList notificationViewList = new NotificationViewList();
            try
            {
                notificationViews = _notificationServices.GetNotificationsByResourceId(pResourceId);
                notificationViewList.NotificationView = notificationViews;
                notificationViewList.unReadCount = _notificationServices.GetNotificationUnReadByResourceId(pResourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = notificationViewList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetNotificationsByResourceId", Convert.ToString(pResourceId));
                return Ok(new 
                { 
                    StatusCode = "FAILURE", 
                    StatusText = ex.ToString(),
                    Data = notificationViewList
                });
            }
        }
        #endregion

        #region Mark All As Read By Resource Id
        [HttpPost]
        [Route("MarkAllAsReadByResourceId")]
        public IActionResult MarkAllAsReadByResourceId(NotificationMarkAsRead notificationMarkAsRead)
        {
            string statusText = "";
            try
            {
                if (_notificationServices.MarkAllAsRead(notificationMarkAsRead.ResourceId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "SUCCESS",
                        Data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/MarkAllAsReadByResourceId", JsonConvert.SerializeObject(notificationMarkAsRead));
                statusText = ex.Message.ToString();
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = ""
            });
        }
        #endregion

        #region Mark As Read By Notification Id
        [HttpPost]
        [Route("MarkAsReadByNotificationId")]
        public IActionResult MarkAsReadByNotificationId(NotificationMarkAsRead notificationMarkAsRead)
        {
            string statusText = "";
            try
            {
                if (_notificationServices.MarkAsReadByNotificationId(notificationMarkAsRead.NotificationId, notificationMarkAsRead.ResourceId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "SUCCESS",
                        Data = ""
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/MarkAsReadByNotificationId", JsonConvert.SerializeObject(notificationMarkAsRead));
                statusText = ex.Message.ToString();
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = ""
            });
        }
        #endregion
    }
}