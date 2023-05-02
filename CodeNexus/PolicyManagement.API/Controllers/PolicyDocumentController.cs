using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PolicyManagement.DAL.Models;
using PolicyManagement.DAL.Services;
using SharedLibraries.Common;
using SharedLibraries.ViewModels.PolicyManagement;
using System.Reflection.Metadata;

namespace PolicyManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyDocumentController : ControllerBase
    {
        private readonly PolicyDocumentService _policyDocumentService;
        private readonly RequestedDocumentService _requestedDocumentService;
        private readonly AnnouncementService _announcementService;

        #region Constructor
        public PolicyDocumentController(PolicyDocumentService policyDocumentService,
                                        RequestedDocumentService requestedDocumentService,
                                        AnnouncementService announcementService)
        {
            _policyDocumentService = policyDocumentService;
            _requestedDocumentService = requestedDocumentService;
            _announcementService = announcementService;
        }
        #endregion

        #region Get Empty Method
        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Policy Document API - GET Method"
            });
        }
        #endregion

        #region Get Folders For Policy
        [HttpGet("GetFoldersForPolicy")]
        public IActionResult GetFoldersForPolicy()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _policyDocumentService.GetFolders()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetFoldersForPolicy");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Get Policy Document By Id
        [HttpGet("GetPolicyDocumentById")]
        public IActionResult GetPolicyDocumentById(int PolicyDocumentId, int UserId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _policyDocumentService.GetPolicyDocumentById(PolicyDocumentId, UserId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetPolicyDocumentById");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Get Policy Document By UserId or Get All
        [HttpGet("GetPolicyDocumentByUserId")]
        public IActionResult GetPolicyDocumentByUserId(int UserId = 0, string DocType = "", int LocationId = 0, int RoleId = 0,
                                                        int DepartmentId = 0, int CurrentWorkLocationId = 0, int CurrentWorkPlaceId = 0)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _policyDocumentService.GetPolicyDocumentByUserId(UserId, DocType, LocationId, RoleId, DepartmentId, CurrentWorkLocationId, CurrentWorkPlaceId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetPolicyDocumentById");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Save Policy Document
        [HttpPost]
        [Route("SavePolicyDocument")]
        public IActionResult SavePolicyDocument(PolicyDocumentView policyDocumentView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _policyDocumentService.SavePolicyDocument(policyDocumentView).Result
                });
            }
            catch (Exception ex)
            {
                if (policyDocumentView.DocumentToUpload != null)
                {
                    policyDocumentView.DocumentToUpload.DocumentAsBase64 = null;
                    policyDocumentView.DocumentToUpload.DocumentAsByteArray = null;
                }
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/SavePolicyDocument", JsonConvert.SerializeObject(policyDocumentView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE",
                Data = 0
            });
        }
        #endregion

        #region Save Folder
        [HttpPost]
        [Route("SaveFolder")]
        public IActionResult SaveFolder(FolderView folderView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _policyDocumentService.SaveFolder(folderView).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/SaveFolder", JsonConvert.SerializeObject(folderView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion        

        #region Delete Policy Document By Id
        [HttpPost("DeletePolicyDocumentById")]
        public IActionResult DeletePolicyDocumentById(int PolicyDocumentId, int ModifiedBy, string ArchivePath)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _policyDocumentService.DeletePolicyDocumentById(PolicyDocumentId, ModifiedBy, ArchivePath).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/DeletePolicyDocumentById", PolicyDocumentId.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Get Document Types
        [HttpGet("GetDocumentTypes")]
        public IActionResult GetDocumentTypes(int documentTypeId = 0)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _requestedDocumentService.GetDocumentTypes(documentTypeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetDocumentTypes");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Get Employee List For Requested Documents
        [HttpGet("GetEmployeeListForRequestedDocuments")]
        public IActionResult GetEmployeeListForRequestedDocuments(string status)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _requestedDocumentService.GetEmployeeListForRequestedDocuments(status)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetEmployeeListForRequestedDocuments");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion 

        #region Get Requested Documents
        [HttpGet("GetRequestedDocuments")]
        public IActionResult GetRequestedDocuments(int UserId = 0)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _requestedDocumentService.GetRequestedDocuments(UserId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetRequestedDocuments");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion 

        #region Save Requested Document
        [HttpPost]
        [Route("SaveRequestedDocument")]
        public IActionResult SaveRequestedDocument(RequestedDocumentView requestedDocument)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _requestedDocumentService.SaveRequestedDocument(requestedDocument).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/SaveRequestedDocument", JsonConvert.SerializeObject(requestedDocument));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Approve Or Reject Requested Document
        [HttpPost]
        [Route("ApproveOrRejectRequestedDocument")]
        public IActionResult ApproveOrRejectRequestedDocument(RequestedDocumentView requestedDocument)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _requestedDocumentService.ApproveOrRejectRequestedDocument(requestedDocument).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/ApproveOrRejectRequestedDocument", JsonConvert.SerializeObject(requestedDocument));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Generate Requested Document
        [HttpGet]
        [Route("GenerateRequestedDocument")]
        public IActionResult GenerateRequestedDocument(int requestedDocumentId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _requestedDocumentService.GenerateRequestedDocument(requestedDocumentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GenerateRequestedDocument", requestedDocumentId.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Save Document Type
        [HttpPost]
        [Route("SaveDocumentType")]
        public IActionResult SaveDocumentType(DocumentTypeView document)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _requestedDocumentService.SaveDocumentType(document).Result
                });
            }
            catch (Exception ex)
            {
                if (document.DocumentToUpload != null)
                {
                    document.DocumentToUpload.DocumentAsBase64 = null;
                    document.DocumentToUpload.DocumentAsByteArray = null;
                }
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/SaveDocumentType", JsonConvert.SerializeObject(document));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE",
                Data = 0
            });
        }
        #endregion

        #region Update Policy Acknowledgement
        [HttpPost]
        [Route("UpdatePolicyAcknowledgement")]
        public IActionResult UpdatePolicyAcknowledgement(PolicyAcknowledgementView document)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _policyDocumentService.UpdatePolicyAcknowledgement(document).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/UpdatePolicyAcknowledgement", JsonConvert.SerializeObject(document));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE",
                Data = 0
            });
        }
        #endregion

        #region Get Employee List For Acknowledgement
        [HttpGet("GetEmployeeListForAcknowledgement")]
        public IActionResult GetEmployeeListForAcknowledgement(int policyDocumentId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _policyDocumentService.GetEmployeeListForAcknowledgement(policyDocumentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetEmployeeListForAcknowledgement");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Get Policy Acknowledgement By Employee
        [HttpGet("GetPolicyAcknowledgementByEmployee")]
        public IActionResult GetPolicyAcknowledgementByEmployee(int EmployeeId, int LocationId = 0, int RoleId = 0,
                                                        int DepartmentId = 0, int CurrentWorkLocationId = 0, int CurrentWorkPlaceId = 0)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _policyDocumentService.GetPolicyAcknowledgementByEmployee(EmployeeId, LocationId, RoleId, DepartmentId,
                                                                                            CurrentWorkLocationId, CurrentWorkPlaceId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetPolicyAcknowledgementByEmployee");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion

        #region Save or Update an Announcement
        [HttpPost]
        [Route("SaveAnnouncement")]
        public IActionResult SaveAnnouncement(AnnouncementView announcementView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = _announcementService.SaveAnnouncement(announcementView).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/SaveAnnouncement",
                                                        JsonConvert.SerializeObject(announcementView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE",
                Data = 0
            });
        }
        #endregion

        #region Get an Announcement
        [HttpGet("GetAnnouncements")]
        public IActionResult GetAnnouncements(int pAnnouncementId = 0)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _announcementService.GetAnnouncements(pAnnouncementId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "PolicyManagement", "PolicyManagement/GetAnnouncements");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "FAILURE"
            });
        }
        #endregion
    }
}