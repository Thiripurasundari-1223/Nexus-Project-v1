using IAM.DAL.Models;
using IAM.DAL.Repository;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IAM.DAL.Services
{
    public interface IIAMService
    {
        void UpdateLoginHistory(string pUserEmailAddress);
        bool SignUp(string pEmailAddress, string lName, string fName, string pMobileNo, string pPassword = "");
        bool SignOut(string Username);
        void UpdateLoginAttempt(string pEmailAddress, string pAttemptStatus);
        User GetLoginUser(string pEmailAddress);
        bool InsertAndUpdateRole(Roles pRole);
        bool DeleteRole(int pRoleID);
        bool InsertAndUpdateRoleSetup(RoleSetup pRoleSetup);
        bool DeleteRoleSetup(int pRoleSetupID);
        bool InsertAndUpdateUserRoles(UserRoles puserRoles);
        bool DeleteUserRoles(int pUserRoleID);
        UserAccessBag GetUserAccess(string pUserEmailAddress);
        bool AddNewUser(User pUser);
    }
    public class IAMService : IIAMService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IUserRepository _userRepository;
        private readonly ILoginHistoryRepository _LoginHistoryRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleSetupRepository _roleSetupRepository;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IUserAccessBagRepository _userAccessBagRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        public IAMService(IUserRepository userRepository,
                          ILoginHistoryRepository loginHistoryRepository,
                          IRoleRepository roleRepository,
                          IRoleSetupRepository roleSetupRepository,
                          IUserRolesRepository userRolesRepository,
                          IUserAccessBagRepository userAccessBagRepository,
                          IUserTokenRepository userTokenRepository)
        {
            _userRepository = userRepository;
            _LoginHistoryRepository = loginHistoryRepository;
            _roleRepository = roleRepository;
            _roleSetupRepository = roleSetupRepository;
            _userRolesRepository = userRolesRepository;
            _userAccessBagRepository = userAccessBagRepository;
            _userTokenRepository = userTokenRepository;
        }

        #region Sign Out
        /// <summary>
        /// Sign Out
        /// </summary>
        /// <param name="pEmailAddress"></param>
        public bool SignOut(string pEmailAddress)
        {
            try
            {
                LoginHistory userDetail = _LoginHistoryRepository.GetLoggedUserByName(pEmailAddress);
                if (userDetail != null)
                {
                    LoginHistory userLoginHistory = _LoginHistoryRepository.GetByID(userDetail.UserID);
                    if (userLoginHistory != null)
                    {
                        userLoginHistory.LogOutTime = DateTime.UtcNow;
                        _LoginHistoryRepository.Update(userLoginHistory);
                        _LoginHistoryRepository.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Get Login User
        //    /// <summary>
        //    /// Get Login User
        //    /// </summary>
        //    /// <param name="pEmailAddress"></param>
        //    /// <returns>User info</returns>
        public User GetLoginUser(string pEmailAddress)
        {
            try
            {
                if (pEmailAddress != "")
                {
                    return _userRepository.GetLoggedUserByName(pEmailAddress);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return null;
        }
        #endregion

        #region Sign Up
        /// <summary>
        /// SignUp the user
        /// </summary>
        /// <param name="pEmailAddress"></param>
        /// <param name="pName"></param>
        /// <param name="pMobileNo"></param>
        /// <param name="pPassword"></param>
        /// <returns></returns>
        public bool SignUp(string pEmailAddress, string lName, string fName, string pMobileNo, string pPassword = "")
        {
            try
            {
                User userDetail = _userRepository.GetLoggedUserByName(pEmailAddress);
                if (userDetail != null)
                {
                    userDetail.LastName = lName;
                    //userDetail.Password = Encryption.Encrypt.EncrptPassword(pPassword);
                    userDetail.Phone = pMobileNo;
                    userDetail.IsActive = true;
                    userDetail.IsLocked = false;
                    userDetail.ModifiedBy = 0;
                    userDetail.ModifiedOn = DateTime.UtcNow;
                    userDetail.LastPasswordChangedDate = DateTime.UtcNow;
                    _userRepository.Update(userDetail);
                    _userRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Update Login Attempt
        /// <summary>
        /// Update Login Attempt
        /// </summary>
        /// <param name="pEmailAddress"></param>
        /// <param name="pAttemptStatus"></param>
        public void UpdateLoginAttempt(string pEmailAddress, string pAttemptStatus)
        {
            try
            {
                User userDetail = _userRepository.GetLoggedUserByName(pEmailAddress);
                if (userDetail != null)
                {
                    userDetail.AttemptCount += 1;
                    userDetail.AttemptStatus = pAttemptStatus;
                    _userRepository.Update(userDetail);
                    _userRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
        }
        #endregion

        #region Update Login History
        /// <summary>
        /// Update Login History
        /// </summary>
        /// <param name="puserLoginHistory"></param>
        public void UpdateLoginHistory(string pUserEmailAddress)
        {
            try
            {
                User user = _userRepository.GetLoggedUserByName(pUserEmailAddress);
                if (user != null)
                {
                    LoginHistory userLoginHistory = new LoginHistory
                    {
                        UserID = user.UserId,
                        Username = user.EmailAddress,
                        CreatedOn = DateTime.UtcNow
                    };
                    _LoginHistoryRepository.AddAsync(userLoginHistory);
                    _LoginHistoryRepository.SaveChangesAsync();
                    user.AttemptCount = 0;
                    user.AttemptStatus = "S";
                    user.Notes = "";
                    user.LastLoginDate = DateTime.UtcNow;
                    _userRepository.Update(user);
                    _userRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
        }
        #endregion

        #region Roles

        #region Insert And Update Role
        public bool InsertAndUpdateRole(Roles pRole)
        {
            try
            {
                Roles role = _roleRepository.GetRoleByID(pRole.RoleId);
                if (role != null)
                {
                    role.RoleName = pRole.RoleName;
                    role.RoleShortName = pRole.RoleShortName;
                    role.RoleDescription = pRole.RoleDescription;
                    _roleRepository.Update(role);
                }
                else
                {
                    Roles newRole = new Roles();
                    newRole = pRole;
                    _roleRepository.AddAsync(newRole);
                }
                _roleRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Delete Role
        public bool DeleteRole(int pRoleID)
        {
            try
            {
                if (pRoleID > 0)
                {
                    Roles role = _roleRepository.GetRoleByID(pRoleID);
                    if (role != null && role.RoleId > 0)
                    {
                        _roleRepository.Delete(role);
                        _roleRepository.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion
        #endregion

        #region Role Setup
        #region Insert And Update Role Setup
        public bool InsertAndUpdateRoleSetup(RoleSetup pRoleSetup)
        {
            try
            {
                RoleSetup roleSetup = _roleSetupRepository.GetByID(pRoleSetup.RoleSetupID);
                if (roleSetup != null)
                {
                    roleSetup.RoleID = pRoleSetup.RoleID;
                    roleSetup.AccountManagement = pRoleSetup.AccountManagement;
                    roleSetup.ProjectManagement = pRoleSetup.ProjectManagement;
                    roleSetup.ResourceManagement = pRoleSetup.ResourceManagement;
                    roleSetup.TimeSheet = pRoleSetup.TimeSheet;
                    roleSetup.TeamTimeSheet = pRoleSetup.TeamTimeSheet;
                    roleSetup.MyPerformance = pRoleSetup.MyPerformance;
                    roleSetup.TeamAppraisal = pRoleSetup.TeamAppraisal;
                    roleSetup.ContinuousFeedback = pRoleSetup.ContinuousFeedback;
                    _roleSetupRepository.Update(roleSetup);
                }
                else
                {
                    RoleSetup newRoleSetup = new RoleSetup();
                    newRoleSetup = pRoleSetup;
                    _roleSetupRepository.AddAsync(newRoleSetup);
                }
                _roleSetupRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Delete Role SetUp
        public bool DeleteRoleSetup(int pRoleSetupID)
        {
            try
            {
                if (pRoleSetupID > 0)
                {
                    RoleSetup roleSetup = _roleSetupRepository.GetByID(pRoleSetupID);
                    if (roleSetup != null && roleSetup.RoleSetupID > 0)
                    {
                        _roleSetupRepository.Delete(roleSetup);
                        _roleSetupRepository.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion
        #endregion

        #region User Roles
        #region Insert And Update User Roles
        public bool InsertAndUpdateUserRoles(UserRoles puserRoles)
        {
            try
            {
                UserRoles userRole = _userRolesRepository.GetByID(puserRoles.UserRoleId);
                if (userRole != null)
                {
                    userRole = puserRoles;
                    userRole.RoleId = puserRoles.RoleId;
                    userRole.UserID = puserRoles.UserID;
                    _userRolesRepository.Update(userRole);
                }
                else
                {
                    UserRoles newpUserRoles = new UserRoles();
                    newpUserRoles = puserRoles;
                    _userRolesRepository.AddAsync(newpUserRoles);
                }
                _userRolesRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Delete User Roles
        public bool DeleteUserRoles(int pUserRoleID)
        {
            try
            {
                if (pUserRoleID > 0)
                {
                    UserRoles userRole = _userRolesRepository.GetByID(pUserRoleID);
                    if (userRole != null && userRole.UserRoleId > 0)
                    {
                        _userRolesRepository.Delete(userRole);
                        _userRolesRepository.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion
        #endregion

        #region Get User Access
        public UserAccessBag GetUserAccess(string pUserEmailAddress)
        {
            return _userAccessBagRepository.GetUserAccessBag(pUserEmailAddress);
        }
        #endregion

        #region Add New User
        public bool AddNewUser(User pUser)
        {
            try
            {
                User user = _userRepository.GetByID(pUser.UserId);
                if (user != null)
                {
                    user = pUser;
                    _userRepository.Update(user);
                }
                else
                {
                    User newUser = new User();
                    newUser = pUser;
                    _userRepository.AddAsync(newUser);

                }
                _userRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Check Access Token
        public UserToken CheckAccessToken(string pEmailAddress, string AccessToken)
        {
            return _userTokenRepository.CheckAccessToken(pEmailAddress, AccessToken);
        }
        #endregion
    }
}