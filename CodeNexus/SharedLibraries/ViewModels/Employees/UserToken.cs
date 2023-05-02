using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.ViewModels.Employees
{
    public class UserToken
    {
        [Key]
        public int UserTokenId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserRole { get; set; }
        public double Expiryon { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public List<RoleFeatureList> RoleFeatures { get; set; }
        public List<UserApps> UserApps { get; set; }
        public int? EmployeeCategoryId { get; set; }
        public string UserDepartment { get; set; }
        public int? UserDepartmentId { get; set; }
        public int? UserRoleId { get; set; }
        public string EmployeeCategoryName { get; set; }
        public string designation { get; set; }
        public string formattedEmployeeId { get; set; }
        public int? userLocationId { get; set; }
        public string userLocation { get; set; }
        public int? userShiftId { get; set; }
        public int? ManagerId { get; set; }
        public string ShiftFromTime { get; set; }
        public string ShiftToTime { get; set; }
        public bool IsFlexyShift { get; set; }
        public List<WeekendDetails> WeekendId { get; set; }
        public DateTime? DOJ { get; set; }
        public int? SystemRoleId { get; set; }
        public string SystemRole { get; set; }
        public string ProfileImage { get; set; }
        public int? PolicyDocumentId { get; set; } 
        public bool? IsPolicyRequiredToAcknowledge { get; set; } 
    }
    
    public class WeekendDetails
    {
        [Key]
        public int WeekendId { get; set; }
    }
    public class UserApps
    {
        [Key]
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string NavigationURL { get; set; }
        public string ModuleIcon { get; set; }
    }
}