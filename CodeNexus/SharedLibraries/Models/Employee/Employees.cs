﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibraries.Models.Employee
{
	[Table("Employees")]
    public class Employees
    {
		[Key]
		public int EmployeeID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public int? EmployeeTypeId { get; set; }
		public int? DepartmentId { get; set; }
		public int? RoleId { get; set; }
		public DateTime? DateOfJoining { get; set; }
		public DateTime? DateOfContract { get; set; }
		public DateTime? DateOfRelieving { get; set; }
		public DateTime? DesignationEffectiveFrom { get; set; }
		public DateTime? ContractEndDate { get; set; }
		public DateTime? ActualConfirmationDate { get; set; }
		public DateTime? MergerDate { get; set; }
		public int? ReportingManagerId { get; set; }
		public bool? IsActive { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public string Gender { get; set; }
		public int? LocationId { get; set; }
		public int? CurrentWorkLocationId { get; set; }
		public int? CurrentWorkPlaceId { get; set; }
		public bool? IsSpecialAbility { get; set; }
        //public int? SpecialAbilityId { get; set; }
        public string SpecialAbilityRemark { get; set; }
        public string Mobile { get; set; }
        public string JobTitle { get; set; }
		public int? DesignationId { get; set; }
		public int? EmployeeCategoryId { get; set; }
		public int? EmployeeGrade { get; set; }
		public DateTime? ProbationExtension { get; set; }
        //public decimal? TVSNextExperience { get; set; }
        //public decimal? TotalExperience { get; set; }
        public decimal? PreviousExperience { get; set; }
		public string OfficialMobileNumber { get; set; }
		public DateTime? BirthDate { get; set; }
		public DateTime? WeddingAnniversary { get; set; }
		public string Maritalstatus { get; set; }
		public int? ProbationStatusId { get; set; }
		public string FormattedEmployeeId { get; set; }
		public int? SystemRoleId { get; set; }
		public string EmployeeName { get; set; }
		public string NoticeCategory { get; set; }
		public string ExitType { get; set; }
		public DateTime? ResignationDate { get; set; }
		public DateTime? RetirementDate { get; set; }
		public string ResignationReason { get; set; }
		public string ResignationStatus { get; set; }
		public bool? IsResign { get; set; }
		public int? Entity { get; set; }
		public int? SourceOfHireId { get; set; }
		public string ProfilePicture { get; set; }
	}

	public static class Globals
	{
		public static string Id { get; set; }
		public static string ReqTime { get; set; }
		public static string ResTime { get; set; }
		public static string Req { get; set; }
		public static string Res { get; set; }
	}
}
