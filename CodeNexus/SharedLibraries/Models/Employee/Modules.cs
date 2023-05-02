using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Employee
{
    public class Modules
    {
		[Key]
		public int ModuleId { get; set; }
		public string ModuleName { get; set; }
		public string ModuleDescription { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public string ModuleShortDescription { get; set; }
		public string ModuleFullDescription { get; set; }
		public bool? IsActive { get; set; }
		public bool? IsMenu { get; set; }
		public string NavigationURL { get; set; }
		public string ModuleIcon { get; set; }
	}
}