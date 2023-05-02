using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notifications.DAL.Services;
using SharedLibraries.Common;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Notifications;

namespace Notifications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportingDocumentsController : ControllerBase
    {
        private readonly NotificationsServices _notificationServices;
        public SupportingDocumentsController(NotificationsServices notificationServices)
        {
            _notificationServices = notificationServices;
        }

        #region Insert supporting documents
        [HttpPost]
        [Route("AddSupportingDocuments")]
        public IActionResult AddSupportingDocuments(SupportingDocumentsView supportingDocuments)
        {
            try
            {
                if (_notificationServices.AddSupportingDocuments(supportingDocuments).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "SUCCESS",
                        Data=true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "FAILURE",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/AddSupportingDocuments", JsonConvert.SerializeObject(supportingDocuments));
                return Ok(new 
                { 
                    StatusCode = "FAILURE", 
                    StatusText = ex.ToString(),
                    Data = false 
                });
            }
        }
        #endregion
        //#region Delete supporting documents
        //[HttpPost]
        //[Route("DeleteSupportingDocuments")]
        //public IActionResult DeleteSupportingDocuments(SupportingDocumentsView supportingDocumentsView)
        //{
        //    try
        //    {
        //        if (_notificationServices.DeleteSupportingDocuments(supportingDocumentsView).Result)
        //        {
        //            return Ok(new
        //            {
        //                StatusCode = "SUCCESS",
        //                StatusText = "SUCCESS",
        //                Data = true
        //            });
        //        }
        //        else
        //        {
        //            return Ok(new
        //            {
        //                StatusCode = "FAILURE",
        //                StatusText = "FAILURE",
        //                Data = false
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = false });
        //    }
        //}
        //#endregion

        #region Delete document by Id
        [HttpGet]
        [Route("DeleteDocumentById")]
        public IActionResult DeleteDocumentById(int documentId)
        {
            try
            {
                if (_notificationServices.DeleteDocumentById(documentId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "SUCCESS",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "FAILURE",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/DeleteDocumentById", Convert.ToString(documentId));
                return Ok(new
                { 
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion
        #region Get documents by source id
        [HttpPost]
        [Route("GetDocumentBySourceIdAndType")]
        public IActionResult GetDocumentBySourceIdAndType(SourceDocuments sourceDocuments)
        {
            List<SupportingDocuments> lstOfDocument = new List<SupportingDocuments>();
            try
            {
                lstOfDocument = _notificationServices.GetDocumentBySourceIdAndType(sourceDocuments);
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "SUCCESS",
                        Data = lstOfDocument
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/GetDocumentBySourceIdAndType", JsonConvert.SerializeObject(sourceDocuments));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = lstOfDocument
                });
            }
        }
        #endregion
        #region Get documents by source id
        [HttpGet]
        [Route("DownloadDocumentById")]
        public IActionResult DownloadDocumentById(int documentId)
        {
            SupportingDocuments supportDocuments = new SupportingDocuments();
            try
            {
                supportDocuments = _notificationServices.DownloadDocumentById(documentId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = supportDocuments
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/DownloadDocumentById", Convert.ToString(documentId));
                return Ok(new 
                { 
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = supportDocuments
                });
            }
        }
        #endregion

        #region Approved the documents by source id and type
        [HttpPost]
        [Route("ApprovedDocumentsBySourceIdAndType")]
        public IActionResult ApprovedDocumentsBySourceIdAndType(SourceDocuments sourceDocuments)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _notificationServices.ApprovedDocumentsBySourceIdAndType(sourceDocuments)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Notifications", "Notifications/ApprovedDocumentsBySourceIdAndType", JsonConvert.SerializeObject(sourceDocuments));
                return Ok(new
                { 
                    StatusCode = "FAILURE", 
                    StatusText = ex.Message, 
                    Data = false
                });
            }
        }
        #endregion
    }
}
