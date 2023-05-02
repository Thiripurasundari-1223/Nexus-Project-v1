using ExitManagement.DAL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.ExitManagement;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using System;
using System.Collections.Generic;

namespace ExitManagement.API.Controllers
{
    [Route("api/[controller]")]
    // [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class ExitManagementController : ControllerBase
    {
        private readonly ExitManagementServices _exitManagementServices;
        private readonly string StatusText = "Something went wrong, please try again later";
        public ExitManagementController(ExitManagementServices exitManagementServices/*, IConfiguration iconfiguration*/)
        {
            _exitManagementServices = exitManagementServices;
        }


        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "ExitManagement API - GET Method"
            });
        }

        #region insert or update Resignation Details        
        [HttpPost]
        [Route("InsertAndUpdateResignationDetails")]
        public IActionResult InsertAndUpdateResignationDetails(EmployeeResignationDetailsView employeeResignationDetails)
        {
            try
            {
                int resignationId = employeeResignationDetails.EmployeeResignationDetailsId;
                EmployeeResignationDetailsView resignationDetails = _exitManagementServices.InsertAndUpdateResignationDetails(employeeResignationDetails).Result;
                if (resignationDetails != null)
                {
                    string status = "Resignation application submitted successfully.";
                    if (employeeResignationDetails.IsWithdrawal == true)
                    {
                        status = "Resignation Withdrawal submitted successfully.";
                    }
                    else if(resignationId > 0 )
                    {
                        status = "Resignation application updated successfully.";
                    }
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = status,
                        Data = resignationDetails
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = new EmployeeResignationDetailsView()
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/InsertAndUpdateResignationDetails", JsonConvert.SerializeObject(employeeResignationDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new EmployeeResignationDetailsView()
                });
            }
        }
        #endregion

        #region insert or update Resignation Reason        
        [HttpPost]
        [Route("InsertAndUpdateResignationReason")]
        public IActionResult InsertAndUpdateResignationReason(EmployeeResignationDetailsView employeeResignationDetails)
        {
            try
            {
                int resignationId = employeeResignationDetails.EmployeeResignationDetailsId;
                EmployeeResignationDetailsView resignationDetails = _exitManagementServices.InsertAndUpdateResignationReason(employeeResignationDetails).Result;
                if (resignationDetails != null)
                {
                    string status = "Resignation Reason submitted successfully.";
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = status,
                        Data = resignationDetails
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = new EmployeeResignationDetailsView()
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/InsertAndUpdateResignationReason", JsonConvert.SerializeObject(employeeResignationDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new EmployeeResignationDetailsView()
                });
            }
        }
        #endregion
        #region
        [HttpPost]
        [Route("GetAllResignationDetails")]
        public IActionResult GetAllResignationDetails(AllResignationInputView resignationInputView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _exitManagementServices.GetAllResignationDetails(resignationInputView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetAllResignationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<EmployeeResignationDetailsView>()
                });
            }
        }
        #endregion
        #region
        [HttpGet]
        [Route("GetResinationReason")]
        public IActionResult GetResinationReason()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _exitManagementServices.GetResignationReasons()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResinationReason");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new List<ResignationReason>()
                });
            }
        }
        #endregion
        #region
        [HttpGet]
        [Route("GetResignationDetailsByResignationId")]
        public IActionResult GetResignationDetailsByResignationId(int resignationId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _exitManagementServices.GetResignationDetailsByResignationId(resignationId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResignationDetailsByResignationId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new EmployeeResignationDetailsView()
                });
            }
        }
        #endregion
        #region Insert or update approvel level
        [HttpPost]
        [Route("InsertOrUpdateResignationApproval")]
        public IActionResult InsertOrUpdateResignationApproval(List<ResignationApproval> resignationApproval)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Level of approval updated sucessfully",
                    Data = _exitManagementServices.InsertOrUpdateResignationApproval(resignationApproval).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/InsertOrUpdateResignationApproval");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = false
                });
            }
        }
        #endregion
        #region Get resign approvel level
        [HttpGet]
        [Route("GetResignationApproval")]
        public IActionResult GetResignationApproval()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _exitManagementServices.GetResignationApproval()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResignationApproval");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = new ApprovalConfiguration()
                });
            }
        }
        #endregion 
        #region  Approve Or Reject Or Cancel Resignation
        [HttpPost]
        [Route("ApproveOrRejectOrCancelResignation")]
        public IActionResult ApproveOrRejectOrCancelResignation(ApproveResignationView resignationDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = ""+ resignationDetails.Status + " successfully",
                    Data = _exitManagementServices.ApproveOrRejectOrCancelResignation(resignationDetails)?.Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/ApproveOrRejectOrCancelResignation");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText,
                    Data = "FAILURE"
                });
            }
        }
        #endregion

        #region insert or update Resignation Interview Details        
        [HttpPost]
        [Route("InsertAndUpdateResignationInterviewDetails")]
        public IActionResult InsertAndUpdateResignationInterviewDetails(ResignationInterviewDetailView resignationInterviewDetail)
        {
            try
            {
                if (resignationInterviewDetail?.ResignationInterviewId == 0)
                {
                    int result = _exitManagementServices.GetResignationInterviewByEmployeeId(resignationInterviewDetail.EmployeeID);
                    if (result==1 || result==2)
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = result == 1?"Already Exit Interview has added, Please review it.":"Invalid entry, Please check the resignation details",
                            Data = new ResignationInterviewDetailView()
                        });
                    }
                }
                ResignationInterviewDetailView resignationInterview  = _exitManagementServices.InsertAndUpdateResignationInterviewDetails(resignationInterviewDetail).Result;
                if (resignationInterview != null)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Exit Interview submitted successfully.",
                        Data = resignationInterview
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = new ResignationInterviewDetailView()
                    }) ;
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/InsertAndUpdateResignationInterviewDetails", JsonConvert.SerializeObject(resignationInterviewDetail));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new ResignationInterviewDetailView()
                });
            }
        }
        #endregion
        #region Get resignation interview details by employee id
        [HttpGet]
        [Route("GetResignationInterviewDetailByEmployeeId")]
        public IActionResult GetResignationInterviewDetailByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _exitManagementServices.GetResignationInterviewDetailByEmployeeId(employeeId)

                }); ;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResignationInterviewDetailByEmployeeId", JsonConvert.SerializeObject(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "",
                    Data = new ResignationInterviewListView()
                });
            }
        }
        #endregion

        #region Get resignation interview details by interview id
        [HttpGet]
        [Route("GetResignationInterviewDetailById")]
        public IActionResult GetResignationInterviewDetailById(int resignationInterviewId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _exitManagementServices.GetResignationInterviewDetailById(resignationInterviewId)

                }); ;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResignationInterviewDetailById", JsonConvert.SerializeObject(resignationInterviewId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "",
                    Data = new ResignationInterviewDetailView()
                });
            }
        }
        #endregion
        #region Get resignation interview master data
        [HttpPost]
        [Route("GetResignationInterviewMasterData")]
        public IActionResult GetResignationInterviewMasterData(List<ResignationEmployeeMasterView> employeeList)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _exitManagementServices.GetResignationInterviewMasterData(employeeList)

                }); ;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/GetResignationInterviewMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "",
                    Data = new ResignationInterviewMasterData()
                });
            }
        }
        #endregion

        #region Delete the exit interview Details
        [HttpGet]
        [Route("DeleteExitInterviewByInterviewId")]
        public IActionResult DeleteExitInterviewByInterviewId(int exitInterviewId)
        {
            try
            {
                if (_exitManagementServices.DeleteExitInterviewByInterviewId(exitInterviewId).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "Exit Interview Deleted Successfully",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "Failure",
                        StatusText = "Unable To Delete Exit Interview",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/DeleteExitInterviewByInterviewId", Convert.ToString(exitInterviewId));
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = false
                });
            }
        }

        #endregion

        #region Check the exit interview Details Enable
        [HttpPost]
        [Route("GetCheckExitInterviewEnable")]
        public IActionResult GetCheckExitInterviewEnable(ExitInterviewEnable exitInterview)
        {
            try
            {
                   return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "",
                        Data = _exitManagementServices.GetCheckExitInterviewEnable(exitInterview).Result
                    });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetCheckExitInterviewEnable", JsonConvert.SerializeObject(exitInterview));
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = false
                });
            }
        }

        #endregion
        #region Get resignation checklist master data
        [HttpPost]
        [Route("GetResignationChecklistMasterData")]
        public IActionResult GetResignationChecklistMasterData(List<ResignationEmployeeMasterView> employeeList)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetResignationChecklistMasterData(employeeList)
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetResignationChecklistMasterData");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new ResignationChecklistMasterData()
                });
            }
        }
        #endregion
        #region inser or update resignation checklist details      
        [HttpPost]
        [Route("InsertOrUpdateResignationChecklist")]
        public IActionResult InsertOrUpdateResignationChecklist(ResignationChecklistView resignationChecklistDetails)
        {
            try
            {
                if(resignationChecklistDetails?.ResignationChecklistId==0)
                {
                    int result = _exitManagementServices.GetResignationChecklistByEmployeeId(resignationChecklistDetails.EmployeeID);
                    if (result ==1 || result==2)
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = result == 1 ? "Already Exit Checklist has added, Please review it." : "Invalid entry, Please check the resignation details",
                            Data = new ResignationChecklistView()
                        });
                    }
                }
                ResignationChecklistView resignationDetails = _exitManagementServices.InsertOrUpdateResignationChecklist(resignationChecklistDetails).Result;
                if (resignationDetails != null)
                {
                    string status = "Exit - Checklist submitted successfully.";
                    if(resignationChecklistDetails.IsSubmit==false)
                    {
                        status = "Exit - Checklist saved successfully.";
                    }                    
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = status,
                        Data = resignationDetails
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = new ResignationChecklistView()
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "ExitManagement", "ExitManagement/InsertOrUpdateResignationChecklist", JsonConvert.SerializeObject(resignationChecklistDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText= "Unexpected error occurred. Try again.",
                    Data = new ResignationChecklistView()
                });
            }
        }
        #endregion
        #region Get resignation checklist details by id
        [HttpPost]
        [Route("GetResignationChecklistById")]
        public IActionResult GetResignationChecklistById(ResignationChecklistView checkList)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetResignationChecklistById(checkList)?.Result
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetResignationChecklistById");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new ResignationChecklistView()
                });
            }
        }
        #endregion
        #region Get my checklist details
        [HttpPost]
        [Route("GetMyCheckListDetails")]
        public IActionResult GetMyCheckListDetails(AllResignationInputView resignationInputView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetMyCheckListDetails(resignationInputView)
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetMyCheckListDetails");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<ResignationChecklistDetails>()
                });
            }
        }
        #endregion

        #region Get reportees checklist details
        [HttpPost]
        [Route("GetReporteesCheckListDetails")]
        public IActionResult GetReporteesCheckListDetails(AllResignationInputView resignationInputView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetReporteesCheckListDetails(resignationInputView)
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetReporteesCheckListDetails");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<ChecklistEmployeeView>()
                });
            }
        }
        #endregion

        #region delete checklist details
        [HttpGet]
        [Route("DeleteChecklistById")]
        public IActionResult DeleteChecklistById(int checklistId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.DeleteChecklistById(checklistId)?.Result
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/DeleteChecklistById");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = false
                });
            }
        }
        #endregion
        #region Get employee resignation details
        [HttpPost]
        [Route("GetLastResignationIdByEmployeeList")]
        public IActionResult GetLastResignationIdByEmployeeList(List<int> employeeIdList)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetLastResignationIdByEmployeeList(employeeIdList)
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetLastResignationIdByEmployeeList");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<KeyWithIntValue>()
                });
            }
        }
        #endregion
        #region Get Checklist Submission Notification Employee
        [HttpGet]
        [Route("GetChecklistSubmissionNotificationEmployee")]
        public IActionResult GetChecklistSubmissionNotificationEmployee(int days)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetChecklistSubmissionNotificationEmployee(days)
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetChecklistSubmissionNotificationEmployee");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<int>()
                });
            }
        }
        #endregion
        #region Get Checklist Complete Notification Employee
        [HttpGet]
        [Route("GetChecklistCompleteNotificationEmployee")]
        public IActionResult GetChecklistCompleteNotificationEmployee(int days)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetChecklistCompleteNotificationEmployee(days)
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetChecklistCompleteNotificationEmployee");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<int>()
                });
            }
        }
        #endregion

        #region Get employee resignation details without interview
        [HttpGet]
        [Route("GetResignationInterviewNotification")]
        public IActionResult GetResignationInterviewNotification(int days)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = _exitManagementServices.GetResignationInterviewNotification(days)
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetResignationWithoutInterview");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<KeyWithIntValue>()
                });
            }
        }
        #endregion

        #region Get Resignation employee filter list 
        [HttpPost]
        [Route("GetResignationEmployeeListByFilter")]
        public IActionResult GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = string.Empty,
                        Data = _exitManagementServices.GetResignationEmployeeListByFilter(resignationEmployeeFilter)
                    });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetResignationEmployeeListByFilter");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<ResignationEmployeeMasterView>()
                });
            }
        }
        #endregion
        #region Get Resignation employee filter list 
        [HttpPost]
        [Route("GetResignationEmployeeListByFilterCount")]
        public IActionResult GetResignationEmployeeListByFilterCount(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = string.Empty,
                        Data = _exitManagementServices.GetResignationEmployeeListByFilterCount(resignationEmployeeFilter)
                    });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Exit Management", "ExitManagement/GetResignationEmployeeListByFilterCount");
                return Ok(new
                {
                    StatusCode = "Failure",
                    StatusText,
                    Data = new List<ResignationEmployeeMasterView>()
                });
            }
        }
        #endregion
    }
}
