using IAM.DAL.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharedLibraries.AzureADClient;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;

namespace IAM.DAL.Services
{
    public interface IAzureADToken
    {
        string GetToken(string pUserEmail, string pPassword);
        string GetAADToken(string pUserEmail, string pPassword);
        //string GetEmployeeDetails(string pEmailAddress);
    }
    public class AzureADToken : IAzureADToken
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IEmployeeCategoryRepository _employeeCategoryRepository;
        private readonly ISystemRoleRepository _systemRoleRepository;
        public AzureADToken(IConfiguration configuration, IEmployeeRepository employeeRepository, IRolePermissionRepository rolePermissionRepository, IEmployeeCategoryRepository employeeCategoryRepository, ISystemRoleRepository systemRoleRepository)
        {
            _configuration = configuration;
            _employeeRepository = employeeRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _employeeCategoryRepository = employeeCategoryRepository;
            _systemRoleRepository = systemRoleRepository;
        }

        #region Get Azure AD Client        
        public string GetAADToken(string username, string password)
        {
            string content = string.Empty, AzureGrantType = string.Empty, AzureClientId = string.Empty, AzureClientSecret = string.Empty;
            string AzureScope = string.Empty, AzureURL = string.Empty, AzureTenentId = string.Empty;
            if (_configuration.GetSection("AzureADSecrets:grant_type") != null)
                AzureGrantType = _configuration.GetSection("AzureADSecrets:grant_type").Value;
            if (_configuration.GetSection("AzureADSecrets:client_id") != null)
                AzureClientId = _configuration.GetSection("AzureADSecrets:client_id").Value;
            if (_configuration.GetSection("AzureADSecrets:client_secret") != null)
                AzureClientSecret = _configuration.GetSection("AzureADSecrets:client_secret").Value;
            if (_configuration.GetSection("AzureADSecrets:scope") != null)
                AzureScope = _configuration.GetSection("AzureADSecrets:scope").Value;
            if (_configuration.GetSection("AzureADSecrets:URL") != null)
                AzureURL = _configuration.GetSection("AzureADSecrets:URL").Value;
            if (_configuration.GetSection("AzureADSecrets:tenent_id") != null)
                AzureTenentId = _configuration.GetSection("AzureADSecrets:tenent_id").Value;
            Dictionary<string, string> formURLContents = new Dictionary<string, string>() {
                {"grant_type", AzureGrantType },
                {"username", username },
                {"password", password },
                {"client_id", AzureClientId },
                {"client_secret", AzureClientSecret },
                {"scope", AzureScope },
            };
            if (AzureURL != string.Empty)
            {
                string url = string.Format(CultureInfo.InvariantCulture, AzureURL, AzureTenentId);
                using var client = new HttpClient();
                HttpResponseMessage response = client.PostAsync(url, new FormUrlEncodedContent(formURLContents)).GetAwaiter().GetResult();
                content = response.Content.ReadAsStringAsync().Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    content = response.Content.ReadAsStringAsync().Result;
                //}
            }
            return content;
        }
        #endregion

        #region Fetch Token
        public string GetToken(string pUserEmail, string pPassword)
        {
            string AccessToken = "", ADAccessToken = "", refresh_token = "", id_token = "";
            double expires_in = 1200;
            Employees user = _employeeRepository.GetEmployeeByEmailId(pUserEmail);
            if (user == null)
            {
                return "invalid";
            }
            if (user?.IsActive == false)
            {
                return "inactive";
            }
            if (_configuration.GetSection("EnvironmentSetup:TestEnvironment") != null && _configuration.GetSection("EnvironmentSetup:TestEnvironment")?.Value?.ToString()?.ToLower() == "true")
            {
                if (user == null || user?.EmployeeID == 0 || _configuration.GetSection("EnvironmentSetup:CommonPassword") == null
                    || pPassword != _configuration.GetSection("EnvironmentSetup:CommonPassword")?.Value?.ToString())
                {
                    return "";
                }
            }
            else
            {
                AccessToken = this.GetAADToken(pUserEmail, pPassword);
                if (AccessToken == string.Empty) return string.Empty;
                JObject json = JObject.Parse(AccessToken);
                var errorCodeData = json["error_codes"];
                if (errorCodeData != null)
                {
                    if (errorCodeData.Count() > 0 && Convert.ToDouble(errorCodeData[0]) != 50076)
                    {
                        return string.Empty;
                    }
                }
                //expires_in = Convert.ToDouble(json["expires_in"]);
                //ADAccessToken = Convert.ToString(json["access_token"]);
                //refresh_token = Convert.ToString(json["refresh_token"]);
                //id_token = Convert.ToString(json["id_token"]);
            }

            UserTokenCacheManager.AddUserTokenCache(new UserTokenCache
            {
                AccessToken = ADAccessToken,
                AppTokenCaches = new List<AppTokenCache>(),
                Username = pUserEmail,
                RefreshToken = refresh_token,
                IDToken = id_token,
                ExpirationTime = DateTime.UtcNow.AddSeconds(expires_in)
            });

            List<RoleFeatureList> roleFeatureLists = _rolePermissionRepository.GetRoleFeatureList(user?.SystemRoleId == null ? 0 : (int)user?.SystemRoleId);
            List<UserApps> userApps = roleFeatureLists.Where(x => x.IsMenu == true).GroupBy(x => new { x.ModuleId, x.ModuleName, x.NavigationURL, x.ModuleIcon }, y => y).Select(x => new UserApps { ModuleId = x.Key.ModuleId, ModuleName = x.Key.ModuleName, NavigationURL = x.Key.NavigationURL, ModuleIcon = x.Key.ModuleIcon }).ToList();
            userApps.Sort((x, y) => x.ModuleId.CompareTo(y.ModuleId));
            string OKRRole = _employeeRepository.GetRoleNameByRoleId(user?.RoleId);
            string systemRoles = _systemRoleRepository.GetSystemRoleNameByRoleId(user?.SystemRoleId);
            string department = _employeeRepository.GetDepartmentNameByDepartmentId(user?.DepartmentId);
            string category = _employeeCategoryRepository.GetEmployeeCategoryNameById(user?.EmployeeCategoryId == null ? 0 : (int)user.EmployeeCategoryId);
            string userdesignation = _employeeRepository.GetDesignationById(user?.DesignationId);
            string location = _employeeRepository.GetLocationNameByLocationId(user?.LocationId);
            return JsonConvert.SerializeObject(new
            {
                accessToken = ADAccessToken,
                userName = pUserEmail,
                refreshToken = refresh_token,
                userId = user?.EmployeeID,
                userRoleId = user?.RoleId,
                userRole = OKRRole == null ? "" : OKRRole,
                systemRoleId = user?.SystemRoleId,
                systemRole = systemRoles == null ? "" : systemRoles,
                userDepartmentId = user?.DepartmentId,
                userDepartment = department == null ? "" : department,
                expiryDate = DateTime.UtcNow.AddSeconds(expires_in),
                expiryon = expires_in,
                createdBy = user?.EmployeeID,
                createdOn = DateTime.UtcNow,
                firstName = user?.FirstName,
                lastName = user?.LastName,
                emailAddress = pUserEmail,
                employeeCategoryId = user?.EmployeeCategoryId,
                employeeCategoryName = category == null ? "" : category,
                roleFeatures = roleFeatureLists,
                UserApps = userApps,
                designation = userdesignation == null ? "" : userdesignation,
                formattedEmployeeId = user?.FormattedEmployeeId,
                userLocationId = user?.LocationId,
                userLocation = location == null ? "" : location,
                userShiftId = _employeeRepository.GetShiftByemployeeId(user?.EmployeeID),
                ManagerId = user?.ReportingManagerId,
                DOJ = user?.DateOfJoining,
                ProfileImage = user?.ProfilePicture
            });
        }
        #endregion

