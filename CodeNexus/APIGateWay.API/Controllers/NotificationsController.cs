using APIGateWay.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateWay.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";

        #region Constructor
        public NotificationsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notification:BaseURL");
        }
        #endregion

        #region Insert Notifications
        [HttpPost]
        [Route("InsertNotifications")]
        public async Task<IActionResult> InsertNotifications(List<Notifications> pNotifications)
        {
            string statusText = "";
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notification:BaseURL"))
                };
                HttpResponseMessage response = await client.PostAsJsonAsync("Notifications/InsertNotifications", pNotifications);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsAsync<SuccessData>();
                    if (result != null && (int)result?.Result?.Data > 0)
                    {
                        return Ok(new
                        {
                            result.Result.StatusCode,
                            result.Result.StatusText,
                        });
                    }
                    else if (result != null)
                    {
                        statusText = result.Result.StatusText;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Notifications/InsertNotifications", JsonConvert.SerializeObject(pNotifications));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText
            });
        }
        #endregion

        #region Get Notification List By Resource Id
        private async Task<NotificationViewList> GetNotificationListByResourceId(int pResourceId)
        {
            NotificationViewList notificationViewList = new();
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notification:BaseURL"))
                };
                HttpResponseMessage response = await client.GetAsync("Notifications/GetNotificationsByResourceId?pResourceId=" + pResourceId);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsAsync<SuccessData>();
                    if (result != null && result?.Result?.Data != null)
                    {
                        notificationViewList = JsonConvert.DeserializeObject<NotificationViewList>(JsonConvert.SerializeObject(result.Result.Data));
                        return notificationViewList;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Notifications/GetNotificationListByResourceId", Convert.ToString(pResourceId));
            }
            return notificationViewList;
        }
        #endregion

        #region Get Notifications By Resource Id
        [HttpGet]
        [Route("GetNotificationsByResourceId")]
        public async Task<IActionResult> GetNotificationsByResourceId(int pResourceId)
        {
            NotificationViewList notificationViewList = new();
            try
            {
                notificationViewList = await GetNotificationListByResourceId(pResourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    notificationViews = notificationViewList.NotificationView,
                    notificationViewList.unReadCount
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Notifications/GetNotificationsByResourceId", Convert.ToString(pResourceId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                notificationViews = notificationViewList.NotificationView,
                notificationViewList.unReadCount
            });
        }
        #endregion

        #region Mark All As Read By Resource Id
        [HttpPost]
        [Route("MarkAllAsReadByResourceId")]
        public async Task<IActionResult> MarkAllAsReadByResourceId(NotificationMarkAsRead notificationMarkAsRead)
        {
            NotificationViewList notificationViewList = new();
            string statusText = "";
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notification:BaseURL"))
                };
                HttpResponseMessage response = await client.PostAsJsonAsync("Notifications/MarkAllAsReadByResourceId", notificationMarkAsRead);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsAsync<SuccessData>();
                    if (result != null)
                    {
                        notificationViewList = await GetNotificationListByResourceId(notificationMarkAsRead.ResourceId);
                        return Ok(new
                        {
                            result.Result.StatusCode,
                            result.Result.StatusText,
                            notificationViews = notificationViewList.NotificationView,
                            notificationViewList.unReadCount
                        });
                    }
                    else if (result != null)
                    {
                        statusText = result.Result.StatusText;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Notifications/MarkAllAsReadByResourceId", JsonConvert.SerializeObject(notificationMarkAsRead));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                notificationViews = notificationViewList.NotificationView,
                notificationViewList.unReadCount
            });
        }
        #endregion

        #region Mark As Read By Notification Id
        [HttpPost]
        [Route("MarkAsReadByNotificationId")]
        public async Task<IActionResult> MarkAsReadByNotificationId(NotificationMarkAsRead notificationMarkAsRead)
        {
            NotificationViewList notificationViewList = new();
            string statusText = "";
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notification:BaseURL"))
                };
                HttpResponseMessage response = await client.PostAsJsonAsync("Notifications/MarkAsReadByNotificationId", notificationMarkAsRead);
                if (response?.IsSuccessStatusCode == true)
                {
                    var result = response.Content.ReadAsAsync<SuccessData>();
                    if (result != null)
                    {
                        notificationViewList = await GetNotificationListByResourceId(notificationMarkAsRead.ResourceId);
                        return Ok(new
                        {
                            result.Result.StatusCode,
                            result.Result.StatusText,
                            notificationViews = notificationViewList.NotificationView,
                            notificationViewList.unReadCount
                        });
                    }
                    else if (result != null)
                    {
                        statusText = result.Result.StatusText;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Notifications/MarkAsReadByNotificationId", JsonConvert.SerializeObject(notificationMarkAsRead));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                notificationViews = notificationViewList.NotificationView,
                notificationViewList.unReadCount
            });
        }
        #endregion

        #region Delete document by id
        [HttpGet]
        [Route("DeleteDocumentById")]
        public async Task<IActionResult> DeleteDocumentById(int documentId)
        {
            bool isDeleted = false;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_notificationBaseURL);
                    HttpResponseMessage response = await client.GetAsync("SupportingDocuments/DeleteDocumentById?documentId=" + documentId);
                    if (response?.IsSuccessStatusCode == true)
                    {
                        var result = response.Content.ReadAsAsync<SuccessData>();
                        isDeleted = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result.Result.Data));
                    }
                };
                if (isDeleted)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "SUCCESS",
                        Data = isDeleted
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Notifications/DeleteDocumentById", Convert.ToString(documentId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Dowload document
        [HttpGet]
        [Route("DownloadDocumentById")]
        public async Task<IActionResult> DownloadDocumentById(int documentId)
        {
            SupportingDocuments documents = new();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_notificationBaseURL);
                    HttpResponseMessage responseData = await client.GetAsync("SupportingDocuments/DownloadDocumentById?documentId=" + documentId);
                    if (responseData?.IsSuccessStatusCode == true)
                    {
                        var result = responseData.Content.ReadAsAsync<SuccessData>();
                        documents = JsonConvert.DeserializeObject<SupportingDocuments>(JsonConvert.SerializeObject(result.Result.Data));
                        //Read the File into a Byte Array.
                        byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(documents.DocumentPath, documents.DocumentName));
                        string contentType;
                        new FileExtensionContentTypeProvider().TryGetContentType(documents.DocumentName, out contentType);
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "",
                            BaseString = Convert.ToBase64String(bytes),
                            ContentType = contentType ?? "application/octet-stream"
                        });
                    }
                };
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Notifications/DownloadDocumentById", Convert.ToString(documentId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                ByteArray = new ByteArrayContent(new byte[1]),
                ContentType = "application/octet-stream"
            });
        }
        #endregion 
    }
}