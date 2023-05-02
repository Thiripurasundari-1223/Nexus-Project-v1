using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class RolePermissions
    {
		[Key]
		public int RolePermissionId { get; set; }
		public int? ModuleId { get; set; }
		public int? FeatureId { get; set; }
		public int? RoleId { get; set; }
		public bool? IsEnabled { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
