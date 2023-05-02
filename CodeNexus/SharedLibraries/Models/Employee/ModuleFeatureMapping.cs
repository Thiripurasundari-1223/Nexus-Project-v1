using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibraries.Models.Employee
{
    public class ModuleFeatureMapping
    {
		[Key]
		public int ModuleFeatureMappingId { get; set; }
		public int? ModuleId { get; set; }
		public int? FeatureId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
	}
}