        //#region Get Employee Details
        //public string GetEmployeeDetails(string pEmailAddress)
        //{
        //    Employees user = _employeeRepository.GetEmployeeByEmailId(pEmailAddress);
        //    //List<UserApps> userApps = new List<UserApps>();
        //    List<RoleFeatureList> roleFeatureLists = _rolePermissionRepository.GetRoleFeatureList(user?.RoleId == null ? 0 : (int)user?.RoleId);
        //    List<UserApps> userApps = roleFeatureLists.Where(x=>x.IsMenu==true).GroupBy(x => new {  x.ModuleId,  x.ModuleName, x.NavigationURL, x.ModuleIcon }, y=>y).Select(x=> new UserApps { ModuleId = x.Key.ModuleId, ModuleName = x.Key.ModuleName, NavigationURL = x.Key.NavigationURL, ModuleIcon = x.Key.ModuleIcon }).ToList();
        //    //foreach (RoleFeatureList roleFeatureList in roleFeatureLists)
        //    //{
        //    //    if (roleFeatureList.IsMenu == false) continue;
        //    //    if (userApps.Count == 0)
        //    //    {
        //    //        UserApps userApp = new UserApps
        //    //        {
        //    //            ModuleId = roleFeatureList.ModuleId,
        //    //            ModuleName = roleFeatureList.ModuleName,
        //    //            NavigationURL = roleFeatureList.NavigationURL
        //    //        };
        //    //        userApps.Add(userApp);
        //    //    }
        //    //    else
        //    //    {
        //    //        bool IsSkip = false;
        //    //        foreach (UserApps apps in userApps)
        //    //        {
        //    //            if (apps.ModuleName == roleFeatureList.ModuleName) { IsSkip = true; continue; }
        //    //        }
        //    //        if (!IsSkip)
        //    //        {
        //    //            UserApps userApp = new UserApps
        //    //            {
        //    //                ModuleId = roleFeatureList.ModuleId,
        //    //                ModuleName = roleFeatureList.ModuleName,
        //    //                NavigationURL = roleFeatureList.NavigationURL
        //    //            };
        //    //            userApps.Add(userApp);
        //    //        }
        //    //    }
        //    //}
        //    userApps.Sort((x, y) => x.ModuleId.CompareTo(y.ModuleId));
        //    UserToken userToken = new UserToken
        //    {
        //        EmailAddress = pEmailAddress,
        //        UserId = user?.EmployeeID,
        //        UserRole = _employeeRepository.GetRoleNameByRoleId(user?.RoleId),
        //        CreatedBy = user?.EmployeeID,
        //        CreatedOn = DateTime.UtcNow,
        //        FirstName = user?.FirstName,
        //        LastName = user?.LastName,
        //        RoleFeatures = roleFeatureLists,
        //        UserApps = userApps,
        //        EmployeeCategoryId = user?.EmployeeCategoryId,
        //        EmployeeCategoryName = _employeeCategoryRepository.GetEmployeeCategoryNameById(user?.EmployeeCategoryId == null ? 0 : (int)user.EmployeeCategoryId)
        //    };
        //    string Json = JsonConvert.SerializeObject(new
        //    {
        //        UserToken = userToken
        //    });
        //    return Json.ToString();
        //}
        //#endregion
    }
}