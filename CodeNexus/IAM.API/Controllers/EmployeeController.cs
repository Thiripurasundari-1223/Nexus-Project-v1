using IAM.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SharedLibraries;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.ViewModels.Notifications;
using SharedLibraries.ViewModels.PolicyManagement;
using SharedLibraries.ViewModels.Reports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        readonly IAzureADToken _azureADToken;
        private readonly IConfiguration _configuration;
        public EmployeeController(IAzureADToken azureADToken, EmployeeService employeeService, IConfiguration configuration)
        {
            _configuration = configuration;
            _employeeService = employeeService;
            _azureADToken = azureADToken;
        }

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "Employee API - GET Method"
            });
        }


        #region Get employee list        
        [HttpGet]
        [Route("GetEmployeeDropDownList")]
        public IActionResult GetEmployeeDropDownList(bool isAll)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeDropDownList(isAll)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/EmployeeDataForDropDown", isAll.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeDataForDropDown>()
                });
            }
        }
        #endregion

        #region Get Department list        
        [HttpGet]
        [Route("GetDepartmentDropDownList")]
        public IActionResult GetDepartmentDropDownList()
        {
            List<Department> depList = new List<Department>();
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _employeeService.GetDepartmentDropDownList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetDepartmentDropDownList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = new List<int>()
                });
            }
        }
        #endregion

        #region Get Designation list        
        [HttpGet]
        [Route("GetDesignationDropDownList")]
        public IActionResult GetDesignationDropDownList()
        {
            List<Designation> desList = new List<Designation>();
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _employeeService.GetDesignationList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetDesignationDropDownList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = new List<int>()
                });
            }
        }
        #endregion

        #region Get Location list        
        [HttpGet]
        [Route("GetLocationDropDownList")]
        public IActionResult GetLocationDropDownList()
        {
            List<EmployeeLocation> locList = new List<EmployeeLocation>();
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _employeeService.GetLocationDropDownList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetLocationDropDownList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = locList
                });
            }
        }
        #endregion

        #region Add or update employees        
        [HttpPost]
        [Route("AddOrUpdateEmployee")]
        public async Task<IActionResult> AddOrUpdateEmployee(EmployeesViewModel employee)
        {
            try
            {
                EmployeeNotificationData data = await _employeeService.AddOrUpdateEmployee(employee);
                if (data.EmployeeId == -1)
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Formatted employee id (" + employee?.Employee?.FormattedEmployeeId + ") already exists!",
                        Data = data
                    });
                }
                else if (data.EmployeeId == -2)
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Email Id(" + employee?.Employee?.EmailAddress + ") already exists!",
                        Data = data
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "",
                        Data = data
                    });
                }


            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddOrUpdateEmployee", JsonConvert.SerializeObject(employee));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeNotificationData()
                });
            }
        }
        #endregion

        #region Get employee by employee id        
        [HttpGet]
        [Route("GetEmployeeDetailsByEmployeeId")]
        public IActionResult GetEmployeeDetailsByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeDetailsByEmployeeId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeDetailsByEmployeeId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeesViewModel()
                });
            }
        }
        #endregion

        #region Get employee master data        
        [HttpGet]
        [Route("GetEmployeeMasterData")]
        public IActionResult GetEmployeeMasterData()
        {
            try
            {
                string employeeIdFormat = _configuration.GetValue<string>("EmployeeIdFormat");
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeMasterData(employeeIdFormat)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeMasterData()
                });
            }
        }
        #endregion

        #region Get employee master data  ForOrgChart       
        [HttpGet]
        [Route("GetEmployeeMasterDataForOrgChart")]
        public IActionResult GetEmployeeMasterDataForOrgChart()
        {
            try
            {
                string employeeIdFormat = _configuration.GetValue<string>("EmployeeIdFormat");
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeMasterDataForOrgChart(employeeIdFormat)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeMasterData()
                });
            }
        }
        #endregion

        #region Delete Employee
        [HttpDelete]
        [Route("DeleteEmployee")]
        public IActionResult DeleteEmployee(int employeeid, int modifiedBy)
        {
            try
            {
                if (_employeeService.DeleteEmployeeById(employeeid, modifiedBy).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        Data = true
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        Data = false

                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/DeleteEmployee", " Employeeid- " + employeeid.ToString() + " ModifiedBy- " + modifiedBy.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }

        #endregion

        #region Add or update department        
        [HttpPost]
        [Route("AddOrUpdateDepartment")]
        public IActionResult AddOrUpdateDepartment(Department department)
        {
            try
            {
                if (_employeeService.DepartmentNameDuplication(department?.DepartmentName))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Department Name is already exists. Please change department name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Department added successfully.",
                        Data = _employeeService.AddOrUpdateDepartment(department).Result
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddOrUpdateDepartment", JsonConvert.SerializeObject(department));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Add or update role        
        [HttpPost]
        [Route("InsertOrUpdateRole")]
        public IActionResult InsertOrUpdateRole(Roles role)
        {
            try
            {
                if (_employeeService.RoleNameDuplication(role?.RoleName))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Role Name is already exists. Please change role name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Role added successfully.",
                        Data = _employeeService.InsertOrUpdateRole(role).Result
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/InsertOrUpdateRole", JsonConvert.SerializeObject(role));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }

        }
        #endregion

        #region Get employee list    
        /// <summary>
        /// Get employee list
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeesList")]
        public IActionResult GetEmployeesList(string pRole = "")
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeesList(pRole)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesList", pRole);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeDetail>()
                });
            }

        }
        #endregion

        #region Get all employee list    
        /// <summary>
        /// Get employee list
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllEmployeesList")]
        public IActionResult GetAllEmployeesList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetAllEmployeesList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeDetail>()
                });
            }

        }
        #endregion

        #region Get employee name    
        /// <summary>
        /// Get employee name
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeNameById")]
        public IActionResult GetEmployeeNameById(List<int> listEmployeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeNameById(listEmployeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeNameById", JsonConvert.SerializeObject(listEmployeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeName>()
                });
            }
        }
        #endregion

        #region Get finance manager id    
        /// <summary>
        /// Get finance manager id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetfinanceManagerId")]
        public IActionResult GetfinanceManagerId()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetfinanceManagerId()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetfinanceManagerId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get reporting employee    
        /// <summary>
        /// Get reporting employee  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetReportingEmployeeById")]
        public IActionResult GetReportingEmployeeById(int? employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetReportingEmployeeById(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetReportingEmployeeById", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<int>()
                });
            }
        }
        #endregion

        #region Get reporting manager list    
        /// <summary>
        /// Get reporting manager list    
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeList")]
        public IActionResult GetEmployeeList(string pRole = "")
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeList(pRole)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeList", pRole);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion

        #region Login
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="pUserEmailAddress"></param>
        /// <param name="pPassWord"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login(Login loginParam)
        {
            try
            {
                string result = _azureADToken.GetToken(loginParam?.UserName, loginParam?.Password);
                if (result.ToString() == "")
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = AuthentcationConstant.UNAUTHORIZED,
                        Data = string.Empty
                    });
                }
                else if (result.ToString() == "inactive")
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Your account has been deactivated, Please contact HR.",
                        Data = string.Empty
                    });
                }
                else if (result.ToString() == "invalid")
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Please enter valid username or password.",
                        Data = string.Empty
                    });
                }
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/Login");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = string.Empty
                });
            }
        }
        #endregion

        #region Get role name by id
        /// <summary>
        /// Get role name by id
        /// </summary>
        [HttpPost("GetRoleNameById")]
        public IActionResult GetRoleNameById(List<int> lstRoleId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetRoleNameById(lstRoleId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetRoleNameById", JsonConvert.SerializeObject(lstRoleId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = new List<RoleName>()
                });
            }
        }
        #endregion

        #region Get team member
        [HttpPost("GetTeamMemberDetails")]
        public IActionResult GetTeamMemberDetails(List<TeamMemberDetails> lstResources)
        {

            string statusText = "";
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = statusText,
                    Data = _employeeService.GetTeamMemberDetails(lstResources)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetTeamMemberDetails", JsonConvert.SerializeObject(lstResources));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new List<TeamMemberDetails>()
                });
            }
        }
        #endregion

        #region Insert Or Update Role Permissions
        [HttpPost("InsertOrUpdateRolePermissions")]
        public IActionResult InsertOrUpdateRolePermissions(List<RolePermissions> rolePermissions)
        {
            string statusText = "";
            try
            {
                if (_employeeService.InsertOrUpdateRolePermissions(rolePermissions).Result)
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = statusText,
                        Data = 1
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/InsertOrUpdateRolePermissions", JsonConvert.SerializeObject(rolePermissions));
                statusText = ex.Message;
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                statusText,
                Data = 0
            });
        }
        #endregion

        #region Get Role Permission's By Email or Role Id
        [HttpGet("GetRolePermissionsByEmailOrRoleId")]
        public IActionResult GetRolePermissionsByEmailOrRoleId(string email, int pRoleId)
        {
            string statusText = "";
            List<RolesDetail> rolePermissionViews = new List<RolesDetail>();
            try
            {
                rolePermissionViews = _employeeService.GetRolePermissionsByEmail(email, pRoleId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = statusText,
                    Data = rolePermissionViews
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetRolePermissionsByEmailOrRoleId", " email- " + email + " RoleId- " + pRoleId.ToString());
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = rolePermissionViews
                });
            }
        }
        #endregion

        #region Get Module Wise Feature Details
        [HttpGet("GetModuleWiseFeatureDetails")]
        public IActionResult GetModuleWiseFeatureDetails()
        {
            string statusText = "";
            List<ModuleWiseFeatureDetails> moduleWiseFeatureDetails = new List<ModuleWiseFeatureDetails>();
            try
            {
                moduleWiseFeatureDetails = _employeeService.GetModuleWiseFeatureDetails();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = statusText,
                    Data = moduleWiseFeatureDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetModuleWiseFeatureDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = moduleWiseFeatureDetails
                });
            }
        }
        #endregion

        #region Get all employees Details
        [HttpGet("GetAllEmployeesDetails")]
        public IActionResult GetAllEmployeesDetails()
        {
            string statusText = "";
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = statusText,
                    Data = _employeeService.GetAllEmployeesDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAllEmployeesDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new List<Employees>()
                });
            }
        }
        #endregion

        #region Get Employee List For reporting manager     
        /// <summary>
        /// Get Employee List For reporting manager     
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeListForManager")]
        public IActionResult GetEmployeeListForManager(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeListForManager(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListForManager", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion

        #region Get rsource employee report
        /// <summary>
        /// Get rsource employee report
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetResourceEmployeeReport")]
        public IActionResult GetResourceEmployeeReport(int employeeId)
        {
            try
            {
                EmployeesMasterData employeeMaster = new EmployeesMasterData()
                {
                    Employees = _employeeService.GetAllEmployeesDetails(),
                    EmployeesSkillset = _employeeService.GetAllEmployeesSkillset(),
                    Skillsets = _employeeService.GetAllSkillset(),
                    AllLevelEmployee = _employeeService.GetEmployeeListForManager(employeeId),
                    RoleNameList = _employeeService.GetRoleNameList()
                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = employeeMaster
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetResourceEmployeeReport", JsonConvert.SerializeObject(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeesMasterData()
                });
            }
        }
        #endregion

        #region Get BU Head Name For Project   
        [HttpGet]
        [Route("GetBUHeadNameForProject")]
        public IActionResult GetBUHeadNameForProject(int? departmentHeadId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetBUAccountableForProjects(departmentHeadId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetBUHeadNameForProject");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = new List<BUAccountableForProject>()
                });
            }
        }
        #endregion

        #region Get project employee master data
        /// <summary>
        /// Get project employee master data
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProjectEmployeeMasterData")]
        public IActionResult GetProjectEmployeeMasterData()
        {
            try
            {
                EmployeesMasterData employeeMaster = new()
                {
                    Skillsets = _employeeService.GetAllSkillset(),
                    RoleNameList = _employeeService.GetRoleNameList(),
                    ListOfDesignation = _employeeService.GetDesignationList(),
                    BUAccountableForProjects = _employeeService.GetBUAccountableForProjects()
                };
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = employeeMaster
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetProjectEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeesMasterData()
                });
            }
        }
        #endregion  

        #region Get employees master data for search   
        /// <summary>
        /// Get employees master data for search  
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeesMasterDataForSearch")]
        public IActionResult GetEmployeesMasterDataForSearch()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeesMasterDataForSearch()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesMasterDataForSearch");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<SearchEmployeesMasterDataViewModel>()
                });
            }
        }
        #endregion

        #region Get role name list        
        [HttpGet]
        [Route("GetRoleNameList")]
        public IActionResult GetRoleNameList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetRoleNameList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetRoleNameList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = new List<RoleName>()
                });
            }
        }
        #endregion

        #region Get active employee list       
        [HttpGet]
        [Route("GetActiveEmployeeIdList")]
        public IActionResult GetActiveEmployeeIdList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _employeeService.GetActiveEmployeeIdList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetActiveEmployeeIdList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = new List<int>()
                });
            }
        }
        #endregion

        #region Get admin employee id       
        [HttpGet]
        [Route("GetAdminEmployeeId")]
        public IActionResult GetAdminEmployeeId(string roleName)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _employeeService.GetAdminEmployeeId(roleName)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAdminEmployeeId", roleName);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Module Description List
        [HttpGet("GetModuleDescription")]
        [AllowAnonymous]
        public IActionResult GetModuleDescription()
        {
            string statusText = "";
            List<Modules> moduleDescriptions = new List<Modules>();
            try
            {
                moduleDescriptions = _employeeService.GetModuleDescription();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    statusText = statusText,
                    Data = moduleDescriptions
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetModuleDescription");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = moduleDescriptions
                });
            }
        }
        #endregion

        #region Get Holiday Employee Master data        
        [HttpGet]
        [Route("GetHolidayEmployeeMasterData")]
        public IActionResult GetHolidayEmployeeMasterData()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetHolidayEmployeeMasterData()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetHolidayEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new HolidayEmployeeMasterData()
                });
            }
        }
        #endregion     

        #region Get report details by employee id
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetReportDetailsByEmployeeId")]
        public IActionResult GetReportDetailsByEmployeeId(int employeeId, int employeeCategoryId)
        {
            string statusText = "";
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetReportDetailsByEmployeeId(employeeId, employeeCategoryId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetReportDetails", employeeId.ToString());
                statusText = ex.Message.ToString();
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = new List<Reports>()
            });
        }
        #endregion

        #region Get employee type name by id
        [HttpPost]
        [Route("GetEmployeeTypeNameById")]
        public IActionResult GetEmployeeTypeNameById(List<int> employeesTypeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeTypeNameById(employeesTypeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeTypeNameById", JsonConvert.SerializeObject(employeesTypeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeTypeNames>()
                });
            }
        }
        #endregion

        #region Get Leave Employee Master data        
        [HttpGet]
        [Route("GetLeaveEmployeeMasterData")]
        public IActionResult GetLeaveEmployeeMasterData()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeLeavesMasterData()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetLeaveEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new HolidayEmployeeMasterData()
                });
            }
        }
        #endregion

        #region Get employee LeaveAdjustment
        [HttpPost]
        [Route("GetEmployeeLeaveAdjustment")]
        public IActionResult GetEmployeeLeaveAdjustment(EmployeeLeaveAdjustmentFilterView employeeLeaveAdjustmentView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeLeaveAdjustment(employeeLeaveAdjustmentView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeLeaveAdjustment");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeLeaveAdjustmentView>()
                });
            }
        }
        #endregion

        #region Get employee Department Id by employee id        
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeDepartmentIdByEmployeeId")]
        public IActionResult GetEmployeeDepartmentIdByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeDepartmentIdByEmployeeId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeDetailsByEmployeeId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get employee Attendance Details
        [HttpPost]
        [Route("GetEmployeeAttendanceDetails")]
        public IActionResult GetEmployeeAttendanceDetails(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeAttendanceDetails(employeesAttendanceFilterView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeAttendanceDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeAttendanceDetails>()
                });
            }
        }
        #endregion

        #region Get reporting Manager employee Id  
        [HttpGet]
        [Route("GetEmployeesForManagerId")]
        public IActionResult GetEmployeesForManagerId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeesForManagerId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesForManagerId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion

        #region Get Employee Details
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeAvailabilityDetails")]
        public IActionResult GetEmployeeAvailabilityDetails(int pResourceId)
        {
            try
            {
                List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
                employeeList = _employeeService.GetEmployeeAvailabilityDetails(pResourceId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeAvailabilityDetails", Convert.ToString(pResourceId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeDetails>()
                });
            }
        }
        #endregion

        #region Get Employee Details
        [HttpGet]
        [Route("GetEmployeeDetailByManagerId")]
        public IActionResult GetEmployeeDetailByManagerId(int employeeId)
        {
            try
            {
                List<EmployeeAssociates> employeeList = new List<EmployeeAssociates>();
                employeeList = _employeeService.GetEmployeeDetailByManagerId(employeeId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeDetailByManagerId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeAssociates>()
                });
            }
        }
        #endregion

        #region Get associate home report
        [HttpGet]
        [Route("GetAssociateHomeReport")]
        public IActionResult GetAssociateHomeReport()
        {
            List<HomeReportData> associateList = new List<HomeReportData>();
            try
            {
                associateList = _employeeService.GetAssociateHomeReport();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = associateList
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAssociateHomeReport");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = associateList
                });
            }
        }
        #endregion

        #region Sync User From AD
        [HttpPost]
        [Route("SyncUserFromAD")]
        public IActionResult SyncUserFromAD(ADUserToken userToken)
        {
            try
            {
                List<ADUserList> userList = new List<ADUserList>();
                string strURLForGetUserListFromAD = "";
                if (_configuration.GetSection("AzureADSecrets:URLForGetUserListFromAD") != null)
                    strURLForGetUserListFromAD = _configuration.GetSection("AzureADSecrets:URLForGetUserListFromAD").Value;
                var client = new RestClient();
                var request = new RestRequest(strURLForGetUserListFromAD, Method.Get);
                request.AddHeader("Authorization", "Bearer " + userToken.authToken);
                RestResponse response = client.Execute(request);
                JObject joResponse = JObject.Parse(response.Content);
                JArray array = (JArray)joResponse["value"];
                userList = array.ToObject<List<ADUserList>>();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.SyncUserFromAD(userList, userToken.userId, userToken.pShiftDetailsId).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/SyncUserFromAD");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Appraisal Dept Role Master Data
        [HttpGet]
        [Route("GetEmployeeAppraisalMasterData")]
        public IActionResult GetEmployeeAppraisalMasterData()
        {
            AppraisalMasterData appraisalDeptRole = new AppraisalMasterData();
            try
            {
                appraisalDeptRole = _employeeService.GetAppraisalDeptRoleMasterData();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = appraisalDeptRole
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAppraisalDeptRoleMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = appraisalDeptRole
                });
            }
        }
        #endregion

        #region Add or update EmployeeCategory
        [HttpPost]
        [Route("AddOrUpdateEmployeeCategory")]
        public IActionResult AddOrUpdateEmployeeCategory(EmployeeCategory employeeCategory)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Employee Category added successfully.",
                    Data = _employeeService.AddOrUpdateEmployeeCategory(employeeCategory).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddOrUpdateEmployeeCategory", JsonConvert.SerializeObject(employeeCategory));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }

        }
        #endregion

        #region Get department location name by id
        [HttpPost]
        [Route("GetDepartmentLocationNameById")]
        public IActionResult GetDepartmentLocationNameById(DepartmentLocationName departmentLocation)
        {
            DepartmentLocationName departmentLocationDetails = new DepartmentLocationName();
            try
            {
                departmentLocationDetails = _employeeService.GetDepartmentLocationNameById(departmentLocation);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = departmentLocationDetails
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetDepartmentLocationNameById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = departmentLocationDetails
                });
            }
        }
        #endregion

        #region Get All Employee List For reporting manager     
        /// <summary>
        /// Get Employee List For reporting manager     
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllEmployeeListForManagerReport")]
        public IActionResult GetAllEmployeeListForManagerReport(int employeeId, bool isAll = false)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetAllEmployeeListForManagerReport(employeeId, isAll)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAllEmployeeListForManagerReport", Convert.ToString(employeeId + ":" + isAll));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new List<EmployeeViewDetails>()
                });
            }
        }
        #endregion

        #region Get Employee shift Details
        [HttpGet]
        [Route("GetEmployeeShiftDetails")]
        public IActionResult GetEmployeeShiftDetails(int employeeID)
        {
            try
            {
                List<EmployeeShiftDetailsView> employeeShiftDetailsView = new List<EmployeeShiftDetailsView>();
                employeeShiftDetailsView = _employeeService.GetEmployeeShiftDetails(employeeID);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeeShiftDetailsView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeShiftDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeShiftDetailsView>()
                });
            }
        }
        #endregion

        #region Get Employees By Permanent and Active New
        [HttpPost]
        [Route("AppraisalManagerEmployeeDetails")]
        public IActionResult AppraisalManagerEmployeeDetails(EmployeelistForAppraisalMaster details)
        {
            try
            {
                AppraisalManagerEmployeeDetailsView employeesView = new AppraisalManagerEmployeeDetailsView();
                employeesView = _employeeService.AppraisalManagerEmployeeDetails(details);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeesView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesByPermanentandActiveNew");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeShiftDetailsView>()
                });
            }
        }
        #endregion

        #region Get Employees By Permanent and Active
        [HttpGet]
        [Route("GetEmployeesByPermanentandActive")]
        public IActionResult GetEmployeesByPermanentandActive()
        {
            try
            {
                List<AppraisalManagerEmployeeDetailsView> employeesView = new List<AppraisalManagerEmployeeDetailsView>();
                employeesView = _employeeService.GetEmployeesByPermanentandActive();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = employeesView
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesByPermanentandActive");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<AppraisalManagerEmployeeDetailsView>()
                });
            }
        }
        #endregion

        #region Get Employee and Manager By Employee ID    
        /// <summary>    
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeandManagerByEmployeeID")]
        public IActionResult GetEmployeeandManagerByEmployeeID(int employeeID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeandManagerByEmployeeID(employeeID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeandManagerByEmployeeID", JsonConvert.SerializeObject(employeeID));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeandManagerView>()
                });
            }
        }
        #endregion

        #region Get Employee Category details by employee id
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("GetEmployeeCategoryDetailsByEmployeeId")]
        public IActionResult GetEmployeeCategoryDetailsByEmployeeId(int employeeId, int employeeCategoryId)
        {
            string statusText = "";
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeCategoryDetailsByEmployeeId(employeeId, employeeCategoryId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeCategoryDetailsByEmployeeId", employeeId.ToString());
                statusText = ex.Message.ToString();
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = statusText,
                Data = new List<Reports>()
            });
        }
        #endregion

        #region Ge tEmployees List By Department
        [HttpPost]
        [Route("GetEmployeesListByDepartment")]
        public IActionResult GetEmployeesListByDepartment(EmployeeListByDepartment employeeView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeesListByDepartment(employeeView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesListByDepartment");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeandManagerView>()
                });
            }
        }
        #endregion

        #region Get all employee type
        /// <summary>    
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEmployeeTypeList")]
        public IActionResult GetEmployeeTypeList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeTypeList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeTypeList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeesTypes>()
                });
            }
        }
        #endregion

        #region Get all employee for leave
        /// <summary>    
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllEmployeeforLeave")]
        public IActionResult GetAllEmployeeforLeave()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetAllEmployeeforLeave()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAllEmployeeforLeave");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeesTypes>()
                });
            }
        }
        #endregion

        #region Get Employee Department And Location  
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeDepartmentAndLocation")]
        public IActionResult GetEmployeeDepartmentAndLocation(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeDepartmentAndLocation(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeDepartmentAndLocation", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeDepartmentAndLocationView()
                });
            }
        }
        #endregion

        #region 
        [HttpGet]
        [Route("GetEmployeeListByManagerId")]
        public IActionResult GetEmployeeListByManagerId(int? employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeListByManagerId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListByManagerId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion
        
        #region Get employee Department Id by employee id        
        [HttpGet]
        [AllowAnonymous]
        [Route("GetEmployeeByEmployeeId")]
        public IActionResult GetEmployeeByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeByEmployeeId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeByEmployeeId", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new Employees()
                });
            }
        }
        #endregion
        
        #region Get grant leave approver        
        [HttpGet]
        [Route("GetGrantLeaveApprover")]
        public IActionResult GetGrantLeaveApprover(int employeeId, string hrDepartmentName)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetGrantLeaveApprover(employeeId, hrDepartmentName)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetGrantLeaveApprover", "Employee Id -" + Convert.ToString(employeeId) + " HR Department Id - " + Convert.ToString(hrDepartmentName));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new GrantLeaveApproverView()
                });
            }
        }
        #endregion
        
        #region Get employee EmployeeName Department Location details
        [HttpPost]
        [Route("EmployeeNameDepartmentLocation")]
        public IActionResult EmployeeNameDepartmentLocation(EmployeeNameDepartmentLocation employeeDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.EmployeeNameDepartmentLocation(employeeDetails)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/EmployeeNameDepartmentLocation", JsonConvert.SerializeObject(employeeDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeNameDepartmentAndLocationView()
                });
            }
        }
        #endregion
        
        #region Get leave employee details
        [HttpPost]
        [Route("LeaveEmployeeDetails")]
        public IActionResult LeaveEmployeeDetails(EmployeeNameDepartmentLocation employeeDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.LeaveEmployeeDetails(employeeDetails)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/LeaveEmployeeDetails", JsonConvert.SerializeObject(employeeDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeNameDepartmentAndLocationView()
                });
            }
        }
        #endregion
        
        #region Get employee shift details
        [HttpGet]
        [Route("GetEmployeeShiftDetailsById")]
        public IActionResult GetEmployeeShiftDetailsById(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeShiftDetailsById(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeShiftDetailsById", JsonConvert.SerializeObject(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeShiftDetailsView>()
                });
            }
        }
        #endregion
        
        #region Get New Employee Detail for leave
        /// <summary>    
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNewEmployeeDetailsbyID")]
        public IActionResult GetNewEmployeeDetailsbyID(int EmployeeID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetNewEmployeeDetailsbyID(EmployeeID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetNewEmployeeDetailsbyID");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message

                });
            }
        }
        #endregion
        
        #region GetAllActiveEmployeeDetails
        /// <summary>    
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllActiveEmployeeDetails")]
        public IActionResult GetAllActiveEmployeeDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetAllActiveEmployeeDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAllActiveEmployeeDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message

                });
            }
        }
        #endregion
        
        #region Get All Skillsets
        [HttpGet("GetAllSkillSets")]
        public IActionResult GetAllSkillSets()
        {
            try
            {
                var skillSets = _employeeService.GetAllSkillset();
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = skillSets
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAllSkillSets");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = ""
                });
            }

        }
        #endregion
        
        #region 
        [HttpGet]
        [Route("GetShifByDate")]
        public IActionResult GetShifByDate(int EmployeeID, DateTime date)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetShifByDate(EmployeeID, date)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetShifByDate");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region
        [HttpGet]
        [Route("GetEmployeeAndApproverDetails")]
        public IActionResult GetEmployeeAndApproverDetails(int employeeId, int approverId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeAndApproverDetails(employeeId, approverId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeAndApproverDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeandManagerView()
                });
            }
        }
        #endregion

        #region
        [HttpGet]
        [Route("GetEmployeeManagerAndHeadDetails")]
        public IActionResult GetEmployeeManagerAndHeadDetails(int employeeId, int approverId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeManagerAndHeadDetails(employeeId, approverId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeManagerAndHeadDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeManagerAndHeadDetailsView()
                });
            }
        }
        #endregion

        #region
        [HttpPost]
        [Route("GetEmployeeDetailsById")]
        public IActionResult GetEmployeeDetailsById(List<int> employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeDetailsById(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeManagerAndHeadDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<ResignedEmployeeView>()
                });
            }
        }
        #endregion
        
        #region Add or update system role        
        [HttpPost]
        [Route("InsertOrUpdateSystemRole")]
        public IActionResult InsertOrUpdateSystemRole(SystemRoles role)
        {
            try
            {
                if (_employeeService.SystemRoleNameDuplication(role.RoleName))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "System Role Name is already exists. Please change role name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "System Role added successfully.",
                        Data = _employeeService.InsertOrUpdateSystemRole(role).Result
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/InsertOrUpdateSystemRole", JsonConvert.SerializeObject(role));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }

        }
        #endregion

        #region Update Employee Status 
        [HttpPost]
        [Route("UpdateEmployeeStatus")]
        public IActionResult UpdateEmployeeStatus(EmployeeStatusView employeeStatus)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.UpdateEmployeeStatus(employeeStatus).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/UpdateEmployeeStatus", "EmployeeId -" + Convert.ToString(employeeStatus.EmployeeId) + " isEnabled -" + Convert.ToString(employeeStatus.IsEnabled) + " updatedBy -" + Convert.ToString(employeeStatus.ModifiedBy));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion
        
        #region Get employee resignation details
        [HttpPost]
        [Route("GetEmployeeResignationDetails")]
        public IActionResult GetEmployeeResignationDetails(List<EmployeeResignationDetailsView> employeeDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeResignationDetails(employeeDetails).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeResignationDetails", "EmployeeId -" + JsonConvert.SerializeObject(employeeDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeResignationDetailsView>()
                });
            }
        }
        #endregion
        
        #region Get employee resignation approver
        [HttpGet]
        [Route("GetResignationApprover")]
        public IActionResult GetResignationApprover(int employeeId, string hrDepartmentName)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetResignationApprover(employeeId, hrDepartmentName).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetResignationApprover", "EmployeeId -" + employeeId + ", Hr department name - " + hrDepartmentName);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new ResignationApproverView()
                });
            }
        }
        #endregion
        
        #region Update employee reliving date
        [HttpPost]
        [Route("UpdateEmployeeRelievingDate")]
        public IActionResult UpdateEmployeeRelievingDate(UpdateEmployeeRelievingDate employee)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.UpdateEmployeeRelievingDate(employee).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/UpdateEmployeeRelievingDate", JsonConvert.SerializeObject(employee));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion

        #region Get employee master data        
        [HttpPost]
        [Route("GetResignationEmployeeMasterData")]
        public IActionResult GetResignationEmployeeMasterData(List<int> employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetResignationEmployeeMasterData(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new ResignationEmployeeMasterView()
                });
            }
        }
        #endregion
        
        #region Get exit interview employee data
        [HttpPost]
        [Route("GetEmployeeExitInterviewDetails")]
        public IActionResult GetEmployeeExitInterviewDetails(List<ResignationInterviewDetailView> employeeDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeExitInterviewDetails(employeeDetails)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeResignationDetails", "EmployeeDetails -" + JsonConvert.SerializeObject(employeeDetails));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<ResignationInterviewDetailView>()
                });
            }
        }
        #endregion

        #region Get exit interview employee data
        [HttpGet]
        [Route("GetEmployeeResignationDate")]
        public IActionResult GetEmployeeResignationDate(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeResignationDate(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeResignationDate", "EmployeeDetails -" + JsonConvert.SerializeObject(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Exit Interview employee master data        
        [HttpPost]
        [Route("GetExitEmployeeMaster")]
        public IActionResult GetExitEmployeeMaster(List<int> employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetExitEmployeeMaster(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetExitEmployeeMaster");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new ResignationEmployeeMasterView()
                });
            }
        }
        #endregion
        
        #region Get project customer employee list
        [HttpGet]
        [Route("GetEmployeeListForProjectAndCustomer")]
        public IActionResult GetEmployeeListForProjectAndCustomer(int employeeId, bool isAllEmployee)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeListForProjectAndCustomer(employeeId, isAllEmployee)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListForProjectAndCustomer", "EmployeeId -" + employeeId + " IsAllEmployee -" + isAllEmployee);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new ProjectCustomerEmployeeList()
                });
            }
        }
        #endregion

        #region Get  employee list by system Role
        [HttpGet]
        [Route("GetEmployeeListBySystemRole")]
        public IActionResult GetEmployeeListBySystemRole(string sRole = "")
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeListBySystemRole(sRole)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListBySystemRole", "system Role Name -" + sRole);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<int>()
                });
            }
        }
        #endregion
        
        #region Get checklist employee role
        [HttpGet]
        [Route("GetExitCheckListRole")]
        public IActionResult GetExitCheckListRole(int employeeId, int loginUserId, bool isAllReportees)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetExitCheckListRole(employeeId, loginUserId, isAllReportees)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetExitCheckListRole");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<string>()
                });
            }
        }
        #endregion
        
        #region Get checklist employee list
        [HttpGet]
        [Route("GetCheckListEmployeeList")]
        public IActionResult GetCheckListEmployeeList(int employeeId, bool isAll)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetCheckListEmployeeList(employeeId, isAll)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetCheckListEmployeeList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeViewDetails>()
                });
            }
        }
        #endregion
        
        #region Get checklist employee details
        [HttpPost]
        [Route("GetCheckListEmployeeDetails")]
        public IActionResult GetCheckListEmployeeDetails(List<ChecklistEmployeeView> employeeDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetCheckListEmployeeDetails(employeeDetails)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetCheckListEmployeeDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<ChecklistEmployeeView>()
                });
            }
        }
        #endregion
        
        #region Get reportees checklist employee details
        [HttpGet]
        [Route("GetReporteesCheckListEmployee")]
        public IActionResult GetReporteesCheckListEmployee(int employeeId, bool isAll)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetReporteesCheckListEmployee(employeeId, isAll)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetReporteesCheckListEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new ReporteesChecklistEmployeeView()
                });
            }
        }
        #endregion 
        
        #region Get employee list for checklist
        [HttpGet]
        [Route("GetEmployeeListForResignation")]
        public IActionResult GetEmployeeListForResignation(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeListForResignation(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListForResignation");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<ResignationEmployeeMasterView>()
                });
            }
        }
        #endregion
        
        #region Get resignation employee list
        [HttpGet]
        [Route("GetResignationEmployeeList")]
        public IActionResult GetResignationEmployeeList(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetResignationEmployeeList(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetResignationEmployeeList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeList>()
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
                        Data = _employeeService.GetResignationEmployeeListByFilter(resignationEmployeeFilter)
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetResignationEmployeeListByFilter");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<ResignationEmployeeMasterView>()
                });
            }
        }
        #endregion

        #region Get employee name by id
        [HttpGet]
        [Route("GetEmployeeNameByEmployeeId")]
        public IActionResult GetEmployeeNameByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeNameByEmployeeId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeNameByEmployeeId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new EmployeeName()
                });
            }
        }
        #endregion
        
        #region Get employee details by system role
        [HttpGet]
        [Route("GetEmployeesDetailsBySystemRole")]
        public IActionResult GetEmployeesDetailsBySystemRole(string sRole)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeesDetailsBySystemRole(sRole)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesDetailsBySystemRole");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<ResignationEmployeeMasterView>()
                });
            }
        }
        #endregion

        #region Deactivate Employee Status 
        [HttpGet]
        [Route("DeactivateEmployeeStatus")]
        public IActionResult DeactivateEmployeeStatus()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.DeactivateEmployeeStatus().Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/DeactivateEmployeeStatus");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion

        #region Bulk Insert Employee
        [HttpPost]
        [Route("BulkInsertEmployee")]
        public async Task<IActionResult> BulkInsertEmployee(ImportEmployeeExcelView importExcelView)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = "Inserted Successfully",
                        Data = await _employeeService.BulkInsertEmployee(importExcelView)
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/BulkInsertEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Bulk Update Employee
        [HttpPost]
        [Route("BulkUpdateEmployee")]
        public IActionResult BulkUpdateEmployee(ImportEmployeeExcelView importExcelView)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = "Inserted Successfully",
                        Data = _employeeService.BulkUpdateEmployee(importExcelView).Result
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/BulkUpdateEmployee");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Add or Update employee Work History
        [HttpPost]
        [Route("AddOrUpdateEmployeeWorkHistory")]
        public IActionResult AddOrUpdateEmployeeWorkHistory(EmployeeWorkAndEducationDetailView workHistoryDetail)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = "Inserted Successfully",
                        Data = _employeeService.AddOrUpdateEmployeeWorkHistory(workHistoryDetail).Result
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddOrUpdateEmployeeWorkHistory");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Get employee Work History
        [HttpGet]
        [Route("GetEmployeeWorkHistory")]
        public IActionResult GetEmployeeWorkHistory(int employeeId)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = "Inserted Successfully",
                        Data = _employeeService.GetWorkHistoryViewDetails(employeeId)
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeWorkHistory");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Add or Update employee Education Detail 
        [HttpPost]
        [Route("AddOrUpdateEducationDetails")]
        public IActionResult AddOrUpdateEducationDetails(EmployeeWorkAndEducationDetailView educationDetail)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = "Inserted Successfully",
                        Data = _employeeService.AddOrUpdateEducationDetails(educationDetail).Result
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddOrUpdateEducationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Get employee Work History
        [HttpGet]
        [Route("GetEmployeeEducationDetail")]
        public IActionResult GetEmployeeEducationDetail(int employeeId)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = "Inserted Successfully",
                        Data = _employeeService.GetEmployeeEducationDetail(employeeId)
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeWorkHistory");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Add or Update employee Compensation Detail 
        [HttpPost]
        [Route("AddOrUpdateEmployeeCompensationDetails")]
        public IActionResult AddOrUpdateEmployeeCompensationDetails(List<CompensationDetailView> compensationDetail)
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = "Inserted Successfully",
                        Data = _employeeService.AddOrUpdateEmployeeCompensationDetails(compensationDetail).Result
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddOrUpdateEmployeeCompensationDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Get employee compensationDetail
        [HttpGet]
        [Route("GetEmployeesCompensationDetail")]
        public IActionResult GetEmployeesCompensationDetail(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeCompensationDetail(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesCompensationDetail");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<CompensationDetail>()
                });
            }
        }
        #endregion

        #region Get employee List
        [HttpPost]
        [Route("GetEmployeesListForGrid")]
        public IActionResult GetEmployeesListForGrid(PaginationView pagination)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeesListForGrid(pagination)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesListForGrid");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<CompensationDetail>()
                });
            }
        }
        #endregion

        #region Get Employee List For Requested Document Grid
        [HttpPost]
        [Route("GetEmployeeListForRequestedDocumentGrid")]
        public IActionResult GetEmployeeListForRequestedDocumentGrid(List<int> employeeIds)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeListForRequestedDocumentGrid(employeeIds)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListForRequestedDocumentGrid");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<CompensationDetail>()
                });
            }
        }
        #endregion

        #region Get employee List for org chart
        [HttpPost]
        [Route("GetEmployeesListForOrgChart")]
        public IActionResult GetEmployeesListForOrgChart(PaginationView pagination)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeesListForOrgChart(pagination)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeesListForOrgChart");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<CompensationDetail>()
                });
            }
        }
        #endregion

        #region Get employee Basic Information Detail By Id
        [HttpGet]
        [Route("GetEmployeeBasicInformationById")]
        public async Task<IActionResult> GetEmployeeBasicInformationById(int employeeId)
        {
            try
            {
                var data = await _employeeService.GetEmployeeBasicInformationById(employeeId);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = data
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeBasicInformationById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeBasicInfoView>()
                });
            }
        }
        #endregion
        
        # region GetEmployeeCompensationDetailForView
        [HttpPost]
        [Route("GetEmployeeCompensationDetailForView")]
        public IActionResult GetEmployeeCompensationDetailForView(EmployeeCompensationCompareView compensationCompareView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeCompensationDetailForView(compensationCompareView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeCompensationDetailForView");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<CompensationDetail>()
                });
            }
        }
        #endregion

        # region Get all employee list for Birthday
        [HttpPost]
        [Route("GetListOfEmployeeBirthDay")]
        public IActionResult GetListOfEmployeeBirthDay(EmployeeDateInput data)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetListOfEmployeeBirthDay(data.FromDate, data.ToDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeCompensationDetailForView");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeDetailListView>()
                });
            }
        }
        #endregion

        #region Add or Update Designation
        [HttpPost]
        [Route("InsertOrUpdateDesignation")]
        public IActionResult InsertOrUpdateDesignation(Designation designation)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.InsertOrUpdateDesignation(designation).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/InsertOrUpdateDesignation", JsonConvert.SerializeObject(designation));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Designation List and count
        [HttpGet]
        [Route("GetDesignationListAndCount")]
        public IActionResult GetDesignationListAndCount()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "",
                    Data = _employeeService.GetDesignationListAndCount()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetDesignationListAndCount");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new List<DesignationDetail>()
                });
            }
        }
        #endregion

        #region Get employee list by designation Id  
        [HttpGet]
        [Route("GetEmployeeListByDesignationId")]
        public IActionResult GetEmployeeListByDesignationId(int designationId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeListByDesignationId(designationId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListByDesignationId", Convert.ToString(designationId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<DesignationEmployeeDetails>()
                });
            }
        }
        #endregion
        
        #region Get employee list by department Id  
        [HttpGet]
        [Route("GetEmployeeListByDepartmentId")]
        public IActionResult GetEmployeeListByDepartmentId(int departmentId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeListByDepartmentId(departmentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListByDepartmentId", Convert.ToString(departmentId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<DepartmentEmployeeList>()
                });
            }
        }
        #endregion

        #region Add or Update Department
        [HttpPost]
        [Route("InsertOrUpdateDepartment")]
        public IActionResult InsertOrUpdateDepartment(Department department)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.InsertOrUpdateDepartment(department).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/InsertOrUpdateDesignation", JsonConvert.SerializeObject(department));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get department master data        
        [HttpGet]
        [Route("GetDepartmentMasterData")]
        public IActionResult GetDepartmentMasterData()
        {
            try
            {
                string employeeIdFormat = _configuration.GetValue<string>("EmployeeIdFormat");
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetDepartmentMasterData(employeeIdFormat)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetDepartmentMasterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new DepartmentMasterData()
                });
            }
        }
        #endregion

        #region Get employee List for filter
        [HttpPost]
        [Route("GetEmployeesListByFilter")]
        public IActionResult employeeFilterData(PaginationView paginationView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.employeeFilterData(paginationView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/employeeFilterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeDetailListView>()
                });
            }
        }
        #endregion
        
        #region Get employee list by skillset Id  
        [HttpPost]
        [Route("GetEmployeeListBySkillsetId")]
        public IActionResult GetEmployeeListBySkillsetId(EmployeeSkillsetCategoryInput skillsetInput)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeListBySkillsetId(skillsetInput)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListBySkillsetId", JsonConvert.SerializeObject(skillsetInput));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<SkillsetEmployeeDetails>()
                });
            }
        }
        #endregion

        #region Add or Update Skillset
        [HttpPost]
        [Route("InsertOrUpdateSkillset")]
        public IActionResult InsertOrUpdateSkillset(Skillsets skillsets)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.InsertOrUpdateSkillset(skillsets).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/InsertOrUpdateSkillset", JsonConvert.SerializeObject(skillsets));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get SkillsetDetails 
        [HttpGet]
        [Route("GetSkillsetDetails")]
        public IActionResult GetSkillsetDetails()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetSkillsetDetails()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetSkillsetDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new SkillsetDetails()
                });
            }
        }
        #endregion

        #region Get SkillsetHistory by skillset Id  
        [HttpGet]
        [Route("GetSkillsetHistoryBySkillsetId")]
        public IActionResult GetSkillsetHistoryBySkillsetId(int skillsetId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetSkillsetHistoryBySkillsetId(skillsetId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetSkillsetHistoryBySkillsetId", Convert.ToString(skillsetId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new SkillsetHistoryView()
                });
            }
        }
        #endregion

        //#region Get SkillsetHistory by skillset Id  
        //[HttpGet]
        //[Route("GetSkillsetHistoryBySkillsetId")]
        //public IActionResult GetSkillsetHistoryBySkillsetId(int skillsetId)
        //{
        //    try
        //    {
        //        return Ok(new
        //        {
        //            StatusCode = "SUCCESS",
        //            Data = _employeeService.GetSkillsetHistoryBySkillsetId(skillsetId)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetSkillsetHistoryBySkillsetId", Convert.ToString(skillsetId));
        //        return Ok(new
        //        {
        //            StatusCode = "FAILURE",
        //            StatusText = ex.Message,
        //            Data = new SkillsetHistoryView()
        //        });
        //    }
        //}
        //#endregion

        #region Add Employee Request	
        [HttpPost]
        [Route("AddEmployeeRequest")]
        public async Task<IActionResult> AddEmployeeRequest(EmployeesViewModel employeesViewModel)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = await _employeeService.AddEmployeeRequest(employeesViewModel)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddEmployeeRequest", JsonConvert.SerializeObject(employeesViewModel));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new ChangeRequestEmailView()
                });
            }
        }
        #endregion

        #region Get all employee list for WorkAnniversaries
        [HttpPost]
        [Route("GetListOfEmployeeWorkAnniversaries")]
        public IActionResult GetListOfEmployeeWorkAnniversaries(EmployeeWorkAnniversariesInput data)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetListOfEmployeeWorkAnniversaries(data.FromDate, data.ToDate)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetListOfEmployeeWorkAnniversaries");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeWorkAnniversaries>()
                });
            }
        }
        #endregion
        
        #region Get Absent Notification Employee List
        [HttpGet]
        [Route("GetAbsentNotificationEmployeeList")]
        public IActionResult GetAbsentNotificationEmployeeList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAbsentNotificationEmployeeList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion                  Data = _employeeService.GetAbsentNotificationEmployeeList()

        #region Get employee list by skillset Id  
        [HttpPost]
        [Route("GetEmployeeListBySkillsetIdForDownload")]
        public IActionResult GetEmployeeListBySkillsetIdForDownload(EmployeeSkillsetCategoryInput skillsetInput)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeListBySkillsetIdForDownload(skillsetInput)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListBySkillsetId", JsonConvert.SerializeObject(skillsetInput));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<SkillsetEmployeeDetails>()
                });
            }
        }
        #endregion
        
        #region Get Organization chart details
        [HttpGet]
        [Route("GetorganizationchartDetails")]
        public IActionResult GetorganizationchartDetails(int employeeid)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetorganizationchartDetails(employeeid)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetorganizationchartDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new EmployeeorganizationcharView()
                });
            }
        }
        #endregion

        #region Get employee List Count
        [HttpPost]
        [Route("GetEmployeesListCount")]
        public IActionResult GetEmployeesListCount(PaginationView paginationView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeesListCount(paginationView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/employeeFilterData");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Get employee List Download data
        [HttpPost]
        [Route("EmployeeListDownload")]
        public IActionResult EmployeeListDownload(PaginationView paginationView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.EmployeeListDownload(paginationView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/EmployeeListDownload");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new EmployeeDownloadData()
                });
            }
        }
        #endregion
        
        #region Get employee List Audit Data
        [HttpGet]
        [Route("GetAuditListByEmployeeId")]
        public IActionResult GetAuditListByEmployeeId(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetAuditListByEmployeeId(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetAuditListByEmployeeId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new List<AuditDetailView>()
                });
            }
        }
        #endregion
        
        #region Add Approve Or Reject EmployeeRequest	
        [HttpPost]
        [Route("ApproveOrRejectEmployeeRequest")]
        // public IActionResult ApproveOrRejectEmployeeRequest(SharedLibraries.ViewModels.Employee.EmployeeRequestList employeeRequestList)	
        public async Task<IActionResult> ApproveOrRejectEmployeeRequest(ApproveOrRejectEmpRequestListView approveOrRejectEmpRequestList)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = await _employeeService.ApproveOrRejectEmployeeRequest(approveOrRejectEmpRequestList)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/ApproveOrRejectEmployeeRequest", JsonConvert.SerializeObject(approveOrRejectEmpRequestList));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Employee Request	
        [HttpGet]
        [Route("GetEmployeeRequest")]
        public IActionResult GetEmployeeRequest(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeRequest(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeRequest", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeRequestListView>()
                });
            }
        }
        #endregion
        
        #region Get Employee Approval	
        [HttpGet]
        [Route("GetEmployeeApproval")]
        public IActionResult GetEmployeeApproval(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeApproval(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeApproval", Convert.ToString(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeesRequestList()
                });
            }
        }
        #endregion

        //#region Get Employee Request for Admin	
        //[HttpGet]
        //[Route("GetEmployeeRequestForAdmin")]
        //public IActionResult GetEmployeeRequestForAdmin()
        //{
        //    try
        //    {
        //        return Ok(new
        //        {
        //            StatusCode = "SUCCESS",
        //            Data = _employeeService.GetEmployeeRequestForAdmin()
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeRequestForAdmin");
        //        return Ok(new
        //        {
        //            StatusCode = "FAILURE",
        //            StatusText = ex.Message,
        //            Data = new EmployeeRequestListView()
        //        });
        //    }
        //}
        //#endregion
        
        #region 
        [HttpPost]
        [Route("GetMyApprovalEmployeeList")]
        public IActionResult GetMyApprovalEmployeeList(PaginationView pagination)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    statusText = "",
                    Data = _employeeService.GetMyApprovalEmployeeList(pagination)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetMyApprovalEmployeeList", JsonConvert.SerializeObject(pagination));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = new List<EmployeeDetailListView>()
                });
            }
        }
        #endregion
        
        #region 
        [HttpPost]
        [Route("GetMyApprovalEmployeeCount")]
        public IActionResult GetMyApprovalEmployeeCount(PaginationView pagination)
        {
            try
            {
                int data = _employeeService.GetMyApprovalEmployeeCount(pagination);
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    statusText = "",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetMyApprovalEmployeeCount", JsonConvert.SerializeObject(pagination));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    statusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        # region Contract End Date Notification
        [HttpGet]
        [Route("ContractEndDateNotification")]
        public IActionResult ContractEndDateNotification()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.ContractEndDateNotification().Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/ApproveOrRejectEmployeeRequest");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        # region Update Employee Designation
        [HttpGet]
        [Route("UpdateEmployeeDesignation")]
        public IActionResult UpdateEmployeeDesignation()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.UpdateEmployeeDesignation().Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/UpdateEmployeeDesignation");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion

        #region Download document
        [HttpGet]
        [Route("DownloadDocumentById")]
        public IActionResult DownloadDocumentById(int documentId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Document downloaded successfully",
                    Data = _employeeService.DownloadDocumentById(documentId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/DownloadDocumentById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = false
                });
            }
        }
        #endregion
        
        #region Add or update department        
        [HttpPost]
        [Route("AddOrUpdateLocation")]
        public IActionResult AddOrUpdateLocation(EmployeeLocationView location)
        {
            try
            {
                if (_employeeService.LocationNameDuplicate(location?.Location))
                {
                    return Ok(new
                    {
                        StatusCode = "FAILURE",
                        StatusText = "Location Name is already exists. Please change location name and try again.",
                        Data = 0
                    });
                }
                else
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Location added successfully.",
                        Data = _employeeService.AddOrUpdateLocation(location).Result
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/AddOrUpdateLocation", JsonConvert.SerializeObject(location));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Employee Basic Info By EmployeeID
        [HttpGet]
        [Route("GetEmployeeBasicInfoByEmployeeID")]
        public IActionResult GetEmployeeBasicInfoByEmployeeID(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.GetEmployeeBasicInfoByEmployeeID(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeBasicInfoByEmployeeID", "EmployeeId -" + employeeId);
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = new EmployeeBasicInfoView()
                });
            }
        }
        #endregion

        #region Get employee list for new resignation
        [HttpGet]
        [Route("GetEmployeeListForNewResignation")]
        public IActionResult GetEmployeeListForNewResignation()
        {
            try
            {
                return Ok(
                    new
                    {
                        StatusCode = "Success",
                        StatusText = string.Empty,
                        Data = _employeeService.GetEmployeeListForNewResignation()
                    });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeListForNewResignation");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeDataForDropDown>()
                });
            }
        }
        #endregion
        
        #region Get employee Attendance Details
        [HttpPost]
        [Route("GetEmployeeAttendanceDetailsCount")]
        public IActionResult GetEmployeeAttendanceDetailsCount(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeAttendanceDetailsCount(employeesAttendanceFilterView)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeAttendanceDetails");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion
        
        #region Update employee reliving date
        [HttpPost]
        [Route("UpdateEmployeePersonalInfo")]
        public IActionResult UpdateEmployeePersonalInfo(UpdateEmployeeRelievingDate employee)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = string.Empty,
                    Data = _employeeService.UpdateEmployeePersonalInfo(employee).Result
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/UpdateEmployeePersonalInfo", JsonConvert.SerializeObject(employee));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.ToString(),
                    Data = false
                });
            }
        }
        #endregion

        #region Get leave employee details
        [HttpGet]
        [Route("EmployeeDetailsForLeave")]
        public IActionResult EmployeeDetailsForLeave(int employeeId)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.EmployeeDetailsForLeave(employeeId)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/EmployeeDetailsForLeave", JsonConvert.SerializeObject(employeeId));
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new EmployeeNameDepartmentAndLocationView()
                });
            }
        }
        #endregion

        #region Get Roles List
        [HttpGet("GetRolesList")]
        public IActionResult GetRolesList()
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetRolesList()
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetRolesList");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message.ToString(),
                    Data = new List<Roles>()
                });
            }
        }
        #endregion

        #region Get Employee Personal Info
        [HttpGet]
        [Route("GetEmployeePersonalInfo")]
        public IActionResult GetEmployeePersonalInfo(int employeeID)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeePersonalInfo(employeeID)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeePersonalInfo");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<EmployeeDataForDropDown>()
                });
            }
        }
        #endregion

        #region Get finance manager id    
        /// <summary>
        /// Get finance manager id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetfinanceManagerDetails")]
        public IActionResult GetfinanceManagerDetail(AccountDetails accountDetails)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetfinanceManagerDetails(accountDetails)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetfinanceManagerId");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = 0
                });
            }
        }
        #endregion

        #region Get Employee List For Acknowledgement
        [HttpPost]
        [Route("GetEmployeeListForAcknowledgement")]
        public IActionResult GetEmployeeListForAcknowledgement(PolicyEmployeeAcknowledgementListView policyEmployeeAcknowledgement)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeListForAcknowledgement(policyEmployeeAcknowledgement)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "APIGateway", "Employee/GetEmployeeListForAcknowledgement");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    Data = new List<EmployeeDataForDropDown>()
                });
            }
        }
        #endregion

        #region Get finance manager id    
        /// <summary>
        /// Get finance manager id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetEmployeeDetailForGrantLeaveById")]
        public IActionResult  GetEmployeeDetailForGrantLeaveById(EmployeeListByDepartment employeeList)
        {
            try
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    Data = _employeeService.GetEmployeeDetailForGrantLeaveById(employeeList)
                });
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/GetEmployeeDetailForGrantLeaveById");
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = ex.Message,
                    Data = new List<EmployeeList>()
                });
            }
        }
        #endregion
    }
}