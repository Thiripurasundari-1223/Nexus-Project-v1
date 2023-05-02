using IAM.API.AzureAuthenticationManager;
using IAM.DAL.DBContext;
using IAM.DAL.Models;
using IAM.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibraries.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IAMController : ControllerBase
    {
        readonly IAzureADToken _azureADToken;
        private readonly IIAMService _iAMService;
        private readonly IAMDBContext _iAMDBContext;
        private readonly IA2DAuthenticationManager _a2DAuthenticationManager;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet("GetEmptyMethod")]
        public IActionResult Get()
        {
            return Ok(new
            {
                StatusCode = "SUCCESS",
                Data = "IAM API - GET Method"
            });
        }

        #region Constructor
        public IAMController(IAzureADToken azureADToken, IIAMService iAMService, IAMDBContext iAMDBContext,
            IA2DAuthenticationManager a2DAuthenticationManager)
        {
            _azureADToken = azureADToken;
            _iAMService = iAMService;
            _iAMDBContext = iAMDBContext;
            _a2DAuthenticationManager = a2DAuthenticationManager;
        }
        #endregion

        #region Login Using Azure AD
        /// <summary>
        /// Login Using Azure AD
        /// </summary>
        /// <param name="pUserEmailAddress"></param>
        /// <param name="pPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("LoginAZureAD")]
        public IActionResult LoginAZureAD(string pUserEmailAddress, string pPassword)
        {
            try
            {
                string AuthToken = _a2DAuthenticationManager.Authenticate(_iAMDBContext, pUserEmailAddress, pPassword);
                if (AuthToken == null) { return Unauthorized(); }
                return Ok(AuthToken);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
        #endregion

        #region Sign Up
        /// <summary>
        /// Sign Up
        /// </summary>
        /// <param name="pEmailAddress"></param>
        /// 
        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(string pEmailAddress, string lName, string fName, string pMobileNo, string pPassword)
        {
            try
            {
                if (_iAMService.SignUp(pEmailAddress, lName, fName, pMobileNo, pPassword))
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Successfully Sign-Up."
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "Unexpected error occurred. Try again."
            });
        }
        #endregion

        #region Sign Out
        /// <summary>
        /// Sign Out
        /// </summary>
        /// <param name="pEmailAddress"></param>
        /// <returns></returns>
        [HttpPost]
        //[AllowAnonymous]
        [Route("SignOut")]
        public IActionResult SignOut(string pEmailAddress)
        {
            try
            {
                if (_iAMService.SignOut(pEmailAddress))
                {
                    return Ok(new
                    {
                        StatusCode = "SUCCESS",
                        StatusText = "Successfully Sign-Out."
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return Ok(new
            {
                StatusCode = "FAILURE",
                StatusText = "Unexpected error occurred. Try again."
            });
        }
        #endregion

        #region Role

        #region Insert And Update Role
        [HttpPost]
        //[AllowAnonymous]
        [Route("InsertAndUpdateRole")]
        public IActionResult InsertAndUpdateRole(Roles pRole)
        {
            if (_iAMService.InsertAndUpdateRole(pRole))
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Record saved successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again."
                });
            }
        }
        #endregion

        #region Delete Role
        [HttpDelete]
        //[AllowAnonymous]
        [Route("DeleteRole")]
        public IActionResult DeleteRole(int pRoleID)
        {
            if (_iAMService.DeleteRole(pRoleID))
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Record saved successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again."
                });
            }
        }

        #endregion

        #endregion

        #region Role Setup

        #region Insert And Update Role Setup
        [HttpPost]
        //[AllowAnonymous]
        [Route("InsertAndUpdateRoleSetup")]
        public IActionResult InsertAndUpdateRoleSetup(RoleSetup pRoleSetup)
        {
            if (_iAMService.InsertAndUpdateRoleSetup(pRoleSetup))
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Record saved successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again."
                });
            }
        }
        #endregion

        #region Delete Role Setup
        [HttpDelete]
        //[AllowAnonymous]
        [Route("DeleteRoleSetup")]
        public IActionResult DeleteRoleSetup(int pRoleSetupID)
        {
            if (_iAMService.DeleteRoleSetup(pRoleSetupID))
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Record saved successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again."
                });
            }
        }
        #endregion

        #endregion

        #region User Role Setup

        #region Insert And Update User Role
        [HttpPost]
        //[AllowAnonymous]
        [Route("InsertAndUpdateUserRole")]
        public IActionResult InsertAndUpdateUserRole(UserRoles puserRoles)
        {
            if (_iAMService.InsertAndUpdateUserRoles(puserRoles))
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Record saved successfully."
                });
            }
            else
            {
                return Ok(new { StatusCode = "FAILURE", StatusText = "Unexpected error occurred. Try again." });
            }
        }
        #endregion

        #region Delete User Role
        [HttpDelete]
        //[AllowAnonymous]
        [Route("DeleteUserRole")]
        public IActionResult DeleteUserRole(int pUserRoleID)
        {
            if (_iAMService.DeleteUserRoles(pUserRoleID))
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Record saved successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again."
                });
            }
        }
        #endregion

        #region Get User Role By User ID
        [HttpGet]
        //[AllowAnonymous]
        [Route("GetUserRoleByUserID")]
        public UserRoles GetUserRoleByUserID(int pUserID)
        {
            return _iAMDBContext.UserRoles.Where(x => x.UserID == pUserID).SingleOrDefault();
        }
        #endregion

        #region Get All User Roles
        [HttpGet]
        [Authorize]
        [Route("GetAllUserRoles")]
        public IEnumerable<UserRoles> GetAllUserRoles()
        {
            return _iAMDBContext.UserRoles.ToList();
        }
        #endregion
        #endregion

        #region User

        #region Insert And Update New User
        [HttpPost]
        //[AllowAnonymous]
        [Route("InsertAndUpdateNewUser")]
        public IActionResult InsertAndUpdateNewUser(User user)
        {
            if (_iAMService.AddNewUser(user))
            {
                return Ok(new
                {
                    StatusCode = "SUCCESS",
                    StatusText = "Record saved successfully."
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = "FAILURE",
                    StatusText = "Unexpected error occurred. Try again."
                });
            }
        }
        #endregion

        #endregion
    }
}