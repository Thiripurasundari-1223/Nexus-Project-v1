using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedLibraries.Models.Employee
{
    public class Roles
    {
		[Key]
		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public string RoleShortName { get; set; }
		public string RoleDescription { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
	}
}
