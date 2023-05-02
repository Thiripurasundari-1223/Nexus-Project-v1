using Appraisal.DAL.Services;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SharedLibraries.ViewModels.Appraisal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SharedLibraries.Common;
using LoggerManager = SharedLibraries.Common.LoggerManager;
using SharedLibraries.Models;
using SharedLibraries.Models.Appraisal;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Appraisal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppraisalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppraisalServices _appraisalServices;
        private readonly WorkDayDetailServices _workDayDetailServices;

        public AppraisalController(AppraisalServices appraisalServices, IConfiguration iConfiguration, WorkDayDetailServices workDayDetailServices)
        {
            _appraisalServices = appraisalServices;
            _configuration = iConfiguration;
            _workDayDetailServices = workDayDetailServices;
        }
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region WorkDayDetail End Points

        #region Get Employee WorkDay Details by Filter  

        // <summary>
        /// Filter the WorkDayDetail
        /// </summary>
        /// <param name="appraisalWorkDayFilterView"></param>
        /// <returns></returns>
        /// [AllowAnonymous]
        [HttpPost]
        [Route("GetEmployeeWorkDayDetailsByFilter")]
        public IActionResult GetEmployeeWorkDayDetailsByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetEmployeeWorkDayDetailsByFilter(appraisalWorkDayFilterView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeWorkDayDetailsByFilter");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AppraisalWorkDayView>()
                });
            }
        }
        #endregion

        #region Save or Update WorkDay Detail
        /// <summary>
        /// Save or update the WorkDay Detail
        /// </summary>
        /// <param name="detailView"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateWorkDayDetail")]
        public IActionResult SaveOrUpdateWorkDayDetail(WorkdayInputView detailView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "WorkDay Detail(s) are saved successfully.",
                    Data = _workDayDetailServices.SaveOrUpdateWorkDayDetail(detailView).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "WorkDayDetail", "WorkDayDetail/SaveOrUpdateWorkDayDetail", JsonConvert.SerializeObject(detailView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Approve Or Reject Workday Detail View
        /// <summary>
        /// Approve Or Reject Workday Detail View
        /// </summary>
        /// <param name="approveOrRejectWorkday"></param>
        /// <returns></returns>
        [HttpPost("ApproveOrRejectWorkdayDetail")]
        public IActionResult ApproveOrRejectWorkDayDetail(ApproveOrRejectWorkdayDetailView approveOrRejectWorkday)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "WorkDay Detail status is updated successfully.",
                    Data = _workDayDetailServices.ApproveOrRejectWorkDayDetail(approveOrRejectWorkday).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/ApproveOrRejectWorkDayDetail", JsonConvert.SerializeObject(approveOrRejectWorkday));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Approve Or Reject Workday List View
        /// <summary>
        /// Approve Or Reject Workday Detail View
        /// </summary>
        /// <param name="approveOrRejectWorkday"></param>
        /// <returns></returns>
        [HttpPost("ApproveOrRejectWorkDayListView")]
        public IActionResult ApproveOrRejectWorkDayListView(ApproveOrRejectWorkdayListView approveOrRejectWorkday)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "WorkDay Detail status is updated successfully.",
                    Data = _workDayDetailServices.ApproveOrRejectWorkDayListView(approveOrRejectWorkday).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/ApproveOrRejectWorkDayListView", JsonConvert.SerializeObject(approveOrRejectWorkday));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Delete WorkDay Detail
        /// <summary>
        /// Delete the WorkDay Detail
        /// </summary>
        /// <param name="WorkDayDetailId"></param>
        /// <returns></returns>
        [HttpGet("DeleteWorkDayDetail")]
        public IActionResult DeleteWorkDayDetail(int WorkDayDetailId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "WorkDay Detail is deleted successfully.",
                    Data = _workDayDetailServices.DeleteWorkDayDetail(WorkDayDetailId).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "WorkDayDetail", "WorkDayDetail/DeleteWorkDayDetail", "WorkDayDetailId " + WorkDayDetailId.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #endregion WorkDayDetail

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Appraisal API - GET Method"
            });
        }

        #region Add or update Entity
        [HttpPost]
        [Route("AddOrUpdateEntity")]
        public IActionResult AddorUpdateEntity(EntityView entityView)
        {
            string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                if (_appraisalServices.EntityNameDuplication(entityView))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Entity Name is already exists. Please change Entity name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    int entityId = _appraisalServices.AddorUpdateEntity(entityView).Result;
                    if (entityId > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = ("Entity Added successfully."),
                            data = entityId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Unexpected error occurred. Try again.",
                            Data = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateEntity", JsonConvert.SerializeObject(entityView));
                StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }
        #endregion

        #region Get All Entities
        [HttpGet]
        [Route("GetEntityDetails")]
        public IActionResult GetAllEntityDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAllEntityDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAllEntityDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EntityMaster>()
                });
            }
        }
        #endregion

        #region Delete Entity
        [HttpGet]
        [Route("DeleteEntity")]
        public IActionResult DeleteEntity(int entityId)
        {
            try
            {
                string isSuccess = _appraisalServices.DeleteEntity(entityId).Result;
                if (isSuccess == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Entity deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = isSuccess,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Add or update Version
        [HttpPost]
        [Route("AddOrUpdateVersion")]
        public IActionResult AddOrUpdateVersion(VersionView versionView)
        {
            string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                if (_appraisalServices.VersionNameDuplication(versionView))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Version Name is already exists. Please change Version name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    int newVersionId = _appraisalServices.AddOrUpdateVersion(versionView).Result;
                    if (newVersionId > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = ("Version Added successfully."),
                            data = newVersionId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Unexpected error occurred. Try again.",
                            Data = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateVersion", JsonConvert.SerializeObject(versionView));
                StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }
        #endregion

        #region Get All Version Details
        [HttpGet]
        [Route("GetAllVersionDetails")]
        public IActionResult GetAllVersionDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAllVersionDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAllVersionDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<VersionMaster>()
                });
            }
        }
        #endregion

        #region Delete Version
        [HttpGet]
        [Route("DeleteVersion")]
        public IActionResult DeleteVersion(int versionId)
        {
            try
            {
                string isSuccess = _appraisalServices.DeleteVersion(versionId).Result;
                if (isSuccess == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Version deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = isSuccess,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Add or update Department & Role Version
        [HttpPost]
        [Route("AddOrUpdateVersionDepartmentRole")]
        public IActionResult AddOrUpdateVersionDepartmentRole(List<DepartmentRoleView> departmentRoleViews)
        {
            try
            {
                int appraisalid = _appraisalServices.AddOrUpdateDepartmentRoles(departmentRoleViews).Result;
                if (appraisalid > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Department Roles saved successfully.",
                        Data = appraisalid
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateDepartmentRoles", JsonConvert.SerializeObject(departmentRoleViews));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Add or Update AppraisalCycle
        [HttpPost]
        [Route("AddorUpdateAppraisalCycle")]
        public IActionResult AddorUpdateAppraisalCycle(AppraisalCycleView appraisalCycleView)
        {
            string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                if (_appraisalServices.AppraisalCycleNameDuplication(appraisalCycleView.AppCycleName, appraisalCycleView.AppCycleId))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Appraisal Cycle Name is already exists. Please change Appraisal Cycle name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    int appCycleId = _appraisalServices.AddorUpdateAppraisalCycle(appraisalCycleView).Result;
                    if (appCycleId > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = ("Appraisal Cycle Added successfully."),
                            data = appCycleId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Unexpected error occurred. Try again.",
                            Data = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateAppraisalCycle", JsonConvert.SerializeObject(appraisalCycleView));
                StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }
        #endregion

        #region Get All AppraisalCycle
        [HttpGet]
        [Route("GetAllAppraisalCycle")]
        public IActionResult GetAllAppraisalCycle()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAllAppraisalCycleDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAllAppraisalCycle");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AppraisalMaster>()
                });
            }
        }
        #endregion

        #region Delete AppraisalCycle
        [HttpGet]
        [Route("DeleteAppraisalCycle")]
        public IActionResult DeleteAppraisalCycle(int appCycleId)
        {
            try
            {
                string isSuccess = _appraisalServices.DeleteAppraisalCycle(appCycleId).Result;
                if (isSuccess == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "AppraisalCycle deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = isSuccess,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Update AppraisalCycle Status
        [HttpPost]
        [Route("UpdateAppraisalCycleStatus")]
        public IActionResult UpdateAppraisalCycleStatus(UpdateAppraisalStatusView appraisalCycleView)
        {
            try
            {
                int appCycleId = _appraisalServices.UpdateAppraisalCycleStatus(appraisalCycleView).Result;
                if (appCycleId > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "AppraisalCycle Status Updated successfully.",
                        Data = appCycleId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/UpdateAppraisalCycleStatus", JsonConvert.SerializeObject(appraisalCycleView));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Add or update Objective
        [HttpPost]
        [Route("AddorUpdateObjective")]
        public IActionResult AddorUpdateObjective(ObjectiveView objectiveView)
        {
            string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                if (_appraisalServices.ObjectiveNameDuplication(objectiveView))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Objective Name is already exists. Please change Objective name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    int objectiveId = _appraisalServices.AddorUpdateObjective(objectiveView).Result;
                    if (objectiveId > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "Objective Added successfully.",
                            data = objectiveId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Unexpected error occurred. Try again.",
                            Data = 0
                        });
                    }
                }
            }

            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateObjective", JsonConvert.SerializeObject(objectiveView));
                StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }
        #endregion

        #region Get All Objective
        [HttpGet]
        [Route("GetObjectiveDetails")]
        public IActionResult GetAllObjectiveDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAllObjectiveDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAllObjectiveDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<ObjectiveMaster>()
                });
            }
        }
        #endregion

        #region Delete Objective
        [HttpGet]
        [Route("DeleteObjective")]
        public IActionResult DeleteObjective(int objectiveId)
        {
            try
            {
                string isSuccess = _appraisalServices.DeleteObjective(objectiveId).Result;
                if (isSuccess == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Objective deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = isSuccess,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Add or update KeyResultMaster
        [HttpPost]
        [Route("AddOrUpdateKeyResultMaster")]
        public IActionResult AddOrUpdateKeyResultMaster(KeyResultMasterView keyResultMasterView)
        {
            string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                if (_appraisalServices.KRANameDuplication(keyResultMasterView))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "KRA Name is already exists. Please change KRA name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    int keyResultId = _appraisalServices.AddOrUpdateKeyResultMaster(keyResultMasterView).Result;
                    if (keyResultId > 0)
                    {
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "KRA Added successfully.",
                            data = keyResultId
                        });
                    }
                    else
                    {
                        return Ok(new
                        {
                            StatusCode = "FAILURE",
                            StatusText = "Unexpected error occurred. Try again.",
                            Data = 0
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateKeyResultMaster", JsonConvert.SerializeObject(keyResultMasterView));
                StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }
        #endregion

        #region Get All Key Result Details
        [HttpGet]
        [Route("GetKeyResultDetails")]
        public IActionResult GetAllKeyResultDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAllKeyResultDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAllKeyResultDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<KeyResultMaster>()
                });
            }
        }
        #endregion

        #region Delete Key Result Master
        [HttpGet]
        [Route("DeleteKeyResultMaster")]
        public IActionResult DeleteKeyResultMaster(int keyResultId)
        {
            try
            {
                string isSuccess = _appraisalServices.DeleteKeyResultMaster(keyResultId).Result;
                if (isSuccess == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "KRA deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = isSuccess,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Get All Version Department & Role Mapping
        [HttpGet]
        [Route("GetAllVersionDepartmentRoleMapping")]
        public IActionResult GetAllVersionDepartmentRoleMapping(int versionId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAllVersionDepartmentRoleMapping(versionId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAllVersionDepartmentRoleMapping");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new VersionKRAMasterdata()
                });
            }
        }
        #endregion

        #region Get All Version Department & Role KRA Mapping
        [HttpGet]
        [Route("GetVersionDepartmentRoleKRAMapping")]
        public IActionResult GetVersionDepartmentRoleKRAMapping(int versionId, int departmentId, int roleId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionDepartmentRoleKRAMapping(versionId, departmentId, roleId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionDepartmentRoleKRAMapping");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new VersionKRAMasterdata()
                });
            }
        }
        #endregion

        #region Add or update Version Objective & KRA 
        [HttpPost]
        [Route("AddOrUpdateVersionObjectiveKRA")]
        public IActionResult AddOrUpdateVersionObjectiveKRA(List<VersionKeyResultsView> versionKeyResultsViews)
        {
            try
            {
                bool isSuccess = _appraisalServices.AddOrUpdateVersionObjectiveKRA(versionKeyResultsViews).Result;
                if (isSuccess)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Objective KRAs saved successfully.",
                        Data = isSuccess
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateVersionObjectiveKRA", JsonConvert.SerializeObject(versionKeyResultsViews));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = false });
            }
        }
        #endregion

        #region Get Version KRA Gridview Data
        [HttpGet]
        [Route("GetVersionKRAGridviewData")]
        public IActionResult GetVersionKRAGridviewData(int versionId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionKRAGridviewData(versionId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionKRAGridviewData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<VersionKRABenchmarkGridDetails>()
                });
            }
        }
        #endregion

        #region Add or update Version Benchmark Objective & KRA 
        [HttpPost]
        [Route("AddOrUpdateVersionBenchmarkKRA")]
        public IActionResult AddOrUpdateVersionBenchmarkKRA(AddVersionBenchmarkView versionBenchmarkInsertViews)
        {
            string statusText = "";
            try
            {
                bool isSuccess = _appraisalServices.AddOrUpdateVersionBenchmarkKRA(versionBenchmarkInsertViews).Result;
                if (isSuccess)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Benchmark Objective KRAs saved successfully.",
                        Data = true
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateVersionBenchmarkKRA", JsonConvert.SerializeObject(versionBenchmarkInsertViews));
                statusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });
        }
        #endregion

        #region Add or update Benchmark Version KRA Range 
        [HttpPost]
        [Route("AddOrUpdateVersionBenchmarkKRARange")]
        public IActionResult AddOrUpdateVersionBenchmarkKRARange(VersionBenchmarkRangeView benchmarkKRARangeViews)
        {
            try
            {
                bool isUpdated = _appraisalServices.AddOrUpdateVersionBenchmarkKRARange(benchmarkKRARangeViews).Result;
                if (isUpdated)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Benchmark KRA Range saved successfully.",
                        Data = isUpdated
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateVersionBenchmarkKRARange", JsonConvert.SerializeObject(benchmarkKRARangeViews));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = false });
            }
        }
        #endregion

        #region Get individual appraisal objective and KRA
        [HttpGet]
        [Route("GetIndAppraisalObjandKRAs")]
        public IActionResult GetIndAppraisalObjandKRAs(int appCycleId, int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetIndAppraisalObjandKRAs(appCycleId, employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetIndAppraisalObjandKRAs");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new AppraisalObjectiveandKRAListView()
                });
            }
        }
        #endregion

        #region Add or update Self Appraisal Objective & KRA Ratings
        [HttpPost]
        [Route("AddOrUpdateSelfAppraisalRating")]
        public IActionResult AddOrUpdateSelfAppraisalRating(List<EmployeeKRRatingView> employeeKRRatingViews)
        {
            try
            {
                int appraisalid = _appraisalServices.AddOrUpdateSelfAppraisalRating(employeeKRRatingViews).Result;
                if (appraisalid > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Self Appraisal Rating saved successfully.",
                        Data = appraisalid
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateSelfAppraisalRating", JsonConvert.SerializeObject(employeeKRRatingViews));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Add Self Appraisal Key Result Appraisal Comment
        [HttpPost]
        [Route("AddSelfAppraisalKRAComment")]
        public IActionResult AddSelfAppraisalKRAComment(IndividualComments individualComments)
        {
            try
            {
                int appraisalid = _appraisalServices.AddSelfAppraisalKRAComment(individualComments).Result;
                if (appraisalid >= 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Submitted successfully.",
                        Data = appraisalid
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddSelfAppraisalKRAComment", JsonConvert.SerializeObject(individualComments));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Add Self Appraisal Comment
        [HttpPost]
        [Route("AddSelfAppraisalComment")]
        public IActionResult AddSelfAppraisalComment(EmployeeAppraisalComment employeeAppraisalComment)
        {
            try
            {
                int appraisalid = _appraisalServices.AddSelfAppraisalComment(employeeAppraisalComment).Result;
                if (appraisalid > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Appraisal Comment saved successfully.",
                        Data = appraisalid
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "please enter comments and try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateSelfAppraisalComment", JsonConvert.SerializeObject(employeeAppraisalComment));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Get Manager appraisal objective and KRA
        [HttpGet]
        [Route("GetManagerAppraisalObjandKRAs")]
        public IActionResult GetManagerAppraisalObjandKRAs(int appCycleId, int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetIndAppraisalObjandKRAs(appCycleId, employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetManagerAppraisalObjandKRAs");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new AppraisalObjectiveandKRAListView()
                });
            }
        }
        #endregion

        #region Update Self Appraisal Objective & KRA Manager Ratings
        [HttpPut]
        [Route("UpdateAppraisalManagerRating")]
        public IActionResult UpdateAppraisalManagerRating(ManagerRatingView managerRatingView)
        {
            try
            {
                int appraisalid = _appraisalServices.UpdateAppraisalManagerRating(managerRatingView).Result;
                if (appraisalid > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Update Appraisal Manager Rating saved successfully.",
                        Data = appraisalid
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/UpdateAppraisalManagerRating", JsonConvert.SerializeObject(managerRatingView));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Get Version Details By Id
        [HttpGet]
        [Route("GetVersionDetailsById")]
        public IActionResult GetVersionDetailsById(int VersionId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionDetailsById(VersionId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionDetailsById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new VersionDetailsView()
                });
            }
        }
        #endregion

        #region Get Version Benchmark Master Data
        [HttpGet]
        [Route("GetVersionBenchmarkMasterData")]
        public IActionResult GetVersionBenchmarkMasterData(int versionId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionBenchmarkMasterData(versionId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionBenchmarkMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new BenchmarkMasterDataView()
                });
            }
        }
        #endregion
        
        #region Get Version role grid data
        [HttpGet]
        [Route("GetVersionRolesGridviewData")]
        public IActionResult GetVersionRolesGridviewData(int versionId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionRolesGridviewData(versionId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionRolesGridviewData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<VersionRoleGridDetails>()
                });
            }
        }
        #endregion
        
        #region Get Version KRA master data
        [HttpGet]
        [Route("GetVersionKRAMasterData")]
        public IActionResult GetVersionKRAMasterData(int versionId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionKRAMasterData(versionId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionKRAMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new VersionKRAMasterdata()
                });
            }
        }
        #endregion
        
        #region Get Version benchmark objective KRA
        [HttpGet]
        [Route("GetVersionBenchmarkObjectiveKRA")]
        public IActionResult GetVersionBenchmarkObjectiveKRA(int versionId, int departmentId, int roleId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionBenchmarkObjectiveKRA(versionId, departmentId, roleId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionBenchmarkObjectiveKRA");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<VersionBenchObjKRAView>()
                });
            }
        }
        #endregion
        
        #region Get Version benchmark Gridview Data
        [HttpGet]
        [Route("GetVersionBenchmarkGridData")]
        public IActionResult GetVersionBenchmarkGridData(int versionId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionBenchmarkGridData(versionId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionBenchmarkGridData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<VersionKRABenchmarkGridDetails>()
                });
            }
        }
        #endregion

        #region Get Department Wise Appraisal Status Report
        [HttpGet]
        [Route("GetDepartmentWiseAppraisalDetails")]
        public IActionResult getDepartmentWiseAppraisalDetails(int DepartmentID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.getDepartmentWiseAppraisalDetails(DepartmentID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/getDepartmentWiseAppraisalDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new AppraisalStatusReport()
                });
            }
        }
        #endregion

        #region Get Appraisal Status Report Count
        [HttpGet]
        [Route("GetAppraisalStatusReportCount")]
        public IActionResult GetAppraisalStatusReportCount()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.getAppraisalStatusReportCount()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAppraisalStatusReportCount");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new AppraisalStatusReportCount()
                });
            }
        }
        #endregion
        
        #region Get Employee Appraisal Details by Managerid
        [HttpPost]
        [Route("GetEmployeeAppraisalListByEmployeeId")]
        public IActionResult GetEmployeeAppraisalListByEmployeeId(EmployeeListAndDepartment empListDepartment)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetEmployeeAppraisalListByEmployeeId(empListDepartment)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeAppraisalListByManagerId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeAppraisalListView>()
                });
            }
        }
        #endregion

        #region  delete benchmark KRA group
        [HttpGet]
        [Route("deleteBenchmarkKRAGroup")]
        public IActionResult deleteBenchmarkKRAGroup(int groupId)
        {
            try
            {
                bool isSuccess = _appraisalServices.deleteBenchmarkKRAGroup(groupId).Result;
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = isSuccess
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/deleteBenchmarkKRAGroup");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region  Get version KRA benchmark range
        [HttpGet]
        [Route("GetVersionKRABenchmarkRange")]
        public IActionResult GetVersionKRABenchmarkRange(int versionId, int departmentId, int roleId, int objectiveId, int kraId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetVersionKRABenchmarkRange(versionId, departmentId, roleId, objectiveId, kraId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetVersionKRABenchmarkRange");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Get KRA Comments By Id
        [HttpGet]
        [Route("GetKRACommentsById")]
        public IActionResult GetKRACommentsById(int appcycleId, int employeeId, int ObjId, int KraId)
        {
            try
            {
                //List<EmployeeKRCommentView> individualComments= _appraisalServices.GetKRACommentsById(appcycleId, employeeId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _appraisalServices.GetKRACommentsById(appcycleId, employeeId, ObjId, KraId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetKRACommentsById",  "appcycleId-" + Convert.ToString(appcycleId) + "employeeId-" + Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<KraComments>()
                });
            }
        }
        #endregion

        #region Get Individual Appraisal DropdownList
        [HttpGet]
        [Route("GetIndividualAppraisalDropdownList")]
        public IActionResult GetIndividualAppraisalDropdownList(int employeeID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetIndividualAppraisalDropdownList(employeeID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetIndividualAppraisalDropdownList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AppraisalCycleMasterData>()
                });
            }
        }
        #endregion

        #region Get Individual Appraisal Details By AppCycleId
        [HttpGet]
        [Route("GetIndividualAppraisalDetailsByAppCycleId")]
        public IActionResult GetIndividualAppraisalDetailsByAppCycleId(int appcycleId, int departmentId, int roleId, int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetIndividualAppraisalDetailsByAppCycleId(appcycleId, departmentId, roleId, employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetIndividualAppraisalDetailsByAppCycleId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new IndividualAppraisalView()
                });
            }
        }
        #endregion

        #region Add Individual Appraisal Documents
        [HttpPost]
        [Route("AddIndividualAppraisalDocuments")]
        public IActionResult AddIndividualAppraisalDocuments(EmployeeKeyResultAttachmentsView Documents)
        {
            try
            {
                if (_appraisalServices.AddIndividualAppraisalDocuments(Documents).Result)
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
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddIndividualAppraisalDocuments", JsonConvert.SerializeObject(Documents));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion

        #region DeleteIndividualAppraisalDocument
        [HttpGet]
        [Route("DeleteIndividualAppraisalDocument")]
        public IActionResult DeleteIndividualAppraisalDocument(int DocID)
        {
            try
            {
                if (_appraisalServices.DeleteIndividualAppraisalDocument(DocID).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Get appraisal documents by id
        [HttpPost]
        [Route("GetDocumentByObjAndKraId")]
        public IActionResult GetDocumentByObjAndKraId(AppraisalSourceDocuments sourceDocuments)
        {
            List<EmployeeKeyResultAttachments> lstOfDocument = new List<EmployeeKeyResultAttachments>();
            try
            {
                lstOfDocument = _appraisalServices.GetDocumentByObjAndKraId(sourceDocuments);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = lstOfDocument
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetDocumentByObjAndKraId", JsonConvert.SerializeObject(sourceDocuments));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = lstOfDocument
                });
            }
        }
        #endregion
        
        #region Get app cycle employee list  
        [HttpGet]
        [Route("GetAllAppCycleEmployee")]
        public IActionResult GetAllAppCycleEmployee(int appCycleId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAllAppCycleEmployee(appCycleId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAllAppCycleEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeAppraisalMasterDetailView>()
                });
            }
        }
        #endregion

        #region Get Appraisal Objective Rating Details
        [HttpGet]
        [Route("GetAppraisalObjectiveRatingDetails")]
        public IActionResult GetAppraisalObjectiveRatingDetails(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _appraisalServices.GetAppraisalObjectiveRatingDetails(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAppraisalObjectiveRatingDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new AppraisalReport()
                });
            }
        }
        #endregion
        
        #region Get Appraisal Milestone Details
        [HttpGet]
        [Route("GetAppraisalMilestonedetails")]
        public IActionResult GetAppraisalMilestonedetails(int appCycleId=0)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAppraisalMilestonedetails(appCycleId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAppraisalMilestonedetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AppraisalMilestonedetails>()
                });
            }
        }
        #endregion
        
        #region Delete app cycle employee
        [HttpGet]
        [Route("DeleteAppCycleEmployee")]
        public IActionResult DeleteAppCycleEmployee(int appCycleId, int employeeId)
        {
            try
            {
                var isSuccess = _appraisalServices.DeleteAppCycleEmployee(appCycleId, employeeId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = isSuccess?.Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/DeleteAppCycleEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion
        
        #region Add app cycle employee
        [HttpPost]
        [Route("AddAppCycleEmployee")]
        public IActionResult AddAppCycleEmployee(List<EmployeeAppraisalMaster> appEmployee)
        {
            try
            {
                var isSuccess = _appraisalServices.AddAppCycleEmployee(appEmployee);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = isSuccess?.Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddAppCycleEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion
        
        #region Get Appraisal Comments By Id
        [HttpGet]
        [Route("GetAppraisalCommentsById")]
        public IActionResult GetAppraisalCommentsById(int appcycleId, int employeeId)
        {
            try
            {
                //List<IndividualAppraisalCommentsView> individualComments= _appraisalServices.GetAppraisalCommentsById(appcycleId, employeeId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _appraisalServices.GetAppraisalCommentsById(appcycleId, employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAppraisalCommentsById", "appcycleId-" + Convert.ToString(appcycleId) + "employeeId-" + Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<IndividualAppraisalCommentsView>()
                });
            }
        }
        #endregion
        
        #region Get App cycle detail by Id
        [HttpGet]
        [Route("GetAppCycleDetailById")]
        public IActionResult GetAppCycleDetailById(int appCycleId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAppCycleDetailById(appCycleId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAppCycleDetailById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new AppraisalMaster()
                });
            }
        }
        #endregion
        
        #region Add or Update Individual Appraisal Rating
        [HttpPost]
        [Route("AddorUpdateIndividualAppraisalRating")]
        public IActionResult AddorUpdateIndividualAppraisalRating(IndividualAppraisalAddView appraisalDetailsView)
        {
            try
            {
                var isSuccess = _appraisalServices.AddorUpdateIndividualAppraisalRating(appraisalDetailsView);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    
                    StatusText = (appraisalDetailsView.IsSubmit == true ? "Self Appraisal submitted successfully." : "Self Appraisal saved successfully."),
                    //appraisalDetailsView.Status
                    Data = isSuccess?.Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateIndividualAppraisalRating", JsonConvert.SerializeObject(appraisalDetailsView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }

        }
        #endregion
        
        #region Delete Objective KRA Mapping
        [HttpGet]
        [Route("DeleteObjectiveKRAMapping")]
        public IActionResult DeleteObjectiveKRAMapping(int versionId, int departmentId, int roleId, int objectiveId, int kRAId)
        {
            try
            {
                string status = _appraisalServices.DeleteObjectiveKRAMapping(versionId, departmentId, roleId, objectiveId, kRAId).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Objective KRA mapping deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Delete Role Objective Mapping
        [HttpGet]
        [Route("DeleteRoleObjectiveMapping")]
        public IActionResult DeleteRoleObjectiveMapping(int versionId, int departmentId, int roleId, int objectiveId)
        {
            try
            {
                string status = _appraisalServices.DeleteRoleObjectiveMapping(versionId, departmentId, roleId, objectiveId).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Role objective mapping deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Delete Department Role Mapping
        [HttpGet]
        [Route("DeleteDepartmentRoleMapping")]
        public IActionResult DeleteDepartmentRoleMapping(int versionId, int departmentId, int roleId)
        {
            try
            {
                string status = _appraisalServices.DeleteDepartmentRoleMapping(versionId, departmentId, roleId).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Department role mapping deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Delete Version Department
        [HttpGet]
        [Route("DeleteVersionDepartmentMapping")]
        public IActionResult DeleteVersionDepartmentMapping(int versionId, int departmentId)
        {
            try
            {
                string status = _appraisalServices.DeleteVersionDepartmentMapping(versionId, departmentId).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Version department mapping deleted successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Get Employee Appraisal Master By ManagerID
        [HttpGet]
        [Route("GetEmployeeAppraisalMasterByManagerID")]
        public IActionResult GetEmployeeAppraisalMasterByManagerID(int appcycleID, int managerID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetEmployeeAppraisalMasterByManagerID(appcycleID, managerID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeAppraisalMasterByManagerID");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeAppraisalByManager>()
                });
            }
        }
        #endregion
        
        #region Get All App Constants
        [HttpGet]
        [Route("GetAppraisalDurationList")]
        public IActionResult GetAppraisalDurationList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetAppraisalDurationList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAppraisalDurationList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<KeyWithValue>()
                });
            }
        }
        #endregion
        
        #region Add or update Appraisal BU Head Comments
        [HttpPost]
        [Route("AddOrUpdateBUHeadComments")]
        public IActionResult AddOrUpdateBUHeadComments(AppBUHeadCommentsView appBUHeadCommentsView)
        {
            string StatusText = "Unexpected error occurred. Try again.";
            try
            {
                int keyResultId = _appraisalServices.AddOrUpdateBUHeadComments(appBUHeadCommentsView).Result;
                //var employeeList = _appraisalServices.GetEmployeeByStatus(appBUHeadCommentsView.AppCycle_Id , appBUHeadCommentsView.Department_Id);
                if (keyResultId > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Comments Added successfully.",
                        data = new List<EmployeeAppraisalByManager>()
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Unexpected error occurred. Try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateBUHeadComments", JsonConvert.SerializeObject(appBUHeadCommentsView));
                StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }
        #endregion
        
        #region Get Appraisal Bu Head Comments
        [HttpGet]
        [Route("GetAppraisalBUHeadComments")]
        public IActionResult GetAppraisalBUHeadComments(int departmentId)
        {
            try
            {               
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _appraisalServices.GetAppraisalBUHeadComments(departmentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetAppraisalBUHeadComments");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<AppraisalBUHeadCommentsView>()
                });
            }
        }
        #endregion
        
        #region Get Employee Appraisal Status ById
        [HttpGet]
        [Route("GetEmployeeAppraisalStatusById")]
        public IActionResult GetEmployeeAppraisalStatusById(int appcycleId, int departmentId, int roleId, int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _appraisalServices.GetEmployeeAppraisalStatusById(appcycleId, departmentId, roleId, employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeAppraisalStatusById", "appcycleId-" + appcycleId.ToString(), "employeeId-" + employeeId.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new AppraisalEmployeeStatusView()
                });
            }
        }
        #endregion
        
        #region Copy and Add Version Details
        [HttpPost]
        [Route("CopyandAddVersionDetails")]
        public IActionResult CopyandAddVersionDetails(CopyVersion copyVersion)
        {
            string StatusText = "Unexpected error occurred. Try again.";
            try
            {

                int version_Id = _appraisalServices.CopyandAddVersionDetails(copyVersion.versionId, copyVersion.createdBy);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Version Copied and Added successfully.",
                    data = version_Id
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/CopyandAddVersionDetails", JsonConvert.SerializeObject(copyVersion));
                StatusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText,
                Data = 0
            });
        }
        #endregion

        #region Approve or Reject By Manager 
        [HttpPost]
        [Route("ApproveOrRejectByManager")]
        public IActionResult ApproveOrRejectByManager(AddApproveandRejectByManagerView DetailsView)
        {
            try
            {
                var isSuccess = _appraisalServices.ApproveOrRejectByManager(DetailsView);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = (DetailsView.IsSubmit == true ? "Submitted Successfully" : "Saved Successfully."),
                    Data = isSuccess?.Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/ApproveOrRejectByManager", JsonConvert.SerializeObject(DetailsView));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }

        }
        #endregion

        #region Get Manager Details
        [HttpGet]
        [Route("GetManagerDetails")]
        public IActionResult GetManagerDetails(int appcycleID, int managerID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetManagerDetails(appcycleID, managerID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetManagerDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<ManagerDetail>()
                });
            }
        }
        #endregion
        
        #region Add or Update AppraisalCycle using excel
        [HttpPost]
        [Route("AddorUpdateAppraisalCycleByExcel")]
        public IActionResult AddorUpdateAppraisalCycleByExcel(ImportExcelView import)
        {
            try
            {
                string status = _appraisalServices.AddorUpdateAppraisalCycleByExcel(import).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Appraisal Cycle Imported Successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateAppraisalCycleByExcel");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Individual Appraisal Document Download By ID
        [HttpGet]
        [Route("IndividualAppraisalDocumentDownloadByID")]
        public IActionResult IndividualAppraisalDocumentDownloadByID(int documentID)
        {
            EmployeeKeyResultAttachments Documents = new EmployeeKeyResultAttachments();
            try
            {
                Documents = _appraisalServices.IndividualAppraisalDocumentDownloadByID(documentID);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = Documents
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/IndividualAppraisalDocumentDownloadByID", Convert.ToString(documentID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = Documents
                });
            }
        }
        #endregion
        
        #region Get Employee Appraisal Details by Manager
        [HttpPost]
        [Route("GetEmployeeAppraisalListByManager")]
        public IActionResult GetEmployeeAppraisalListByManager(List<int> employeeids)
        {
            List<EmployeeAppraisalMasterDetailView> EmployeeAppraisalList = new List<EmployeeAppraisalMasterDetailView>();
            try
            {
                EmployeeAppraisalList = _appraisalServices.GetEmployeeAppraisalListByManager(employeeids);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "SUCCESS",
                    Data = EmployeeAppraisalList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeAppraisalListByManager", Convert.ToString(employeeids));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeAppraisalMasterDetailView()
                });
            }
        }
        #endregion

        #region BU Head Revert By Employee
        [HttpPost]
        [Route("BUHeadRevertByEmployee")]
        public IActionResult BUHeadRevertByEmployee(BuHeadRevertByEmployeeView buHeadRevertByEmployee)
        {
        
            try
            {
                string isSuccess = _appraisalServices.BUHeadRevertByEmployee(buHeadRevertByEmployee)?.Result;
                if(isSuccess== "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Revert Successfully.",
                        Data = isSuccess
                    });
                }                
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/BUHeadRevertByEmployee", JsonConvert.SerializeObject(buHeadRevertByEmployee));
                
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "",
                Data = false
            });
        }
        #endregion
        
        #region Add or Update entity Master using excel
        [HttpPost]
        [Route("AddorUpdateEntityByExcel")]
        public IActionResult AddorUpdateEntityByExcel(ImportExcelView import)
        {
            try
            {
                string status = _appraisalServices.AddorUpdateEntityByExcel(import).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Entity Imported Successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateEntityByExcel");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Add or Update objective Master using excel
        [HttpPost]
        [Route("AddorUpdateObjectiveByExcel")]
        public IActionResult AddorUpdateObjectiveByExcel(ImportExcelView import)
        {
            try
            {
                string status = _appraisalServices.AddorUpdateObjectiveByExcel(import).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Objective Imported Successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateObjectiveByExcel");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Add or Update KeyResult Using Excel
        [HttpPost]
        [Route("AddorUpdateKeyResultByExcel")]
        public IActionResult AddorUpdateKeyResultByExcel(ImportExcelView import)
        {
            try
            {
                string status = _appraisalServices.AddorUpdateKeyResultByExcel(import).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "KRA Imported Successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateKeyResultByExcel");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Add or Update Version Using Excel
        [HttpPost]
        [Route("AddorUpdateVersionByExcel")]
        public IActionResult AddorUpdateVersionByExcel(ImportExcelView import)
        {
            try
            {                
                string status = _appraisalServices.AddorUpdateVersionByExcel(import).Result;
                if (status == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Version Imported Successfully.",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = status,
                        Data = false
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddorUpdateVersionByExcel");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region GetIndividualAppraisalRating Details
        [HttpGet]
        [Route("GetEmployeeAppraisalRating")]
        public IActionResult GetEmployeeAppraisalRating(int employeeID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetEmployeeAppraisalRating(employeeID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeAppraisalRating");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new IndividualEmployeeAppraisalRating()
                });
            }
        }
        #endregion
        
        #region Add Self Appraisal Comment
        [HttpPost]
        [Route("AddSelfAppraisalBUHeadComment")]
        public IActionResult AddSelfAppraisalBUHeadComment(EmployeeAppraisalBUHeadComment employeeAppraisalComment)
        {
            try
            {
                int appraisalid = _appraisalServices.AddSelfAppraisalBUHeadComment(employeeAppraisalComment).Result;
                if (appraisalid > 0)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Appraisal Comment saved successfully.",
                        Data = appraisalid
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "please enter comments and try again.",
                        Data = 0
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/AddOrUpdateSelfAppraisalComment", JsonConvert.SerializeObject(employeeAppraisalComment));
                return Ok(new { StatusCode = "FAILURE", StatusText = ex.ToString(), Data = 0 });
            }
        }
        #endregion

        #region Get Employee Appraisal List By EmployeeId For Review
        [HttpPost]
        [Route("GetEmployeeAppraisalListByEmployeeIdForReview")]
        public IActionResult GetEmployeeAppraisalListByEmployeeIdForReview(EmployeeListAndDepartment empListDepartment)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _appraisalServices.GetEmployeeAppraisalListByEmployeeIdForReview(empListDepartment)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeAppraisalListByManagerId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeAppraisalMasterDetailView>()
                });
            }
        }
        #endregion

        #region Get Employee Appraisal Details by Managerid
        [HttpPost]
        [Route("GetEmployeeWorkDayDetailByEmployeeId")]
        public IActionResult GetEmployeeWorkDayDetailByEmployeeId(WorkdayFilterView workdayFilterView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _workDayDetailServices.GetWorkDayDetailList(workdayFilterView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Appraisal", "Appraisal/GetEmployeeWorkDayDetailByEmployeeId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<WorkdayListView>()
                });
            }
        }
        #endregion

        #region Workday Detail Count
        [HttpPost]
        [Route("WorkdayDetailCount")]
        public IActionResult WorkdayDetailCount(List<int> employeeIdList)
        {

            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _workDayDetailServices.WorkdayDetailCount(employeeIdList)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/LeaveRequestCount", Newtonsoft.Json.JsonConvert.SerializeObject(employeeIdList));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = employeeIdList
                });
            }
        }
        #endregion
    }
}