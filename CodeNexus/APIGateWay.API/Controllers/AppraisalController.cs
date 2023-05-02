using APIGateWay.API.Common;
using APIGateWay.API.Model;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharedLibraries;
using SharedLibraries.Common;
using SharedLibraries.Models.Appraisal;
using SharedLibraries.Models.Employee;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;


namespace APIGateWay.API.Controllers
{

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "NexusAPI")]
    [ApiController]
    public class AppraisalController : ControllerBase
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IConfiguration _configuration;
        private readonly HTTPClient _client;
        private readonly string _appraisalBaseURL = string.Empty;
        private readonly string _projectBaseURL = string.Empty;
        private readonly string _employeeBaseURL = string.Empty;
        private readonly string _notificationBaseURL = string.Empty;
        private readonly string strErrorMsg = "Something went wrong, please try again later";

        #region Constructor
        public AppraisalController(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new HTTPClient();
            _appraisalBaseURL = _configuration.GetValue<string>("ApplicationURL:Appraisal:BaseURL");
            _projectBaseURL = _configuration.GetValue<string>("ApplicationURL:Projects:BaseURL");
            _employeeBaseURL = _configuration.GetValue<string>("ApplicationURL:Employees:BaseURL");
            _notificationBaseURL = _configuration.GetValue<string>("ApplicationURL:Notifications");

        }
        #endregion

        #region Add or update entity        
        [HttpPost]

        [Route("AddOrUpdateEntity")]
        public async Task<IActionResult> AddOrUpdateEntity(EntityView entityView)
        {
            int EntityId = 0; string statusCode = "";
            try
            {
                var result = await _client.PostAsJsonAsync(entityView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateEntity"));
                EntityId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && EntityId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Entity updated successfully",
                        EntityId
                    });
                }
                else
                {
                    if (statusCode == "")
                    {
                        return Ok(new
                        {
                            result?.StatusCode,
                            result?.StatusText,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateEntity", JsonConvert.SerializeObject(entityView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                EntityId
            });
        }
        #endregion

        #region Delete Entity    
        [HttpGet]
        [Route("DeleteEntity")]
        public async Task<IActionResult> DeleteEntity(int entityid)
        {
            try
            {
                var Result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteEntity") + entityid);
                if (Result != null)
                {
                    return Ok(new
                    {
                        StatusCode = Result?.StatusCode,
                        StatusText = Result?.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteEntity", JsonConvert.SerializeObject(entityid));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get Entity Details List
        [HttpGet]
        [Route("GetEntityDetails")]
        public async Task<IActionResult> GetEntityDetails()
        {
            List<EntityMaster> entityList = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEntityDetails"));
                entityList = JsonConvert.DeserializeObject<List<EntityMaster>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = entityList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetEntityDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = entityList
            });
        }
        #endregion

        #region Add or Update AppraisalCycle
        [HttpPost]
        [Route("AddorUpdateAppraisalCycle")]
        public async Task<IActionResult> AddorUpdateAppraisalCycle(AppraisalCycleView appraisalCycleView)
        {
            int AppCycleId = 0; string statusCode = "";
            try
            {
                var result = await _client.PostAsJsonAsync(appraisalCycleView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddorUpdateAppraisalCycle"));
                AppCycleId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && AppCycleId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "AppraisalCycle updated successfully",
                        AppCycleId
                    });

                }
                else
                {
                    if (statusCode == "")
                    {
                        return Ok(new
                        {
                            result?.StatusCode,
                            result?.StatusText,

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateAppraisalCycle", JsonConvert.SerializeObject(appraisalCycleView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                AppCycleId
            });
        }
        #endregion

        #region Get All AppraisalCycle
        [HttpGet]
        [Route("GetAllAppraisalCycle")]
        public async Task<IActionResult> GetAllAppraisalCycle()
        {
            List<AppraisalMasterView> appraisalMaster = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAllAppraisalCycle"));
                appraisalMaster = JsonConvert.DeserializeObject<List<AppraisalMasterView>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = appraisalMaster
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetAllAppraisalCycle");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = appraisalMaster
            });
        }
        #endregion

        #region Delete AppraisalCycle        
        [HttpGet]
        [Route("DeleteAppraisalCycle")]
        public async Task<IActionResult> DeleteAppraisalCycle(int appCycleId)
        {
            try
            {
                var Result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteAppraisalCycle") + appCycleId);
                if (Result != null)
                {
                    return Ok(new
                    {
                        StatusCode = Result?.StatusCode,
                        StatusText = Result?.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteAppraisalCycle", JsonConvert.SerializeObject(appCycleId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Update AppraisalCycle Status
        [HttpPost]
        [Route("UpdateAppraisalCycleStatus")]
        public async Task<IActionResult> UpdateAppraisalCycleStatus(UpdateAppraisalStatusView appraisalCycleView)
        {
            int AppCycleId = 0;
            try
            {
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeesDetails"));
                appraisalCycleView.EmployeeDetails = JsonConvert.DeserializeObject<List<Employees>>(JsonConvert.SerializeObject(employeeResult?.Data));
                var result = await _client.PostAsJsonAsync(appraisalCycleView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:UpdateAppraisalCycleStatus"));
                AppCycleId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && AppCycleId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "AppraisalCycle Status updated successfully",
                        AppCycleId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetAppraisalCycleStatus", JsonConvert.SerializeObject(appraisalCycleView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                AppCycleId
            });
        }
        #endregion

        #region Add or update Objective        
        [HttpPost]
        [Route("AddOrUpdateObjective")]
        public async Task<IActionResult> AddOrUpdateObjective(ObjectiveView objectiveView)
        {
            int objectiveId = 0; string statusCode = "";
            try
            {
                var result = await _client.PostAsJsonAsync(objectiveView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateObjective"));
                objectiveId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && objectiveId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Objective updated successfully",
                        objectiveId
                    });
                }
                else
                {
                    if (statusCode == "")
                    {
                        return Ok(new
                        {
                            result?.StatusCode,
                            result?.StatusText,

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateObjective", JsonConvert.SerializeObject(objectiveView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                objectiveId
            });
        }
        #endregion

        #region Get Objective Details List
        [HttpGet]
        [Route("GetObjectiveDetails")]
        public async Task<IActionResult> GetObjectiveDetails()
        {
            List<ObjectiveMaster> objectiveList = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetObjectiveDetails"));
                objectiveList = JsonConvert.DeserializeObject<List<ObjectiveMaster>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = objectiveList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetObjectiveDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = objectiveList
            });
        }
        #endregion

        #region Delete Objective        
        [HttpGet]
        [Route("DeleteObjective")]
        public async Task<IActionResult> DeleteObjective(int objectiveId)
        {
            try
            {
                var Result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteObjective") + objectiveId);
                if (Result != null)
                {
                    return Ok(new
                    {
                        StatusCode = Result?.StatusCode,
                        StatusText = Result?.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteObjective", JsonConvert.SerializeObject(objectiveId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Add or update KeyResultMaster        
        [HttpPost]
        [Route("AddOrUpdateKeyResultMaster")]
        public async Task<IActionResult> AddOrUpdateKeyResultMaster(KeyResultMasterView keyResultMasterView)
        {
            int KeyResultId = 0; string statusCode = "";
            try
            {
                var result = await _client.PostAsJsonAsync(keyResultMasterView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateKeyResultMaster"));
                KeyResultId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && KeyResultId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "KRA updated successfully",
                        KeyResultId
                    });
                }
                else
                {
                    if (statusCode == "")
                    {
                        return Ok(new
                        {
                            result?.StatusCode,
                            result?.StatusText,

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateKeyResultMaster", JsonConvert.SerializeObject(keyResultMasterView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                KeyResultId
            });
        }
        #endregion

        #region Get KeyResult Details List
        [HttpGet]
        [Route("GetKeyResultDetails")]
        public async Task<IActionResult> GetKeyResultDetails()
        {
            List<KeyResultMaster> keyResultList = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetKeyResultDetails"));
                keyResultList = JsonConvert.DeserializeObject<List<KeyResultMaster>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = keyResultList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetKeyResultDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = keyResultList
            });
        }
        #endregion

        #region Delete Key Result        
        [HttpGet]
        [Route("DeleteKeyResultMaster")]
        public async Task<IActionResult> DeleteKeyResultMaster(int keyResultId)
        {
            try
            {
                var Result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteKeyResultMaster") + keyResultId);
                if (Result != null)
                {
                    return Ok(new
                    {
                        StatusCode = Result?.StatusCode,
                        StatusText = Result?.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteKeyResultMaster", JsonConvert.SerializeObject(keyResultId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Add or update version        
        [HttpPost]
        [Route("AddOrUpdateVersion")]
        public async Task<IActionResult> AddOrUpdateVersion(VersionView versionView)
        {
            int VersionId = 0; string statusCode = "";
            try
            {
                var result = await _client.PostAsJsonAsync(versionView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateVersion"));
                VersionId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && VersionId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Version updated successfully",
                        VersionId
                    });
                }
                else
                {
                    if (statusCode == "")
                    {
                        return Ok(new
                        {
                            result?.StatusCode,
                            result?.StatusText,

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateVersion", JsonConvert.SerializeObject(versionView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                VersionId
            });
        }
        #endregion

        #region Get Version Details List
        [HttpGet]
        [Route("GetAllVersionDetails")]
        public async Task<IActionResult> GetAllVersionDetails()
        {
            List<VersionMaster> versionList = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAllVersionDetails"));
                versionList = JsonConvert.DeserializeObject<List<VersionMaster>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = versionList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetAllVersionDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = versionList
            });
        }
        #endregion

        #region Delete Version        
        [HttpGet]
        [Route("DeleteVersion")]
        public async Task<IActionResult> DeleteVersion(int versionId)
        {
            try
            {
                var Result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteVersion") + versionId);
                if (Result != null)
                {
                    return Ok(new
                    {
                        StatusCode = Result?.StatusCode,
                        StatusText = Result?.StatusText
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteVersion", JsonConvert.SerializeObject(versionId));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Get Appraisal Master Data
        [HttpGet]
        [Route("GetAppraisalMasterData")]
        public async Task<IActionResult> GetAppraisalMasterData()
        {
            AppraisalMasterData appraisalMasterData = new();
            try
            {
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(result?.Data));
                if (appraisalMasterData != null)
                {
                    var resultVersions = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAllVersionDetails"));
                    appraisalMasterData.VersionMaster = JsonConvert.DeserializeObject<List<VersionMaster>>(JsonConvert.SerializeObject(resultVersions?.Data));
                    var resultEntity = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEntityDetails"));
                    appraisalMasterData.EntityMaster = JsonConvert.DeserializeObject<List<EntityMaster>>(JsonConvert.SerializeObject(resultEntity?.Data));
                    var resultDuration = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAppraisalDurationList"));
                    appraisalMasterData.Durations = JsonConvert.DeserializeObject<List<KeyWithValue>>(JsonConvert.SerializeObject(resultDuration?.Data));
                }
                if (appraisalMasterData != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = appraisalMasterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Employees/GetAppraisalMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = appraisalMasterData
            });
        }
        #endregion

        #region Add or update Department Role        
        [HttpPost]
        [Route("AddOrUpdateVersionDepartmentRole")]
        public async Task<IActionResult> AddOrUpdateVersionDepartmentRole(List<DepartmentRoleView> departmentRoleViews)
        {
            int VersionId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(departmentRoleViews, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateVersionDepartmentRole"));
                VersionId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && VersionId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Version Department Role Added successfully",
                        VersionId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateVersionDepartmentRole", JsonConvert.SerializeObject(departmentRoleViews));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                VersionId
            });
        }
        #endregion

        #region Get Version Role Master Data
        [HttpGet]
        [Route("GetVersionRoleMasterData")]
        public async Task<IActionResult> GetVersionRoleMasterData()
        {
            AppraisalMasterData appraisalMasterData = new();
            VersionRoleData versionRoleMasterData = new();
            try
            {
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(result?.Data));
                if (appraisalMasterData != null)
                {
                    versionRoleMasterData.Departments = (from dept in appraisalMasterData.Department
                                                         select new DepartmentList
                                                         {
                                                             DepartmentId = dept.DepartmentId,
                                                             DepartmentName = dept.DepartmentName
                                                         }).Distinct().ToList();
                    versionRoleMasterData.Roles = (from dept in appraisalMasterData.Roles
                                                   select new RolesList
                                                   {
                                                       RoleId = dept.RoleId,
                                                       RoleName = dept.RoleName
                                                   }).Distinct().ToList();
                }
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = versionRoleMasterData
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionRoleMasterData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = versionRoleMasterData
            });
        }
        #endregion

        #region Get Version KRA Master Data
        [HttpGet]
        [Route("GetVersionKRAMasterData")]
        public async Task<IActionResult> GetVersionKRAMasterData(int VersionId)
        {
            string statusText = "";
            AppraisalMasterData appraisalMasterData = new();
            VersionKRAMasterdata versionMappingList = new();
            List<RolesList> RoleList = new();
            List<ObjectiveData> ObjectivesList = new();
            List<KRADetails> KeyResultMaster = new();
            try
            {
                var mappingResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionKRAMasterData") + VersionId);
                versionMappingList = JsonConvert.DeserializeObject<VersionKRAMasterdata>(JsonConvert.SerializeObject(mappingResult?.Data));
                if (versionMappingList?.VersionDepartmentRoleMapping?.Count > 0)
                {
                    var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                    appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(result?.Data));
                    if ((appraisalMasterData?.Department?.Count > 0 || appraisalMasterData?.Roles?.Count > 0))
                    {
                        foreach (var department in versionMappingList?.VersionDepartmentRoleMapping)
                        {
                            department.DepartmentName = appraisalMasterData?.Department?.Where(x => x.DepartmentId == department.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                            foreach (var item in department?.Roles)
                            {
                                item.RoleName = appraisalMasterData?.Roles?.Where(x => x.RoleId == item.RoleId).Select(x => x.RoleName).FirstOrDefault();
                            }
                        }
                    }
                }
                ObjectivesList = (from obj in versionMappingList.ObjectiveMaster
                                  select new ObjectiveData
                                  {
                                      ObjectiveId = obj.OBJECTIVE_ID,
                                      ObjectiveName = obj.OBJECTIVE_NAME
                                  }).Distinct().ToList();
                KeyResultMaster = (from kr in versionMappingList.KeyResultMasters
                                   select new KRADetails
                                   {
                                       KRAId = kr.KEY_RESULT_ID,
                                       KRAName = kr.KEY_RESULT_NAME
                                   }).Distinct().ToList();

                VersionKRAData versionKRAMasterData = new()
                {
                    Departments = versionMappingList.VersionDepartmentRoleMapping,
                    Objectives = ObjectivesList,
                    KRAs = KeyResultMaster
                };
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = statusText,
                    Data = versionKRAMasterData
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionKRAMasterData");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = new VersionKRAData()
            });
        }
        #endregion

        #region Add or update Version Objective KRA      
        [HttpPost]
        [Route("AddOrUpdateVersionObjectiveKRA")]
        public async Task<IActionResult> AddOrUpdateVersionObjectiveKRA(List<VersionKeyResultsView> versionKeyResultsViews)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(versionKeyResultsViews, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateVersionObjectiveKRA"));
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && isSuccess)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Version Objective KRAs Added successfully",
                        Data = isSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateVersionObjectiveKRA", JsonConvert.SerializeObject(versionKeyResultsViews));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Get Version Roles Gridview Data
        [HttpGet]
        [Route("GetVersionRolesGridviewData")]
        public async Task<IActionResult> GetVersionRolesGridviewData(int versionId)
        {
            AppraisalMasterData appraisalMasterData = new();
            List<VersionRoleGridDetails> VersionRoleGridResult = new();
            try
            {
                var mappingResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionRolesGridviewData") + versionId);
                VersionRoleGridResult = JsonConvert.DeserializeObject<List<VersionRoleGridDetails>>(JsonConvert.SerializeObject(mappingResult.Data));
                if (VersionRoleGridResult?.Count > 0)
                {
                    var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                    appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(result.Data));
                    if (appraisalMasterData?.Department?.Count > 0 || appraisalMasterData?.Roles?.Count > 0)
                    {
                        foreach (var item in VersionRoleGridResult)
                        {
                            item.DepartmentName = appraisalMasterData?.Department?.Where(x => x.DepartmentId == item.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                            item.RoleName = appraisalMasterData?.Roles?.Where(x => x.RoleId == item.RoleId).Select(x => x.RoleName).FirstOrDefault();
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = "Success",
                        Data = VersionRoleGridResult
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionRolesGridviewData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = VersionRoleGridResult
            });
        }
        #endregion

        #region Get Version KRA Gridview Data
        [HttpGet]
        [Route("GetVersionKRAGridviewData")]
        public async Task<IActionResult> GetVersionKRAGridviewData(int versionId)
        {
            AppraisalMasterData appraisalMasterData = new();
            List<VersionKRABenchmarkGridDetails> VersionKRAGridResult = new();
            try
            {
                var mappingResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionKRAGridviewData") + versionId);
                VersionKRAGridResult = JsonConvert.DeserializeObject<List<VersionKRABenchmarkGridDetails>>(JsonConvert.SerializeObject(mappingResult?.Data));
                if (VersionKRAGridResult?.Count > 0)
                {
                    var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                    appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(result?.Data));
                    if (appraisalMasterData?.Department?.Count > 0 || appraisalMasterData?.Roles?.Count > 0)
                    {
                        foreach (var item in VersionKRAGridResult)
                        {
                            item.DepartmentName = appraisalMasterData?.Department?.Where(x => x.DepartmentId == item.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                            item.RoleName = appraisalMasterData?.Roles?.Where(x => x.RoleId == item.RoleId).Select(x => x.RoleName).FirstOrDefault();
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = "Success",
                        Data = VersionKRAGridResult
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionKRAGridviewData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = VersionKRAGridResult
            });
        }
        #endregion

        #region Get Version Benchmark Objective & KRAs
        [HttpGet]
        [Route("GetVersionBenchmarkObjectiveKRA")]
        public async Task<IActionResult> GetVersionBenchmarkObjectiveKRA(int versionId, int departmentId, int roleId)
        {
            List<VersionBenchObjKRAView> VersionBenchObjKRAList = new();
            try
            {
                var mappingResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionBenchmarkObjectiveKRA") + versionId + "&departmentId=" + departmentId + "&roleId=" + roleId);
                VersionBenchObjKRAList = JsonConvert.DeserializeObject<List<VersionBenchObjKRAView>>(JsonConvert.SerializeObject(mappingResult?.Data));
                return Ok(new
                {
                    StatusCode = "Success",
                    Data = VersionBenchObjKRAList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionBenchmarkObjectiveKRA");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = VersionBenchObjKRAList
            });
        }
        #endregion

        #region Add or update Version Benchmark Objective KRA      
        [HttpPost]
        [Route("AddOrUpdateVersionBenchmarkKRA")]
        public async Task<IActionResult> AddOrUpdateVersionBenchmarkKRA(AddVersionBenchmarkView versionBenchmarkInsertViews)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(versionBenchmarkInsertViews, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateVersionBenchmarkKRA"));
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (isSuccess)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Version benchmark KRAs updated successfully",
                        Data = isSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateVersionObjectiveKRA", JsonConvert.SerializeObject(versionBenchmarkInsertViews));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Get Individual Appraisal Objective and KRAs
        [HttpGet]
        [Route("GetIndAppraisalObjandKRAs")]
        public async Task<IActionResult> GetIndAppraisalObjandKRAs(int appCycleId, int employeeId)
        {
            AppraisalObjectiveandKRAListView EmployeeObjKRRating = new();
            VersionKRAMasterdata objKRAMaster = new();
            List<ObjectiveandKRAs> ObjectiveandKRAsList = new();
            try
            {
                var mappingResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetIndAppraisalObjandKRAs"));
                EmployeeObjKRRating = JsonConvert.DeserializeObject<AppraisalObjectiveandKRAListView>(JsonConvert.SerializeObject(mappingResult.Data));
                var objandKRAResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAllVersionDepartmentRoleMapping") + appCycleId);
                objKRAMaster = JsonConvert.DeserializeObject<VersionKRAMasterdata>(JsonConvert.SerializeObject(objandKRAResult.Data));
                if (EmployeeObjKRRating != null)
                {
                    var Objective = (from kra in EmployeeObjKRRating.employeeObjectiveRatingsList
                                     join Obj in objKRAMaster.ObjectiveMaster on kra.OBJECTIVE_ID equals Obj.OBJECTIVE_ID
                                     where kra.APP_CYCLE_ID == appCycleId && kra.EMPLOYEE_ID == employeeId
                                     select new
                                     {
                                         AppCycleId = kra.APP_CYCLE_ID,
                                         EmployeeId = kra.EMPLOYEE_ID,
                                         ObjectiveId = Obj.OBJECTIVE_ID,
                                         ObjectiveName = Obj.OBJECTIVE_NAME,
                                         ObjectivemaxRating = kra.OBJECTIVE_MAX_RATING,
                                         OBJECTIVE_RATING = kra.OBJECTIVE_RATING
                                     }).Distinct().ToList();
                    foreach (var obj in Objective)
                    {
                        List<AppraisalKRAView> AppraisalKRA = new List<AppraisalKRAView>();
                        AppraisalKRA = (from kras in EmployeeObjKRRating.employeeKeyResultRatingsList
                                        join kramaster in objKRAMaster.KeyResultMasters on kras.KEY_RESULT_ID equals kramaster.KEY_RESULT_ID
                                        where kras.APP_CYCLE_ID == appCycleId && kras.EMPLOYEE_ID == employeeId && kras.OBJECTIVE_ID == obj.ObjectiveId
                                        select new AppraisalKRAView
                                        {
                                            KeyResult_Id = kramaster.KEY_RESULT_ID,
                                            KeyResult_Name = kramaster.KEY_RESULT_NAME,
                                            Key_Result_Actual_Value = kras.KEY_RESULT_ACTUAL_VALUE,
                                            Key_Result_Max_Rating = kras.KEY_RESULT_MAX_RATING,
                                            Key_Result_Rating = kras.KEY_RESULT_RATING,
                                            Key_Result_Status = kras.KEY_RESULT_STATUS,
                                            IS_Addressed = kras.IS_ADDRESSED
                                        }).Distinct().ToList();
                        ObjectiveandKRAs objectiveandKRAs = new ObjectiveandKRAs()
                        {
                            AppCycleId = obj.AppCycleId,
                            EmployeeId = obj.EmployeeId,
                            ObjectiveId = obj.ObjectiveId,
                            ObjectiveName = obj.ObjectiveName,
                            AppraisalKRAView = AppraisalKRA
                        };
                        ObjectiveandKRAsList.Add(objectiveandKRAs);
                    }
                    return Ok(new
                    {
                        statusText = "Success",
                        Data = ObjectiveandKRAsList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetIndAppraisalObjandKRAs", " appCycleId- " + appCycleId.ToString() + " employeeId- " + employeeId.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = ObjectiveandKRAsList
            });
        }
        #endregion

        #region Get Individual Appraisal Objective and KRAs
        [HttpGet]
        [Route("GetIndividualAppraisalObjandKRAs")]
        public async Task<IActionResult> GetIndividualAppraisalObjandKRAs(int appCycleId, int employeeId)
        {
            AppraisalObjectiveandKRAListView AppraisalObjectiveandKRAList = new();
            try
            {
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetIndAppraisalObjandKRAs?appCycleId=" + appCycleId + "&employeeId=" + employeeId));
                AppraisalObjectiveandKRAList = JsonConvert.DeserializeObject<AppraisalObjectiveandKRAListView>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = AppraisalObjectiveandKRAList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetIndividualAppraisalObjandKRAs");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = AppraisalObjectiveandKRAList
            });
        }
        #endregion

        #region Add or update Self Appraisal Rating      
        [HttpPost]
        [Route("AddorUpdateSelfAppraisalRating")]
        public async Task<IActionResult> AddorUpdateSelfAppraisalRating(List<EmployeeKRRatingView> employeeKRRatingViews)
        {
            int KraId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(employeeKRRatingViews, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateSelfAppraisalRating"));
                KraId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && KraId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Self Ratings Updated successfully",
                        KraId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateSelfAppraisalRating", JsonConvert.SerializeObject(employeeKRRatingViews));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                KraId
            });
        }
        #endregion

        #region Add or update Self Appraisal Key Result Comment      
        [HttpPost]
        [Route("AddSelfAppraisalKRAComment")]
        public async Task<IActionResult> AddSelfAppraisalKRAComment(IndividualComments individualComments)
        {
            int KraId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(individualComments, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddSelfAppraisalKRAComment"));
                KraId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && KraId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "KRA Comment Updated successfully",
                        KraId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateSelfAppraisalKRComment", JsonConvert.SerializeObject(individualComments));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                KraId
            });
        }
        #endregion

        #region Add Self Appraisal Comment      
        [HttpPost]
        [Route("AddSelfAppraisalComment")]
        public async Task<IActionResult> AddSelfAppraisalComment(EmployeeAppraisalComment employeeAppraisalComment)
        {
            int CommentID = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(employeeAppraisalComment, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddSelfAppraisalComment"));
                CommentID = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && CommentID > 0)
                {
                    EmployeeandManagerView employeeandManager = new();
                    var results = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeAppraisalComment.Employee_Id);
                    employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(results?.Data));
                    string Subject = "";
                    string Body = "";
                    var data = "";
                    var ToMailID = "";
                    if (employeeAppraisalComment.IsAppraisalReveiew)
                    {
                        Subject = @"Regards - {ManagerName} appraisal Comments.";
                        Body = @"<html>
                                    <body>
                                    <b>
                                    <p>Dear {EmployeeName},</p>
                                    </b>
                                    <p> {ManagerName} has added below comments ,Please review and take needful action.</p>                                     
                                    <p> {Comments} </p>
                                     <p> Thanks & Regards,  </p>
                                     <p> {ManagerName} </p>
                                    </body>
                                    </html>";
                        data = Body.Replace("{ManagerName}", employeeandManager.ManagerName);
                        data = data.Replace("{EmployeeName}", employeeandManager.EmployeeName);
                        data = data.Replace("{Comments}", employeeAppraisalComment.Comment);
                        Subject = Subject.Replace("{ManagerName}", employeeandManager.ManagerName);
                        ToMailID = employeeandManager.EmployeeEmailID;
                    }
                    else
                    {
                        Subject = @"Regards - {EmployeeName} appraisal Comments.";
                        Body = @"<html>
                                    <body>
                                    <b>
                                    <p>Dear {ManagerName},</p>
                                    </b>
                                    <p> {EmployeeName} has added below comments ,Please review and take necessary action.</p> 
                                    
                                    <p>{Comments}</p>  
                                    
                                     <p> Thanks & Regards,  </p>
                                    
                                     <p> {EmployeeName} </p>
                                    </body>
                                    </html>";
                        data = Body.Replace("{EmployeeName}", employeeandManager.EmployeeName);
                        data = data.Replace("{ManagerName}", employeeandManager.ManagerName);
                        data = data.Replace("{Comments}", employeeAppraisalComment.Comment);
                        Subject = Subject.Replace("{EmployeeName}", employeeandManager.EmployeeName);
                        ToMailID = employeeandManager.ManagerEmailID;
                    }
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    SendEmailView sendMailbyIndividual = new SendEmailView();
                    sendMailbyIndividual = new SendEmailView
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = ToMailID,
                        Subject = Subject,
                        MailBody = data,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                    };
                    SendEmail.Sendmail(sendMailbyIndividual);
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Appraisal Comment Updated successfully",
                        CommentID
                    });
                }
                else
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = result?.StatusText,
                        CommentID
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddSelfAppraisalComment", JsonConvert.SerializeObject(employeeAppraisalComment));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                CommentID
            });
        }
        #endregion

        #region Get Version Benchmark KRA Gridview Data
        [HttpGet]
        [Route("GetVersionBenchmarkGridViewData")]
        public async Task<IActionResult> GetVersionBenchmarkGridData(int versionId)
        {
            AppraisalMasterData appraisalMasterData = new();
            List<VersionKRABenchmarkGridDetails> VersionBenchmarkGridResult = new();
            try
            {
                var mappingResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionBenchmarkGridData") + versionId);
                VersionBenchmarkGridResult = JsonConvert.DeserializeObject<List<VersionKRABenchmarkGridDetails>>(JsonConvert.SerializeObject(mappingResult?.Data));
                if (VersionBenchmarkGridResult != null)
                {
                    var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                    appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(result?.Data));
                    if (appraisalMasterData?.Department?.Count > 0 || appraisalMasterData?.Roles?.Count > 0)
                    {
                        foreach (var item in VersionBenchmarkGridResult)
                        {
                            item.DepartmentName = appraisalMasterData?.Department?.Where(x => x.DepartmentId == item.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                            item.RoleName = appraisalMasterData?.Roles?.Where(x => x.RoleId == item.RoleId).Select(x => x.RoleName).FirstOrDefault();
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = "Success",
                        Data = VersionBenchmarkGridResult
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionBenchmarkGridData");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = VersionBenchmarkGridResult
            });
        }
        #endregion

        #region Get Manager Appraisal Objective and KRAs
        [HttpGet]
        [Route("GetManagerAppraisalObjandKRAs")]
        public async Task<IActionResult> GetManagerAppraisalObjandKRAs(int appCycleId, int employeeId)
        {
            AppraisalObjectiveandKRAListView AppraisalObjectiveandKRAList = new();
            try
            {
                var result = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetManagerAppraisalObjandKRAs?appCycleId=" + appCycleId + "&employeeId=" + employeeId));
                AppraisalObjectiveandKRAList = JsonConvert.DeserializeObject<AppraisalObjectiveandKRAListView>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = AppraisalObjectiveandKRAList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetManagerAppraisalObjandKRAs");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = AppraisalObjectiveandKRAList
            });
        }
        #endregion

        #region Update Self Appraisal Manager Rating      
        [HttpPost]
        [Route("UpdateAppraisalManagerRating")]
        public async Task<IActionResult> UpdateAppraisalManagerRating(ManagerRatingView managerRatingView)
        {
            int KraId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(managerRatingView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:UpdateAppraisalManagerRating"));
                KraId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && KraId > 0)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Manager Ratings Updated successfully",
                        KraId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/UpdateAppraisalManagerRating", JsonConvert.SerializeObject(managerRatingView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                KraId
            });
        }
        #endregion

        #region Get Version Details By Id
        [HttpGet]
        [Route("GetVersionDetailsById")]
        public async Task<IActionResult> GetVersionDetailsById(int VersionId)
        {
            string statusText = "";
            VersionDetailsView versionDetail = new();
            AppraisalMasterData appraisalMasterData = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionDetailsById") + VersionId);
                versionDetail = JsonConvert.DeserializeObject<VersionDetailsView>(JsonConvert.SerializeObject(result?.Data));
                if (versionDetail != null)
                {
                    var masterResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                    appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(masterResult.Data));
                    //Role mapping
                    if (versionDetail?.VersionRoleMapping?.Count > 0 && (appraisalMasterData?.Department?.Count > 0 || appraisalMasterData?.Roles?.Count > 0))
                    {
                        foreach (var department in versionDetail?.VersionRoleMapping)
                        {
                            department.DepartmentName = appraisalMasterData?.Department?.Where(x => x.DepartmentId == department.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                            foreach (var item in department.Roles)
                            {
                                item.RoleName = appraisalMasterData?.Roles?.Where(x => x.RoleId == item.RoleId).Select(x => x.RoleName).FirstOrDefault();
                            }
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = statusText,
                        Data = versionDetail
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionDetailsByVersionId");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = versionDetail
            });
        }
        #endregion

        #region Get Version Objective KRA By Department and Role
        [HttpGet]
        [Route("GetVersionKRAObjectiveByDepartmentRole")]
        public async Task<IActionResult> GetVersionKRAObjectiveByDepartmentRole(int VersionId, int departmentId, int roleId)
        {
            string statusText = "";
            List<ObjectiveKRA> ObjectiveKRAs = new();
            try
            {
                var mappingResult = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionDepartmentRoleKRAMapping") + VersionId + "&departmentId=" + departmentId + "&roleId=" + roleId);
                ObjectiveKRAs = JsonConvert.DeserializeObject<List<ObjectiveKRA>>(JsonConvert.SerializeObject(mappingResult?.Data));
                if (ObjectiveKRAs != null)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = statusText,
                        Data = ObjectiveKRAs
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionKRAObjectiveByDepartmentRole");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = ObjectiveKRAs
            });
        }
        #endregion

        #region Get Version Benchmark Master Data
        [HttpGet]
        [Route("GetVersionBenchmarkMasterData")]
        public async Task<IActionResult> GetVersionBenchmarkMasterData(int versionId)
        {
            string statusText = "";
            List<AppConstants> AppConstants = new();
            BenchmarkMasterDataView benchmarkMasterdata = new();
            AppraisalMasterData appraisalMasterData = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionBenchmarkMasterData") + versionId);
                benchmarkMasterdata = JsonConvert.DeserializeObject<BenchmarkMasterDataView>(JsonConvert.SerializeObject(result?.Data));
                if (benchmarkMasterdata != null)
                {
                    if (benchmarkMasterdata?.VersionDepartmentRoleMapping?.Count > 0)
                    {
                        var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                        appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                        foreach (var department in benchmarkMasterdata?.VersionDepartmentRoleMapping)
                        {
                            department.DepartmentName = appraisalMasterData?.Department?.Where(x => x.DepartmentId == department.DepartmentId).Select(x => x.DepartmentName).FirstOrDefault();
                            foreach (var item in department?.Roles)
                            {
                                item.RoleName = appraisalMasterData?.Roles?.Where(x => x.RoleId == item.RoleId).Select(x => x.RoleName).FirstOrDefault();
                            }
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = statusText,
                        Data = benchmarkMasterdata
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionBenchmarkMasterData");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = benchmarkMasterdata
            });
        }
        #endregion

        #region Add or update Benchmark Version KRA Range 
        [HttpPost]
        [Route("AddOrUpdateVersionBenchmarkKRARange")]
        public async Task<IActionResult> AddOrUpdateVersionBenchmarkKRARange(VersionBenchmarkRangeView benchmarkKRARangeViews)
        {
            string statusText = "";
            try
            {
                var mappingResult = await _client.PostAsJsonAsync(benchmarkKRARangeViews, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateVersionBenchmarkKRARange"));
                bool ObjectiveKRAs = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(mappingResult?.Data));
                if (ObjectiveKRAs)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = statusText,
                        Data = ObjectiveKRAs
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateVersionBenchmarkKRARange");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });
        }
        #endregion

        #region Get Department Wise Appraisal Status Report
        [HttpGet]
        [Route("GetDepartmentWiseAppraisalStatusReport")]
        public async Task<IActionResult> GetDepartmentWiseAppraisalStatusReport(int departmentID)
        {
            AppraisalStatusReport appraisalStatusReport = new();
            AppraisalMasterData appraisalMasterData = new();
            List<AppraisalStatusGridview> AppraisalStatusGridview = new();
            List<DepartmentDetails> departmentDetails = new();
            try
            {
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetDepartmentWiseAppraisalDetails") + departmentID);
                appraisalStatusReport = JsonConvert.DeserializeObject<AppraisalStatusReport>(JsonConvert.SerializeObject(result?.Data));
                if (appraisalMasterData != null && appraisalStatusReport != null)
                {
                    foreach (AppraisalStatusGridview appraisal in appraisalStatusReport.appraisalStatusGridviews)
                    {
                        AppraisalStatusGridview gridData = new();
                        gridData.Employee_Id = appraisal.Employee_Id;
                        gridData.Employee_Name = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.EmployeeName).FirstOrDefault();
                        gridData.EmployeeId = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.FormattedEmployeeId == null ? "" : x.FormattedEmployeeId).FirstOrDefault(); //appraisal.Employee_Id == 0 ? "" : "EMP" + (10000 + appraisal.Employee_Id).ToString(); //string.Format("TVSN_{0, 0:D6}", appraisal.Employee_Id);
                        gridData.EmployeeEmailId = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.EmployeeEmailId).FirstOrDefault();
                        gridData.Manager_Id = appraisal.Manager_Id;
                        gridData.Manager_Name = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Manager_Id).Select(x => x.EmployeeName).FirstOrDefault();
                        gridData.Department_Id = appraisal.Department_Id;
                        gridData.Department_Name = appraisalMasterData?.Department?.Where(x => x.DepartmentId == appraisal.Department_Id).Select(x => x.DepartmentName).FirstOrDefault();
                        gridData.Role_Id = appraisal.Role_Id;
                        gridData.Role_Name = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.DesignationName).FirstOrDefault();
                        gridData.Rating = appraisal.Rating;
                        gridData.AppraisalStatus = appraisal.AppraisalStatus;
                        gridData.AppraisalStatusId = appraisal.AppraisalStatusId;
                        gridData.ReportingTo = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Manager_Id).Select(x => x.EmployeeName).FirstOrDefault();
                        gridData.ReportingEmailId = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Manager_Id).Select(x => x.EmployeeEmailId).FirstOrDefault();
                        gridData.IsBuheadApproved = appraisal.IsBuheadApproved;
                        gridData.AppCycleID = appraisal.AppCycleID;
                        //gridData.IsBUHeadRevert = appraisal.IsBUHeadRevert;
                        //gridData.IsRevertRating = appraisal.IsRevertRating;
                        AppraisalStatusGridview.Add(gridData);
                    }
                    departmentDetails = appraisalMasterData.Department.Select(rs => new DepartmentDetails { DepartmentId = rs.DepartmentId, DepartmentName = rs.DepartmentName }).ToList();
                    AppraisalStatusReport appraisalStatusReport1 = new()
                    {
                        AppraisalStatus = appraisalStatusReport.AppraisalStatus,
                        TeamRatingSummary = appraisalStatusReport.TeamRatingSummary,
                        appraisalStatusGridviews = AppraisalStatusGridview,
                        DepartmentDetails = departmentDetails
                    };
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = appraisalStatusReport1
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetDepartmentWiseAppraisalDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = appraisalStatusReport
            });
        }
        #endregion

        #region delete benchmark KRA group
        [HttpGet]
        [Route("deleteBenchmarkKRAGroup")]
        public async Task<IActionResult> deleteBenchmarkKRAGroup(int groupId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:deleteBenchmarkKRAGroup") + groupId);
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = isSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/deleteBenchmarkKRAGroup");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region get version KRA banchmark range
        [HttpGet]
        [Route("GetVersionKRABenchmarkRange")]
        public async Task<IActionResult> GetVersionKRABenchmarkRange(int versionId, int departmentId, int roleId, int objectiveId, int kraId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetVersionKRABenchmarkRange") + versionId + "&departmentId=" + departmentId + "&roleId=" + roleId + "&objectiveId=" + objectiveId + "&kraId=" + kraId);
                List<VersionBenchMarks> benchmarkList = JsonConvert.DeserializeObject<List<VersionBenchMarks>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = benchmarkList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetVersionKRABenchmarkRange");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = new List<VersionBenchMarks>()
            });
        }
        #endregion

        #region Get Individual Appraisal Comments By AppCycleId
        [HttpGet]
        [Route("GetIndividualAppraisalCommentsAndDocByAppCycleId")]
        public async Task<IActionResult> GetIndividualAppraisalCommentsAndDocByAppCycleId(int appcycleId, int employeeId, int ObjId, int KraId)
        {
            EmployeeKRCommentView employeeKRCommentView = new();
            List<EmployeeName> createdEmployeeName = new();
            List<KraComments> kraComment = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetKRACommentsById") + appcycleId + "&employeeId=" + employeeId + "&KraId=" + KraId + "&ObjId=" + ObjId);
                kraComment = JsonConvert.DeserializeObject<List<KraComments>>(JsonConvert.SerializeObject(result?.Data));
                createdEmployeeName = GetEmployeeNameById(kraComment.Select(x => x.CreatedBy).ToList()).Result;
                kraComment.ForEach(x => x.CreatedByName = createdEmployeeName.Where(y => y.EmployeeId == x.CreatedBy).Select(y => y.EmployeeFullName).FirstOrDefault());
                employeeKRCommentView.KraComments = kraComment;
                if (kraComment != null)
                {
                    employeeKRCommentView.ListOfDocuments = new();
                    using var documentClient = new HttpClient();
                    AppraisalSourceDocuments sourceDocuments = new();
                    sourceDocuments.ObjectiveId = ObjId;
                    sourceDocuments.KraId = KraId;
                    sourceDocuments.employeeId = employeeId;
                    sourceDocuments.appcycleId = appcycleId;
                    var results = await _client.PostAsJsonAsync(sourceDocuments, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetDocumentByObjAndKraId"));
                    List<EmployeeKeyResultAttachments> lstOfSupDocument = JsonConvert.DeserializeObject<List<EmployeeKeyResultAttachments>>(JsonConvert.SerializeObject(results.Data));
                    if (lstOfSupDocument?.Count > 0)
                    {
                        employeeKRCommentView.ListOfDocuments = lstOfSupDocument.Where(x => x.DOC_NAME != null).Select(x => new DocumentDetail { DOC_ID = x.DOC_ID, DOC_NAME = x.DOC_NAME, DOC_TYPE = x.DOC_TYPE, APP_CYCLE_ID = x.APP_CYCLE_ID, OBJECTIVE_ID = x.OBJECTIVE_ID, KEY_RESULT_ID = x.KEY_RESULT_ID }).ToList();
                    }
                }
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = employeeKRCommentView
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetIndividualAppraisalCommentsByAppCycleId");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = employeeKRCommentView
            });
        }
        #endregion

        #region Get Employee Appraisal Details by Managerid
        [HttpGet]
        [Route("GetEmployeeAppraisalListByManagerId")]
        public async Task<IActionResult> GetEmployeeAppraisalListByManagerId(int empManagerId, bool isAll, int departmentId, int employeeCategoryId)
        {
            EmployeeAppraisalListView EmployeAppraisalList = new();
            List<EmployeeViewDetails> EmployeeList = new();
            List<EmployeeAppraisalStatusView> employeeAppraisalStatusViewsList = new();
            List<Employees> AllEmployeeList = new();
            EmployeeListAndDepartment empListDepartment = new();
            //List<int> managerId = new List<int>();
            //managerId.Add(empManagerId);
            AppraisalMasterData appraisalMasterData = new();
            try
            {
                var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeListForManagerReport") + empManagerId + "&isAll=" + isAll);
                EmployeeList = JsonConvert.DeserializeObject<List<EmployeeViewDetails>>(JsonConvert.SerializeObject(employeeListresult.Data));
                empListDepartment.employeeids = EmployeeList.Select(ea => ea.EmployeeId).ToList();
                empListDepartment.departmentId = departmentId;
                empListDepartment.IsAllReportees = isAll;
                var employeeappraisalresult = await _client.PostAsJsonAsync(empListDepartment, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEmployeeAppraisalListByEmployeeId"));
                EmployeAppraisalList = JsonConvert.DeserializeObject<EmployeeAppraisalListView>(JsonConvert.SerializeObject(employeeappraisalresult.Data));
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                /*********************** Employee Appraisal Status View ******************************/
                EmployeeAppraisalReportView employeeappraisalstatus = new();
                if (appraisalMasterData != null && EmployeAppraisalList != null)
                {
                    foreach (EmployeeAppraisalMasterDetailView appraisal in EmployeAppraisalList.EmployeeAppraisalMasterDetailView)
                    {
                        if (appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Any() == true)
                        {
                            EmployeeAppraisalStatusView employeeAppraisalStatusView = new EmployeeAppraisalStatusView();
                            employeeAppraisalStatusView.AppCycleId = appraisal.APP_CYCLE_ID;
                            employeeAppraisalStatusView.EmployeeId = appraisal.EMPLOYEE_ID; // Convert.ToInt32(string.Format("{0, 0:D6}", rs.ea.EMPLOYEE_ID)),
                            string formattedEmployeeId = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Select(x => x.FormattedEmployeeId).FirstOrDefault();
                            employeeAppraisalStatusView.Employee_Id = formattedEmployeeId == null ? "" : formattedEmployeeId; //rs.ea.EMPLOYEE_ID == 0 ? "" : "EMP" + (10000 + rs.ea.EMPLOYEE_ID).ToString(), // string.Format("TVSN_{0, 0:D6}", rs.ea.EMPLOYEE_ID),
                            employeeAppraisalStatusView.EmployeeName = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Select(x => x.EmployeeName).FirstOrDefault();
                            employeeAppraisalStatusView.EmployeeEmail = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Select(x => x.EmployeeEmailId).FirstOrDefault(); ;
                            //   EmployeeType = rs.e.EmployeeType,
                            employeeAppraisalStatusView.EntityId = appraisal.ENTITY_ID;
                            employeeAppraisalStatusView.EntityName = appraisal.ENTITY_NAME;
                            employeeAppraisalStatusView.EntityShortName = appraisal.ENTITY_SHORT_NAME;
                            employeeAppraisalStatusView.EmployeeRoleId = appraisal.EMPLOYEE_ROLE_ID;
                            employeeAppraisalStatusView.EmployeeDepId = appraisal.EMPLOYEE_DEPT_ID;
                            employeeAppraisalStatusView.EmployeeRoleName = appraisalMasterData.Roles.Where(x => x.RoleId == appraisal.EMPLOYEE_ROLE_ID).Select(x => x.RoleName).FirstOrDefault();
                            employeeAppraisalStatusView.DepartmentName = appraisalMasterData.Department.Where(x => x.DepartmentId == appraisal.EMPLOYEE_DEPT_ID).Select(x => x.DepartmentName).FirstOrDefault();
                            employeeAppraisalStatusView.EmployeeManagerId = appraisal.EMPLOYEE_MANAGER_ID;
                            employeeAppraisalStatusView.EmployeeSelfRating = appraisal.EMPLOYEE_SELF_RATING;
                            employeeAppraisalStatusView.EmployeeAppraiserRating = appraisal.EMPLOYEE_APPRAISER_RATING;
                            employeeAppraisalStatusView.EmployeeFinalRating = appraisal.EMPLOYEE_FINAL_RATING;
                            employeeAppraisalStatusView.AppraisalStatus = appraisal.APPRAISAL_STATUS;
                            employeeAppraisalStatusView.AppraisalStatusName = appraisal.APPRAISAL_STATUS_NAME;
                            employeeAppraisalStatusView.ReportingTo = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_MANAGER_ID).Select(x => x.EmployeeName).FirstOrDefault();
                            employeeAppraisalStatusView.ReportingEmail = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_MANAGER_ID).Select(x => x.EmployeeEmailId).FirstOrDefault();
                            employeeAppraisalStatusView.IsBUHeadRevert = appraisal.IsBUHeadRevert;
                            employeeAppraisalStatusView.IsRevertRating = appraisal.IsRevertRating;
                            employeeAppraisalStatusView.IsBuheadApproved = appraisal.IsBuheadApproved;
                            employeeAppraisalStatusViewsList.Add(employeeAppraisalStatusView);

                        }
                    }
                }
                employeeappraisalstatus.EmployeeAppraisalStatusView = employeeAppraisalStatusViewsList;
                //employeeappraisalstatus.EmployeeAppraisalStatusView = appraisalMasterData.ReportingManagerEmployeeList.Join(EmployeAppraisalList.EmployeeAppraisalMasterDetailView, e => e.EmployeeId, ea => ea.EMPLOYEE_ID, (e, ea) => new { e, ea })
                //.Select(rs => new EmployeeAppraisalStatusView
                //{
                //    AppCycleId = rs.ea.APP_CYCLE_ID,
                //    EmployeeId = rs.ea.EMPLOYEE_ID, // Convert.ToInt32(string.Format("{0, 0:D6}", rs.ea.EMPLOYEE_ID)),
                //    Employee_Id = rs.e.FormattedEmployeeId==null?"": rs.e.FormattedEmployeeId, //rs.ea.EMPLOYEE_ID == 0 ? "" : "EMP" + (10000 + rs.ea.EMPLOYEE_ID).ToString(), // string.Format("TVSN_{0, 0:D6}", rs.ea.EMPLOYEE_ID),
                //    EmployeeName = rs.e.EmployeeName,
                //    EmployeeEmail = rs.e.EmployeeEmailId,
                // //   EmployeeType = rs.e.EmployeeType,
                //    EntityId = rs.ea.ENTITY_ID,
                //    EntityName = rs.ea.ENTITY_NAME,
                //    EntityShortName = rs.ea.ENTITY_SHORT_NAME,
                //    EmployeeRoleId = rs.ea.EMPLOYEE_ROLE_ID,
                //    EmployeeDepId = rs.ea.EMPLOYEE_DEPT_ID,
                //    EmployeeRoleName = appraisalMasterData.Roles.Where(x=>x.RoleId== rs.ea.EMPLOYEE_ROLE_ID).Select(x=>x.RoleName).FirstOrDefault(),
                //    DepartmentName =appraisalMasterData.Department.Where(x=>x.DepartmentId== rs.ea.EMPLOYEE_DEPT_ID).Select(x=>x.DepartmentName).FirstOrDefault(),
                //    EmployeeManagerId = rs.ea.EMPLOYEE_MANAGER_ID,
                //    EmployeeSelfRating = rs.ea.EMPLOYEE_SELF_RATING,
                //    EmployeeAppraiserRating = rs.ea.EMPLOYEE_APPRAISER_RATING,
                //    EmployeeFinalRating = rs.ea.EMPLOYEE_FINAL_RATING,
                //    AppraisalStatus = rs.ea.APPRAISAL_STATUS,
                //    AppraisalStatusName = rs.ea.APPRAISAL_STATUS_NAME,
                //    ReportingTo = EmployeeList.Where(x=>x.EmployeeId== rs.ea.EMPLOYEE_MANAGER_ID).Select(x=>x.EmployeeName).FirstOrDefault() ,
                //    ReportingEmail = EmployeeList.Where(x => x.EmployeeId == rs.ea.EMPLOYEE_MANAGER_ID).Select(x => x.EmployeeEmail).FirstOrDefault(),
                //    IsBUHeadRevert = rs.ea.IsBUHeadRevert,
                //    IsRevertRating = rs.ea.IsRevertRating,
                //    IsBuheadApproved = rs.ea.IsBuheadApproved

                //}).ToList();
                /************************************** Appraisal Status Count ************************/
                employeeappraisalstatus.EmployeeAppraisalStatus = EmployeAppraisalList.EmployeeAppraisalMasterDetailView.GroupBy(ea => new { ea.APPRAISAL_STATUS, ea.APPRAISAL_STATUS_NAME }).Select(rs => new EmployeeAppraisalStatus { AppraisalStatusId = rs.Key.APPRAISAL_STATUS, AppraisalStatusName = rs.Key.APPRAISAL_STATUS_NAME, AppraisalStatusCount = rs.Count() }).ToList();
                /************************************** Appraisal Rating Status Count *****************/
                employeeappraisalstatus.EmployeeAppraisalRatingStatus = EmployeAppraisalList.EmployeeAppraisalMasterDetailView.GroupBy(ea => Math.Round((decimal)ea.EMPLOYEE_FINAL_RATING)).Select(rs => new EmployeeAppraisalRatingStatus { RatingValue = rs.Key, RatingCount = rs.Count() }).ToList();
                /************************************** Appraisal Milestone Status *****************/
                employeeappraisalstatus.AppraisalMilestonedetails = EmployeAppraisalList.AppraisalMilestonedetails;
                /************************************** Appraisal BU Head Commands *****************/
                employeeappraisalstatus.appraisalBUHeadCommentsViews = EmployeAppraisalList.AppraisalBUHeadCommentsView;
                var allemployeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeesDetails"));
                AllEmployeeList = JsonConvert.DeserializeObject<List<Employees>>(JsonConvert.SerializeObject(allemployeeListresult.Data));
                employeeappraisalstatus.appraisalBUHeadCommentsViews = employeeappraisalstatus.appraisalBUHeadCommentsViews.Join(AllEmployeeList, abc => abc.Employee_Id, ae => ae.EmployeeID, (abc, ae) => new { abc, ae })
                    .Select(rs => new AppraisalBUHeadCommentsView
                    {
                        AppraisalBUHeadCommentsId = rs.abc.AppraisalBUHeadCommentsId,
                        AppCycle_Id = rs.abc.AppCycle_Id,
                        Department_Id = rs.abc.Department_Id,
                        Employee_Id = Convert.ToInt32(string.Format("{0, 0:D6}", rs.abc.Employee_Id)),
                        Comment = rs.abc.Comment,
                        Created_By = rs.abc.Created_By,
                        Created_On = rs.abc.Created_On,
                        Employee_Name = rs.ae.FirstName + " " + rs.ae.LastName
                    }).ToList();
                //var employeeCategoryReportsResponse = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeCategoryDetailsByEmployeeId") + empManagerId + "&employeeCategoryId=" + employeeCategoryId);
                //EmployeeCategoryView employeeCategory = JsonConvert.DeserializeObject<EmployeeCategoryView>(JsonConvert.SerializeObject(employeeCategoryReportsResponse?.Data));
                //employeeappraisalstatus.EmployeeCategoryView = employeeCategory;
                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = employeeappraisalstatus
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetEmployeeAppraisalListByManagerId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new EmployeeAppraisalReportView()
                });
            }
        }
        #endregion

        #region Individual Appraisal APIs

        #region Get Individual Appraisal DropdownList
        [HttpGet]
        [Route("GetIndividualAppraisalDropdownList")]
        public async Task<IActionResult> GetIndividualAppraisalDropdownList(int employeeID)
        {
            List<AppraisalCycleMasterData> appraisalMaster = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetIndividualAppraisalDropdownList") + employeeID);
                appraisalMaster = JsonConvert.DeserializeObject<List<AppraisalCycleMasterData>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = appraisalMaster
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetIndividualAppraisalDropdownList");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = appraisalMaster
            });
        }
        #endregion

        #region Get Individual Appraisal Details By AppCycleId
        [HttpGet]
        [Route("GetIndividualAppraisalDetailsByAppCycleId")]
        public async Task<IActionResult> GetIndividualAppraisalDetailsByAppCycleId(int appcycleId, int departmentId, int roleId, int employeeId)
        {
            IndividualAppraisalView individualAppraisalView = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetIndividualAppraisalDetailsByAppCycleId") + appcycleId + "&departmentId=" + departmentId + "&roleId=" + roleId + "&employeeId=" + employeeId);
                individualAppraisalView = JsonConvert.DeserializeObject<IndividualAppraisalView>(JsonConvert.SerializeObject(result?.Data));
                List<EmployeeName> lstEmployeeName = GetEmployeeNameById(individualAppraisalView?.individualAppraisalCommentsViews?.Select(x => x.CreatedBy).ToList()).Result;
                individualAppraisalView?.individualAppraisalCommentsViews?.ForEach(x => x.CreatedBy_Name = lstEmployeeName.Where(y => y.EmployeeId == x.CreatedBy).Select(y => y.EmployeeFullName).FirstOrDefault());
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = individualAppraisalView
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetIndividualAppraisalDetailsByAppCycleId");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = individualAppraisalView
            });
        }
        #endregion

        #region Add Individual Appraisal Doc and Comments
        [HttpPost]
        [Route("AddIndividualAppraisalDocandComments")]
        public async Task<IActionResult> AddIndividualAppraisalDocandComments(IndividualAppraisalAddDocandCommentView DocumentandComments)
        {
            string statusCode = "", statusText = "";
            try
            {
                IndividualComments individualComments = DocumentandComments?.IndividualComments;
                IndividualDocDetails individualDocDetails = DocumentandComments?.IndividualDocDetails;
                EmployeeKeyResultAttachmentsView Documents = new()
                {
                    ListOfDocuments = new List<DocumentDetail>(),
                    KEY_RESULT_ID = DocumentandComments.IndividualDocDetails.KeyResultID,
                    OBJECTIVE_ID = DocumentandComments.IndividualDocDetails.ObjectiveID,
                    OBJECTIVE_NAME = DocumentandComments.IndividualDocDetails.Objective_Name,
                    EMPLOYEE_ID = DocumentandComments.IndividualDocDetails.EmployeeID,
                    APP_CYCLE_ID = DocumentandComments.IndividualDocDetails.AppCycleID,
                    APP_CYCLE_NAME = DocumentandComments.IndividualDocDetails.AppCycle_Name,
                    SourceType = _configuration.GetValue<string>("AppraisalSourceType"),
                    BaseDirectory = _configuration.GetValue<string>("SupportingDocumentsBaseDirectory"),
                    CREATED_BY = DocumentandComments.IndividualDocDetails.EmployeeID
                };
                if (!string.IsNullOrEmpty(Documents.BaseDirectory))
                {
                    //Create Base Directory
                    if (!Directory.Exists(Documents.BaseDirectory))
                    {
                        Directory.CreateDirectory(Documents.BaseDirectory);
                    }
                    //Create Source Type Directory
                    if (!Directory.Exists(Path.Combine(Documents.BaseDirectory, Documents.SourceType)))
                    {
                        Directory.CreateDirectory(Path.Combine(Documents.BaseDirectory, Documents.SourceType));
                    }
                    //Create AppCycleName  Directory
                    if (!Directory.Exists(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString())))
                    {
                        Directory.CreateDirectory(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString()));
                    }
                    //Create EmployeeID  Directory
                    if (!Directory.Exists(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString(), Documents.EMPLOYEE_ID.ToString())))
                    {
                        Directory.CreateDirectory(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString(), Documents.EMPLOYEE_ID.ToString()));
                    }
                    //Create ObjectiveName  Directory
                    if (!Directory.Exists(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString(), Documents.EMPLOYEE_ID.ToString(), Documents.OBJECTIVE_NAME.ToString())))
                    {
                        Directory.CreateDirectory(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString(), Documents.EMPLOYEE_ID.ToString(), Documents.OBJECTIVE_NAME.ToString()));
                    }
                    //Create KeyResultID  Directory
                    if (!Directory.Exists(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString(), Documents.EMPLOYEE_ID.ToString(), Documents.OBJECTIVE_NAME.ToString(), Documents.KEY_RESULT_ID.ToString())))
                    {
                        Directory.CreateDirectory(Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString(), Documents.EMPLOYEE_ID.ToString(), Documents.OBJECTIVE_NAME.ToString(), Documents.KEY_RESULT_ID.ToString()));
                    }
                }
                string directoryPath = Path.Combine(Documents.BaseDirectory, Documents.SourceType, Documents.APP_CYCLE_NAME.ToString(), Documents.EMPLOYEE_ID.ToString(), Documents.OBJECTIVE_NAME.ToString(), Documents.KEY_RESULT_ID.ToString());
                List<DocumentDetail> docList = new();
                foreach (var item in DocumentandComments?.ListOfDocuments)
                {
                    string documentPath = Path.Combine(directoryPath, item.DOC_NAME);
                    if (!System.IO.File.Exists(item.DOC_NAME))    //&& item.DocumentSize > 0
                    {
                        if (item.DocumentAsBase64.Contains(","))
                        {
                            item.DocumentAsBase64 = item.DocumentAsBase64.Substring(item.DocumentAsBase64.IndexOf(",") + 1);
                        }
                        item.DocumentAsByteArray = Convert.FromBase64String(item.DocumentAsBase64);
                        using (Stream fileStream = new FileStream(documentPath, FileMode.Create))
                        {
                            fileStream.Write(item.DocumentAsByteArray, 0, item.DocumentAsByteArray.Length);
                        }
                    }
                    DocumentDetail docDetail = new()
                    {
                        DOC_NAME = item.DOC_NAME
                    };
                    string doc = Path.GetExtension(item.DOC_NAME);
                    docDetail.DOC_TYPE = doc.Substring(1);
                    docList.Add(docDetail);
                }
                Documents.ListOfDocuments = docList;
                var result = await _client.PostAsJsonAsync(Documents, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddIndividualAppraisalDocuments"));
                var results = await _client.PostAsJsonAsync(individualComments, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddSelfAppraisalKRAComment"));
                int KraId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(results?.Data));
                if (statusCode == "")
                {
                    return Ok(new
                    {
                        results?.StatusCode,
                        results?.StatusText,
                    });
                }
                else
                    statusText = results?.StatusText;
            }
            catch (Exception ex)
            {

                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddIndividualAppraisalDocandComments");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = ""
            });
        }
        #endregion

        #region Delete Individual Appraisal Document        
        [HttpGet]
        [Route("DeleteIndividualAppraisalDocument")]
        public async Task<IActionResult> DeleteIndividualAppraisalDocument(int DocID)
        {
            try
            {
                var Result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteIndividualAppraisalDocument") + DocID);
                if (Result != null)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Deleted successfully."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteIndividualAppraisalDocument", JsonConvert.SerializeObject(DocID));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion
        #endregion

        #region Get Appraisal Objective Rating Details
        [HttpGet]
        [Route("GetAppraisalObjectiveRatingReport")]
        public async Task<IActionResult> GetAppraisalObjectiveRatingReport(int employeeId)
        {
            AppraisalReport appraisalReport = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAppraisalObjectiveRatingDetails") + employeeId);
                appraisalReport = JsonConvert.DeserializeObject<AppraisalReport>(JsonConvert.SerializeObject(result?.Data));
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = appraisalReport
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetAppraisalObjectiveRatingReport");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = appraisalReport
            });
        }
        #endregion

        #region Get app cycle employee list        
        [HttpGet]
        [Route("GetAllAppCycleEmployee")]
        public async Task<IActionResult> GetAllAppCycleEmployee(int appCycleId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAllAppCycleEmployee") + appCycleId);
                List<EmployeeAppraisalMasterDetailView> employeeList = JsonConvert.DeserializeObject<List<EmployeeAppraisalMasterDetailView>>(JsonConvert.SerializeObject(result?.Data));
                if (employeeList?.Count > 0)
                {
                    var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                    AppraisalMasterData appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                    if (appraisalMasterData != null)
                    {
                        foreach (EmployeeAppraisalMasterDetailView item in employeeList)
                        {
                            item.FORMATTED_EMPLOYEE_ID = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == item.EMPLOYEE_ID)?.Select(x => x.FormattedEmployeeId == null ? "" : x.FormattedEmployeeId).FirstOrDefault(); // item.EMPLOYEE_ID == 0 ? "" : "EMP" + (10000 + item.EMPLOYEE_ID).ToString();
                            item.EMPLOYEE_NAME = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == item.EMPLOYEE_ID)?.Select(x => x.EmployeeName).FirstOrDefault();
                            item.EMPLOYEE_DEPT_NAME = appraisalMasterData?.Department?.Where(x => x.DepartmentId == item.EMPLOYEE_DEPT_ID)?.Select(x => x.DepartmentName).FirstOrDefault();
                            item.EMPLOYEE_ROLE_NAME = appraisalMasterData?.Roles?.Where(x => x.RoleId == item.EMPLOYEE_ROLE_ID)?.Select(x => x.RoleName).FirstOrDefault();
                            item.EMPLOYEE_MANAGER_NAME = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == item.EMPLOYEE_MANAGER_ID)?.Select(x => x.EmployeeName).FirstOrDefault();
                            item.EMPLOYEE_EMAILADDRESS = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == item.EMPLOYEE_ID)?.Select(x => x.EmployeeEmailId).FirstOrDefault();
                        }
                    }
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "",
                        Data = employeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetAllAppCycleEmployee", appCycleId.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = new List<EmployeeAppraisalMasterDetailView>()
            });
        }
        #endregion

        #region Delete app cycle employee
        [HttpGet]
        [Route("DeleteAppCycleEmployee")]
        public async Task<IActionResult> DeleteAppCycleEmployee(int appCycleId, int employeeId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteAppCycleEmployee") + appCycleId + "&employeeId=" + employeeId);
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (isSuccess)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = isSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteAppCycleEmployee");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Add app cycle employee
        [HttpPost]
        [Route("AddAppCycleEmployee")]
        public async Task<IActionResult> AddAppCycleEmployee(List<EmployeeAppraisalMaster> appEmployee)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(appEmployee, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddAppCycleEmployee"));
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (isSuccess)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = isSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddAppCycleEmployee");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Get Appraisal Comments ById
        [HttpGet]
        [Route("GetAppraisalCommentsById")]
        public async Task<IActionResult> GetAppraisalCommentsById(int appcycleId, int employeeId)
        {
            List<IndividualAppraisalCommentsView> Comments = new();
            List<EmployeeName> lstEmployeeName = new();
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAppraisalCommentsById") + appcycleId + "&employeeId=" + employeeId);
                Comments = JsonConvert.DeserializeObject<List<IndividualAppraisalCommentsView>>(JsonConvert.SerializeObject(result?.Data));
                lstEmployeeName = GetEmployeeNameById(Comments.Select(x => x.CreatedBy).ToList()).Result;
                Comments.ForEach(x => x.CreatedBy_Name = lstEmployeeName.Where(y => y.EmployeeId == x.CreatedBy).Select(y => y.EmployeeFullName).FirstOrDefault());
                if (result != null)
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = Comments
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetAppraisalCommentsById");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = Comments
            });
        }
        #endregion

        #region Get app cycle employee list        
        [HttpGet]
        [Route("GetNotEligibleAppCycleEmployee")]
        public async Task<IActionResult> GetNotEligibleAppCycleEmployee(int appCycleId)
        {
            List<EmployeeAppraisalMasterDetailView> notEligibleEmployeeList = new();
            try
            {
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeesDetails"));
                List<Employees> allEmployeeList = JsonConvert.DeserializeObject<List<Employees>>(JsonConvert.SerializeObject(employeeResult?.Data));
                if (allEmployeeList?.Count > 0)
                {
                    var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetAllAppCycleEmployee") + appCycleId);
                    List<EmployeeAppraisalMasterDetailView> employeeList = JsonConvert.DeserializeObject<List<EmployeeAppraisalMasterDetailView>>(JsonConvert.SerializeObject(result?.Data));
                    if (employeeList?.Count > 0)
                    {
                        List<Employees> notEligibleEmployee = allEmployeeList?.Where(x => !employeeList.Select(x => x.EMPLOYEE_ID).Contains(x.EmployeeID)).ToList();
                        notEligibleEmployeeList = notEligibleEmployee?.Where(x => x.IsActive == true).Select(x => new EmployeeAppraisalMasterDetailView
                        {
                            EMPLOYEE_ID = x.EmployeeID,
                            ENTITY_ID = employeeList.Select(x => x.ENTITY_ID).FirstOrDefault(),
                            EMPLOYEE_NAME = x.FirstName?.Trim() == "" ? x.LastName?.Trim() : x.FirstName?.Trim() + " " + x.LastName?.Trim(),
                            EMPLOYEE_EMAILADDRESS = x.EmailAddress,
                            APP_CYCLE_ID = appCycleId,
                            EMPLOYEE_ROLE_ID = x.RoleId == null ? 0 : (int)x.RoleId,
                            EMPLOYEE_DEPT_ID = x.DepartmentId == null ? 0 : (int)x.DepartmentId,
                            EMPLOYEE_MANAGER_ID = x.ReportingManagerId == null ? 0 : (int)x.ReportingManagerId
                        }).ToList();
                    }
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "",
                        Data = notEligibleEmployeeList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetNotEligibleAppCycleEmployee", appCycleId.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = notEligibleEmployeeList
            });
        }
        #endregion

        #region Get Department Wise Appraisal Status Report in excel
        [HttpGet]
        [Route("GetAppraisalStatusReportInExcel")]
        public async Task<IActionResult> GetAppraisalStatusReportInExcel(int departmentId, int roleId, int? StatusId, int ratingValue = -1)
        {
            AppraisalStatusReport appraisalStatusReport = new();
            AppraisalMasterData appraisalMasterData = new();
            List<AppraisalStatusGridview> AppraisalStatusGridview = new();
            string fileName = "AppraisalReport.xlsx";
            try
            {
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetDepartmentWiseAppraisalDetails") + departmentId);
                appraisalStatusReport = JsonConvert.DeserializeObject<AppraisalStatusReport>(JsonConvert.SerializeObject(result?.Data));
                if (appraisalMasterData != null && appraisalStatusReport != null)
                {
                    foreach (AppraisalStatusGridview appraisal in appraisalStatusReport.appraisalStatusGridviews)
                    {
                        AppraisalStatusGridview gridData = new()
                        {
                            Employee_Id = appraisal.Employee_Id,
                            Employee_Name = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.EmployeeName).FirstOrDefault(),
                            EmployeeId = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.FormattedEmployeeId == null ? "" : x.FormattedEmployeeId).FirstOrDefault(),// 0 ? "" : "EMP" + (10000 + appraisal.Employee_Id).ToString(), //string.Format("TVSN_{0, 0:D6}", appraisal.Employee_Id),
                            EmployeeEmailId = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.EmployeeEmailId).FirstOrDefault(),
                            Manager_Id = appraisal.Manager_Id,
                            Manager_Name = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Manager_Id).Select(x => x.EmployeeName).FirstOrDefault(),
                            Department_Id = appraisal.Department_Id,
                            Department_Name = appraisalMasterData?.Department?.Where(x => x.DepartmentId == appraisal.Department_Id).Select(x => x.DepartmentName).FirstOrDefault(),
                            Role_Id = appraisal.Role_Id,
                            Role_Name = appraisalMasterData?.Roles?.Where(x => x.RoleId == appraisal.Role_Id).Select(x => x.RoleName).FirstOrDefault(),
                            Rating = appraisal.Rating,
                            AppraisalStatus = appraisal.AppraisalStatus,
                            AppraisalStatusId = appraisal.AppraisalStatusId,
                            ReportingTo = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.ReportingTo).FirstOrDefault(),
                            ReportingEmailId = appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.Employee_Id).Select(x => x.ReportingEmailId).FirstOrDefault(),
                            AppCycleID = appraisal.AppCycleID,
                            IsBUHeadRevert = appraisal.IsBUHeadRevert,
                            IsRevertRating = appraisal.IsRevertRating
                        };
                        AppraisalStatusGridview.Add(gridData);
                    }
                    AppraisalStatusGridview = AppraisalStatusGridview.Where(rs => (roleId == 0 || rs.Role_Id == roleId) && (StatusId == 0 || rs.AppraisalStatusId == StatusId) && (ratingValue == -1 || rs.Rating == ratingValue)).ToList();
                    var workbook = new XLWorkbook();
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Employees");
                    worksheet.Cell(1, 1).Value = "Employee_Name";
                    worksheet.Cell(1, 2).Value = "EmployeeId";
                    worksheet.Cell(1, 3).Value = "Employee_EmailId";
                    worksheet.Cell(1, 4).Value = "Reporting_To";
                    worksheet.Cell(1, 5).Value = "Reporting_EmailId";
                    worksheet.Cell(1, 6).Value = "Department_Name";
                    worksheet.Cell(1, 7).Value = "Role_Name";
                    worksheet.Cell(1, 8).Value = "Rating";
                    worksheet.Cell(1, 9).Value = "AppraisalStatus";
                    for (int index = 1; index <= AppraisalStatusGridview.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = AppraisalStatusGridview[index - 1].Employee_Name;
                        worksheet.Cell(index + 1, 2).Value = AppraisalStatusGridview[index - 1].EmployeeId;
                        worksheet.Cell(index + 1, 3).Value = AppraisalStatusGridview[index - 1].EmployeeEmailId;
                        worksheet.Cell(index + 1, 4).Value = AppraisalStatusGridview[index - 1].ReportingTo;
                        worksheet.Cell(index + 1, 5).Value = AppraisalStatusGridview[index - 1].ReportingEmailId;
                        worksheet.Cell(index + 1, 6).Value = AppraisalStatusGridview[index - 1].Department_Name;
                        worksheet.Cell(index + 1, 7).Value = AppraisalStatusGridview[index - 1].Role_Name;
                        worksheet.Cell(index + 1, 8).Value = AppraisalStatusGridview[index - 1].Rating;
                        worksheet.Cell(index + 1, 9).Value = AppraisalStatusGridview[index - 1].AppraisalStatus;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        byte[] bytes = stream.ToArray();
                        string contentType;
                        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "",
                            BaseString = Convert.ToBase64String(bytes),
                            ContentType = contentType ?? "application/octet-stream"
                        });
                    }
                }
                /* var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                 appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                 var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetDepartmentWiseAppraisalDetails") + departmentId);
                 appraisalStatusReport = JsonConvert.DeserializeObject<AppraisalStatusReport>(JsonConvert.SerializeObject(result?.Data));
                 if (appraisalMasterData != null && appraisalStatusReport != null)
                 {
                     AppraisalStatusGridview = (from status in appraisalStatusReport.appraisalStatusGridviews
                                                join emp in appraisalMasterData.ReportingManagerEmployeeList on status.Employee_Id equals emp.EmployeeId
                                                join rptmgr in appraisalMasterData.ReportingManagerEmployeeList on status.Manager_Id equals rptmgr.EmployeeId
                                                join dept in appraisalMasterData.Department on status.Department_Id equals dept.DepartmentId
                                                join role in appraisalMasterData.Roles on status.Role_Id equals role.RoleId
                                                where status.Department_Id == (departmentId > 0 ? departmentId : status.Department_Id) && status.Role_Id == (roleId > 0 ? roleId : status.Role_Id)
                                                && status.AppraisalStatusId == (StatusId > 0 ? StatusId : status.AppraisalStatusId) 
                                                select new AppraisalStatusGridview
                                                {
                                                    Employee_Id = status.Employee_Id,
                                                    Employee_Name = emp.EmployeeName,
                                                    EmployeeId = "NXT" + emp.EmployeeId,
                                                    Manager_Id = status.Manager_Id,
                                                    Manager_Name = rptmgr.EmployeeName,
                                                    Department_Id = status.Department_Id,
                                                    Department_Name = dept.DepartmentName,
                                                    Role_Id = status.Role_Id,
                                                    Role_Name = role.RoleName,
                                                    Rating = status.Rating,
                                                    AppraisalStatus = status.AppraisalStatus,
                                                    EmployeeEmailId = rptmgr.EmployeeEmailId,
                                                    ReportingEmailId = rptmgr.ReportingEmailId,
                                                    ReportingTo = rptmgr.ReportingTo
                                                }).Distinct().ToList();

                     var workbook = new XLWorkbook();
                     IXLWorksheet worksheet = workbook.Worksheets.Add("Employees");
                     worksheet.Cell(1, 1).Value = "Employee_Name";
                     worksheet.Cell(1, 2).Value = "EmployeeId";
                     worksheet.Cell(1, 3).Value = "Employee_EmailId";
                     worksheet.Cell(1, 4).Value = "Reporting_To";
                     worksheet.Cell(1, 5).Value = "Reporting_EmailId";
                     worksheet.Cell(1, 6).Value = "Department_Name";
                     worksheet.Cell(1, 7).Value = "Role_Name";
                     worksheet.Cell(1, 8).Value = "Rating";
                     worksheet.Cell(1, 9).Value = "AppraisalStatus";
                     for (int index = 1; index <= AppraisalStatusGridview.Count; index++)
                     {
                         worksheet.Cell(index + 1, 1).Value = AppraisalStatusGridview[index - 1].Employee_Name;
                         worksheet.Cell(index + 1, 2).Value = AppraisalStatusGridview[index - 1].EmployeeId;
                         worksheet.Cell(index + 1, 3).Value = AppraisalStatusGridview[index - 1].EmployeeEmailId;
                         worksheet.Cell(index + 1, 4).Value = AppraisalStatusGridview[index - 1].ReportingTo;
                         worksheet.Cell(index + 1, 5).Value = AppraisalStatusGridview[index - 1].ReportingEmailId;
                         worksheet.Cell(index + 1, 6).Value = AppraisalStatusGridview[index - 1].Department_Name;
                         worksheet.Cell(index + 1, 7).Value = AppraisalStatusGridview[index - 1].Role_Name;
                         worksheet.Cell(index + 1, 8).Value = AppraisalStatusGridview[index - 1].Rating;
                         worksheet.Cell(index + 1, 9).Value = AppraisalStatusGridview[index - 1].AppraisalStatus;
                     }
                     using (var stream = new MemoryStream())
                     {
                         workbook.SaveAs(stream);
                         // var content = stream.ToArray();
                         // return File(content, contentType, fileName);

                         byte[] bytes = stream.ToArray();
                         string contentType;
                         new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
                         return Ok(new
                         {
                             StatusCode = "SUCCESS",
                             StatusText = "",
                             BaseString = Convert.ToBase64String(bytes),
                             ContentType = contentType ?? "application/octet-stream"
                         });
                     }
                 }
                */
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetAppraisalStatusReportInExcel");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                BaseString = "",
                ContentType = ""
            });
        }
        #endregion

        #region Add or Update Individual Appraisal Rating
        [HttpPost]
        [Route("AddorUpdateIndividualAppraisalRating")]
        public async Task<IActionResult> AddorUpdateIndividualAppraisalRating(IndividualAppraisalAddView appraisalDetailsView)
        {
            string statusText = "";
            try
            {
                var result = await _client.PostAsJsonAsync(appraisalDetailsView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddorUpdateIndividualAppraisalRating"));
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                if (isSuccess)
                {
                    if (appraisalDetailsView.IsSubmit)
                    {
                        EmployeeandManagerView employeeandManager = new();
                        var employeeID = appraisalDetailsView.EmployeeKeyResultRatings.FirstOrDefault();
                        var results = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + employeeID.EMPLOYEE_ID);
                        employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(results?.Data));
                        List<Notifications> notifications = new();
                        Notifications notification = new();
                        notification = new()
                        {
                            CreatedBy = employeeID == null ? 0 : (int)employeeandManager.EmployeeID,
                            CreatedOn = DateTime.UtcNow,
                            FromId = employeeandManager?.EmployeeID == null ? 0 : (int)employeeandManager.EmployeeID,
                            ToId = employeeandManager?.ReportingManagerID == null ? 0 : (int)employeeandManager.ReportingManagerID,
                            MarkAsRead = false,
                            NotificationSubject = "You have been requested to review appraisal from  " + employeeandManager.EmployeeName + ".",
                            NotificationBody = "You have been requested to review appraisal from  " + employeeandManager.EmployeeName + ".",
                            PrimaryKeyId = appraisalDetailsView.EmployeeKeyResultRatings.FirstOrDefault().APP_CYCLE_ID,
                            ButtonName = "Review Appraisal",
                            SourceType = "Appraisal"
                        };
                        notifications.Add(notification);
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                        if (notificationResponse?.IsSuccessStatusCode == false)
                        {
                            statusText = notificationResult?.Result?.StatusText;
                        }
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        var Subject = @"Request - Review {EmployeeName} self - appraisal.";
                        var Body = @"<html>
                                    <body>
                                   
                                    <p>Dear {ManagerName},</p>
                                    
                                    <p>{EmployeeName} has requested to review self - appraisal.</p>   
                                    <p>Thanks & Regards,</p>
                                    <p>
                                    {EmployeeName}
                                    </p>
                                    </body>
                                    </html>";
                        Subject = Subject.Replace("{EmployeeName}", employeeandManager.EmployeeName);
                        Body = Body.Replace("{ManagerName}", employeeandManager.ManagerName);
                        Body = Body.Replace("{EmployeeName}", employeeandManager.EmployeeName);
                        SendEmailView sendMailbyIndividual = new SendEmailView();
                        sendMailbyIndividual = new SendEmailView
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = employeeandManager.ManagerEmailID,
                            Subject = Subject,
                            MailBody = Body,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                        };
                        SendEmail.Sendmail(sendMailbyIndividual);
                    }
                    return Ok(new
                    {
                        result?.StatusCode,
                        result?.StatusText,
                        Data = isSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateIndividualAppraisalRating");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });
        }
        #endregion

        #region Get employee name by id
        [NonAction]
        public async Task<List<EmployeeName>> GetEmployeeNameById(List<int?> lstEmployeeId)
        {
            List<EmployeeName> lstEmployeeName = new();
            try
            {
                using var employeeNameClient = new HttpClient
                {
                    BaseAddress = new Uri(_employeeBaseURL)
                };
                var lstEmpId = lstEmployeeId.Where(x => x != 0).Select(x => x).Distinct().ToList();
                HttpResponseMessage employeenameResponse = await employeeNameClient.PostAsJsonAsync("Employee/GetEmployeeNameById", lstEmpId);
                if (employeenameResponse?.IsSuccessStatusCode == true)
                {
                    var employeenameResult = employeenameResponse.Content.ReadAsAsync<SuccessData>();
                    lstEmployeeName = JsonConvert.DeserializeObject<List<EmployeeName>>(JsonConvert.SerializeObject(employeenameResult.Result.Data));
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetEmployeeNameById", JsonConvert.SerializeObject(lstEmployeeId));
            }
            return lstEmployeeName;
        }
        #endregion

        #region Delete Objective KRA Mapping
        [HttpGet]
        [Route("DeleteObjectiveKRAMapping")]
        public async Task<IActionResult> DeleteObjectiveKRAMapping(int versionId, int departmentId, int roleId, int objectiveId, int kRAId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteObjectiveKRAMapping") + versionId + "&departmentId=" + departmentId + "&roleId=" + roleId + "&objectiveId=" + objectiveId + "&kRAId=" + kRAId);
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    result?.StatusCode,
                    result?.StatusText,
                    Data = isSuccess
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteObjectiveKRAMapping");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Delete Role Objective Mapping
        [HttpGet]
        [Route("DeleteRoleObjectiveMapping")]
        public async Task<IActionResult> DeleteRoleObjectiveMapping(int versionId, int departmentId, int roleId, int objectiveId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteRoleObjectiveMapping") + versionId + "&departmentId=" + departmentId + "&roleId=" + roleId + "&objectiveId=" + objectiveId);
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    result?.StatusCode,
                    result?.StatusText,
                    Data = isSuccess
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteRoleObjectiveMapping");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Delete Department Role Mapping
        [HttpGet]
        [Route("DeleteDepartmentRoleMapping")]
        public async Task<IActionResult> DeleteDepartmentRoleMapping(int versionId, int departmentId, int roleId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteDepartmentRoleMapping") + versionId + "&departmentId=" + departmentId + "&roleId=" + roleId);
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    result?.StatusCode,
                    result?.StatusText,
                    Data = isSuccess
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteDepartmentRoleMapping");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Delete Version Department Mapping
        [HttpGet]
        [Route("DeleteVersionDepartmentMapping")]
        public async Task<IActionResult> DeleteVersionDepartmentMapping(int versionId, int departmentId)
        {
            try
            {
                var result = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteVersionDepartmentMapping") + versionId + "&departmentId=" + departmentId);
                bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(result?.Data));
                return Ok(new
                {
                    result?.StatusCode,
                    result?.StatusText,
                    Data = isSuccess
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/DeleteVersionDepartmentMapping");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Get EmployeesAppraisal By ManagerID
        [HttpGet]
        [Route("GetEmployeesAppraisalByManagerID")]
        public async Task<IActionResult> GetEmployeesAppraisalByManagerID(int appcycleID, int managerID)
        {
            List<EmployeeAppraisalByManager> empAppraisalmaster = new();
            EmployeeAppraisalByManagerView employeebymanager = new();
            try
            {
                var master = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEmployeeAppraisalMasterByManagerID") + appcycleID + "&managerID=" + managerID);
                empAppraisalmaster = JsonConvert.DeserializeObject<List<EmployeeAppraisalByManager>>(JsonConvert.SerializeObject(master?.Data));
                EmployeelistForAppraisalMaster employeeDetail = new();
                employeeDetail.ManagerID = managerID;
                if (empAppraisalmaster?.Count > 0)
                {
                    employeeDetail.listEmployeeID = empAppraisalmaster.Select(x => x.EMPLOYEE_ID).ToList();
                }
                var employee = await _client.PostAsJsonAsync(employeeDetail, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:AppraisalManagerEmployeeDetails"));
                AppraisalManagerEmployeeDetailsView employeetable = JsonConvert.DeserializeObject<AppraisalManagerEmployeeDetailsView>(JsonConvert.SerializeObject(employee?.Data));
                if (employeetable != null && empAppraisalmaster?.Count > 0)
                {
                    employeebymanager.AppCycleID = appcycleID;
                    employeebymanager.EmployeeManagerID = managerID;
                    employeebymanager.EmployeeManagerName = employeetable.ReportingManagerName;
                    employeebymanager.ManagerDeptID = employeetable.ReportingManagerDeptID;
                    employeebymanager.ManagerRoleID = employeetable.ReportingManagerRoleID;
                    employeebymanager.EmployeeLists = (from empapps in empAppraisalmaster
                                                       select new AppraisalEmployeeLists
                                                       {
                                                           AppCycleID = empapps.APP_CYCLE_ID,
                                                           EntityID = empapps.ENTITY_ID,
                                                           EmployeeID = empapps.EMPLOYEE_ID,
                                                           EmployeeDeptID = empapps.EMPLOYEE_DEPT_ID,
                                                           EmployeeRoleID = empapps.EMPLOYEE_ROLE_ID,
                                                           FullName = employeetable.EmployeeDetails.Where(x => x.EmployeeId == empapps.EMPLOYEE_ID).Select(x => x.EmployeeFullName).FirstOrDefault(),
                                                           Status = empapps.APPRAISAL_STATUS_NAME
                                                       }).ToList();
                }
                if (employeebymanager != null)
                {
                    return Ok(new
                    {
                        master?.StatusCode,
                        master?.StatusText,
                        Data = employeebymanager
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetEmployeesAppraisalByManagerID");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = employeebymanager
            });
        }
        #endregion

        #region Add or update Appraisal BU Head Comments        
        [HttpPost]
        [Route("AddOrUpdateBUHeadComments")]
        public async Task<IActionResult> AddOrUpdateBUHeadComment(AppBUHeadCommentsView appBUHeadCommentsView)
        {
            int commentId = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(appBUHeadCommentsView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddOrUpdateBUHeadComments"));
                List<EmployeeAppraisalByManager> employeeList = JsonConvert.DeserializeObject<List<EmployeeAppraisalByManager>>(JsonConvert.SerializeObject(result?.Data));
                //EmployeeListByDepartment employeeView = new();
                //employeeView.EmployeeId = employeeList.Where(x => x.APPRAISAL_STATUS_NAME == "Management Review Completed").Select(x => x.EMPLOYEE_ID).ToList();
                //employeeView.DepartmentId = appBUHeadCommentsView.Department_Id;
                //var results = await _client.PostAsJsonAsync(employeeView, _employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeesListByDepartment"));
                //List<EmployeeandManagerView> employeeandManager = JsonConvert.DeserializeObject<List<EmployeeandManagerView>>(JsonConvert.SerializeObject(results?.Data));
                var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                string Subject = @"Sub -> {DepartmentName} department BU head approved.";
                string Body = @"<html>
                                    <body>
                                   
                                    <p>Dear Team,</p>
                                   
                                    <p>{DepartmentName} department BU head has approved employee rating. Please take needful action.</p>   
                                    <p>Thanks & Regards,</p>
                                   <p>{BUHeadName}</p>
                                    </body>
                                    </html>";
                Body = Body.Replace("{DepartmentName}", appBUHeadCommentsView.DepartmentName);
                Body = Body.Replace("{BUHeadName}", appBUHeadCommentsView.BUHeadName);
                Subject = Subject.Replace("{DepartmentName}", appBUHeadCommentsView.DepartmentName);
                SendEmailView sendMailbyIndividual = new SendEmailView();
                sendMailbyIndividual = new SendEmailView
                {
                    FromEmailID = appsetting.GetSection("FromEmailId").Value,
                    ToEmailID = appsetting.GetSection("PeopleExperience").Value,
                    Subject = Subject,
                    MailBody = Body,
                    ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                    Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                    Host = appsetting.GetSection("EmailHost").Value,
                    FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                };
                SendEmail.Sendmail(sendMailbyIndividual);
                if (result != null)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "Approved successfully"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "Failure",
                        result?.StatusText,

                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddOrUpdateBUHeadComment", JsonConvert.SerializeObject(appBUHeadCommentsView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                commentId
            });
        }
        #endregion

        #region Copy and Create Version Details
        [HttpPost]
        [Route("CopyandCreateVersionDetails")]
        public async Task<IActionResult> CopyandCreateVersionDetails(CopyVersion copyVersion)
        {
            try
            {
                var copyResult = await _client.PostAsJsonAsync(copyVersion, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:CopyandAddVersionDetails"));
                int VersionId = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(copyResult?.Data));
                if (VersionId < 0)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "New version created successfully.",
                        Data = VersionId
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/CopyandCreateVersionDetails");
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = false
            });
        }
        #endregion

        #region Approve Or Reject By Manager
        [HttpPost]
        [Route("ApproveOrRejectByManager")]
        public async Task<IActionResult> ApproveOrRejectByManager(AddApproveandRejectByManagerView DetailsView)
        {
            string statusText = "";
            try
            {
                var result = await _client.PostAsJsonAsync(DetailsView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:ApproveOrRejectByManager"));
                if (result.StatusCode == "SUCCESS")
                {
                    var EmployeID = DetailsView?.EmployeeId;
                    EmployeeandManagerView employeeandManager = new();
                    var results = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeandManagerByEmployeeID") + EmployeID);
                    employeeandManager = JsonConvert.DeserializeObject<EmployeeandManagerView>(JsonConvert.SerializeObject(results?.Data));
                    string Subject, Body;
                    if (DetailsView.Status == "Appraiser Review Completed" || DetailsView.Status == "Self Appraisal Review Sent Back" || DetailsView.Status == "Self-Appraisal Completed-Feedback")
                    {
                        if (DetailsView.Status == "Appraiser Review Completed")
                        {
                            Subject = "Your Appraiser Review Completed.";
                            Body = "Your Appraiser Review Completed.";
                        }
                        else if (DetailsView.Status == "Self-Appraisal Completed-Feedback")
                        {
                            Subject = "Your Self-Appraisal Completed-Feedback";
                            Body = "Your Self-Appraisal Completed-Feedback";
                        }
                        else //if (result.Data == "Self Appraisal Review Sent Back")
                        {
                            Subject = "Your Self Appraisal Review Sent Back to you.";
                            Body = "Your Self Appraisal Review Sent Back to you.";
                        }
                        var employeeID = DetailsView.EmployeeKeyResultRatingStatus.FirstOrDefault();
                        List<Notifications> notifications = new();
                        Notifications notification = new();
                        notification = new()
                        {
                            CreatedBy = employeeID == null ? 0 : (int)employeeID.EMPLOYEE_ID,
                            CreatedOn = DateTime.UtcNow,
                            FromId = DetailsView?.ManagerID == null ? 0 : (int)DetailsView.ManagerID,
                            ToId = employeeID == null ? 0 : (int)employeeID.EMPLOYEE_ID,
                            MarkAsRead = false,
                            NotificationSubject = Subject,
                            NotificationBody = Body,
                            PrimaryKeyId = DetailsView.EmployeeKeyResultRatingStatus.FirstOrDefault().APP_CYCLE_ID,//employeeID.EMPLOYEE_ID,
                            ButtonName = "view Appraisal",
                            SourceType = "Appraisal"
                        };
                        notifications.Add(notification);
                        using var notificationClient = new HttpClient
                        {
                            BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationURL:Notifications"))
                        };
                        HttpResponseMessage notificationResponse = await notificationClient.PostAsJsonAsync("Notifications/InsertNotifications", notifications);
                        var notificationResult = notificationResponse.Content.ReadAsAsync<SuccessData>();
                        if (notificationResponse?.IsSuccessStatusCode == false)
                        {
                            statusText = notificationResult?.Result?.StatusText;
                        }
                        var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                        string MailSubject, MailBody;
                        if (result.Data == "Appraiser Review Completed" || DetailsView.Status == "Self-Appraisal Completed-Feedback")
                        {
                            MailSubject = @"Appraisal review completed.";
                            MailBody = @"<html>
                                    <body>
                                    
                                    <p>Dear {EmployeeName},</p>
                                    
                                    <p>{ManagerName} has approved your self-appraisal.</p>   
                                    <p>Thanks & Regards,</p>
                                    <p>
                                    {ManagerName}
                                    </p>
                                    </body>
                                    </html>";
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager.EmployeeName);
                            MailBody = MailBody.Replace("{ManagerName}", employeeandManager.ManagerName);
                        }
                        else //if(result.Data == "Self Appraisal Review Sent Back")
                        {
                            MailSubject = @"Self Appraisal review sent back";
                            MailBody = @"<html>
                                    <body>
                                   
                                    <p>Dear {EmployeeName},</p>
                                   
                                    <p>{ManagerName} has requested details in self - appraisal. Please review comments and take needful action. </p>   
                                    <p>Thanks & Regards, </p>
                                    <p>
                                    {ManagerName}
                                    
                                    </p>
                                    </body>
                                    </html>";
                            MailBody = MailBody.Replace("{EmployeeName}", employeeandManager.EmployeeName);
                            MailBody = MailBody.Replace("{ManagerName}", employeeandManager.ManagerName);
                        }

                        SendEmailView sendMailbyIndividual = new();
                        sendMailbyIndividual = new()
                        {
                            FromEmailID = appsetting.GetSection("FromEmailId").Value,
                            ToEmailID = employeeandManager.EmployeeEmailID,
                            Subject = MailSubject,
                            MailBody = MailBody,
                            ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                            Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                            Host = appsetting.GetSection("EmailHost").Value,
                            FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value
                        };
                        SendEmail.Sendmail(sendMailbyIndividual);
                    }
                    return Ok(new
                    {
                        StatusCode = result?.StatusCode,
                        StatusText = result?.StatusText,
                        result?.Data
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/ApproveOrRejectByManager", JsonConvert.SerializeObject(DetailsView));
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText
            });
        }
        #endregion

        #region Add or Update KRA Using Excel   
        [HttpPost]
        [Route("AddorUpdateKeyResultByExcel")]
        public async Task<IActionResult> AddorUpdateKeyResultByExcel(IFormFile uploafFile)
        {
            string statusText = "";
            try
            {
                if (uploafFile.Length > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = new MemoryStream())
                    {
                        uploafFile.CopyTo(stream);
                        SharedLibraries.ViewModels.Appraisal.ImportExcelView import = new();
                        import.Base64Format = Convert.ToBase64String(stream.ToArray());
                        var results = await _client.PostAsJsonAsync(import, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddorUpdateKeyResultByExcel"));
                        bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
                        return Ok(new
                        {
                            StatusCode = results?.StatusCode,
                            StatusText = results?.StatusText,
                            Data = isSuccess
                        });
                    }
                }
                else
                {
                    statusText = "File content was empty. Please try again.";
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateKeyResultByExcel");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });

        }
        #endregion

        #region Add or Update Entity Master Using Excel   
        [HttpPost]
        [Route("AddorUpdateEntityByExcel")]
        public async Task<IActionResult> AddorUpdateEntityByExcel(IFormFile uploafFile)
        {
            string statusText = "";
            try
            {
                if (uploafFile.Length > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = new MemoryStream())
                    {
                        uploafFile.CopyTo(stream);
                        SharedLibraries.ViewModels.Appraisal.ImportExcelView import = new();
                        import.Base64Format = Convert.ToBase64String(stream.ToArray());
                        var results = await _client.PostAsJsonAsync(import, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddorUpdateEntityByExcel"));
                        bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
                        return Ok(new
                        {
                            StatusCode = results?.StatusCode,
                            StatusText = results?.StatusText,
                            Data = isSuccess
                        });
                    }
                }
                else
                {
                    statusText = "File content was empty. Please try again.";
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateEntityByExcel");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });
        }
        #endregion

        #region Add or Update Objective Master Using Excel   
        [HttpPost]
        [Route("AddorUpdateObjectiveByExcel")]
        public async Task<IActionResult> AddorUpdateObjectiveByExcel(IFormFile uploafFile)
        {
            string statusText = "";
            try
            {
                if (uploafFile.Length > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = new MemoryStream())
                    {
                        uploafFile.CopyTo(stream);
                        SharedLibraries.ViewModels.Appraisal.ImportExcelView import = new();
                        import.Base64Format = Convert.ToBase64String(stream.ToArray());
                        var results = await _client.PostAsJsonAsync(import, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddorUpdateObjectiveByExcel"));
                        bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
                        return Ok(new
                        {
                            StatusCode = results?.StatusCode,
                            StatusText = results?.StatusText,
                            Data = isSuccess
                        });
                    }
                }
                else
                {
                    statusText = "File content was empty. Please try again.";
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateObjectiveByExcel");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });
        }
        #endregion

        #region Add or Update Appraisal Cycle Using Excel   
        [HttpPost]
        [Route("AddorUpdateAppraisalCycleByExcel")]
        public async Task<IActionResult> AddorUpdateAppraisalCycleByExcel(IFormFile uploafFile)
        {
            string statusText = "";
            try
            {
                if (uploafFile.Length > 0)
                {
                    var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeTypeList"));
                    List<EmployeesTypes> employeeType = JsonConvert.DeserializeObject<List<EmployeesTypes>>(JsonConvert.SerializeObject(employeeResult?.Data));
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = new MemoryStream())
                    {
                        uploafFile.CopyTo(stream);
                        SharedLibraries.ViewModels.Appraisal.ImportExcelView import = new();
                        import.Base64Format = Convert.ToBase64String(stream.ToArray());
                        import.EmployeesTypes = employeeType;
                        var results = await _client.PostAsJsonAsync(import, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddorUpdateAppraisalCycleByExcel"));
                        bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
                        return Ok(new
                        {
                            StatusCode = results?.StatusCode,
                            StatusText = results?.StatusText,
                            Data = isSuccess
                        });
                    }
                }
                else
                {
                    statusText = "File content was empty. Please try again.";
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateAppraisalCycleByExcel");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });
        }
        #endregion

        #region Add or Update Version Using Excel
        [HttpPost]
        [Route("AddorUpdateVersionByExcel")]
        public async Task<IActionResult> AddorUpdateVersionByExcel(IFormFile uploafFile)
        {
            string statusText = "";
            try
            {
                if (uploafFile.Length > 0)
                {
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var stream = new MemoryStream())
                    {
                        uploafFile.CopyTo(stream);
                        AppraisalMasterData employeeMasterdata = new();
                        var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                        employeeMasterdata = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                        SharedLibraries.ViewModels.Appraisal.ImportExcelView import = new SharedLibraries.ViewModels.Appraisal.ImportExcelView();
                        import.Base64Format = Convert.ToBase64String(stream.ToArray());
                        import.Roles = employeeMasterdata.Roles;
                        import.Department = employeeMasterdata.Department;
                        var results = await _client.PostAsJsonAsync(import, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddorUpdateVersionByExcel"));
                        bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
                        return Ok(new
                        {
                            StatusCode = "SUCCESS",
                            StatusText = "Version Imported Successfully."
                        });
                    }
                }
                else
                {
                    statusText = "File content was empty. Please try again.";
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddorUpdateVersionByExcel");
                statusText = strErrorMsg;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = false
            });
        }
        #endregion

        #region Individual Appraisal Document Download By ID
        [HttpGet]
        [Route("IndividualAppraisalDocumentDownloadByID")]
        public async Task<IActionResult> IndividualAppraisalDocumentDownloadByID(int documentID)
        {
            try
            {
                EmployeeKeyResultAttachments documents = new();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_appraisalBaseURL);
                    HttpResponseMessage responseData = await client.GetAsync("Appraisal/IndividualAppraisalDocumentDownloadByID?documentID=" + documentID);
                    if (responseData?.IsSuccessStatusCode == true)
                    {
                        var result = responseData.Content.ReadAsAsync<SuccessData>();
                        documents = JsonConvert.DeserializeObject<EmployeeKeyResultAttachments>(JsonConvert.SerializeObject(result.Result.Data));
                        //Read the File into a Byte Array.
                        byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(documents.DOC_URL, documents.DOC_NAME));
                        string contentType;
                        new FileExtensionContentTypeProvider().TryGetContentType(documents.DOC_NAME, out contentType);
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
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/IndividualAppraisalDocumentDownloadByID", Convert.ToString(documentID));
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

        #region BU Head Revert By Employee        
        [HttpPost]
        [Route("BUHeadRevertByEmployee")]
        public async Task<IActionResult> BUHeadRevertByEmployee(BuHeadRevertByEmployeeView buHeadRevertByEmployee)
        {
            try
            {
                var result = await _client.PostAsJsonAsync(buHeadRevertByEmployee, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:BUHeadRevertByEmployee"));
                if (result?.StatusCode == "SUCCESS")
                {
                    string Subject = @"BU head request to revert {EmployeeName} appraisal rating.";
                    string Body = @"<html>
                                    <body>
                                    
                                    <p>Dear {ManagerName},</p>
                                    
                                    <p>Please revert appraisal rating for below employee.</p>
                                    
                                    <p>Employee Name - {EmployeeName}.</p>
                                    <p>Employee Email ID - {EmployeeMailID}</p>
                                    <p>If you have any query, Please let us know.</p>
                                    <p>Thanks & Regards,</p>
                                   <p>
                                    {BUHeadName}
                                    </p>
                                    </body>
                                    </html>";
                    Body = Body.Replace("{ManagerName}", buHeadRevertByEmployee?.ManagerName);
                    Body = Body.Replace("{EmployeeName}", buHeadRevertByEmployee?.EmployeeName);
                    Body = Body.Replace("{EmployeeMailID}", buHeadRevertByEmployee?.EmployeeMailID);
                    Body = Body.Replace("{BUHeadName}", buHeadRevertByEmployee?.BUHeadName);
                    Subject = Subject.Replace("{EmployeeName}", buHeadRevertByEmployee?.EmployeeName);
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    SendEmailView sendMailbyIndividual = new();
                    sendMailbyIndividual = new()
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = buHeadRevertByEmployee.ManagerMailID,
                        Subject = Subject,
                        MailBody = Body,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = buHeadRevertByEmployee.EmployeeMailID + "," + buHeadRevertByEmployee.BUHeadMailID + "," + appsetting.GetSection("HRGroup").Value
                    };
                    SendEmail.Sendmail(sendMailbyIndividual);
                    return Ok(new
                    {
                        StatusCode = result?.StatusCode,
                        StatusText = result?.StatusText,
                        result?.Data
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/BUHeadRevertByEmployee", JsonConvert.SerializeObject(buHeadRevertByEmployee));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Add Self Appraisal BUHead Comment      
        [HttpPost]
        [Route("AddSelfAppraisalBUHeadComment")]
        public async Task<IActionResult> AddSelfAppraisalBUHeadComment(EmployeeAppraisalBUHeadComment employeeAppraisalComment)
        {
            int CommentID = 0;
            try
            {
                var result = await _client.PostAsJsonAsync(employeeAppraisalComment, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:AddSelfAppraisalBUHeadComment"));
                CommentID = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && CommentID > 0)
                {
                    string Subject = "";
                    string Body = "";
                    var data = "";
                    var ToMailID = "";
                    Subject = @"Regards - {BUHeadName} appraisal Comments.";
                    Body = @"<html>
                                    <body>
                                    <b>
                                    <p>Dear {EmployeeName},</p>
                                    </b>
                                    <p> {BUHeadName} has added below comments ,Please review and take needful action.</p>                                     
                                    <p> {Comments} </p>
                                     <p> Thanks & Regards,  </p>
                                     <p> {BUHeadName} </p>
                                    </body>
                                    </html>";
                    data = Body.Replace("{BUHeadName}", employeeAppraisalComment.BUHeadName);
                    data = data.Replace("{EmployeeName}", employeeAppraisalComment.EmployeeName);
                    data = data.Replace("{Comments}", employeeAppraisalComment.Comment);
                    Subject = Subject.Replace("{BUHeadName}", employeeAppraisalComment.BUHeadName);
                    ToMailID = employeeAppraisalComment.EmployeeEmailId;
                    var appsetting = _configuration.GetSection("ApplicationURL:EmailSettings");
                    SendEmailView sendMailbyIndividual = new SendEmailView();
                    sendMailbyIndividual = new SendEmailView
                    {
                        FromEmailID = appsetting.GetSection("FromEmailId").Value,
                        ToEmailID = ToMailID,
                        Subject = Subject,
                        MailBody = data,
                        ResourceEmail = appsetting.GetSection("ResourceEmail").Value,
                        Port = Convert.ToInt32(appsetting.GetSection("EmailPort").Value),
                        Host = appsetting.GetSection("EmailHost").Value,
                        FromEmailPassword = appsetting.GetSection("FromEmailPassword").Value,
                        CC = (employeeAppraisalComment.BUHeadEmailId == employeeAppraisalComment.ManagerEmailId ? "" : employeeAppraisalComment.BUHeadEmailId + ",") + employeeAppraisalComment.ManagerEmailId
                    };
                    SendEmail.Sendmail(sendMailbyIndividual);
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = "Appraisal Comment Updated successfully",
                        CommentID
                    });
                }
                else
                {
                    return Ok(new
                    {
                        result?.StatusCode,
                        StatusText = result?.StatusText,
                        CommentID
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/AddSelfAppraisalComment", JsonConvert.SerializeObject(employeeAppraisalComment));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                CommentID
            });
        }
        #endregion

        #region Get Employee Appraisal Details by BU Head
        [HttpGet]
        [Route("GetEmployeeAppraisalListByBUHead")]
        public async Task<IActionResult> GetEmployeeAppraisalListByBUHead(int BuHeadId, bool IsAdmin)
        {
            List<EmployeeViewDetails> EmployeeList = new();
            List<EmployeeViewDetails> actualEmployeeList = new();
            EmployeeListAndDepartment empListDepartment = new();
            //List<int> managerId = new List<int>();
            empListDepartment.IsAllReportees = true;
            empListDepartment.IsAdmin = IsAdmin;
            bool isAll = true;
            try
            {
                // managerId.Add(BuHeadId);
                AppraisalMasterData appraisalMasterData = new();
                List<EmployeeAppraisalMasterDetailView> EmployeAppraisalList = new();
                var employeeResult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetEmployeeAppraisalMasterData"));
                appraisalMasterData = JsonConvert.DeserializeObject<AppraisalMasterData>(JsonConvert.SerializeObject(employeeResult?.Data));
                if (IsAdmin == true)
                {
                    empListDepartment.employeeids = appraisalMasterData?.ReportingManagerEmployeeList?.Select(ea => ea.EmployeeId).ToList();
                }
                else
                {
                    var employeeListresult = await _client.GetAsync(_employeeBaseURL, _configuration.GetValue<string>("ApplicationURL:Employees:GetAllEmployeeListForManagerReport") + BuHeadId + "&isAll=" + isAll);
                    EmployeeList = JsonConvert.DeserializeObject<List<EmployeeViewDetails>>(JsonConvert.SerializeObject(employeeListresult.Data));
                    empListDepartment.employeeids = EmployeeList.Select(ea => ea.EmployeeId).ToList();
                }
                var employeeappraisalresult = await _client.PostAsJsonAsync(empListDepartment, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetEmployeeAppraisalListByEmployeeIdForReview"));
                EmployeAppraisalList = JsonConvert.DeserializeObject<List<EmployeeAppraisalMasterDetailView>>(JsonConvert.SerializeObject(employeeappraisalresult.Data));

                /*********************** EmployeEmployeeViewDetailse Appraisal Status View ******************************/
                EmployeeAppraisalReportView employeeappraisalstatus = new();
                if (appraisalMasterData != null && EmployeAppraisalList != null)
                {
                    foreach (EmployeeAppraisalMasterDetailView appraisal in EmployeAppraisalList)
                    {
                        if (appraisalMasterData?.ReportingManagerEmployeeList?.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Any() == true)
                        {
                            EmployeeViewDetails employeeDetail = new EmployeeViewDetails();
                            employeeDetail.EmployeeId = appraisal.EMPLOYEE_ID; // Convert.ToInt32(string.Format("{0, 0:D6}", rs.ea.EMPLOYEE_ID)),
                            string formattedEmployeeId = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Select(x => x.FormattedEmployeeId).FirstOrDefault();
                            employeeDetail.FormattedEmployeeId = formattedEmployeeId == null ? "" : formattedEmployeeId; //rs.ea.EMPLOYEE_ID == 0 ? "" : "EMP" + (10000 + rs.ea.EMPLOYEE_ID).ToString(), // string.Format("TVSN_{0, 0:D6}", rs.ea.EMPLOYEE_ID),
                            employeeDetail.EmployeeName = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Select(x => x.EmployeeName).FirstOrDefault();
                            employeeDetail.EmployeeEmail = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_ID).Select(x => x.EmployeeEmailId).FirstOrDefault(); ;
                            //   EmployeeType = rs.e.EmployeeType,
                            employeeDetail.RoleId = appraisal.EMPLOYEE_ROLE_ID;
                            employeeDetail.DepartmentId = appraisal.EMPLOYEE_DEPT_ID;
                            employeeDetail.RoleName = appraisalMasterData.Roles.Where(x => x.RoleId == appraisal.EMPLOYEE_ROLE_ID).Select(x => x.RoleName).FirstOrDefault();
                            employeeDetail.DepartmentName = appraisalMasterData.Department.Where(x => x.DepartmentId == appraisal.EMPLOYEE_DEPT_ID).Select(x => x.DepartmentName).FirstOrDefault();
                            employeeDetail.ReportingTo = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_MANAGER_ID).Select(x => x.EmployeeName).FirstOrDefault();
                            employeeDetail.ReportingEmail = appraisalMasterData.ReportingManagerEmployeeList.Where(x => x.EmployeeId == appraisal.EMPLOYEE_MANAGER_ID).Select(x => x.EmployeeEmailId).FirstOrDefault();
                            actualEmployeeList.Add(employeeDetail);
                        }
                    }
                }


                return Ok(new
                {
                    StatusCode = "Success",
                    StatusText = "",
                    Data = actualEmployeeList
                });

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetEmployeeAppraisalListByBUHead");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = strErrorMsg,
                    Data = new EmployeeViewDetails()
                });
            }
        }
        #endregion

        #region Get WorkDaya Employee Appraisal Details by Filter     
        [HttpPost]
        [Route("GetWorkDayEmployeeAppraisalListByFilter")]
        public async Task<IActionResult> GetWorkDayEmployeeAppraisalListByFilter(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            List<AppraisalWorkDayView> appraisalWorkDayViewList = new();
            try
            {
                var result = await _client.PostAsJsonAsync(appraisalWorkDayFilterView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetWorkDayEmployeeAppraisalListByFilter"));
                appraisalWorkDayViewList = JsonConvert.DeserializeObject<List<AppraisalWorkDayView>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && appraisalWorkDayViewList != null)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "",
                        Data = appraisalWorkDayViewList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetWorkDayEmployeeAppraisalListByFilter", JsonConvert.SerializeObject(appraisalWorkDayFilterView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = appraisalWorkDayViewList
            });
        }
        #endregion

        #region Get WorkDay Detail List
        [HttpPost]
        [Route("GetWorkDayDetailList")]
        public async Task<IActionResult> GetWorkDayDetailList(WorkdayFilterView workdayFilterView)
        {
            List<WorkdayListView> appraisalWorkDayViewList = new();
            List<EmployeeProjectNames> projectList = new();
            try
            {
                var result = await _client.PostAsJsonAsync(workdayFilterView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:GetWorkDayDetailList"));
                appraisalWorkDayViewList = JsonConvert.DeserializeObject<List<WorkdayListView>>(JsonConvert.SerializeObject(result?.Data));
                if (result != null && projectList != null)
                {
                    return Ok(new
                    {
                        StatusCode = "Success",
                        StatusText = "",
                        Data = appraisalWorkDayViewList
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetWorkDayDetailList", JsonConvert.SerializeObject(workdayFilterView));
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg,
                Data = appraisalWorkDayViewList
            });
        }
        #endregion

        #region Save or Update WorkDay Detail
        /// <summary>
        /// Save or update the WorkDay Detail
        /// </summary>
        /// <param name="detailView"></param>
        /// <returns></returns>
        [HttpPost("SaveOrUpdateWorkDayDetail")]
        public async Task<IActionResult> SaveOrUpdateWorkDayDetail(WorkdayInputView detailView)
        {
            try
            {
                var results = await _client.PostAsJsonAsync(detailView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:SaveOrUpdateWorkDayDetail"));
                WorkdayInputView WorkDayDetail = JsonConvert.DeserializeObject<WorkdayInputView>(JsonConvert.SerializeObject(results?.Data));
                if (results.StatusCode == "SUCCESS")
                    return Ok(new
                    {
                        StatusCode = WorkDayDetail.WorkDayDetailId > 0 ? "SUCCESS" : "FAILURE",
                        StatusText = WorkDayDetail.WorkDayDetailId > 0 ? "WorkDay Detail is saved successfully." : strErrorMsg,
                        WorkDayDetail
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/SaveOrUpdateWorkDayDetail", JsonConvert.SerializeObject(detailView));

            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Approve Or Reject Workday Detail View
        /// <summary>
        /// Approve Or Reject Workday Detail View
        /// </summary>
        /// <param name="detailView"></param>
        /// <returns></returns>
        [HttpPost("ApproveOrRejectWorkdayDetail")]
        public async Task<IActionResult> ApproveOrRejectWorkdayDetail(ApproveOrRejectWorkdayDetailView detailView)
        {
            var results = await _client.PostAsJsonAsync(detailView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:ApproveOrRejectWorkdayDetail"));
            bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
            return Ok(new
            {
                StatusCode = isSuccess ? "SUCCESS" : "FAILURE",
                StatusText = isSuccess ? "WorkDay Detail status is updated successfully." : strErrorMsg
            });
        }
        #endregion

        #region Delete WorkDay Detail
        /// <summary>
        /// Delete WorkDay Detail
        /// </summary>
        /// <param name="WorkDayDetailId"></param>
        /// <returns></returns>
        [HttpGet("DeleteWorkDayDetail")]
        public async Task<IActionResult> DeleteWorkDayDetail(int WorkDayDetailId)
        {
            var results = await _client.GetAsync(_appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:DeleteWorkDayDetail") +
                                                    "?WorkDayDetailId=" + WorkDayDetailId);
            bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
            return Ok(new
            {
                StatusCode = isSuccess ? "SUCCESS" : "FAILURE",
                StatusText = isSuccess ? "WorkDay Detail are deleted successfully." : strErrorMsg
            });
        }
        #endregion

        #region Save or Update WorkDay Detail
        /// <summary>
        /// Save or update the WorkDay Detail
        /// </summary>
        /// <param name="detailView"></param>
        /// <returns></returns>
        [HttpPost("GetListOfProjectForWorkday")]
        public async Task<IActionResult> GetListOfProjectForWorkday(AppraisalWorkDayFilterView appraisalWorkDayFilterView)
        {
            try
            {
                var results = await _client.PostAsJsonAsync(appraisalWorkDayFilterView, _projectBaseURL, _configuration.GetValue<string>("ApplicationURL:Projects:GetEmployeeProjectListById"));
                List<EmployeeProjectList> employeeProjectLists = JsonConvert.DeserializeObject<List<EmployeeProjectList>>(JsonConvert.SerializeObject(results?.Data));
                if (results.StatusCode == "SUCCESS")
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        Data = employeeProjectLists
                    });
                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateWay", "Appraisal/GetListOfProjectForWorkday", JsonConvert.SerializeObject(appraisalWorkDayFilterView));

            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = strErrorMsg
            });
        }
        #endregion

        #region Approve Or Reject Workday List View
        /// <summary>
        /// Approve Or Reject Workday Detail View
        /// </summary>
        /// <param name="detailView"></param>
        /// <returns></returns>
        [HttpPost("ApproveOrRejectWorkDayListView")]
        public async Task<IActionResult> ApproveOrRejectWorkDayListView(ApproveOrRejectWorkdayListView detailView)
        {
            var results = await _client.PostAsJsonAsync(detailView, _appraisalBaseURL, _configuration.GetValue<string>("ApplicationURL:Appraisal:ApproveOrRejectWorkDayListView"));
            bool isSuccess = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(results?.Data));
            return Ok(new
            {
                StatusCode = isSuccess ? "SUCCESS" : "FAILURE",
                StatusText = isSuccess ? "WorkDay status is updated successfully." : strErrorMsg
            });
        }
        #endregion
    }
}